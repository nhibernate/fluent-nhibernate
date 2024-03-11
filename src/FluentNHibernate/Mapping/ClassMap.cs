using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;
using NHibernate.Persister.Entity;

namespace FluentNHibernate.Mapping;

/// <summary>
/// Defines a mapping for an entity. Derive from this class to create a mapping,
/// and use the constructor to control how your entity is persisted.
/// </summary>
/// <example>
/// public class PersonMap : ClassMap&lt;Person&gt;
/// {
///   public PersonMap()
///   {
///     Id(x => x.PersonId);
///     Map(x => x.Name);
///     Map(x => x.Age);
///   }
/// }
/// </example>
/// <typeparam name="T">Entity type to map</typeparam>
public class ClassMap<T> : ClasslikeMapBase<T>, IMappingProvider
{
    protected readonly AttributeStore attributes;
    readonly MappingProviderStore providers;

    readonly IList<ImportPart> imports = new List<ImportPart>();
    bool nextBool = true;

    public ClassMap()
        : this(new AttributeStore(), new MappingProviderStore())
    {}

    protected ClassMap(AttributeStore attributes, MappingProviderStore providers)
        : base(providers)
    {
        this.attributes = attributes;
        this.providers = providers;
        OptimisticLock = new OptimisticLockBuilder<ClassMap<T>>(this, value => attributes.Set("OptimisticLock", Layer.UserSupplied, value));
        Polymorphism = new PolymorphismBuilder<ClassMap<T>>(this, value => attributes.Set("Polymorphism", Layer.UserSupplied, value));
        SchemaAction = new SchemaActionBuilder<ClassMap<T>>(this, value => attributes.Set("SchemaAction", Layer.UserSupplied, value));
        Cache = new CachePart(typeof(T));
    }

    /// <summary>
    /// Specify the caching for this entity.
    /// </summary>
    /// <example>
    /// Cache.ReadWrite();
    /// </example>
    public CachePart Cache { get; }

    /// <summary>
    /// Specify settings for the container/hibernate-mapping for this class.
    /// Note: Avoid using this, if possible prefer using conventions.
    /// </summary>
    /// <example>
    /// HibernateMapping.Schema("dto");
    /// </example>
    public HibernateMappingPart HibernateMapping { get; } = new HibernateMappingPart();

    #region Ids

    /// <summary>
    /// Specify the identifier for this entity.
    /// </summary>
    /// <param name="memberExpression">Identity property</param>
    /// <example>
    /// Id(x => x.PersonId);
    /// </example>
    public IdentityPart Id(Expression<Func<T, object>> memberExpression)
    {
        return Id(memberExpression, null);
    }

    /// <summary>
    /// Specify the identifier for this entity.
    /// </summary>
    /// <param name="memberExpression">Identity property</param>
    /// <param name="column">Column name</param>
    /// <example>
    /// Id(x => x.PersonId, "id");
    /// </example>
    public IdentityPart Id(Expression<Func<T, object>> memberExpression, string column)
    {
        var member = memberExpression.ToMember();

        OnMemberMapped(member);

        var part = new IdentityPart(EntityType, member);

        if (column is not null)
            part.Column(column);

        providers.Id = part;

        return part;
    }

    /// <summary>
    /// Create an Id that doesn't have a corresponding property in
    /// the domain object, or a column in the database. This is mainly
    /// for use with read-only access and/or views. Defaults to an int
    /// identity with an "increment" generator.
    /// </summary>
    public IdentityPart Id()
    {
        return Id<int>(null)
            .GeneratedBy.Increment();
    }

    /// <summary>
    /// Create an Id that doesn't have a corresponding property in
    /// the domain object, or a column in the database. This is mainly
    /// for use with read-only access and/or views.
    /// </summary>
    /// <typeparam name="TId">Type of the id</typeparam>
    public IdentityPart Id<TId>()
    {
        return Id<TId>(null);
    }

    /// <summary>
    /// Create an Id that doesn't have a corresponding property in
    /// the domain object.
    /// </summary>
    /// <typeparam name="TId">Type of the id</typeparam>
    /// <param name="column">Name and column name of the id</param>
    public IdentityPart Id<TId>(string column)
    {
        var part = new IdentityPart(typeof(T), typeof(TId));

        if (column is not null)
        {
            part.SetName(column);
            part.Column(column);
        }

        providers.Id = part;

        return part;
    }

    /// <summary>
    /// Create a natural identity. This is a secondary identifier
    /// that has "business meaning" moreso than the primary key.
    /// </summary>
    /// <example>
    /// NaturalId()
    ///   .Property(x => x.Name);
    /// </example>
    public NaturalIdPart<T> NaturalId()
    {
        var part = new NaturalIdPart<T>();

        providers.NaturalId = part;

        return part;
    }

    /// <summary>
    /// Create a composite identity. This is an identity composed of multiple
    /// columns.
    /// Note: Prefer using a surrogate key over a composite key whenever possible.
    /// </summary>
    /// <example>
    /// CompositeId()
    ///   .KeyProperty(x => x.FirstName)
    ///   .KeyProperty(x => x.LastName);
    /// </example>
    public CompositeIdentityPart<T> CompositeId()
    {
        var part = new CompositeIdentityPart<T>(OnMemberMapped);

        providers.CompositeId = part;

        return part;
    }

    /// <summary>
    /// Create a composite identity represented by an identity class. This is an
    /// identity composed of multiple columns.
    /// Note: Prefer using a surrogate key over a composite key whenever possible.
    /// </summary>
    /// <param name="memberExpression">Composite id property</param>
    /// <example>
    /// CompositeId(x => x.Id)
    ///   .KeyProperty(x => x.FirstName)
    ///   .KeyProperty(x => x.LastName);
    /// </example>
    public CompositeIdentityPart<TId> CompositeId<TId>(Expression<Func<T, TId>> memberExpression)
    {
        var member = memberExpression.ToMember();

        OnMemberMapped(member);

        var part = new CompositeIdentityPart<TId>(member.Name, OnMemberMapped);

        providers.CompositeId = part;

        return part;
    }

    #endregion

    /// <summary>
    /// Specifies that this class should be versioned/timestamped using the
    /// given property.
    /// </summary>
    /// <param name="memberExpression">Version/timestamp property</param>
    /// <example>
    /// Version(x => x.Timestamp);
    /// </example>
    public VersionPart Version(Expression<Func<T, object>> memberExpression)
    {
        return Version(memberExpression.ToMember());
    }

    VersionPart Version(Member member)
    {
        OnMemberMapped(member);

        var versionPart = new VersionPart(typeof(T), member);

        providers.Version = versionPart;

        return versionPart;
    }

    /// <summary>
    /// Specify that this entity should use a discriminator with it's subclasses.
    /// This is a mapping strategy called table-per-inheritance-hierarchy; where all
    /// subclasses are stored in the same table, differenciated by a discriminator
    /// column value.
    /// </summary>
    /// <typeparam name="TDiscriminator">Type of the discriminator column</typeparam>
    /// <param name="columnName">Discriminator column name</param>
    /// <param name="baseClassDiscriminator">Default discriminator value</param>
    public DiscriminatorPart DiscriminateSubClassesOnColumn<TDiscriminator>(string columnName, TDiscriminator baseClassDiscriminator)
    {
        var part = new DiscriminatorPart(columnName, typeof(T), providers.Subclasses.Add, new TypeReference(typeof(TDiscriminator)));

        providers.Discriminator = part;

        attributes.Set("DiscriminatorValue", Layer.UserSupplied, baseClassDiscriminator);

        return part;
    }

    /// <summary>
    /// Specify that this entity should use a discriminator with it's subclasses.
    /// This is a mapping strategy called table-per-inheritance-hierarchy; where all
    /// subclasses are stored in the same table, differenciated by a discriminator
    /// column value.
    /// </summary>
    /// <typeparam name="TDiscriminator">Type of the discriminator column</typeparam>
    /// <param name="columnName">Discriminator column name</param>
    public DiscriminatorPart DiscriminateSubClassesOnColumn<TDiscriminator>(string columnName)
    {
        var part = new DiscriminatorPart(columnName, typeof(T), providers.Subclasses.Add, new TypeReference(typeof(TDiscriminator)));

        providers.Discriminator = part;

        return part;
    }

    /// <summary>
    /// Specify that this entity should use a discriminator with it's subclasses.
    /// This is a mapping strategy called table-per-inheritance-hierarchy; where all
    /// subclasses are stored in the same table, differenciated by a discriminator
    /// column value.
    /// </summary>
    /// <param name="columnName">Discriminator column name</param>
    public DiscriminatorPart DiscriminateSubClassesOnColumn(string columnName)
    {
        return DiscriminateSubClassesOnColumn<string>(columnName);
    }

    /// <summary>
    /// Specifies that any subclasses of this entity should be treated as union-subclass
    /// mappings. Don't use this in combination with a discriminator, as they are mutually
    /// exclusive.
    /// </summary>
    public void UseUnionSubclassForInheritanceMapping()
    {
        attributes.Set("Abstract", Layer.UserSupplied, true);
        attributes.Set("IsUnionSubclass", Layer.UserSupplied, true);
    }

    [Obsolete("Inline definitions of subclasses are depreciated. Please create a derived class from SubclassMap in the same way you do with ClassMap.")]
    public void JoinedSubClass<TSubclass>(string keyColumn, Action<JoinedSubClassPart<TSubclass>> action) where TSubclass : T
    {
        var subclass = new JoinedSubClassPart<TSubclass>(keyColumn);

        action(subclass);

        providers.Subclasses[typeof(TSubclass)] = subclass;
    }

    /// <summary>
    /// Sets the schema for this class.
    /// </summary>
    /// <param name="schema">Schema name</param>
    public void Schema(string schema)
    {
        attributes.Set("Schema", Layer.UserSupplied, schema);
    }

    /// <summary>
    /// Sets the table for the class.
    /// </summary>
    /// <param name="tableName">Table name</param>
    public void Table(string tableName)
    {
        attributes.Set("TableName", Layer.UserSupplied, tableName);
    }

    /// <summary>
    /// Inverts the next boolean option
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ClassMap<T> Not
    {
        get
        {
            nextBool = !nextBool;
            return this;
        }
    }

    /// <summary>
    /// Sets this entity to be lazy-loaded (overrides the default lazy load configuration).
    /// </summary>
    public void LazyLoad()
    {
        attributes.Set("Lazy", Layer.UserSupplied, nextBool);
        nextBool = true;
    }

    /// <summary>
    /// Links this entity to another table, to create a composite entity from two or
    /// more tables.
    /// </summary>
    /// <param name="tableName">Joined table name</param>
    /// <param name="action">Joined table mapping</param>
    /// <example>
    /// Join("another_table", join =>
    /// {
    ///   join.Map(x => x.Name);
    ///   join.Map(x => x.Age);
    /// });
    /// </example>
    public void Join(string tableName, Action<JoinPart<T>> action)
    {
        var join = new JoinPart<T>(tableName);

        action(join);

        providers.Joins.Add(join);
    }

    /// <summary>
    /// Imports an existing type for use in the mapping.
    /// </summary>
    /// <typeparam name="TImport">Type to import.</typeparam>
    public ImportPart ImportType<TImport>()
    {
        var part = new ImportPart(typeof(TImport));
            
        imports.Add(part);

        return part;
    }

    /// <summary>
    /// Set the mutability of this class, sets the mutable attribute.
    /// </summary>
    public void ReadOnly()
    {
        attributes.Set("Mutable", Layer.UserSupplied, !nextBool);
        nextBool = true;
    }

    /// <summary>
    /// Sets this entity to be dynamic update
    /// </summary>
    public void DynamicUpdate()
    {
        attributes.Set("DynamicUpdate", Layer.UserSupplied, nextBool);
        nextBool = true;
    }

    /// <summary>
    /// Sets this entity to be dynamic insert
    /// </summary>
    public void DynamicInsert()
    {
        attributes.Set("DynamicInsert", Layer.UserSupplied, nextBool);
        nextBool = true;
    }

    /// <summary>
    /// Sets the query batch size for this entity.
    /// </summary>
    /// <param name="size">Batch size</param>
    public ClassMap<T> BatchSize(int size)
    {
        attributes.Set("BatchSize", Layer.UserSupplied, size);
        return this;
    }

    /// <summary>
    /// Sets the optimistic locking strategy
    /// </summary>
    public OptimisticLockBuilder<ClassMap<T>> OptimisticLock { get; }

    /// <summary>
    /// Sets the polymorphism behaviour
    /// </summary>
    public PolymorphismBuilder<ClassMap<T>> Polymorphism { get; }

    /// <summary>
    /// Sets the schema action behaviour
    /// </summary>
    public SchemaActionBuilder<ClassMap<T>> SchemaAction { get; }

    /// <summary>
    /// Specifies a check constraint
    /// </summary>
    /// <param name="constraint">Constraint name</param>
    public void CheckConstraint(string constraint)
    {
        attributes.Set("Check", Layer.UserSupplied, constraint);
    }

    /// <summary>
    /// Specifies a persister to be used with this entity
    /// </summary>
    /// <typeparam name="TPersister">Persister type</typeparam>
    public void Persister<TPersister>() where TPersister : IEntityPersister
    {
        Persister(typeof(TPersister));
    }

    /// <summary>
    /// Specifies a persister to be used with this entity
    /// </summary>
    /// <param name="type">Persister type</param>
    private void Persister(Type type)
    {
        Persister(type.AssemblyQualifiedName);
    }

    /// <summary>
    /// Specifies a persister to be used with this entity
    /// </summary>
    /// <param name="type">Persister type</param>
    public void Persister(string type)
    {
        attributes.Set("Persister", Layer.UserSupplied, type);
    }

    /// <summary>
    /// Specifies a proxy class for this entity.
    /// </summary>
    /// <typeparam name="TProxy">Proxy type</typeparam>
    public void Proxy<TProxy>()
    {
        Proxy(typeof(TProxy));
    }

    /// <summary>
    /// Specifies a proxy class for this entity.
    /// </summary>
    /// <param name="type">Proxy type</param>
    public void Proxy(Type type)
    {
        Proxy(type.AssemblyQualifiedName);
    }

    /// <summary>
    /// Specifies a proxy class for this entity.
    /// </summary>
    /// <param name="type">Proxy type</param>
    public void Proxy(string type)
    {
        attributes.Set("Proxy", Layer.UserSupplied, type);
    }

    /// <summary>
    /// Specifies that a select should be performed before updating
    /// this entity
    /// </summary>
    public void SelectBeforeUpdate()
    {
        attributes.Set("SelectBeforeUpdate", Layer.UserSupplied, nextBool);
        nextBool = true;
    }

    /// <summary>
    /// Defines a SQL 'where' clause used when retrieving objects of this type.
    /// </summary>
    public void Where(string where)
    {
        attributes.Set("Where", Layer.UserSupplied, where);
    }

    /// <summary>
    /// Sets the SQL statement used in subselect fetching.
    /// </summary>
    /// <param name="subselectSql">Subselect SQL Query</param>
    public void Subselect(string subselectSql)
    {
        attributes.Set("Subselect", Layer.UserSupplied, subselectSql);
    }

    /// <summary>
    /// Specifies an entity-name.
    /// </summary>
    /// <remarks>See https://nhibernate.info/blog/2008/10/21/entity-name-in-action-a-strongly-typed-entity.html </remarks>
    public void EntityName(string entityName)
    {
        attributes.Set("EntityName", Layer.UserSupplied, entityName);
    }

    /// <summary>
    /// Applies a filter to this entity given its name.
    /// </summary>
    /// <param name="name">The filter's name</param>
    /// <param name="condition">The condition to apply</param>
    public ClassMap<T> ApplyFilter(string name, string condition)
    {
        var part = new FilterPart(name, condition);
        providers.Filters.Add(part);
        return this;
    }

    /// <summary>
    /// Applies a filter to this entity given its name.
    /// </summary>
    /// <param name="name">The filter's name</param>
    public ClassMap<T> ApplyFilter(string name)
    {
        return ApplyFilter(name, null);
    }

    /// <summary>
    /// Applies a named filter to this entity.
    /// </summary>
    /// <param name="condition">The condition to apply</param>
    /// <typeparam name="TFilter">
    /// The type of a <see cref="FilterDefinition"/> implementation
    /// defining the filter to apply.
    /// </typeparam>
    public ClassMap<T> ApplyFilter<TFilter>(string condition) where TFilter : FilterDefinition, new()
    {
        return ApplyFilter(new TFilter().Name, condition);
    }

    /// <summary>
    /// Applies a named filter to this entity.
    /// </summary>
    /// <typeparam name="TFilter">
    /// The type of a <see cref="FilterDefinition"/> implementation
    /// defining the filter to apply.
    /// </typeparam>
    public ClassMap<T> ApplyFilter<TFilter>() where TFilter : FilterDefinition, new()
    {
        return ApplyFilter<TFilter>(null);
    }

    /// <summary>
    /// Configures the tuplizer for this entity. The tuplizer defines how to transform
    /// a Property-Value to its persistent representation, and viceversa a Column-Value
    /// to its in-memory representation, and the EntityMode defines which tuplizer is in use.
    /// </summary>
    /// <param name="mode">Tuplizer entity-mode</param>
    /// <param name="tuplizerType">Tuplizer type</param>
    public TuplizerPart Tuplizer(TuplizerMode mode, Type tuplizerType)
    {
        providers.TuplizerMapping = new TuplizerMapping();
        providers.TuplizerMapping.Set(x => x.Mode, Layer.UserSupplied, mode);
        providers.TuplizerMapping.Set(x => x.Type, Layer.UserSupplied, new TypeReference(tuplizerType));

        return new TuplizerPart(providers.TuplizerMapping)
            .Type(tuplizerType)
            .Mode(mode);
    }

    ClassMapping IMappingProvider.GetClassMapping()
    {
        var mapping = new ClassMapping(attributes.Clone());

        mapping.Set(x => x.Type, Layer.Defaults, typeof(T));
        mapping.Set(x => x.Name, Layer.Defaults, typeof(T).AssemblyQualifiedName);

        if (providers.Version is not null)
            mapping.Set(x => x.Version, Layer.Defaults, providers.Version.GetVersionMapping());

        foreach (var provider in providers.OrderedProviders) {
            var x = provider.Item2;
            switch (provider.Item1) {
                case MappingProviderStore.ProviderType.Property:
                    mapping.AddProperty(((IPropertyMappingProvider) x).GetPropertyMapping());
                    break;
                case MappingProviderStore.ProviderType.Component:
                    mapping.AddComponent(((IComponentMappingProvider) x).GetComponentMapping());
                    break;
                case MappingProviderStore.ProviderType.OneToOne:
                    mapping.AddOneToOne(((IOneToOneMappingProvider) x).GetOneToOneMapping());
                    break;
                case MappingProviderStore.ProviderType.Subclass:
                    mapping.AddSubclass(((ISubclassMappingProvider) x).GetSubclassMapping());
                    break;
                case MappingProviderStore.ProviderType.Collection:
                    mapping.AddCollection(((ICollectionMappingProvider) x).GetCollectionMapping());
                    break;
                case MappingProviderStore.ProviderType.ManyToOne:
                    mapping.AddReference(((IManyToOneMappingProvider) x).GetManyToOneMapping());
                    break;
                case MappingProviderStore.ProviderType.Any:
                    mapping.AddAny(((IAnyMappingProvider) x).GetAnyMapping());
                    break;
                case MappingProviderStore.ProviderType.Filter:
                    mapping.AddFilter(((IFilterMappingProvider) x).GetFilterMapping());
                    break;
                case MappingProviderStore.ProviderType.StoredProcedure:
                    mapping.AddStoredProcedure(((IStoredProcedureMappingProvider) x).GetStoredProcedureMapping());
                    break;
                case MappingProviderStore.ProviderType.Join:
                    mapping.AddJoin(((IJoinMappingProvider) x).GetJoinMapping());
                    break;
                case MappingProviderStore.ProviderType.Identity:
                    mapping.Set(y => y.Id, Layer.Defaults, ((IIdentityMappingProvider) x).GetIdentityMapping());
                    break;
                case MappingProviderStore.ProviderType.CompositeId:
                    mapping.Set(y => y.Id, Layer.Defaults, ((ICompositeIdMappingProvider) x).GetCompositeIdMapping());
                    break;
                case MappingProviderStore.ProviderType.NaturalId:
                    mapping.Set(y => y.NaturalId, Layer.Defaults, ((INaturalIdMappingProvider) x).GetNaturalIdMapping());
                    break;
                case MappingProviderStore.ProviderType.Version:
                    mapping.Set(y => y.Version, Layer.Defaults, ((IVersionMappingProvider) x).GetVersionMapping());
                    break;
                case MappingProviderStore.ProviderType.Discriminator:
                    mapping.Set(y => y.Discriminator, Layer.Defaults, ((IDiscriminatorMappingProvider) x).GetDiscriminatorMapping());
                    break;
                case MappingProviderStore.ProviderType.Tupilizer:
                    mapping.Set(y => y.Tuplizer, Layer.Defaults, (TuplizerMapping)x);
                    break;
                default:
                    throw new Exception("Internal Error");
            }
        }

        if (Cache.IsDirty)
            mapping.Set(x  => x.Cache, Layer.Defaults, ((ICacheMappingProvider)Cache).GetCacheMapping());

        mapping.Set(x => x.TableName, Layer.Defaults, GetDefaultTableName());

        mapping.Set(x => x.Tuplizer, Layer.Defaults, providers.TuplizerMapping);

        return mapping;
    }

    HibernateMapping IMappingProvider.GetHibernateMapping()
    {
        var hibernateMapping = ((IHibernateMappingProvider)HibernateMapping).GetHibernateMapping();

        foreach (var import in imports)
            hibernateMapping.AddImport(import.GetImportMapping());

        return hibernateMapping;
    }

    string GetDefaultTableName()
    {
#pragma warning disable 612,618
        var tableName = EntityType.Name;

        if (EntityType.IsGenericType)
        {
            // special case for generics: GenericType_GenericParameterType
            var genericQuoteIndex = EntityType.Name.IndexOf('`');
            if (genericQuoteIndex >= 0)
            {
                tableName = EntityType.Name.Substring(0, genericQuoteIndex);
            } // else generic declaration not directly in this class

            foreach (var argument in EntityType.GetGenericArguments())
            {
                tableName += "_";
                tableName += argument.Name;
            }
        }
#pragma warning restore 612,618

        return "`" + tableName + "`";
    }

    IEnumerable<Member> IMappingProvider.GetIgnoredProperties()
    {
        return Array.Empty<Member>();
    }
}
