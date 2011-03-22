using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;
using NHibernate.Persister.Entity;

namespace FluentNHibernate.Mapping
{
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
        readonly AttributeStore<ClassMapping> attributes;
        private readonly MappingProviderStore providers;
        readonly OptimisticLockBuilder<ClassMap<T>> optimisticLock;

        readonly IList<ImportPart> imports = new List<ImportPart>();
        private bool nextBool = true;

        readonly HibernateMappingPart hibernateMappingPart = new HibernateMappingPart();
        readonly PolymorphismBuilder<ClassMap<T>> polymorphism;
        readonly SchemaActionBuilder<ClassMap<T>> schemaAction;

        public ClassMap()
            : this(new AttributeStore<ClassMapping>(), new MappingProviderStore())
        {}

        protected ClassMap(AttributeStore<ClassMapping> attributes, MappingProviderStore providers)
            : base(providers)
        {
            this.attributes = attributes;
            this.providers = providers;
            optimisticLock = new OptimisticLockBuilder<ClassMap<T>>(this, value => attributes.Set(x => x.OptimisticLock, value));
            polymorphism = new PolymorphismBuilder<ClassMap<T>>(this, value => attributes.Set(x => x.Polymorphism, value));
            schemaAction = new SchemaActionBuilder<ClassMap<T>>(this, value => attributes.Set(x => x.SchemaAction, value));
            Cache = new CachePart(typeof(T));
        }

        /// <summary>
        /// Specify the caching for this entity.
        /// </summary>
        /// <example>
        /// Cache.ReadWrite();
        /// </example>
        public CachePart Cache { get; private set; }

        /// <summary>
        /// Specify settings for the container/hibernate-mapping for this class.
        /// Note: Avoid using this, if possible prefer using conventions.
        /// </summary>
        /// <example>
        /// HibernateMapping.Schema("dto");
        /// </example>
        public HibernateMappingPart HibernateMapping
        {
            get { return hibernateMappingPart; }
        }

        #region Ids

        /// <summary>
        /// Specify the identifier for this entity.
        /// </summary>
        /// <param name="memberExpression">Identity property</param>
        /// <example>
        /// Id(x => x.PersonId);
        /// </example>
        public virtual IdentityPart Id(Expression<Func<T, object>> memberExpression)
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
        public virtual IdentityPart Id(Expression<Func<T, object>> memberExpression, string column)
        {
            var member = memberExpression.ToMember();
#pragma warning disable 612,618
            var part = new IdentityPart(EntityType, member);
#pragma warning restore 612,618

            if (column != null)
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

            if (column != null)
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
        public virtual NaturalIdPart<T> NaturalId()
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
        public virtual CompositeIdentityPart<T> CompositeId()
        {
            var part = new CompositeIdentityPart<T>();

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
        public virtual CompositeIdentityPart<TId> CompositeId<TId>(Expression<Func<T, TId>> memberExpression)
        {
            var part = new CompositeIdentityPart<TId>(memberExpression.ToMember().Name);

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

        protected virtual VersionPart Version(Member property)
        {
            var versionPart = new VersionPart(typeof(T), property);

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
        public virtual DiscriminatorPart DiscriminateSubClassesOnColumn<TDiscriminator>(string columnName, TDiscriminator baseClassDiscriminator)
        {
            var part = new DiscriminatorPart(columnName, typeof(T), providers.Subclasses.Add, new TypeReference(typeof(TDiscriminator)));

            providers.Discriminator = part;

            attributes.Set(x => x.DiscriminatorValue, baseClassDiscriminator);

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
        public virtual DiscriminatorPart DiscriminateSubClassesOnColumn<TDiscriminator>(string columnName)
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
        public virtual DiscriminatorPart DiscriminateSubClassesOnColumn(string columnName)
        {
            return DiscriminateSubClassesOnColumn<string>(columnName);
        }

        /// <summary>
        /// Specifies that any subclasses of this entity should be treated as union-subclass
        /// mappings. Don't use this in combination with a discriminator, as they are mutually
        /// exclusive.
        /// </summary>
        public virtual void UseUnionSubclassForInheritanceMapping()
        {
            attributes.Set(x => x.Abstract, true);
            attributes.Set(x => x.IsUnionSubclass, true);
        }

        [Obsolete("Inline definitions of subclasses are depreciated. Please create a derived class from SubclassMap in the same way you do with ClassMap.")]
        public virtual void JoinedSubClass<TSubclass>(string keyColumn, Action<JoinedSubClassPart<TSubclass>> action) where TSubclass : T
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
            attributes.Set(x => x.Schema, schema);
        }

        /// <summary>
        /// Sets the table for the class.
        /// </summary>
        /// <param name="tableName">Table name</param>
        public void Table(string tableName)
        {
            attributes.Set(x => x.TableName, tableName);
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
            attributes.Set(x => x.Lazy, nextBool);
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
        public virtual void Join(string tableName, Action<JoinPart<T>> action)
        {
            var join = new JoinPart<T>(tableName);

            action(join);

            providers.Joins.Add(join);
        }

        /// <summary>
        /// Imports an existing type for use in the mapping.
        /// </summary>
        /// <typeparam name="TImport">Type to import.</typeparam>
        public virtual ImportPart ImportType<TImport>()
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
            attributes.Set(x => x.Mutable, !nextBool);
            nextBool = true;
        }

        /// <summary>
        /// Sets this entity to be dynamic update
        /// </summary>
        public void DynamicUpdate()
        {
            attributes.Set(x => x.DynamicUpdate, nextBool);
            nextBool = true;
        }

        /// <summary>
        /// Sets this entity to be dynamic insert
        /// </summary>
        public void DynamicInsert()
        {
            attributes.Set(x => x.DynamicInsert, nextBool);
            nextBool = true;
        }

        /// <summary>
        /// Sets the query batch size for this entity.
        /// </summary>
        /// <param name="size">Batch size</param>
        public ClassMap<T> BatchSize(int size)
        {
            attributes.Set(x => x.BatchSize, size);
            return this;
        }

        /// <summary>
        /// Sets the optimistic locking strategy
        /// </summary>
        public OptimisticLockBuilder<ClassMap<T>> OptimisticLock
        {
            get { return optimisticLock; }
        }

        /// <summary>
        /// Sets the polymorphism behaviour
        /// </summary>
        public PolymorphismBuilder<ClassMap<T>> Polymorphism
        {
            get { return polymorphism; }
        }

        /// <summary>
        /// Sets the schema action behaviour
        /// </summary>
        public SchemaActionBuilder<ClassMap<T>> SchemaAction
        {
            get { return schemaAction; }
        }

        /// <summary>
        /// Specifies a check constraint
        /// </summary>
        /// <param name="constraint">Constraint name</param>
        public void CheckConstraint(string constraint)
        {
            attributes.Set(x => x.Check, constraint);
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
            attributes.Set(x => x.Persister, type);
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
            attributes.Set(x => x.Proxy, type);
        }

        /// <summary>
        /// Specifies that a select should be performed before updating
        /// this entity
        /// </summary>
        public void SelectBeforeUpdate()
        {
            attributes.Set(x => x.SelectBeforeUpdate, nextBool);
            nextBool = true;
        }

        /// <summary>
		/// Defines a SQL 'where' clause used when retrieving objects of this type.
		/// </summary>
    	public void Where(string where)
    	{
            attributes.Set(x => x.Where, where);
    	}

        /// <summary>
        /// Sets the SQL statement used in subselect fetching.
        /// </summary>
        /// <param name="subselectSql">Subselect SQL Query</param>
        public void Subselect(string subselectSql)
        {
            attributes.Set(x => x.Subselect, subselectSql);
        }

        /// <summary>
        /// Specifies an entity-name.
        /// </summary>
        /// <remarks>See http://nhforge.org/blogs/nhibernate/archive/2008/10/21/entity-name-in-action-a-strongly-typed-entity.aspx</remarks>
        public void EntityName(string entityName)
        {
            attributes.Set(x => x.EntityName, entityName);
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
            return this.ApplyFilter(name, null);
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
            providers.TuplizerMapping.Mode = mode;
            providers.TuplizerMapping.Type = new TypeReference(tuplizerType);

            return new TuplizerPart(providers.TuplizerMapping)
                .Type(tuplizerType)
                .Mode(mode);
        }

        ClassMapping IMappingProvider.GetClassMapping()
        {
            var mapping = new ClassMapping(attributes.CloneInner());

            mapping.Type = typeof(T);
            mapping.Name = typeof(T).AssemblyQualifiedName;

            foreach (var property in providers.Properties)
                mapping.AddProperty(property.GetPropertyMapping());

            foreach (var component in providers.Components)
                mapping.AddComponent(component.GetComponentMapping());

            if (providers.Version != null)
                mapping.Version = providers.Version.GetVersionMapping();

            foreach (var oneToOne in providers.OneToOnes)
                mapping.AddOneToOne(oneToOne.GetOneToOneMapping());

            foreach (var collection in providers.Collections)
                mapping.AddCollection(collection.GetCollectionMapping());

            foreach (var reference in providers.References)
                mapping.AddReference(reference.GetManyToOneMapping());

            foreach (var any in providers.Anys)
                mapping.AddAny(any.GetAnyMapping());

            foreach (var subclass in providers.Subclasses.Values)
                mapping.AddSubclass(subclass.GetSubclassMapping());

            foreach (var join in providers.Joins)
                mapping.AddJoin(join.GetJoinMapping());

            if (providers.Discriminator != null)
                mapping.Discriminator = providers.Discriminator.GetDiscriminatorMapping();

            if (Cache.IsDirty)
                mapping.Cache = ((ICacheMappingProvider)Cache).GetCacheMapping();

            if (providers.Id != null)
                mapping.Id = providers.Id.GetIdentityMapping();

            if (providers.CompositeId != null)
                mapping.Id = providers.CompositeId.GetCompositeIdMapping();

            if (providers.NaturalId != null)
                mapping.NaturalId = providers.NaturalId.GetNaturalIdMapping();

            if (!mapping.IsSpecified("TableName"))
                mapping.SetDefaultValue(x => x.TableName, GetDefaultTableName());

            foreach (var filter in providers.Filters)
                mapping.AddFilter(filter.GetFilterMapping());

            foreach (var storedProcedure in providers.StoredProcedures)
                mapping.AddStoredProcedure(storedProcedure.GetStoredProcedureMapping());

            mapping.Tuplizer = providers.TuplizerMapping;

            return mapping;
        }

        HibernateMapping IMappingProvider.GetHibernateMapping()
        {
            var hibernateMapping = ((IHibernateMappingProvider)hibernateMappingPart).GetHibernateMapping();

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
                tableName = EntityType.Name.Substring(0, EntityType.Name.IndexOf('`'));

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
            return new Member[0];
        }
    }
}