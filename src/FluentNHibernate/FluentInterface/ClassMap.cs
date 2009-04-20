using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public class ClassMap<T> : ClasslikeMapBase<T>, IClassMap
    {
        public Cache<string, string> Attributes { get; private set; }
        public Cache<string, string> HibernateMappingAttributes { get; private set; }
        private readonly AccessStrategyBuilder<ClassMap<T>> defaultAccess;
        
        /// <summary>
        /// Specify caching for this entity.
        /// </summary>
        public ICache Cache { get; private set; }
        
        private readonly IList<ImportPart> imports = new List<ImportPart>();
        private bool nextBool = true;

        private readonly ClassMapping mapping;

        public ClassMap()
            : this(new ClassMapping(typeof(T)))
        {
            Attributes = new Cache<string, string>();
            HibernateMappingAttributes = new Cache<string, string>();
            //defaultAccess = new DefaultAccessStrategyBuilder<T>(this);
            Cache = new CachePart();
        }

        public ClassMap(ClassMapping mapping)
        {
            this.mapping = mapping;
        }

        public string TableName
        {
            get { return mapping.Tablename; }
        }

        public ClassMapping GetClassMapping()
        {
            AddPart(Cache);

            foreach (var part in Parts)
            {
                mapping.AddUnmigratedPart(part);
            }

            Attributes.ForEachPair(mapping.AddUnmigratedAttribute);

            return mapping;
        }

        public Type EntityType
        {
            get { return typeof(T); }
        }

        public void UseIdentityForKey(Expression<Func<T, object>> expression, string columnName)
        {
            PropertyInfo property = ReflectionHelper.GetProperty(expression);
            var part = new IdentityPart(EntityType, property, columnName);

            AddPart(part);
        }

        public CompositeIdentityPart<T> UseCompositeId()
        {
            var part = new CompositeIdentityPart<T>();
			
            AddPart(part);

            return part;
        }

        public virtual DiscriminatorPart<TDiscriminator, T> DiscriminateSubClassesOnColumn<TDiscriminator>(string columnName, TDiscriminator baseClassDiscriminator)
        {
            var part = new DiscriminatorPart<TDiscriminator, T>(columnName, baseClassDiscriminator, this);
            
            AddPart(part);

            return part;
        }

        public virtual DiscriminatorPart<TDiscriminator, T> DiscriminateSubClassesOnColumn<TDiscriminator>(string columnName)
        {
            var part = new DiscriminatorPart<TDiscriminator, T>(columnName, this);
            
            AddPart(part);

            return part;
        }

        public virtual DiscriminatorPart<string, T> DiscriminateSubClassesOnColumn(string columnName)
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
            Attributes.Store(name, value);
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
            var id = column == null ? new IdentityPart(EntityType, property) : new IdentityPart(EntityType, property, column);
            AddPart(id);
            return id;
        }

        public virtual JoinedSubClassPart<TSubclass> JoinedSubClass<TSubclass>(string keyColumn, Action<JoinedSubClassPart<TSubclass>> action) where TSubclass : T
        {
            var subclass = new JoinedSubClassPart<TSubclass>(keyColumn);

            action(subclass);
            AddPart(subclass);

            return subclass;
        }

        /// <summary>
        /// Sets the hibernate-mapping schema for this class.
        /// </summary>
        /// <param name="schema">Schema name</param>
        public void SchemaIs(string schema)
        {
            SetAttribute("schema", schema);
        }

        /// <summary>
        /// Sets the hibernate-mapping auto-import for this class.
        /// </summary>
        public void AutoImport()
        {
            SetHibernateMappingAttribute("auto-import", nextBool);
            nextBool = true;
        }

        /// <summary>
        /// Override the inferred assembly for this class
        /// </summary>
        /// <param name="assembly">Assembly to use</param>
        public void OverrideAssembly(Assembly assembly)
        {
        }

        /// <summary>
        /// Override the inferred assembly for this class
        /// </summary>
        /// <param name="assembly">Assembly to use</param>
        public void OverrideAssembly(string assembly)
        {
        }

        /// <summary>
        /// Override the inferred namespace for this class
        /// </summary>
        /// <param name="namespace">Namespace to use</param>
        public void OverrideNamespace(string @namespace)
        {
        }

        public string FileName
        {
            get { return string.Format("{0}.hbm.xml", typeof (T).Name); }
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
            mapping.Tablename = tableName;
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
            Attributes.Store("lazy", nextBool.ToString().ToLowerInvariant());
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
            AddPart(join);
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
            Attributes.Store("mutable", (!nextBool).ToString().ToLowerInvariant());
            nextBool = true;
        }

        /// <summary>
        /// Sets this entity to be dynamic update
        /// </summary>
        public void DynamicUpdate()
        {
            Attributes.Store("dynamic-update", nextBool.ToString().ToLowerInvariant());
            nextBool = true;
        }

        /// <summary>
        /// Sets this entity to be dynamic insert
        /// </summary>
        public void DynamicInsert()
        {
            Attributes.Store("dynamic-insert", nextBool.ToString().ToLowerInvariant());
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
        public OptimisticLockBuilder OptimisticLock
        {
            get { return new OptimisticLockBuilder(Attributes); }
        }
    }
}