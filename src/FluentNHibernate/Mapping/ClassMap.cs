using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.AutoMap;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public class ClassMap<T> : ClasslikeMapBase<T>, IClassMap
    {
        private readonly Cache<string, string> unmigratedAttributes;
        public Cache<string, string> HibernateMappingAttributes { get; private set; }
        private readonly IOptimisticLockBuilder optimisticLock;
        private readonly AccessStrategyBuilder<ClassMap<T>> defaultAccess;

        /// <summary>
        /// Specify caching for this entity.
        /// </summary>
        public ICache Cache { get; private set; }
        private IIdentityPart id;

        private readonly IList<ImportPart> imports = new List<ImportPart>();
        private bool nextBool = true;

        private readonly ClassMapping mapping;
        private readonly HibernateMapping hibernateMapping = new HibernateMapping();
        private IDiscriminatorPart discriminator;

        public ClassMap()
            : this(new ClassMapping(typeof(T)))
        {}

        public ClassMap(ClassMapping mapping)
        {
            this.mapping = mapping;
            unmigratedAttributes = new Cache<string, string>();
            HibernateMappingAttributes = new Cache<string, string>();
            defaultAccess = new AccessStrategyBuilder<ClassMap<T>>(this, value => hibernateMapping.DefaultAccess = value);
            optimisticLock = new OptimisticLockBuilder<ClassMap<T>>(this, value => mapping.OptimisticLock = value);
            Cache = new CachePart();
        }

        public string TableName
        {
            get { return mapping.TableName; }
        }

        public ClassMapping GetClassMapping()
        {
            mapping.Name = typeof(T).FullName;

            foreach (var property in properties)
                mapping.AddProperty(property.GetPropertyMapping());

            foreach (var component in components)
                mapping.AddComponent(component.GetComponentMapping());

            foreach (var version in versions)
                mapping.AddVersion(version.GetVersionMapping());

            foreach (var oneToOne in oneToOnes)
                mapping.AddOneToOne(oneToOne.GetOneToOneMapping());

            foreach (var collection in collections)
                mapping.AddCollection(collection.GetCollectionMapping());

            foreach (var reference in references)
                mapping.AddReference(reference.GetManyToOneMapping());

            if (discriminator != null)
                mapping.Discriminator = discriminator.GetDiscriminatorMapping();

            if (Cache.IsDirty)
                mapping.Cache = Cache.GetCacheMapping();

            if (id != null)
                mapping.Id = id.GetIdMapping();

            foreach (var part in Parts)
                mapping.AddUnmigratedPart(part);

            unmigratedAttributes.ForEachPair(mapping.AddUnmigratedAttribute);

            return mapping;
        }

        public HibernateMapping GetHibernateMapping()
        {
            foreach (var import in imports)
                hibernateMapping.AddImport(import.GetImportMapping());

            HibernateMappingAttributes.ForEachPair(hibernateMapping.AddUnmigratedAttribute);

            return hibernateMapping;
        }

        public CompositeIdentityPart<T> UseCompositeId()
        {
            var part = new CompositeIdentityPart<T>();
			
            AddPart(part);

            return part;
        }

        public virtual DiscriminatorPart DiscriminateSubClassesOnColumn<TDiscriminator>(string columnName, TDiscriminator baseClassDiscriminator)
        {
            var part = new DiscriminatorPart(this, mapping, columnName);

            discriminator = part;

            mapping.DiscriminatorBaseValue = baseClassDiscriminator;

            return part;
        }

        public virtual DiscriminatorPart DiscriminateSubClassesOnColumn<TDiscriminator>(string columnName)
        {
            var part = new DiscriminatorPart(this, mapping, columnName);

            discriminator = part;

            return part;
        }

        public virtual DiscriminatorPart DiscriminateSubClassesOnColumn(string columnName)
        {
            return DiscriminateSubClassesOnColumn<string>(columnName);
        }

        /// <summary>
        /// Set an attribute on the xml element produced by this class mapping.
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
        public virtual void SetAttribute(string name, string value)
        {
            unmigratedAttributes.Store(name, value);
        }

        public virtual void SetAttributes(Attributes atts)
        {
            foreach (var key in atts.Keys)
            {
                SetAttribute(key, atts[key]);
            }
        }

        public void SetHibernateMappingAttribute(string name, string value)
        {
            HibernateMappingAttributes.Store(name, value);
        }

        public void SetHibernateMappingAttribute(string name, bool value)
        {
            HibernateMappingAttributes.Store(name, value.ToString().ToLowerInvariant());
        }

        public virtual IIdentityPart Id(Expression<Func<T, object>> expression)
        {
            return Id(expression, null);
        }

        public virtual IIdentityPart Id(Expression<Func<T, object>> expression, string column)
        {
            PropertyInfo property = ReflectionHelper.GetProperty(expression);
            
            id = column == null ? new IdentityPart(EntityType, property) : new IdentityPart(EntityType, property, column);
            
            return id;
        }

        public virtual void JoinedSubClass<TSubclass>(string keyColumn, Action<JoinedSubClassPart<TSubclass>> action) where TSubclass : T
        {
            var subclass = new JoinedSubClassPart<TSubclass>(keyColumn);

            action(subclass);

            joinedSubclasses.Add(subclass);
            
            mapping.AddSubclass(subclass.GetJoinedSubclassMapping());
        }

        /// <summary>
        /// Sets the hibernate-mapping schema for this class.
        /// </summary>
        /// <param name="schema">Schema name</param>
        public void SchemaIs(string schema)
        {
            mapping.Schema = schema;
        }

        /// <summary>
        /// Sets the hibernate-mapping auto-import for this class.
        /// </summary>
        public void AutoImport()
        {
            hibernateMapping.AutoImport = nextBool;
            nextBool = true;
        }

        /// <summary>
        /// Set the default access and naming strategies for this entire mapping.
        /// </summary>
        public AccessStrategyBuilder<ClassMap<T>> DefaultAccess
        {
            get { return defaultAccess; }
        }

        /// <summary>
        /// Sets the table for the class.
        /// </summary>
        /// <param name="tableName">Table name</param>
        public void WithTable(string tableName)
        {
            mapping.TableName = tableName;
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

        IClassMap IClassMap.Not
        {
            get { return Not; }
        }

        /// <summary>
        /// Sets this entity to be lazy-loaded (overrides the default lazy load configuration).
        /// </summary>
        public void LazyLoad()
        {
            mapping.LazyLoad = nextBool;
            nextBool = true;
        }

        /// <summary>
        /// Sets additional tables for the class via the NH 2.0 Join element.
        /// </summary>
        /// <param name="tableName">Joined table name</param>
        /// <param name="action">Joined table mapping</param>
        public void WithTable(string tableName, Action<JoinPart<T>> action)
        {
            var join = new JoinPart<T>(tableName);
            action(join);
            mapping.AddJoin(join.GetJoinMapping());
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
            mapping.Mutable = !nextBool;
            nextBool = true;
        }

        /// <summary>
        /// Sets this entity to be dynamic update
        /// </summary>
        public void DynamicUpdate()
        {
            mapping.DynamicUpdate = nextBool;
            nextBool = true;
        }

        /// <summary>
        /// Sets this entity to be dynamic insert
        /// </summary>
        public void DynamicInsert()
        {
            mapping.DynamicInsert = nextBool;
            nextBool = true;
        }

        public IClassMap BatchSize(int size)
        {
            mapping.BatchSize = size;
            return this;
        }

        /// <summary>
        /// Sets the optimistic locking strategy
        /// </summary>
        public IOptimisticLockBuilder OptimisticLock
        {
            get { return optimisticLock; }
        }

        Cache<string, string> IClassMap.Attributes
        {
            get { return unmigratedAttributes; }
        }
    }
}