using System;
using System.Reflection;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Persister.Entity;

namespace FluentNHibernate.Conventions.Instances
{
    public class CollectionInstance : CollectionInspector, ICollectionInstance
    {
        protected readonly ICollectionMapping mapping;
        protected bool nextBool = true;

        public CollectionInstance(ICollectionMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public new IRelationshipInstance Relationship
        {
            get { return new RelationshipInstance(mapping.Relationship); }
        }

        ICollectionCascadeInstance ICollectionInstance.Cascade
        {
            get
            {
                return new CollectionCascadeInstance(value =>
                {
                    if (!mapping.IsSpecified(x => x.Cascade))
                        mapping.Cascade = value;
                });
            }
        }

        IFetchInstance ICollectionInstance.Fetch
        {
            get
            {
                return new FetchInstance(value =>
                {
                    if (!mapping.IsSpecified(x => x.Fetch))
                        mapping.Fetch = value;
                });
            }
        }

        IOptimisticLockInstance ICollectionInstance.OptimisticLock
        {
            get
            {
                return new OptimisticLockInstance(value =>
                {
                    if (!mapping.IsSpecified(x => x.Fetch))
                        mapping.Fetch = value;
                });
            }
        }

        IOuterJoinInstance ICollectionInstance.OuterJoin
        {
            get
            {
                return new OuterJoinInstance(value =>
                {
                    if (!mapping.IsSpecified(x => x.Fetch))
                        mapping.Fetch = value;
                });
            }
        }

        string ICollectionInspector.OuterJoin
        {
            get { return mapping.OuterJoin; }
        }

        string ICollectionInspector.OptimisticLock
        {
            get { return mapping.OptimisticLock; }
        }

        bool ICollectionInspector.Generic
        {
            get { return mapping.Generic; }
        }

        bool ICollectionInspector.Inverse
        {
            get { return mapping.Inverse; }
        }

        string ICollectionInspector.Fetch
        {
            get { return mapping.Fetch; }
        }

        public void Check(string constraint)
        {
            if (!mapping.IsSpecified(x => x.Check))
                mapping.Check = constraint;
        }

        public void CollectionType<T>()
        {
            if (!mapping.IsSpecified(x => x.CollectionType))
                mapping.CollectionType = new TypeReference(typeof(T));
        }

        public void CollectionType(string type)
        {
            if (!mapping.IsSpecified(x => x.CollectionType))
                mapping.CollectionType = new TypeReference(type);
        }

        public void CollectionType(Type type)
        {
            if (!mapping.IsSpecified(x => x.CollectionType))
                mapping.CollectionType = new TypeReference(type);
        }

        void ICollectionInstance.Generic()
        {
            if (mapping.IsSpecified(x => x.Generic))
                return;
                
            mapping.Generic = nextBool;
            nextBool = true;
        }

        void ICollectionInstance.Inverse()
        {
            if (mapping.IsSpecified(x => x.Inverse))
                return;

            mapping.Inverse = nextBool;
            nextBool = true;
        }

        public void Persister<T>() where T : IEntityPersister
        {
            if (!mapping.IsSpecified(x => x.Persister))
                mapping.Persister = new TypeReference(typeof(T));
        }

        public void Where(string whereClause)
        {
            if (!mapping.IsSpecified(x => x.Where))
                mapping.Where = whereClause;
        }

        string ICollectionInspector.Cascade
        {
            get { return mapping.Cascade; }
        }

        public void SetTableName(string tableName)
        {
            if (!mapping.IsSpecified(x => x.TableName))
                mapping.TableName = tableName;
        }

        public void Name(string name)
        {
            if (!mapping.IsSpecified(x => x.Name))
                mapping.Name = name;
        }

        public void SchemaIs(string schema)
        {
            if (!mapping.IsSpecified(x => x.Schema))
                mapping.Schema = schema;
        }

        public void LazyLoad()
        {
            if (!mapping.IsSpecified(x => x.Lazy))
                mapping.Lazy = nextBool ? Laziness.True : Laziness.False;
        }

        public void BatchSize(int batchSize)
        {
            if (!mapping.IsSpecified(x => x.BatchSize))
                mapping.BatchSize = batchSize;
        }

        public ICollectionInstance Not
        {
            get 
            {
                nextBool = !nextBool;
                return this; 
            }
        }
        
        public ICacheInstance Cache
        {
            get
            {
                if (mapping.Cache == null)
                    // conventions are hitting it, user must want a cache
                    mapping.Cache = new CacheMapping();

                return new CacheInstance(mapping.Cache);
            }
        }

        IKeyInstance ICollectionInstance.Key
        {
            get { return Key; }
        }
        public string TableName
        {
            get { return mapping.TableName; }
        }
        public bool IsMethodAccess
        {
            get { return Member != null && Member.MemberType == MemberTypes.Method; }
        }
        public MemberInfo Member
        {
            get { return mapping.MemberInfo; }
        }
        IRelationshipInspector ICollectionInspector.Relationship
        {
            get { return Relationship; }
        }

        public IAccessInstance Access
        {
            get
            {
                return new AccessInstance(value =>
                {
                    if (!mapping.IsSpecified(x => x.Access))
                        mapping.Access = value;
                });
            }
        }

        public IKeyInstance Key
        {
            get { return new KeyInstance(mapping.Key); }
        }
    }
}