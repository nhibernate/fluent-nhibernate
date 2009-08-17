using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using NHibernate.Persister.Entity;

namespace FluentNHibernate.Mapping
{
    public abstract class ToManyBase<T, TChild, TRelationshipAttributes> : ICollectionMappingProvider
        where T : ToManyBase<T, TChild, TRelationshipAttributes>, ICollectionMappingProvider
        where TRelationshipAttributes : ICollectionRelationshipMapping
    {
        private readonly AccessStrategyBuilder<T> access;
        private readonly FetchTypeExpression<T> fetch;
        private readonly OptimisticLockBuilder<T> optimisticLock;
        private readonly CollectionCascadeExpression<T> cascade;
        protected ElementPart elementPart;
        protected ICompositeElementMappingProvider componentMapping;
        protected bool nextBool = true;

        protected readonly AttributeStore<ICollectionMapping> collectionAttributes = new AttributeStore<ICollectionMapping>();
        protected readonly AttributeStore<KeyMapping> keyAttributes = new AttributeStore<KeyMapping>();
        protected readonly AttributeStore<TRelationshipAttributes> relationshipAttributes = new AttributeStore<TRelationshipAttributes>();
        private Func<AttributeStore, ICollectionMapping> collectionBuilder;
        private IndexMapping indexMapping;
        protected MemberInfo member;
        private Type entity;

        protected ToManyBase(Type entity, MemberInfo member, Type type)
        {
            this.entity = entity;
            this.member = member;
            AsBag();
            access = new AccessStrategyBuilder<T>((T)this, value => collectionAttributes.Set(x => x.Access, value));
            fetch = new FetchTypeExpression<T>((T)this, value => collectionAttributes.Set(x => x.Fetch, value));
            optimisticLock = new OptimisticLockBuilder<T>((T)this, value => collectionAttributes.Set(x => x.OptimisticLock, value));
            cascade = new CollectionCascadeExpression<T>((T)this, value => collectionAttributes.Set(x => x.Cascade, value));

            SetDefaultCollectionType(type);
            SetCustomCollectionType(type);
            Cache = new CachePart(entity);

            collectionAttributes.SetDefault(x => x.Name, member.Name);
            relationshipAttributes.SetDefault(x => x.Class, new TypeReference(typeof(TChild)));
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

            collectionAttributes.Set(x => x.CollectionType, new TypeReference(type));
        }

        public virtual ICollectionMapping GetCollectionMapping()
        {
            var mapping = collectionBuilder(collectionAttributes.CloneInner());

            if (!mapping.IsSpecified(x => x.Name))
                mapping.SetDefaultValue(x => x.Name, member.Name);

            mapping.ContainingEntityType = entity;
            mapping.ChildType = typeof(TChild);
            mapping.MemberInfo = member;
            mapping.Key = new KeyMapping(keyAttributes.CloneInner()) { ContainingEntityType = entity };
            mapping.Relationship = GetRelationship();

            if (Cache.IsDirty)
                mapping.Cache = ((ICacheMappingProvider)Cache).GetCacheMapping();

            if (componentMapping != null)
            {
                mapping.CompositeElement = componentMapping.GetCompositeElementMapping();
                mapping.Relationship = null; // HACK: bad design
            }

            // HACK: Index only on list and map - shouldn't have to do this!
            if (indexMapping != null && mapping is IIndexedCollectionMapping)
                ((IIndexedCollectionMapping)mapping).Index = indexMapping;

            if (elementPart != null)
            {
                mapping.Element = elementPart.GetElementMapping();
                mapping.Relationship = null;
            }

            return mapping;
        }

        protected abstract ICollectionRelationshipMapping GetRelationship();

        /// <summary>
        /// Specify caching for this entity.
        /// </summary>
        public CachePart Cache { get; private set; }

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
            collectionBuilder = attrs => new SetMapping(attrs);
            return (T)this;
        }

        public T AsSet(SortType sort)
        {
            collectionBuilder = attrs => new SetMapping(attrs) { Sort = sort.ToString().ToLowerInvariant() };
            return (T)this;
        }

        public T AsSet<TComparer>() where TComparer : IComparer
        {
            collectionBuilder = attrs => new SetMapping(attrs) { Sort = typeof(TComparer).AssemblyQualifiedName };
            return (T)this;
        }

        public T AsBag()
        {
            collectionBuilder = attrs => new BagMapping(attrs);
            return (T)this;
        }

        public T AsList()
        {
            collectionBuilder = attrs => new ListMapping(attrs);
            CreateIndexMapping(null);
            return (T)this;
        }

        public T AsList(Action<IndexPart> customIndexMapping)
        {
            AsList();
            CreateIndexMapping(customIndexMapping);
            return (T)this;
        }

        public T AsMap<TIndex>(Expression<Func<TChild, TIndex>> indexSelector)
        {
            return AsMap(indexSelector, null);
        }

        public T AsMap(string indexColumnName)
        {
            collectionBuilder = attrs => new MapMapping(attrs);
            AsIndexedCollection<Int32>(indexColumnName, null);
            return (T)this;
        }

        public T AsMap(string indexColumnName, SortType sort)
        {
            collectionBuilder = attrs => new MapMapping(attrs) { Sort = sort.ToString().ToLowerInvariant() };
            AsIndexedCollection<Int32>(indexColumnName, null);
            return (T)this;
        }

        public T AsMap<TIndex>(string indexColumnName)
        {
            collectionBuilder = attrs => new MapMapping(attrs);
            AsIndexedCollection<TIndex>(indexColumnName, null);
            return (T)this;
        }

        public T AsMap<TIndex>(string indexColumnName, SortType sort)
        {
            collectionBuilder = attrs => new MapMapping(attrs) { Sort = sort.ToString().ToLowerInvariant() };
            AsIndexedCollection<TIndex>(indexColumnName, null);
            return (T)this;
        }

        public T AsMap<TIndex, TComparer>(string indexColumnName) where TComparer : IComparer
        {
            collectionBuilder = attrs => new MapMapping(attrs) { Sort = typeof(TComparer).AssemblyQualifiedName };
            AsIndexedCollection<TIndex>(indexColumnName, null);
            return (T)this;
        }

        public T AsMap<TIndex>(Expression<Func<TChild, TIndex>> indexSelector, Action<IndexPart> customIndexMapping)
        {
            collectionBuilder = attrs => new MapMapping(attrs);
            return AsIndexedCollection(indexSelector, customIndexMapping);
        }

        // I'm not proud of this. The fluent interface for maps really needs to be rethought. But I've let maps sit unsupported for way too long
        // so a hack is better than nothing.
        public T AsMap<TIndex>(Action<IndexPart> customIndexMapping, Action<ElementPart> customElementMapping)
        {
            collectionBuilder = attrs => new MapMapping(attrs);
            AsIndexedCollection<TIndex>(string.Empty, customIndexMapping);
            Element(string.Empty);
            customElementMapping(elementPart);
            return (T)this;
        }

        public T AsArray<TIndex>(Expression<Func<TChild, TIndex>> indexSelector)
        {
            return AsArray(indexSelector, null);
        }

        public T AsArray<TIndex>(Expression<Func<TChild, TIndex>> indexSelector, Action<IndexPart> customIndexMapping)
        {
            collectionBuilder = attrs => new ArrayMapping(attrs);
            return AsIndexedCollection(indexSelector, customIndexMapping);
        }

        public T AsIndexedCollection<TIndex>(Expression<Func<TChild, TIndex>> indexSelector, Action<IndexPart> customIndexMapping)
        {
            var indexProperty = ReflectionHelper.GetProperty(indexSelector);
            return AsIndexedCollection<TIndex>(indexProperty.Name, customIndexMapping);
        }

        public T AsIndexedCollection<TIndex>(string indexColumn, Action<IndexPart> customIndexMapping)
        {
            CreateIndexMapping(customIndexMapping);

            if (!indexMapping.IsSpecified(x => x.Type))
                indexMapping.SetDefaultValue(x => x.Type, new TypeReference(typeof(TIndex)));

            if (indexMapping.Columns.IsEmpty())
                indexMapping.AddDefaultColumn(new ColumnMapping { Name = indexColumn });

            return (T)this;
        }

        private void CreateIndexMapping(Action<IndexPart> customIndex)
        {
            var indexPart = new IndexPart(typeof(T));

            if (customIndex != null)
                customIndex(indexPart);

            indexMapping = indexPart.GetIndexMapping();
        }

        public T Element(string columnName)
        {
            elementPart = new ElementPart(typeof(T));
            elementPart.Type<TChild>();

            if (!string.IsNullOrEmpty(columnName))
                elementPart.Column(columnName);

            return (T)this;
        }

        /// <summary>
        /// Maps this collection as a collection of components.
        /// </summary>
        /// <param name="action">Component mapping</param>
        public T Component(Action<CompositeElementPart<TChild>> action)
        {
            var part = new CompositeElementPart<TChild>(typeof(T));

            action(part);

            componentMapping = part;

            return (T)this;
        }

        /// <summary>
        /// Sets the table name for this one-to-many.
        /// </summary>
        /// <param name="name">Table name</param>
        public T Table(string name)
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
            collectionAttributes.Set(x => x.Persister, new TypeReference(typeof(TPersister)));
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
            return CollectionType(new TypeReference(type));
        }

        /// <summary>
        /// Sets a custom collection type
        /// </summary>
        public T CollectionType(string type)
        {
            return CollectionType(new TypeReference(type));
        }

        /// <summary>
        /// Sets a custom collection type
        /// </summary>
        public T CollectionType(TypeReference type)
        {
            collectionAttributes.Set(x => x.CollectionType, type);
            return (T)this;
        }

        public T Schema(string schema)
        {
            collectionAttributes.Set(x => x.Schema, schema);
            return (T)this;
        }
    }
}