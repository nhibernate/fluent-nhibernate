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

        /// <summary>
        /// Sets caching to read-write
        /// </summary>
        public CachePart ReadWrite()
        {
            attributes.Set(x => x.Usage, "read-write");
            return this;
        }

        /// <summary>
        /// Sets caching to non-strict read-write
        /// </summary>
        public CachePart NonStrictReadWrite()
        {
            attributes.Set(x => x.Usage, "nonstrict-read-write");
            return this;
        }

        /// <summary>
        /// Sets caching to read-only
        /// </summary>
        public CachePart ReadOnly()
        {
            attributes.Set(x => x.Usage, "read-only");
            return this;
        }

        /// <summary>
        /// Sets caching to transactional
        /// </summary>
        public CachePart Transactional()
        {
            attributes.Set(x => x.Usage, "transactional");
            return this;
        }

        /// <summary>
        /// Specifies a custom cache behaviour
        /// </summary>
        /// <param name="custom">Custom behaviour</param>
        public CachePart CustomUsage(string custom)
        {
            attributes.Set(x => x.Usage, custom);
            return this;
        }

        /// <summary>
        /// Specifies the cache region
        /// </summary>
        /// <returns></returns>
        public CachePart Region(string name)
        {
            attributes.Set(x => x.Region, name);
            return this;
        }

        /// <summary>
        /// Include all properties for caching
        /// </summary>
        /// <returns></returns>
        public CachePart IncludeAll()
        {
            attributes.Set(x => x.Include, "all");
            return this;
        }

        /// <summary>
        /// Include only non-lazy properties for caching
        /// </summary>
        public CachePart IncludeNonLazy()
        {
            attributes.Set(x => x.Include, "non-lazy");
            return this;
        }

        /// <summary>
        /// Specify a custom property inclusion strategy
        /// </summary>
        /// <param name="custom">Inclusion strategy</param>
        public CachePart CustomInclude(string custom)
        {
            attributes.Set(x => x.Include, custom);
            return this;
        }

        internal bool IsDirty
        {
            get { return attributes.IsSpecified(x => x.Region) || attributes.IsSpecified(x => x.Usage) || attributes.IsSpecified(x => x.Include); }
        }

        CacheMapping ICacheMappingProvider.GetCacheMapping()
        {
            var mapping = new CacheMapping(attributes.CloneInner());
            mapping.ContainedEntityType = entityType;

            return mapping;
        }
    }
}