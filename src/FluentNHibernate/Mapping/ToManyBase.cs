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
    public abstract class ToManyBase<T, TChild> : ICollectionMappingProvider
        where T : ToManyBase<T, TChild>, ICollectionMappingProvider
    {
        readonly AccessStrategyBuilder<T> access;
        readonly FetchTypeExpression<T> fetch;
        readonly OptimisticLockBuilder<T> optimisticLock;
        readonly CollectionCascadeExpression<T> cascade;
        protected ElementPart elementPart;
        protected ICompositeElementMappingProvider componentMapping;
        protected bool nextBool = true;

        protected readonly AttributeStore collectionAttributes = new AttributeStore();
        protected readonly KeyMapping keyMapping = new KeyMapping();
        protected readonly AttributeStore relationshipAttributes = new AttributeStore();
        readonly IList<IFilterMappingProvider> filters = new List<IFilterMappingProvider>();
        Func<AttributeStore, CollectionMapping> collectionBuilder;
        IndexMapping indexMapping;
        protected Member member;
        readonly Type entity;

        protected ToManyBase(Type entity, Member member, Type type)
        {
            this.entity = entity;
            this.member = member;
            AsBag();
            access = new AccessStrategyBuilder<T>((T)this, value => collectionAttributes.Set("Access", Layer.UserSupplied, value));
            fetch = new FetchTypeExpression<T>((T)this, value => collectionAttributes.Set("Fetch", Layer.UserSupplied, value));
            optimisticLock = new OptimisticLockBuilder<T>((T)this, value => collectionAttributes.Set("OptimisticLock", Layer.UserSupplied, value));
            cascade = new CollectionCascadeExpression<T>((T)this, value => collectionAttributes.Set("Cascade", Layer.UserSupplied, value));

            SetDefaultCollectionType();
            SetCustomCollectionType(type);
            SetDefaultAccess();
            Cache = new CachePart(entity);

            collectionAttributes.Set("Name", Layer.Defaults, member.Name);
            relationshipAttributes.Set("Class", Layer.Defaults, new TypeReference(typeof(TChild)));
        }

		/// <summary>
		/// Return the type of the owning entity
		/// </summary>
		/// <returns>Type</returns>
    	public Type EntityType
    	{
			get { return entity; }
    	}

        /// <summary>
        /// This method is used to set a different key column in this table to be used for joins.
        /// The output is set as the property-ref attribute in the "key" subelement of the collection
        /// </summary>
        /// <param name="propertyRef">The name of the column in this table which is linked to the foreign key</param>
        /// <returns>OneToManyPart</returns>
        public T PropertyRef(string propertyRef)
        {
            keyMapping.Set(x => x.PropertyRef, Layer.UserSupplied, propertyRef);
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
            collectionAttributes.Set("Lazy", Layer.UserSupplied, nextBool ? Lazy.True : Lazy.False);
            nextBool = true;
            return (T)this;
        }

        /// <summary>
        /// Specify extra lazy loading
        /// </summary>
        public T ExtraLazyLoad()
        {
            collectionAttributes.Set("Lazy", Layer.UserSupplied, nextBool ? Lazy.Extra : Lazy.True);
            nextBool = true;
            return (T)this;
        }

        /// <summary>
        /// Inverse the ownership of this entity. Make the other side of the relationship
        /// responsible for saving.
        /// </summary>
        public T Inverse()
        {
            collectionAttributes.Set("Inverse", Layer.UserSupplied, nextBool);
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
                collection.Set(x => x.Sort, Layer.UserSupplied, sort.ToLowerInvariantString());
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
                collection.Set(x => x.Sort, Layer.UserSupplied, typeof(TComparer).AssemblyQualifiedName);
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
            {
                var columnMapping = new ColumnMapping();
                columnMapping.Set(x => x.Name, Layer.Defaults, "Index");
                indexMapping.AddColumn(Layer.Defaults, columnMapping);
            }

            return (T)this;
        }

        /// <summary>
        /// Use a list collection with an index
        /// </summary>
        /// <param name="customIndexMapping">Index mapping</param>
        public T AsList(Action<ListIndexPart> customIndexMapping)
        {
            collectionBuilder = attrs => CollectionMapping.List(attrs);
            CreateListIndexMapping(customIndexMapping);

            if (indexMapping.Columns.IsEmpty())
            {
                var columnMapping = new ColumnMapping();
                columnMapping.Set(x => x.Name, Layer.Defaults, "Index");
                indexMapping.AddColumn(Layer.Defaults, columnMapping);
            }

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
                collection.Set(x => x.Sort, Layer.UserSupplied, sort.ToLowerInvariantString());
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
                collection.Set(x => x.Sort, Layer.UserSupplied, sort.ToLowerInvariantString());
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
                collection.Set(x => x.Sort, Layer.UserSupplied, typeof(TComparer).AssemblyQualifiedName);
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
                collection.Set(x => x.Sort, Layer.UserSupplied, sort.ToLowerInvariantString());
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

            indexMapping.Set(x => x.Type, Layer.Defaults, new TypeReference(typeof(TIndex)));

            if (indexMapping.Columns.IsEmpty())
            {
                var columnMapping = new ColumnMapping();
                columnMapping.Set(x => x.Name, Layer.Defaults, indexColumn);
                indexMapping.AddColumn(Layer.Defaults, columnMapping);
            }

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

        private void CreateListIndexMapping(Action<ListIndexPart> customIndex)
        {
            indexMapping = new IndexMapping();
            var builder = new ListIndexPart(indexMapping);

            if (customIndex != null)
                customIndex(builder);
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
            collectionAttributes.Set("TableName", Layer.UserSupplied, name);
            return (T)this;
        }

        /// <summary>
        /// Specify that the deletes should be cascaded
        /// </summary>
        public T ForeignKeyCascadeOnDelete()
        {
            keyMapping.Set(x => x.OnDelete, Layer.Defaults, "cascade");
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
        /// Specifies whether this collection should be optimistically locked.
        /// </summary>
        public T OptimisticLock()
        {
            collectionAttributes.Set("OptimisticLock", Layer.UserSupplied, nextBool);
            nextBool = true;
            return (T)this;
        }

        /// <summary>
        /// Specify a custom persister
        /// </summary>
        /// <typeparam name="TPersister">Persister</typeparam>
        public T Persister<TPersister>()
        {
            collectionAttributes.Set("Persister", Layer.UserSupplied, new TypeReference(typeof(TPersister)));
            return (T)this;
        }

        /// <summary>
        /// Specify a check constraint
        /// </summary>
        /// <param name="constraintName">Constraint name</param>
        public T Check(string constraintName)
        {
            collectionAttributes.Set("Check", Layer.UserSupplied, constraintName);
            return (T)this;
        }

        /// <summary>
        /// Specify that this collection is generic (optional)
        /// </summary>
        public T Generic()
        {
            collectionAttributes.Set("Generic", Layer.UserSupplied, nextBool);
            nextBool = true;
            return (T)this;
        }

        /// <summary>
        /// Sets the where clause for this one-to-many relationship.
        /// </summary>
        public T Where(string where)
        {
            collectionAttributes.Set("Where", Layer.UserSupplied, where);
            return (T)this;
        }

        /// <summary>
        /// Specify the select batch size
        /// </summary>
        /// <param name="size">Batch size</param>
        public T BatchSize(int size)
        {
            collectionAttributes.Set("BatchSize", Layer.UserSupplied, size);
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
            collectionAttributes.Set("CollectionType", Layer.UserSupplied, type);
            return (T)this;
        }

        /// <summary>
        /// Specify the table schema
        /// </summary>
        /// <param name="schema">Schema name</param>
        public T Schema(string schema)
        {
            collectionAttributes.Set("Schema", Layer.UserSupplied, schema);
            return (T)this;
        }

        /// <summary>
        /// Specifies an entity-name.
        /// </summary>
        /// <remarks>See http://nhforge.org/blogs/nhibernate/archive/2008/10/21/entity-name-in-action-a-strongly-typed-entity.aspx</remarks>
        public T EntityName(string entityName)
        {
            relationshipAttributes.Set("EntityName", Layer.UserSupplied, entityName);
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
            return ApplyFilter(name, null);
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
            return ApplyFilter(new TFilter().Name, condition);
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
            return ApplyFilter<TFilter>(null);
        }

        protected IList<IFilterMappingProvider> Filters
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

            collectionAttributes.Set("Access", Layer.Defaults, resolvedAccess.ToString());
        }

        void SetCustomCollectionType(Type type)
        {
            if (type.Namespace.StartsWith("Iesi") || type.Namespace.StartsWith("System") || type.IsArray)
                return;

            collectionAttributes.Set("CollectionType", Layer.Defaults, new TypeReference(type));
        }

        CollectionMapping ICollectionMappingProvider.GetCollectionMapping()
        {
            return GetCollectionMapping();
        }

        protected virtual CollectionMapping GetCollectionMapping()
        {
            var mapping = collectionBuilder(collectionAttributes.Clone());

            mapping.ContainingEntityType = entity;
            mapping.Member = member;
            mapping.Set(x => x.Name, Layer.Defaults, GetDefaultName());
            mapping.Set(x => x.ChildType, Layer.Defaults, typeof(TChild));
            mapping.Set(x => x.Key, Layer.Defaults, keyMapping);
            mapping.Set(x => x.Relationship, Layer.Defaults, GetRelationship());
            mapping.Key.ContainingEntityType = entity;

            if (Cache.IsDirty)
                mapping.Set(x => x.Cache, Layer.Defaults, ((ICacheMappingProvider)Cache).GetCacheMapping());

            if (componentMapping != null)
            {
                mapping.Set(x => x.CompositeElement, Layer.Defaults, componentMapping.GetCompositeElementMapping());
                mapping.Set(x => x.Relationship, Layer.Defaults, null); // HACK: bad design
            }

            // HACK: Index only on list and map - shouldn't have to do this!
            if (mapping.Collection == Collection.Array || mapping.Collection == Collection.List || mapping.Collection == Collection.Map)
                mapping.Set(x => x.Index, Layer.Defaults, indexMapping);

            if (elementPart != null)
            {
                mapping.Set(x => x.Element, Layer.Defaults, ((IElementMappingProvider)elementPart).GetElementMapping());
                mapping.Set(x => x.Relationship, Layer.Defaults, null); // HACK: bad design
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
