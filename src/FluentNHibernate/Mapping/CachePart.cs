using System;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public class CachePart : ICacheMappingProvider
    {
        private readonly Type entityType;
        private readonly AttributeStore<CacheMapping> attributes = new AttributeStore<CacheMapping>();

        public CachePart(Type entityType)
        {
            this.entityType = entityType;
        }

        CacheMapping ICacheMappingProvider.GetCacheMapping()
        {
            var mapping = new CacheMapping(attributes.CloneInner());
            mapping.ContainedEntityType = entityType;

            return mapping;
        }

        public CachePart ReadWrite()
        {
            attributes.Set(x => x.Usage, "read-write");
            return this;
        }

        public CachePart NonStrictReadWrite()
        {
            attributes.Set(x => x.Usage, "nonstrict-read-write");
            return this;
        }

        public CachePart ReadOnly()
        {
            attributes.Set(x => x.Usage, "read-only");
            return this;
        }

        public CachePart Transactional()
        {
            attributes.Set(x => x.Usage, "transactional");
            return this;
        }

        public CachePart CustomUsage(string custom)
        {
            attributes.Set(x => x.Usage, custom);
            return this;
        }

        public CachePart Region(string name)
        {
            attributes.Set(x => x.Region, name);
            return this;
        }

        public CachePart IncludeAll()
        {
            attributes.Set(x => x.Include, "all");
            return this;
        }

        public CachePart IncludeNonLazy()
        {
            attributes.Set(x => x.Include, "non-lazy");
            return this;
        }

        public CachePart CustomInclude(string custom)
        {
            attributes.Set(x => x.Include, custom);
            return this;
        }

        public bool IsDirty
        {
            get { return attributes.IsSpecified(x => x.Region) || attributes.IsSpecified(x => x.Usage) || attributes.IsSpecified(x => x.Include); }
        }
    }
}