using System;
using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Instances
{
#pragma warning disable 612,618
    public class CollectionInstance : CollectionInspector, ICollectionInstance,
        IArrayInstance, IBagInstance, IListInstance, IMapInstance, ISetInstance
#pragma warning restore 612,618
    {
        readonly CollectionMapping mapping;
        protected bool nextBool = true;

        public CollectionInstance(CollectionMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public new IRelationshipInstance Relationship
        {
            get
            {
                if (mapping.Relationship is ManyToManyMapping)
                    return new ManyToManyInstance((ManyToManyMapping)mapping.Relationship);

                return new OneToManyInstance((OneToManyMapping)mapping.Relationship);
            }
        }

        public new ICollectionCascadeInstance Cascade
        {
            get
            {
                return new CollectionCascadeInstance(value =>
                {
                    if (!mapping.IsSpecified("Cascade"))
                        mapping.Cascade = value;
                });
            }
        }

        public new IFetchInstance Fetch
        {
            get
            {
                return new FetchInstance(value =>
                {
                    if (!mapping.IsSpecified("Fetch"))
                        mapping.Fetch = value;
                });
            }
        }

        public new IOptimisticLockInstance OptimisticLock
        {
            get
            {
                return new OptimisticLockInstance(value =>
                {
                    if (!mapping.IsSpecified("OptimisticLock"))
                        mapping.OptimisticLock = value;
                });
            }
        }

        public new void Check(string constraint)
        {
            if (!mapping.IsSpecified("Check"))
                mapping.Check = constraint;
        }

        public new void CollectionType<T>()
        {
            if (!mapping.IsSpecified("CollectionType"))
                mapping.CollectionType = new TypeReference(typeof(T));
        }

        public new void CollectionType(string type)
        {
            if (!mapping.IsSpecified("CollectionType"))
                mapping.CollectionType = new TypeReference(type);
        }

        public new void CollectionType(Type type)
        {
            if (!mapping.IsSpecified("CollectionType"))
                mapping.CollectionType = new TypeReference(type);
        }

        public new void Generic()
        {
            if (!mapping.IsSpecified("Generic"))
                mapping.Generic = nextBool;
            nextBool = true;
        }

        public new void Inverse()
        {
            if (!mapping.IsSpecified("Inverse"))
                mapping.Inverse = nextBool;
            nextBool = true;
        }

        public new void Persister<T>()
        {
            if (!mapping.IsSpecified("Persister"))
                mapping.Persister = new TypeReference(typeof(T));
        }

        public new void Where(string whereClause)
        {
            if (!mapping.IsSpecified("Where"))
                mapping.Where = whereClause;
        }

        public new IIndexInstanceBase Index
        {
            get
            {
                if (mapping.Index == null)
                    return new IndexInstance(new IndexMapping());

                if (mapping.Index is IndexMapping)
                    return new IndexInstance(mapping.Index as IndexMapping);

                if (mapping.Index is IndexManyToManyMapping)
                    return new IndexManyToManyInstance(mapping.Index as IndexManyToManyMapping);

                throw new InvalidOperationException("This IIndexMapping is not a valid type for inspecting");
            }
        }

        public new void OrderBy(string orderBy)
        {
            if (!mapping.IsSpecified("OrderBy"))
                mapping.OrderBy = orderBy;
        }

        public new void Sort(string sort)
        {
            if (mapping.IsSpecified("Sort"))
                return;

            mapping.Sort = sort;
        }

        public void Subselect(string subselect)
        {
            if (!mapping.IsSpecified("Subselect"))
                mapping.Subselect = subselect;
        }

        public void Table(string tableName)
        {
            if (!mapping.IsSpecified("TableName"))
                mapping.TableName = tableName;
        }

        public new void Name(string name)
        {
            if (!mapping.IsSpecified("Name"))
                mapping.Name = name;
        }

        public new void Schema(string schema)
        {
            if (!mapping.IsSpecified("Schema"))
                mapping.Schema = schema;
        }

        public new void LazyLoad()
        {
            if (!mapping.IsSpecified("Lazy"))
                mapping.Lazy = nextBool ? Lazy.True : Lazy.False;
            nextBool = true;
        }
        
        public override void ExtraLazyLoad()
        {
            if (!mapping.IsSpecified("Lazy"))
                mapping.Lazy = nextBool ? Lazy.Extra : Lazy.True;
            nextBool = true;
        }

        public new void BatchSize(int batchSize)
        {
            if (!mapping.IsSpecified("BatchSize"))
                mapping.BatchSize = batchSize;
        }

        public void ReadOnly()
        {
            if (!mapping.IsSpecified("Mutable"))
                mapping.Mutable = !nextBool;
            nextBool = true;
        }

        void ICollectionInstance.AsArray()
        {
            mapping.Collection = Collection.Array;
        }

        void ICollectionInstance.AsBag()
        {
            mapping.Collection = Collection.Bag;
        }

        void ICollectionInstance.AsList()
        {
            mapping.Collection = Collection.List;
        }

        void ICollectionInstance.AsMap()
        {
            mapping.Collection = Collection.Map;
        }

        void ICollectionInstance.AsSet()
        {
            mapping.Collection = Collection.Set;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ICollectionInstance Not
        {
            get 
            {
                nextBool = !nextBool;
                return this; 
            }
        }
        
        public new ICacheInstance Cache
        {
            get
            {
                if (mapping.Cache == null)
                    // conventions are hitting it, user must want a cache
                    mapping.Cache = new CacheMapping();

                return new CacheInstance(mapping.Cache);
            }
        }

        public void SetOrderBy(string orderBy)
        {
            if (mapping.IsSpecified("OrderBy"))
                return;

            mapping.OrderBy = orderBy;
        }

        public new IAccessInstance Access
        {
            get
            {
                return new AccessInstance(value =>
                {
                    if (!mapping.IsSpecified("Access"))
                        mapping.Access = value;
                });
            }
        }

        public new IKeyInstance Key
        {
            get { return new KeyInstance(mapping.Key); }
        }

        public new IElementInstance Element
        {
            get
            {
                if (!mapping.IsSpecified("Element"))
                    mapping.Element = new ElementMapping();
                
                return new ElementInstance(mapping.Element);
            }
        }

        public void ApplyFilter(string name, string condition)
        {
            mapping.AddFilter(new FilterMapping
            {
                Name = name,
                Condition = condition
            });
        }

        public void ApplyFilter(string name)
        {
            ApplyFilter(name, null);
        }

        public void ApplyFilter<TFilter>(string condition) where TFilter : FilterDefinition, new()
        {
            ApplyFilter(new TFilter().Name, condition);
        }

        public void ApplyFilter<TFilter>() where TFilter : FilterDefinition, new()
        {
            ApplyFilter<TFilter>(null);
        }
    }
}