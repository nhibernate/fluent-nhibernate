using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public abstract class ToManyBase<T, CHILD>
        : IMappingPart where T : ToManyBase<T, CHILD>, ICollectionRelationship, IMappingPart, IHasAttributes
    {
        public MemberInfo Member { get; private set; }
        protected readonly Cache<string, string> _properties = new Cache<string, string>();
        protected readonly Cache<string, string> _keyProperties = new Cache<string, string>();
        private readonly AccessStrategyBuilder<T> access;
        protected IndexMapping _indexMapping;
        protected ElementMapping _elementMapping;
        protected CompositeElementPart<CHILD> _componentMapping;
        public string TableName { get; private set; }
        public Type EntityType { get; private set; }
        protected string _collectionType;
        private bool nextBool = true;
        protected int batchSize;

        protected ToManyBase(Type entity, MemberInfo member, Type type)
        {
            EntityType = entity;
            Member = member;
            _collectionType = "bag";
            access = new AccessStrategyBuilder<T>((T)this);

            SetDefaultCollectionType(type);
            SetCustomCollectionType(type);
            Cache = new CachePart();
        }

        private void SetDefaultCollectionType(Type type)
        {
            if (type.Namespace == "Iesi.Collections.Generic")
                AsSet();
        }

        private void SetCustomCollectionType(Type type)
        {
            if (type.Namespace.StartsWith("Iesi") || type.Namespace.StartsWith("System") || type.IsArray)
                return;

            _properties.Store("collection-type", type.AssemblyQualifiedName);
        }

        /// <summary>
        /// Specify caching for this entity.
        /// </summary>
        public ICache Cache { get; private set; }

        public T LazyLoad()
        {
            _properties.Store("lazy", nextBool.ToString().ToLowerInvariant());
            nextBool = true;
            return (T)this;
        }

        public T Inverse()
        {
            _properties.Store("inverse", nextBool.ToString().ToLowerInvariant());
            nextBool = true;
            return (T)this;
        }

        public CollectionCascadeExpression<T> Cascade
        {
            get { return new CollectionCascadeExpression<T>((T)this); }
        }

        public T AsSet()
        {
            _collectionType = "set";
            return (T)this;
        }

        public T AsBag()
        {
            _collectionType = "bag";
            return (T)this;
        }

        public T AsList()
        {
            _indexMapping = new IndexMapping();
            _collectionType = "list";
            return (T)this;
        }

        public T AsList(Action<IndexMapping> customIndexMapping)
        {
            AsList();
            customIndexMapping(_indexMapping);
            return (T)this;
        }

        public T AsMap<INDEX_TYPE>(Expression<Func<CHILD, INDEX_TYPE>> indexSelector)
        {
            return AsMap(indexSelector, null);
        }

        public T AsMap(string indexColumnName)
        {
            _collectionType = "map";
            AsIndexedCollection<string>(indexColumnName, null);
            return (T)this;
        }

        public T AsMap<INDEX_TYPE>(string indexColumnName)
        {
            _collectionType = "map";
            AsIndexedCollection<INDEX_TYPE>(indexColumnName, null);
            return (T)this;
        }

        public T AsMap<INDEX_TYPE>(Expression<Func<CHILD, INDEX_TYPE>> indexSelector, Action<IndexMapping> customIndexMapping)
        {
            _collectionType = "map";
            return AsIndexedCollection(indexSelector, customIndexMapping);
        }

        // I'm not proud of this. The fluent interface for maps really needs to be rethought. But I've let maps sit unsupported for way too long
        // so a hack is better than nothing.
        public T AsMap<INDEX_TYPE>(Action<IndexMapping> customIndexMapping, Action<ElementMapping> customElementMapping)
        {
            _collectionType = "map";
            AsIndexedCollection<INDEX_TYPE>(string.Empty, customIndexMapping);
            AsElement(string.Empty);
            customElementMapping(_elementMapping);
            return (T)this;
        }

        public T AsArray<INDEX_TYPE>(Expression<Func<CHILD, INDEX_TYPE>> indexSelector)
        {
            return AsArray(indexSelector, null);
        }

        public T AsArray<INDEX_TYPE>(Expression<Func<CHILD, INDEX_TYPE>> indexSelector, Action<IndexMapping> customIndexMapping)
        {
            _collectionType = "array";
            return AsIndexedCollection(indexSelector, customIndexMapping);
        }

        private T AsIndexedCollection<INDEX_TYPE>(Expression<Func<CHILD, INDEX_TYPE>> indexSelector, Action<IndexMapping> customIndexMapping)
        {
            var indexProperty = ReflectionHelper.GetProperty(indexSelector);
            return AsIndexedCollection<INDEX_TYPE>(indexProperty.Name, customIndexMapping);
        }

        private T AsIndexedCollection<INDEX_TYPE>(string indexColumn, Action<IndexMapping> customIndexMapping)
        {
            _indexMapping = new IndexMapping();
            _indexMapping.WithType<INDEX_TYPE>();
            _indexMapping.WithColumn(indexColumn);

            if (customIndexMapping != null)
                customIndexMapping(_indexMapping);

            return (T)this;
        }

        public T AsElement(string columnName)
        {
            _elementMapping = new ElementMapping();
            _elementMapping.WithColumn(columnName);
            _elementMapping.WithType<CHILD>();            
            return (T)this;
        }

        /// <summary>
        /// Maps this collection as a collection of components.
        /// </summary>
        /// <param name="action">Component mapping</param>
        public T Component(Action<CompositeElementPart<CHILD>> action)
        {
            _componentMapping = new CompositeElementPart<CHILD>();

            action(_componentMapping);

            return (T)this;
        }

        /// <summary>
        /// Sets the table name for this one-to-many.
        /// </summary>
        /// <param name="name">Table name</param>
        public T WithTableName(string name)
        {
            TableName = name;
            return (T)this;
        }

        public T WithForeignKeyConstraintName(string foreignKeyName)
        {
            _keyProperties.Store("foreign-key", foreignKeyName);
            return (T)this;
        }

        public T ForeignKeyCascadeOnDelete()
        {
            _keyProperties.Store("on-delete", "cascade");
            return (T)this;
        }

        /// <summary>
        /// Set the access and naming strategy for this one-to-many.
        /// </summary>
        public AccessStrategyBuilder<T> Access
        {
            get { return access; }
        }

        /// <summary>
        /// Sets the where clause for this one-to-many relationship.
        /// Note: This only supports simple cases, use the string overload for more complex clauses.
        /// </summary>
        public T Where(Expression<Func<CHILD, bool>> where)
        {
            var sql = ExpressionToSql.Convert(where);

            return Where(sql);
        }

        /// <summary>
        /// Sets the where clause for this one-to-many relationship.
        /// </summary>
        public T Where(string where)
        {
            _properties.Store("where", where);

            return (T)this;
        }

        public T BatchSize(int size)
        {
            batchSize = size;
            return (T)this;
        }

        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        public T Not
        {
            get
            {
                nextBool = !nextBool;
                return (T)this;
            }
        }

        /// <summary>
        /// Sets a custom collection type
        /// </summary>
        public T CollectionType<TCollection>()
        {
            return CollectionType(typeof(TCollection));
        }

        /// <summary>
        /// Sets a custom collection type
        /// </summary>
        public T CollectionType(Type type)
        {
            return CollectionType(type.AssemblyQualifiedName);
        }

        /// <summary>
        /// Sets a custom collection type
        /// </summary>
        public T CollectionType(string type)
        {
            _properties.Store("collection-type", type);
            return (T)this;
        }

        public bool IsMethodAccess
        {
            get { return Member is MethodInfo; }
        }

        public class IndexMapping
        {
            private readonly Cache<string, string> _properties = new Cache<string, string>();

            public IndexMapping WithColumn(string indexColumnName)
            {
                _properties.Store("column", indexColumnName);
                return this;
            }

            public IndexMapping WithType<INDEXTYPE>()
            {
                _properties.Store("type", typeof(INDEXTYPE).AssemblyQualifiedName);
                return this;
            }

            internal void WriteAttributesToIndexElement(XmlElement indexElement)
            {
                indexElement.WithProperties(_properties);
            }
        }

        public class ElementMapping
        {
            private readonly Cache<string, string> _properties = new Cache<string, string>();

            public ElementMapping WithColumn(string indexColumnName)
            {
                _properties.Store("column", indexColumnName);
                return this;
            }

            public ElementMapping WithType<INDEXTYPE>()
            {
                _properties.Store("type", typeof(INDEXTYPE).AssemblyQualifiedName);
                return this;
            }

            internal void WriteAttributesToIndexElement(XmlElement indexElement)
            {
                indexElement.WithProperties(_properties);
            }
        }

        public abstract void SetAttribute(string name, string value);
        public abstract void SetAttributes(Attributes attributes);
        public abstract void Write(XmlElement classElement, IMappingVisitor visitor);
        public abstract int Level { get; }
        public abstract PartPosition Position { get; }
    }
}