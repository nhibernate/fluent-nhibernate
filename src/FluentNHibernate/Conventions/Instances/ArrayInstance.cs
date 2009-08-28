using System;
using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Persister.Entity;

namespace FluentNHibernate.Conventions.Instances
{
    public class ArrayInstance : ArrayInspector, IArrayInstance
    {
        private readonly ArrayMapping mapping;
        private bool nextBool = true;

        public ArrayInstance(ArrayMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        new public IIndexInstanceBase Index
        {
            get
            {
                if (mapping.Index is IndexMapping)
                { return new IndexInstance(mapping.Index as IndexMapping); }
                if (mapping.Index is IndexManyToManyMapping)
                { return new IndexManyToManyInstance(mapping.Index as IndexManyToManyMapping); }

                throw new InvalidOperationException("IIndexMapping is not a valid type for building an Index Instance ");
            }
        }

        public new IKeyInstance Key
        {
            get { return new KeyInstance(mapping.Key); }
        }

        public new IRelationshipInstance Relationship
        {
            get
            {
                if (mapping.Relationship is ManyToManyMapping)
                    return new ManyToManyInstance((ManyToManyMapping)mapping.Relationship);

                if (mapping.Relationship is OneToManyMapping)
                    return new OneToManyInstance((OneToManyMapping)mapping.Relationship);

                throw new InvalidOperationException("Unsupported Relationship '" + mapping.Relationship.GetType().Name + "'");
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
            if (mapping.IsSpecified("Generic"))
                return;

            mapping.Generic = nextBool;
            nextBool = true;
        }

        public new void Inverse()
        {
            if (mapping.IsSpecified("Inverse"))
                return;

            mapping.Inverse = nextBool;
            nextBool = true;
        }

        public new void Persister<T>() where T : IEntityPersister
        {
            if (!mapping.IsSpecified("Persister"))
                mapping.Persister = new TypeReference(typeof(T));
        }

        public new void Where(string whereClause)
        {
            if (!mapping.IsSpecified("Where"))
                mapping.Where = whereClause;
        }

        public new void OrderBy(string orderBy)
        {
            if (!mapping.IsSpecified("OrderBy"))
                mapping.OrderBy = orderBy;
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
            if (mapping.IsSpecified("Lazy"))
                return;

            mapping.Lazy = nextBool;
            nextBool = true;
        }

        public new void BatchSize(int batchSize)
        {
            if (!mapping.IsSpecified("BatchSize"))
                mapping.BatchSize = batchSize;
        }

        public void ReadOnly()
        {
            if (mapping.IsSpecified("Mutable"))
                return;

            mapping.Mutable = !nextBool;
            nextBool = true;
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

        public new ICacheInstance Cache
        {
            get
            {
                if (mapping.Cache == null)
                    mapping.Cache = new CacheMapping();

                return new CacheInstance(mapping.Cache);
            }
        }
    }
}
