using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;
using NHibernate.Persister.Entity;

namespace FluentNHibernate.Mapping
{
    public class ClassMap<T> : ClasslikeMapBase<T>, IMappingProvider
    {
        protected readonly AttributeStore<ClassMapping> attributes = new AttributeStore<ClassMapping>();
        private readonly IList<JoinMapping> joins = new List<JoinMapping>();
        private readonly OptimisticLockBuilder<ClassMap<T>> optimisticLock;

        /// <summary>
        /// Specify caching for this entity.
        /// </summary>
        public CachePart Cache { get; private set; }
        protected IIdentityMappingProvider id;

        private readonly IList<ImportPart> imports = new List<ImportPart>();
        private bool nextBool = true;

        private DiscriminatorPart discriminator;
        protected IVersionMappingProvider version;
        private ICompositeIdMappingProvider compositeId;
        private readonly HibernateMappingPart hibernateMappingPart = new HibernateMappingPart();
        private readonly PolymorphismBuilder<ClassMap<T>> polymorphism;

        public ClassMap()
        {
            optimisticLock = new OptimisticLockBuilder<ClassMap<T>>(this, value => attributes.Set(x => x.OptimisticLock, value));
            polymorphism = new PolymorphismBuilder<ClassMap<T>>(this, value => attributes.Set(x => x.Polymorphism, value));
            Cache = new CachePart(typeof(T));
        }

        ClassMapping IMappingProvider.GetClassMapping()
        {
		    var mapping = new ClassMapping(attributes.CloneInner());

            mapping.Type = typeof(T);
            mapping.Name = typeof(T).AssemblyQualifiedName;

            foreach (var property in properties)
                mapping.AddProperty(property.GetPropertyMapping());

            foreach (var component in components)
                mapping.AddComponent(component.GetComponentMapping());

            if (version != null)
                mapping.Version = version.GetVersionMapping();

            foreach (var oneToOne in oneToOnes)
                mapping.AddOneToOne(oneToOne.GetOneToOneMapping());

            foreach (var collection in collections)
                mapping.AddCollection(collection.GetCollectionMapping());

            foreach (var reference in references)
                mapping.AddReference(reference.GetManyToOneMapping());

            foreach (var any in anys)
                mapping.AddAny(any.GetAnyMapping());

            foreach (var subclass in subclasses.Values)
                mapping.AddSubclass(subclass.GetSubclassMapping());

		    foreach (var join in joins)
		        mapping.AddJoin(join);

            if (discriminator != null)
                mapping.Discriminator = ((IDiscriminatorMappingProvider)discriminator).GetDiscriminatorMapping();

            if (Cache.IsDirty)
                mapping.Cache = ((ICacheMappingProvider)Cache).GetCacheMapping();

            if (id != null)
                mapping.Id = id.GetIdentityMapping();

            if (compositeId != null)
                mapping.Id = compositeId.GetCompositeIdMapping();

            if (!mapping.IsSpecified(x => x.TableName))
                mapping.SetDefaultValue(x => x.TableName, GetDefaultTableName());

            return mapping;
        }

        private string GetDefaultTableName()
        {
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

            return "`" + tableName + "`";
        }

        public HibernateMapping GetHibernateMapping()
        {
            var hibernateMapping = ((IHibernateMappingProvider)hibernateMappingPart).GetHibernateMapping();

            foreach (var import in imports)
                hibernateMapping.AddImport(import.GetImportMapping());

            return hibernateMapping;
        }

        IEnumerable<string> IMappingProvider.GetIgnoredProperties()
        {
            return new string[0];
        }

        public HibernateMappingPart HibernateMapping
        {
            get { return hibernateMappingPart; }
        }

        public CompositeIdentityPart<T> CompositeId()
        {
            var part = new CompositeIdentityPart<T>();

            compositeId = part;

            return part;
        }

        public VersionPart Version(Expression<Func<T, object>> expression)
        {
            return Version(ReflectionHelper.GetProperty(expression));
        }

        protected virtual VersionPart Version(PropertyInfo property)
        {
            var versionPart = new VersionPart(typeof(T), property);

            version = versionPart;

            return versionPart;
        }

        public virtual DiscriminatorPart DiscriminateSubClassesOnColumn<TDiscriminator>(string columnName, TDiscriminator baseClassDiscriminator)
        {
            var part = new DiscriminatorPart(columnName, typeof(T), subclasses.Add, new TypeReference(typeof(TDiscriminator)));

            discriminator = part;

            attributes.Set(x => x.DiscriminatorValue, baseClassDiscriminator);

            return part;
        }

        public virtual DiscriminatorPart DiscriminateSubClassesOnColumn<TDiscriminator>(string columnName)
        {
            var part = new DiscriminatorPart(columnName, typeof(T), subclasses.Add, new TypeReference(typeof(TDiscriminator)));

            discriminator = part;

            return part;
        }

        public virtual DiscriminatorPart DiscriminateSubClassesOnColumn(string columnName)
        {
            return DiscriminateSubClassesOnColumn<string>(columnName);
        }

        public virtual IdentityPart Id(Expression<Func<T, object>> expression)
        {
            return Id(expression, null);
        }

        public virtual IdentityPart Id(Expression<Func<T, object>> expression, string column)
        {
            PropertyInfo property = ReflectionHelper.GetProperty(expression);
            var part = column == null ? new IdentityPart(EntityType, property) : new IdentityPart(EntityType, property);

            if (column != null)
                part.Column(column);

            id = part;
            
            return part;
        }

        [Obsolete("Inline definitions of subclasses are depreciated. Please create a derived class from SubclassMap in the same way you do with ClassMap.")]
        public virtual void JoinedSubClass<TSubclass>(string keyColumn, Action<JoinedSubClassPart<TSubclass>> action) where TSubclass : T
        {
            var subclass = new JoinedSubClassPart<TSubclass>(keyColumn);

            action(subclass);

            subclasses[typeof(TSubclass)] = subclass;
        }

        /// <summary>
        /// Sets the hibernate-mapping schema for this class.
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
        /// Inverse next boolean
        /// </summary>
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
        /// Sets additional tables for the class via the NH 2.0 Join element.
        /// </summary>
        /// <param name="tableName">Joined table name</param>
        /// <param name="action">Joined table mapping</param>
        public void Join(string tableName, Action<JoinPart<T>> action)
        {
            var join = new JoinPart<T>(tableName);

            action(join);

            joins.Add(((IJoinMappingProvider)join).GetJoinMapping());
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

        public PolymorphismBuilder<ClassMap<T>> Polymorphism
        {
            get { return polymorphism; }
        }

        public void CheckConstraint(string constraint)
        {
            attributes.Set(x => x.Check, constraint);
        }

        public void Persister<TPersister>() where TPersister : IEntityPersister
        {
            Persister(typeof(TPersister));
        }

        private void Persister(Type type)
        {
            Persister(type.AssemblyQualifiedName);
        }

        private void Persister(string type)
        {
            attributes.Set(x => x.Persister, type);
        }

        public void Proxy<TProxy>()
        {
            Proxy(typeof(TProxy));
        }

        private void Proxy(Type type)
        {
            Proxy(type.AssemblyQualifiedName);
        }

        private void Proxy(string type)
        {
            attributes.Set(x => x.Proxy, type);
        }

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
    }
}