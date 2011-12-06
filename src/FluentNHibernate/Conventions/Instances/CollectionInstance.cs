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
            get { return new CollectionCascadeInstance(value => mapping.Set(x => x.Cascade, Layer.Conventions, value)); }
        }

        public new IFetchInstance Fetch
        {
            get { return new FetchInstance(value => mapping.Set(x => x.Fetch, Layer.Conventions, value)); }
        }

        public new void OptimisticLock()
        {
            mapping.Set(x => x.OptimisticLock, Layer.Conventions, nextBool);
            nextBool = true;
        }

        public new void Check(string constraint)
        {
            mapping.Set(x => x.Check, Layer.Conventions, constraint);
        }

        public new void CollectionType<T>()
        {
            CollectionType(typeof(T));
        }

        public new void CollectionType(string type)
        {
            mapping.Set(x => x.CollectionType, Layer.Conventions, new TypeReference(type));
        }

        public new void CollectionType(Type type)
        {
            mapping.Set(x => x.CollectionType, Layer.Conventions, new TypeReference(type));
        }

        public new void Generic()
        {
            mapping.Set(x => x.Generic, Layer.Conventions, nextBool);
            nextBool = true;
        }

        public new void Inverse()
        {
            mapping.Set(x => x.Inverse, Layer.Conventions, nextBool);
            nextBool = true;
        }

        public new void Persister<T>()
        {
            mapping.Set(x => x.Persister, Layer.Conventions, new TypeReference(typeof(T)));
        }

        public new void Where(string whereClause)
        {
            mapping.Set(x => x.Where, Layer.Conventions, whereClause);
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
            mapping.Set(x => x.OrderBy, Layer.Conventions, orderBy);
        }

        public new void Sort(string sort)
        {
            mapping.Set(x => x.Sort, Layer.Conventions, sort);
        }

        public void Subselect(string subselect)
        {
            mapping.Set(x => x.Subselect, Layer.Conventions, subselect);
        }

        public void KeyNullable()
        {
            mapping.Key.Set(x => x.NotNull, Layer.Conventions, !nextBool);
            nextBool = true;
        }

        public void Table(string tableName)
        {
            mapping.Set(x => x.TableName, Layer.Conventions, tableName);
        }

        public new void Name(string name)
        {
            mapping.Set(x => x.Name, Layer.Conventions, name);
        }

        public new void Schema(string schema)
        {
            mapping.Set(x => x.Schema, Layer.Conventions, schema);
        }

        public new void LazyLoad()
        {
            mapping.Set(x => x.Lazy, Layer.Conventions, nextBool ? Lazy.True : Lazy.False);
            nextBool = true;
        }
        
        public override void ExtraLazyLoad()
        {
            mapping.Set(x => x.Lazy, Layer.Conventions, nextBool ? Lazy.Extra : Lazy.True);
            nextBool = true;
        }

        public new void BatchSize(int batchSize)
        {
            mapping.Set(x => x.BatchSize, Layer.Conventions, batchSize);
        }

        public void ReadOnly()
        {
            mapping.Set(x => x.Mutable, Layer.Conventions, !nextBool);
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
            if (mapping.Index == null)
            {
                var indexMapping = new IndexMapping();
                var columnMapping = new ColumnMapping();
                columnMapping.Set(x => x.Name, Layer.Defaults, "Index");
                indexMapping.AddColumn(Layer.Defaults, columnMapping);
                mapping.Set(x => x.Index, Layer.Defaults, indexMapping);
            };
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
                    mapping.Set(x => x.Cache, Layer.Conventions, new CacheMapping());

                return new CacheInstance(mapping.Cache);
            }
        }

        public void SetOrderBy(string orderBy)
        {
            mapping.Set(x => x.OrderBy, Layer.Conventions, orderBy);
        }

        public new IAccessInstance Access
        {
            get { return new AccessInstance(value => mapping.Set(x => x.Access, Layer.Conventions, value)); }
        }

        public new IKeyInstance Key
        {
            get { return new KeyInstance(mapping.Key); }
        }

        public new IElementInstance Element
        {
            get
            {
                if (mapping.Element == null)
                    mapping.Set(x => x.Element, Layer.Conventions, new ElementMapping());
                
                return new ElementInstance(mapping.Element);
            }
        }

        public void ApplyFilter(string name, string condition)
        {
            var filterMapping = new FilterMapping();
            filterMapping.Set(x => x.Name, Layer.Conventions, name);
            filterMapping.Set(x => x.Condition, Layer.Conventions, condition);
            mapping.AddFilter(filterMapping);
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