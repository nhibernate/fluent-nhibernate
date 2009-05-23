using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public abstract class ToManyBase<T, TChild> : ICollectionRelationship
        where T : ToManyBase<T, TChild>, ICollectionRelationship, IMappingPart, IHasAttributes
    {
        public MemberInfo Member { get; private set; }
        protected readonly Cache<string, string> properties = new Cache<string, string>();
        protected readonly Cache<string, string> keyProperties = new Cache<string, string>();
        private readonly AccessStrategyBuilder<T> access;
        protected IndexMapping indexMapping;
        protected ElementMapping elementMapping;
        protected CompositeElementPart<TChild> componentMapping;
        public string TableName { get; private set; }
        public Type EntityType { get; private set; }
        protected string collectionType;
        private bool nextBool = true;
        protected int batchSize;

        protected ToManyBase(Type entity, MemberInfo member, Type type)
        {
            EntityType = entity;
            Member = member;
            collectionType = "bag";
            access = new AccessStrategyBuilder<T>((T)this, value => SetAttribute("access", value));

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

            properties.Store("collection-type", type.AssemblyQualifiedName);
        }

        /// <summary>
        /// Specify caching for this entity.
        /// </summary>
        public ICache Cache { get; private set; }

        public T LazyLoad()
        {
            properties.Store("lazy", nextBool.ToString().ToLowerInvariant());
            nextBool = true;
            return (T)this;
        }

        public T Inverse()
        {
            properties.Store("inverse", nextBool.ToString().ToLowerInvariant());
            nextBool = true;
            return (T)this;
        }

        public CollectionCascadeExpression<T> Cascade
        {
            get { return new CollectionCascadeExpression<T>((T)this); }
        }

        public T AsSet()
        {
            collectionType = "set";
            return (T)this;
        }

        public T AsBag()
        {
            collectionType = "bag";
            return (T)this;
        }

        public T AsList()
        {
            indexMapping = new IndexMapping();
            collectionType = "list";
            return (T)this;
        }

        public T AsList(Action<IndexMapping> customIndexMapping)
        {
            AsList();
            customIndexMapping(indexMapping);
            return (T)this;
        }

        public T AsMap<TIndex>(Expression<Func<TChild, TIndex>> indexSelector)
        {
            return AsMap(indexSelector, null);
        }

        public T AsMap(string indexColumnName)
        {
            collectionType = "map";
            AsIndexedCollection<Int32>(indexColumnName, null);
            return (T)this;
        }

        public T AsMap<TIndex>(string indexColumnName)
        {
            collectionType = "map";
            AsIndexedCollection<TIndex>(indexColumnName, null);
            return (T)this;
        }

        public T AsMap<TIndex>(Expression<Func<TChild, TIndex>> indexSelector, Action<IndexMapping> customIndexMapping)
        {
            collectionType = "map";
            return AsIndexedCollection(indexSelector, customIndexMapping);
        }

        // I'm not proud of this. The fluent interface for maps really needs to be rethought. But I've let maps sit unsupported for way too long
        // so a hack is better than nothing.
        public T AsMap<TIndex>(Action<IndexMapping> customIndexMapping, Action<ElementMapping> customElementMapping)
        {
            collectionType = "map";
            AsIndexedCollection<TIndex>(string.Empty, customIndexMapping);
            AsElement(string.Empty);
            customElementMapping(elementMapping);
            return (T)this;
        }

        public T AsArray<TIndex>(Expression<Func<TChild, TIndex>> indexSelector)
        {
            return AsArray(indexSelector, null);
        }

        public T AsArray<TIndex>(Expression<Func<TChild, TIndex>> indexSelector, Action<IndexMapping> customIndexMapping)
        {
            collectionType = "array";
            return AsIndexedCollection(indexSelector, customIndexMapping);
        }

        public T AsIndexedCollection<TIndex>(Expression<Func<TChild, TIndex>> indexSelector, Action<IndexMapping> customIndexMapping)
        {
            var indexProperty = ReflectionHelper.GetProperty(indexSelector);
            return AsIndexedCollection<TIndex>(indexProperty.Name, customIndexMapping);
        }

        public T AsIndexedCollection<TIndex>(string indexColumn, Action<IndexMapping> customIndexMapping)
        {
            indexMapping = new IndexMapping();
            indexMapping.WithType<TIndex>();
            indexMapping.WithColumn(indexColumn);

            if (customIndexMapping != null)
                customIndexMapping(indexMapping);

            return (T)this;
        }

        protected void WriteIndexElement(XmlElement collectionElement)
        {
            var indexElement = collectionElement.AddElement("index");
            indexMapping.WriteAttributesToIndexElement(indexElement);
        }

        public T AsElement(string columnName)
        {
            elementMapping = new ElementMapping();
            elementMapping.WithColumn(columnName);
            elementMapping.WithType<TChild>();            
            return (T)this;
        }

        /// <summary>
        /// Maps this collection as a collection of components.
        /// </summary>
        /// <param name="action">Component mapping</param>
        public T Component(Action<CompositeElementPart<TChild>> action)
        {
            componentMapping = new CompositeElementPart<TChild>();

            action(componentMapping);

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
            keyProperties.Store("foreign-key", foreignKeyName);
            return (T)this;
        }

        public T ForeignKeyCascadeOnDelete()
        {
            keyProperties.Store("on-delete", "cascade");
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
        public T Where(Expression<Func<TChild, bool>> where)
        {
            var sql = ExpressionToSql.Convert(where);

            return Where(sql);
        }

        /// <summary>
        /// Sets the where clause for this one-to-many relationship.
        /// </summary>
        public T Where(string where)
        {
            properties.Store("where", where);

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
            properties.Store("collection-type", type);
            return (T)this;
        }

        public bool IsMethodAccess
        {
            get { return Member is MethodInfo; }
        }

        #region Implementation of ICollectionRelationship

        ICollectionCascadeExpression ICollectionRelationship.Cascade
        {
            get { return Cascade; }
        }

        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        ICollectionRelationship ICollectionRelationship.Not
        {
            get { return Not; }
        }
        ICollectionRelationship ICollectionRelationship.LazyLoad()
        {
            return LazyLoad();
        }

        ICollectionRelationship ICollectionRelationship.Inverse()
        {
            return Inverse();
        }

        ICollectionRelationship ICollectionRelationship.AsSet()
        {
            return AsSet();
        }

        ICollectionRelationship ICollectionRelationship.AsBag()
        {
            return AsBag();
        }

        ICollectionRelationship ICollectionRelationship.AsList()
        {
            return AsList();
        }

        ICollectionRelationship ICollectionRelationship.AsMap(string indexColumnName)
        {
            return AsMap(indexColumnName);
        }

        ICollectionRelationship ICollectionRelationship.AsMap<TIndex>(string indexColumnName)
        {
            return AsMap(indexColumnName);
        }

        ICollectionRelationship ICollectionRelationship.AsElement(string columnName)
        {
            return AsElement(columnName);
        }

        /// <summary>
        /// Maps this collection as a collection of components.
        /// </summary>
        /// <param name="action">Component mapping</param>
        ICollectionRelationship ICollectionRelationship.Component(Action<IClasslike> action)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the table name for this one-to-many.
        /// </summary>
        /// <param name="name">Table name</param>
        ICollectionRelationship ICollectionRelationship.WithTableName(string name)
        {
            return WithTableName(name);
        }

        ICollectionRelationship ICollectionRelationship.WithForeignKeyConstraintName(string foreignKeyName)
        {
            return WithForeignKeyConstraintName(foreignKeyName);
        }

        ICollectionRelationship ICollectionRelationship.ForeignKeyCascadeOnDelete()
        {
            return ForeignKeyCascadeOnDelete();
        }

        /// <summary>
        /// Sets the where clause for this one-to-many relationship.
        /// </summary>
        ICollectionRelationship ICollectionRelationship.Where(string where)
        {
            return Where(where);
        }

        ICollectionRelationship ICollectionRelationship.BatchSize(int size)
        {
            return BatchSize(size);
        }

        /// <summary>
        /// Sets a custom collection type
        /// </summary>
        ICollectionRelationship ICollectionRelationship.CollectionType<TCollection>()
        {
            return CollectionType<TCollection>();
        }

        /// <summary>
        /// Sets a custom collection type
        /// </summary>
        ICollectionRelationship ICollectionRelationship.CollectionType(Type type)
        {
            return CollectionType(type);
        }

        /// <summary>
        /// Sets a custom collection type
        /// </summary>
        ICollectionRelationship ICollectionRelationship.CollectionType(string type)
        {
            return CollectionType(type);
        }

        IAccessStrategyBuilder IRelationship.Access
        {
            get { return Access; }
        }

        #endregion

        public class IndexMapping
        {
            private readonly Cache<string, string> properties = new Cache<string, string>();

            public IndexMapping WithColumn(string indexColumnName)
            {
                properties.Store("column", indexColumnName);
                return this;
            }

            public IndexMapping WithType<TIndex>()
            {
                properties.Store("type", typeof(TIndex).AssemblyQualifiedName);
                return this;
            }

            internal void WriteAttributesToIndexElement(XmlElement indexElement)
            {
                indexElement.WithProperties(properties);
            }
        }

        public class ElementMapping
        {
            private readonly Cache<string, string> properties = new Cache<string, string>();

            public ElementMapping WithColumn(string indexColumnName)
            {
                properties.Store("column", indexColumnName);
                return this;
            }

            public ElementMapping WithType<TIndex>()
            {
                properties.Store("type", typeof(TIndex).AssemblyQualifiedName);
                return this;
            }

            internal void WriteAttributesToIndexElement(XmlElement indexElement)
            {
                indexElement.WithProperties(properties);
            }
        }

        public abstract void SetAttribute(string name, string value);
        public abstract void SetAttributes(Attributes attributes);
        public abstract void Write(XmlElement classElement, IMappingVisitor visitor);
        public abstract int LevelWithinPosition { get; }
        public abstract PartPosition PositionOnDocument { get; }


    }
}