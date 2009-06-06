using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using NHibernate.Persister.Entity;

namespace FluentNHibernate.Mapping
{
    public abstract class ToManyBase<T, TChild, TRelationshipAttributes> : ICollectionRelationship
        where T : ToManyBase<T, TChild, TRelationshipAttributes>, ICollectionRelationship, IMappingPart
        where TRelationshipAttributes : ICollectionRelationshipMapping
    {
        public MemberInfo Member { get; private set; }
        private readonly AccessStrategyBuilder<T> access;
        private readonly OuterJoinBuilder<T> outerJoin;
        private readonly FetchTypeExpression<T> fetch;
        private readonly OptimisticLockBuilder<T> optimisticLock;
        private readonly CollectionCascadeExpression<T> cascade;
        protected IndexPart indexPart;
        protected ElementMapping elementMapping;
        protected ICompositeElementMappingProvider componentMapping;
        public string TableName { get; private set; }
        public Type EntityType { get; private set; }
        protected bool nextBool = true;

        protected readonly AttributeStore<ICollectionMapping> collectionAttributes = new AttributeStore<ICollectionMapping>();
        protected readonly AttributeStore<KeyMapping> keyAttributes = new AttributeStore<KeyMapping>();
        protected readonly AttributeStore<TRelationshipAttributes> relationshipAttributes = new AttributeStore<TRelationshipAttributes>();
        private Func<ICollectionMapping> collectionBuilder;

        protected ToManyBase(Type entity, MemberInfo member, Type type)
        {
            EntityType = entity;
            Member = member;
            AsBag();
            access = new AccessStrategyBuilder<T>((T)this, value => collectionAttributes.Set(x => x.Access, value));
            outerJoin = new OuterJoinBuilder<T>((T)this, value => collectionAttributes.Set(x => x.OuterJoin, value));
            fetch = new FetchTypeExpression<T>((T)this, value => collectionAttributes.Set(x => x.Fetch, value));
            optimisticLock = new OptimisticLockBuilder<T>((T)this, value => collectionAttributes.Set(x => x.OptimisticLock, value));
            cascade = new CollectionCascadeExpression<T>((T)this, value => collectionAttributes.Set(x => x.Cascade, value));

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

            collectionAttributes.Set(x => x.CollectionType, type.AssemblyQualifiedName);
        }

        public virtual ICollectionMapping GetCollectionMapping()
        {
            var mapping = collectionBuilder();

            collectionAttributes.CopyTo(mapping.Attributes);

            mapping.Key = new KeyMapping();
            keyAttributes.CopyTo(mapping.Key.Attributes);

            mapping.Relationship = GetRelationship();

            if (Cache.IsDirty)
                mapping.Cache = Cache.GetCacheMapping();

            if (componentMapping != null)
                mapping.CompositeElement = componentMapping.GetCompositeElementMapping();

            // HACK: Index only on list and map - shouldn't have to do this!
            if (indexPart != null && mapping is ListMapping)
                ((ListMapping)mapping).Index = indexPart.GetIndexMapping();

            if (indexPart != null && mapping is MapMapping)
                ((MapMapping)mapping).Index = indexPart.GetIndexMapping();

            return mapping;
        }

        protected abstract ICollectionRelationshipMapping GetRelationship();

        /// <summary>
        /// Specify caching for this entity.
        /// </summary>
        public ICache Cache { get; private set; }

        public T LazyLoad()
        {
            collectionAttributes.Set(x => x.Lazy, nextBool);
            nextBool = true;
            return (T)this;
        }

        public T Inverse()
        {
            collectionAttributes.Set(x => x.Inverse, nextBool);
            nextBool = true;
            return (T)this;
        }

        public CollectionCascadeExpression<T> Cascade
        {
            get { return cascade; }
        }

        public T AsSet()
        {
            collectionBuilder = () => new SetMapping();
            return (T)this;
        }

        public T AsBag()
        {
            collectionBuilder = () => new BagMapping();
            return (T)this;
        }

        public T AsList()
        {
            indexPart = new IndexPart();
            collectionBuilder = () => new ListMapping();
            return (T)this;
        }

        public T AsList(Action<IndexPart> customIndexMapping)
        {
            AsList();
            indexPart = new IndexPart();
            customIndexMapping(indexPart);
            return (T)this;
        }

        public T AsMap<TIndex>(Expression<Func<TChild, TIndex>> indexSelector)
        {
            return AsMap(indexSelector, null);
        }

        public T AsMap(string indexColumnName)
        {
            collectionBuilder = () => new MapMapping();
            AsIndexedCollection<Int32>(indexColumnName, null);
            return (T)this;
        }

        public T AsMap<TIndex>(string indexColumnName)
        {
            collectionBuilder = () => new MapMapping();
            AsIndexedCollection<TIndex>(indexColumnName, null);
            return (T)this;
        }

        public T AsMap<TIndex>(Expression<Func<TChild, TIndex>> indexSelector, Action<IndexPart> customIndexMapping)
        {
            collectionBuilder = () => new MapMapping();
            return AsIndexedCollection(indexSelector, customIndexMapping);
        }

        // I'm not proud of this. The fluent interface for maps really needs to be rethought. But I've let maps sit unsupported for way too long
        // so a hack is better than nothing.
        public T AsMap<TIndex>(Action<IndexPart> customIndexMapping, Action<ElementMapping> customElementMapping)
        {
            collectionBuilder = () => new MapMapping();
            AsIndexedCollection<TIndex>(string.Empty, customIndexMapping);
            AsElement(string.Empty);
            customElementMapping(elementMapping);
            return (T)this;
        }

        public T AsArray<TIndex>(Expression<Func<TChild, TIndex>> indexSelector)
        {
            return AsArray(indexSelector, null);
        }

        public T AsArray<TIndex>(Expression<Func<TChild, TIndex>> indexSelector, Action<IndexPart> customIndexMapping)
        {
            collectionBuilder = () => new ArrayMapping();
            return AsIndexedCollection(indexSelector, customIndexMapping);
        }

        public T AsIndexedCollection<TIndex>(Expression<Func<TChild, TIndex>> indexSelector, Action<IndexPart> customIndexMapping)
        {
            var indexProperty = ReflectionHelper.GetProperty(indexSelector);
            return AsIndexedCollection<TIndex>(indexProperty.Name, customIndexMapping);
        }

        public T AsIndexedCollection<TIndex>(string indexColumn, Action<IndexPart> customIndexMapping)
        {
            indexPart = new IndexPart();
            indexPart.WithType<TIndex>();
            indexPart.WithColumn(indexColumn);

            if (customIndexMapping != null)
                customIndexMapping(indexPart);

            return (T)this;
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
            var part = new CompositeElementPart<TChild>();

            action(part);

            componentMapping = part;

            return (T)this;
        }

        /// <summary>
        /// Sets the table name for this one-to-many.
        /// </summary>
        /// <param name="name">Table name</param>
        public T WithTableName(string name)
        {
            collectionAttributes.Set(x => x.TableName, name);
            return (T)this;
        }

        public T ForeignKeyCascadeOnDelete()
        {
            keyAttributes.Set(x => x.OnDelete, "cascade");
            return (T)this;
        }

        public FetchTypeExpression<T> Fetch
        {
            get { return fetch; }
        }

        /// <summary>
        /// Set the access and naming strategy for this one-to-many.
        /// </summary>
        public AccessStrategyBuilder<T> Access
        {
            get { return access; }
        }

        public OptimisticLockBuilder<T> OptimisticLock
        {
            get { return optimisticLock; }
        }

        public T Persister<TPersister>() where TPersister : IEntityPersister
        {
            collectionAttributes.Set(x => x.Persister, typeof(TPersister).AssemblyQualifiedName);
            return (T)this;
        }

        public T Check(string checkSql)
        {
            collectionAttributes.Set(x => x.Check, checkSql);
            return (T)this;
        }

        public T Generic()
        {
            collectionAttributes.Set(x => x.Generic, nextBool);
            nextBool = true;
            return (T)this;
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
            collectionAttributes.Set(x => x.Where, where);
            return (T)this;
        }

        public T BatchSize(int size)
        {
            collectionAttributes.Set(x => x.BatchSize, size);
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
            collectionAttributes.Set(x => x.CollectionType, type);
            return (T)this;
        }

        public bool IsMethodAccess
        {
            get { return Member is MethodInfo; }
        }

        public T SchemaIs(string schema)
        {
            collectionAttributes.Set(x => x.Schema, schema);
            return (T)this;
        }

        public OuterJoinBuilder<T> OuterJoin
        {
            get { return outerJoin; }
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
        /// Sets the table name for this one-to-many.
        /// </summary>
        /// <param name="name">Table name</param>
        ICollectionRelationship ICollectionRelationship.WithTableName(string name)
        {
            return WithTableName(name);
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
    }
}