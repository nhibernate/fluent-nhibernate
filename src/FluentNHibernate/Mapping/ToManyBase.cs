using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

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

        protected readonly AttributeStore<CollectionMapping> collectionAttributes = new AttributeStore<CollectionMapping>();
        protected readonly KeyMapping keyMapping = new KeyMapping();
        protected readonly AttributeStore<TRelationshipAttributes> relationshipAttributes = new AttributeStore<TRelationshipAttributes>();
        private readonly IList<FilterPart> filters = new List<FilterPart>();
        private Func<AttributeStore, CollectionMapping> collectionBuilder;
        private IndexMapping indexMapping;
        protected Member member;
        private Type entity;

        protected ToManyBase(Type entity, Member member, Type type)
        {
            this.entity = entity;
            this.member = member;
            AsBag();
            access = new AccessStrategyBuilder<T>((T)this, value => collectionAttributes.Set(x => x.Access, value));
            fetch = new FetchTypeExpression<T>((T)this, value => collectionAttributes.Set(x => x.Fetch, value));
            optimisticLock = new OptimisticLockBuilder<T>((T)this, value => collectionAttributes.Set(x => x.OptimisticLock, value));
            cascade = new CollectionCascadeExpression<T>((T)this, value => collectionAttributes.Set(x => x.Cascade, value));

            SetDefaultCollectionType();
            SetCustomCollectionType(type);
            SetDefaultAccess();
            Cache = new CachePart(entity);

            collectionAttributes.SetDefault(x => x.Name, member.Name);
            relationshipAttributes.SetDefault(x => x.Class, new TypeReference(typeof(TChild)));
        }

        /// <summary>
        /// This method is used to set a different key column in this table to be used for joins.
        /// The output is set as the property-ref attribute in the "key" subelement of the collection
        /// </summary>
        /// <param name="propertyRef">The name of the column in this table which is linked to the foreign key</param>
        /// <returns>OneToManyPart</returns>
        public T PropertyRef(string propertyRef)
        {
            keyMapping.PropertyRef = propertyRef;
            return (T)this;
        }

        /// <summary>
        /// Specify caching for this entity.
        /// </summary>
        public CachePart Cache { get; private set; }

        /// <summary>
        /// Specify the lazy-load behaviour
        /// </summary>
        public T LazyLoad()
        {
            collectionAttributes.Set(x => x.Lazy, nextBool ? Lazy.True : Lazy.False);
            nextBool = true;
            return (T)this;
        }

        /// <summary>
        /// Specify extra lazy loading
        /// </summary>
        public T ExtraLazyLoad()
        {
            collectionAttributes.Set(x => x.Lazy, nextBool ? Lazy.Extra : Lazy.True);
            nextBool = true;
            return (T)this;
        }

        /// <summary>
        /// Inverse the ownership of this entity. Make the other side of the relationship
        /// responsible for saving.
        /// </summary>
        public T Inverse()
        {
            collectionAttributes.Set(x => x.Inverse, nextBool);
            nextBool = true;
            return (T)this;
        }

        /// <summary>
        /// Specify the cascade behaviour
        /// </summary>
        public CollectionCascadeExpression<T> Cascade
        {
            get { return cascade; }
        }

        /// <summary>
        /// Use a set collection
        /// </summary>
        public T AsSet()
        {
            collectionBuilder = attrs => CollectionMapping.Set(attrs);
            return (T)this;
        }

        /// <summary>
        /// Use a set collection
        /// </summary>
        /// <param name="sort">Sorting</param>
        public T AsSet(SortType sort)
        {
            collectionBuilder = attrs =>
            {
                var collection = CollectionMapping.Set(attrs);
                collection.Sort = sort.ToLowerInvariantString();
                return collection;
            };
            return (T)this;
        }

        /// <summary>
        /// Use a set collection
        /// </summary>
        /// <typeparam name="TComparer">Item comparer</typeparam>
        public T AsSet<TComparer>() where TComparer : IComparer<TChild>
        {
            collectionBuilder = attrs =>
            {
                var collection = CollectionMapping.Set(attrs);
                collection.Sort = typeof(TComparer).AssemblyQualifiedName;
                return collection;
            };
            return (T)this;
        }

        /// <summary>
        /// Use a bag collection
        /// </summary>
        public T AsBag()
        {
            collectionBuilder = attrs => CollectionMapping.Bag(attrs);
            return (T)this;
        }

        /// <summary>
        /// Use a list collection
        /// </summary>
        public T AsList()
        {
            collectionBuilder = attrs => CollectionMapping.List(attrs);
            CreateIndexMapping(null);

            if (indexMapping.Columns.IsEmpty())
                indexMapping.AddDefaultColumn(new ColumnMapping { Name = "Index" });

            return (T)this;
        }

        /// <summary>
        /// Use a list collection with an index
        /// </summary>
        /// <param name="customIndexMapping">Index mapping</param>
        public T AsList(Action<IndexPart> customIndexMapping)
        {
            collectionBuilder = attrs => CollectionMapping.List(attrs);
            CreateIndexMapping(customIndexMapping);

            if (indexMapping.Columns.IsEmpty())
                indexMapping.AddDefaultColumn(new ColumnMapping { Name = "Index" });

            return (T)this;
        }

        /// <summary>
        /// Use a map collection
        /// </summary>
        /// <typeparam name="TIndex">Index type</typeparam>
        /// <param name="indexSelector">Index property</param>
        public T AsMap<TIndex>(Expression<Func<TChild, TIndex>> indexSelector)
        {
            return AsMap(indexSelector, null);
        }

        /// <summary>
        /// Use a map collection
        /// </summary>
        /// <typeparam name="TIndex">Index type</typeparam>
        /// <param name="indexSelector">Index property</param>
        /// <param name="sort">Sorting</param>
        public T AsMap<TIndex>(Expression<Func<TChild, TIndex>> indexSelector, SortType sort)
        {
            return AsMap(indexSelector, null, sort);
        }

        /// <summary>
        /// Use a map collection
        /// </summary>
        /// <param name="indexColumnName">Index column name</param>
        public T AsMap(string indexColumnName)
        {
            collectionBuilder = attrs => CollectionMapping.Map(attrs);
            AsIndexedCollection<Int32>(indexColumnName, null);
            return (T)this;
        }

        /// <summary>
        /// Use a map collection
        /// </summary>
        /// <param name="indexColumnName">Index column name</param>
        /// <param name="sort">Sorting</param>
        public T AsMap(string indexColumnName, SortType sort)
        {
            collectionBuilder = attrs =>
            {
                var collection = CollectionMapping.Map(attrs);
                collection.Sort = sort.ToString().ToLowerInvariant();
                return collection;
            };
            AsIndexedCollection<Int32>(indexColumnName, null);
            return (T)this;
        }

        /// <summary>
        /// Use a map collection
        /// </summary>
        /// <typeparam name="TIndex">Index type</typeparam>
        /// <param name="indexColumnName">Index column name</param>
        public T AsMap<TIndex>(string indexColumnName)
        {
            collectionBuilder = attrs => CollectionMapping.Map(attrs);
            AsIndexedCollection<TIndex>(indexColumnName, null);
            return (T)this;
        }

        /// <summary>
        /// Use a map collection
        /// </summary>
        /// <typeparam name="TIndex">Index type</typeparam>
        /// <param name="indexColumnName">Index column name</param>
        /// <param name="sort">Sorting</param>
        public T AsMap<TIndex>(string indexColumnName, SortType sort)
        {
            collectionBuilder = attrs =>
            {
                var collection = CollectionMapping.Map(attrs);
                collection.Sort = sort.ToString().ToLowerInvariant();
                return collection;
            };
            AsIndexedCollection<TIndex>(indexColumnName, null);
            return (T)this;
        }

        /// <summary>
        /// Use a map collection
        /// </summary>
        /// <typeparam name="TIndex">Index type</typeparam>
        /// <typeparam name="TComparer">Comparer</typeparam>
        /// <param name="indexColumnName">Index column name</param>
        public T AsMap<TIndex, TComparer>(string indexColumnName) where TComparer : IComparer<TChild>
        {
            collectionBuilder = attrs =>
            {
                var collection = CollectionMapping.Map(attrs);
                collection.Sort = typeof(TComparer).AssemblyQualifiedName;
                return collection;
            };
            AsIndexedCollection<TIndex>(indexColumnName, null);
            return (T)this;
        }

        /// <summary>
        /// Use a map collection
        /// </summary>
        /// <typeparam name="TIndex">Index type</typeparam>
        /// <param name="indexSelector">Index property</param>
        /// <param name="customIndexMapping">Index mapping</param>
        public T AsMap<TIndex>(Expression<Func<TChild, TIndex>> indexSelector, Action<IndexPart> customIndexMapping)
        {
            collectionBuilder = attrs => CollectionMapping.Map(attrs);
            return AsIndexedCollection(indexSelector, customIndexMapping);
        }

        /// <summary>
        /// Use a map collection
        /// </summary>
        /// <typeparam name="TIndex">Index type</typeparam>
        /// <param name="indexSelector">Index property</param>
        /// <param name="customIndexMapping">Index mapping</param>
        /// <param name="sort">Sorting</param>
        public T AsMap<TIndex>(Expression<Func<TChild, TIndex>> indexSelector, Action<IndexPart> customIndexMapping, SortType sort)
        {
            collectionBuilder = attrs =>
            {
                var collection = CollectionMapping.Map(attrs);
                collection.Sort = sort.ToString().ToLowerInvariant();
                return collection;
            };
            return AsIndexedCollection(indexSelector, customIndexMapping);
        }

        // I'm not proud of this. The fluent interface for maps really needs to be rethought. But I've let maps sit unsupported for way too long
        // so a hack is better than nothing.
        /// <summary>
        /// Use a map collection
        /// </summary>
        /// <typeparam name="TIndex">Index type</typeparam>
        /// <param name="customIndexMapping">Index mapping</param>
        /// <param name="customElementMapping">Element mapping</param>
        public T AsMap<TIndex>(Action<IndexPart> customIndexMapping, Action<ElementPart> customElementMapping)
        {
            collectionBuilder = attrs => CollectionMapping.Map(attrs);
            AsIndexedCollection<TIndex>(string.Empty, customIndexMapping);
            Element(string.Empty);
            customElementMapping(elementPart);
            return (T)this;
        }

        /// <summary>
        /// Use an array
        /// </summary>
        /// <typeparam name="TIndex">Index type</typeparam>
        /// <param name="indexSelector">Index property</param>
        public T AsArray<TIndex>(Expression<Func<TChild, TIndex>> indexSelector)
        {
            return AsArray(indexSelector, null);
        }

        /// <summary>
        /// Use an array
        /// </summary>
        /// <typeparam name="TIndex">Index type</typeparam>
        /// <param name="indexSelector">Index property</param>
        /// <param name="customIndexMapping">Index mapping</param>
        public T AsArray<TIndex>(Expression<Func<TChild, TIndex>> indexSelector, Action<IndexPart> customIndexMapping)
        {
            collectionBuilder = attrs => CollectionMapping.Array(attrs);
            return AsIndexedCollection(indexSelector, customIndexMapping);
        }

        /// <summary>
        /// Make this collection indexed
        /// </summary>
        /// <typeparam name="TIndex">Index type</typeparam>
        /// <param name="indexSelector">Index property</param>
        /// <param name="customIndexMapping">Index mapping</param>
        public T AsIndexedCollection<TIndex>(Expression<Func<TChild, TIndex>> indexSelector, Action<IndexPart> customIndexMapping)
        {
            var indexMember = indexSelector.ToMember();
            return AsIndexedCollection<TIndex>(indexMember.Name, customIndexMapping);
        }

        /// <summary>
        /// Make this collection index
        /// </summary>
        /// <typeparam name="TIndex">Index type</typeparam>
        /// <param name="indexColumn">Index column</param>
        /// <param name="customIndexMapping">Index mapping</param>
        public T AsIndexedCollection<TIndex>(string indexColumn, Action<IndexPart> customIndexMapping)
        {
            CreateIndexMapping(customIndexMapping);

            if (!indexMapping.IsSpecified("Type"))
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

#pragma warning disable 612,618
            indexMapping = indexPart.GetIndexMapping();
#pragma warning restore 612,618
        }

        /// <summary>
        /// Map an element/value type
        /// </summary>
        /// <param name="columnName">Column name</param>
        public T Element(string columnName)
        {
            elementPart = new ElementPart(typeof(T));
            elementPart.Type<TChild>();

            if (!string.IsNullOrEmpty(columnName))
                elementPart.Column(columnName);

            return (T)this;
        }

        /// <summary>
        /// Map an element/value type
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <param name="customElementMapping">Custom mapping</param>
        public T Element(string columnName, Action<ElementPart> customElementMapping)
        {
            Element(columnName);
            if (customElementMapping != null) customElementMapping(elementPart);
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

        /// <summary>
        /// Specify that the deletes should be cascaded
        /// </summary>
        public T ForeignKeyCascadeOnDelete()
        {
            keyMapping.OnDelete = "cascade";
            return (T)this;
        }

        /// <summary>
        /// Specify the fetching behaviour
        /// </summary>
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

        /// <summary>
        /// Specify the optimistic locking behaviour
        /// </summary>
        public OptimisticLockBuilder<T> OptimisticLock
        {
            get { return optimisticLock; }
        }

        /// <summary>
        /// Specify a custom persister
        /// </summary>
        /// <typeparam name="TPersister">Persister</typeparam>
        public T Persister<TPersister>()
        {
            collectionAttributes.Set(x => x.Persister, new TypeReference(typeof(TPersister)));
            return (T)this;
        }

        /// <summary>
        /// Specify a check constraint
        /// </summary>
        /// <param name="constraintName">Constraint name</param>
        public T Check(string constraintName)
        {
            collectionAttributes.Set(x => x.Check, constraintName);
            return (T)this;
        }

        /// <summary>
        /// Specify that this collection is generic (optional)
        /// </summary>
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

        /// <summary>
        /// Specify the select batch size
        /// </summary>
        /// <param name="size">Batch size</param>
        public T BatchSize(int size)
        {
            collectionAttributes.Set(x => x.BatchSize, size);
            return (T)this;
        }

        /// <summary>
        /// Inverts the next boolean operation
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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

        /// <summary>
        /// Specify the table schema
        /// </summary>
        /// <param name="schema">Schema name</param>
        public T Schema(string schema)
        {
            collectionAttributes.Set(x => x.Schema, schema);
            return (T)this;
        }

        /// <summary>
        /// Specifies an entity-name.
        /// </summary>
        /// <remarks>See http://nhforge.org/blogs/nhibernate/archive/2008/10/21/entity-name-in-action-a-strongly-typed-entity.aspx</remarks>
        public T EntityName(string entityName)
        {
            relationshipAttributes.Set(x => x.EntityName, entityName);
            return (T)this;
        }

        /// <overloads>
        /// Applies a filter to this entity given it's name.
        /// </overloads>
        /// <summary>
        /// Applies a filter to this entity given it's name.
        /// </summary>
        /// <param name="name">The filter's name</param>
        /// <param name="condition">The condition to apply</param>
        public T ApplyFilter(string name, string condition)
        {
            var part = new FilterPart(name, condition);
            filters.Add(part);
            return (T)this;
        }

        /// <overloads>
        /// Applies a filter to this entity given it's name.
        /// </overloads>
        /// <summary>
        /// Applies a filter to this entity given it's name.
        /// </summary>
        /// <param name="name">The filter's name</param>
        public T ApplyFilter(string name)
        {
            return (T)this.ApplyFilter(name, null);
        }

        /// <overloads>
        /// Applies a named filter to this one-to-many.
        /// </overloads>
        /// <summary>
        /// Applies a named filter to this one-to-many.
        /// </summary>
        /// <param name="condition">The condition to apply</param>
        /// <typeparam name="TFilter">
        /// The type of a <see cref="FilterDefinition"/> implementation
        /// defining the filter to apply.
        /// </typeparam>
        public T ApplyFilter<TFilter>(string condition) where TFilter : FilterDefinition, new()
        {
            return this.ApplyFilter(new TFilter().Name, condition);
        }

        /// <summary>
        /// Applies a named filter to this one-to-many.
        /// </summary>
        /// <typeparam name="TFilter">
        /// The type of a <see cref="FilterDefinition"/> implementation
        /// defining the filter to apply.
        /// </typeparam>
        public T ApplyFilter<TFilter>() where TFilter : FilterDefinition, new()
        {
            return this.ApplyFilter<TFilter>(null);
        }

        protected IList<FilterPart> Filters
        {
            get { return filters; }
        }

        void SetDefaultCollectionType()
        {
            var collection = CollectionTypeResolver.Resolve(member);

            switch (collection)
            {
                case Collection.Bag:
                    AsBag();
                    break;
                case Collection.Set:
                    AsSet();
                    break;
            }
        }

        void SetDefaultAccess()
        {
            var resolvedAccess = MemberAccessResolver.Resolve(member);

            if (resolvedAccess == Mapping.Access.Property || resolvedAccess == Mapping.Access.Unset)
                return; // property is the default so we don't need to specify it

            collectionAttributes.SetDefault(x => x.Access, resolvedAccess.ToString());
        }

        void SetCustomCollectionType(Type type)
        {
            if (type.Namespace.StartsWith("Iesi") || type.Namespace.StartsWith("System") || type.IsArray)
                return;

            collectionAttributes.Set(x => x.CollectionType, new TypeReference(type));
        }

        CollectionMapping ICollectionMappingProvider.GetCollectionMapping()
        {
            return GetCollectionMapping();
        }

        protected virtual CollectionMapping GetCollectionMapping()
        {
            var mapping = collectionBuilder(collectionAttributes.CloneInner());

            if (!mapping.IsSpecified("Name"))
                mapping.SetDefaultValue(x => x.Name, GetDefaultName());

            mapping.ContainingEntityType = entity;
            mapping.ChildType = typeof(TChild);
            mapping.Member = member;
            mapping.Key = keyMapping;
            mapping.Key.ContainingEntityType = entity;
            mapping.Relationship = GetRelationship();

            if (Cache.IsDirty)
                mapping.Cache = ((ICacheMappingProvider)Cache).GetCacheMapping();

            if (componentMapping != null)
            {
                mapping.CompositeElement = componentMapping.GetCompositeElementMapping();
                mapping.Relationship = null; // HACK: bad design
            }

            // HACK: Index only on list and map - shouldn't have to do this!
            if (mapping.Collection == Collection.Array || mapping.Collection == Collection.List || mapping.Collection == Collection.Map)
                mapping.Index = indexMapping;

            if (elementPart != null)
            {
                mapping.Element = ((IElementMappingProvider)elementPart).GetElementMapping();
                mapping.Relationship = null;
            }

            foreach (var filterPart in Filters)
                mapping.AddFilter(filterPart.GetFilterMapping());

            return mapping;
        }

        private string GetDefaultName()
        {
            if (member.IsMethod)
            {
                Member backingField;

                if (member.TryGetBackingField(out backingField))
                    return backingField.Name;

                // try to guess the backing field name (GetSomething -> something)
                if (member.Name.StartsWith("Get"))
                {
                    var name = member.Name.Substring(3);

                    if (char.IsUpper(name[0]))
                        name = char.ToLower(name[0]) + name.Substring(1);

                    return name;
                }
            }

            return member.Name;
        }

        protected abstract ICollectionRelationshipMapping GetRelationship();
    }
}
