using System;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public class CachePart : ICacheMappingProvider
    {
        private readonly CacheMapping mapping = new CacheMapping();
        private readonly Type entityType;

        public CachePart(Type entityType)
        {
            this.entityType = entityType;
        }

        CacheMapping ICacheMappingProvider.GetCacheMapping()
        {
            mapping.ContainedEntityType = entityType;

            return mapping;
        }

        public CachePart ReadWrite()
        {
           mapping.Usage = "read-write";
           return this;
        }

        public CachePart NonStrictReadWrite()
        {
            mapping.Usage = "nonstrict-read-write";
            return this;
        }

        public CachePart ReadOnly()
        {
            mapping.Usage = "read-only";
            return this;
        }

        public CachePart Custom(string custom)
        {
            mapping.Usage = custom;
            return this;
        }

        public CachePart Region(string name)
        {
            mapping.Region = name;
            return this;
        }

        public bool IsDirty
        {
            get { return mapping.IsSpecified(x => x.Region) || mapping.IsSpecified(x => x.Usage); }
        }
    }
}