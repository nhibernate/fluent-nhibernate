using System;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public class CachePart : ICacheMappingProvider
    {
        readonly Type entityType;
        readonly AttributeStore attributes = new AttributeStore();

        public CachePart(Type entityType)
        {
            this.entityType = entityType;
        }

        /// <summary>
        /// Sets caching to read-write
        /// </summary>
        public CachePart ReadWrite()
        {
            attributes.Set("Usage", Layer.UserSupplied, "read-write");
            return this;
        }

        /// <summary>
        /// Sets caching to non-strict read-write
        /// </summary>
        public CachePart NonStrictReadWrite()
        {
            attributes.Set("Usage", Layer.UserSupplied, "nonstrict-read-write");
            return this;
        }

        /// <summary>
        /// Sets caching to read-only
        /// </summary>
        public CachePart ReadOnly()
        {
            attributes.Set("Usage", Layer.UserSupplied, "read-only");
            return this;
        }

        /// <summary>
        /// Sets caching to transactional
        /// </summary>
        public CachePart Transactional()
        {
            attributes.Set("Usage", Layer.UserSupplied, "transactional");
            return this;
        }

        /// <summary>
        /// Specifies a custom cache behaviour
        /// </summary>
        /// <param name="custom">Custom behaviour</param>
        public CachePart CustomUsage(string custom)
        {
            attributes.Set("Usage", Layer.UserSupplied, custom);
            return this;
        }

        /// <summary>
        /// Specifies the cache region
        /// </summary>
        /// <returns></returns>
        public CachePart Region(string name)
        {
            attributes.Set("Region", Layer.UserSupplied, name);
            return this;
        }

        /// <summary>
        /// Include all properties for caching
        /// </summary>
        /// <returns></returns>
        public CachePart IncludeAll()
        {
            attributes.Set("Include", Layer.UserSupplied, "all");
            return this;
        }

        /// <summary>
        /// Include only non-lazy properties for caching
        /// </summary>
        public CachePart IncludeNonLazy()
        {
            attributes.Set("Include", Layer.UserSupplied, "non-lazy");
            return this;
        }

        /// <summary>
        /// Specify a custom property inclusion strategy
        /// </summary>
        /// <param name="custom">Inclusion strategy</param>
        public CachePart CustomInclude(string custom)
        {
            attributes.Set("Include", Layer.UserSupplied, custom);
            return this;
        }

        internal bool IsDirty
        {
            get { return attributes.IsSpecified("Region") || attributes.IsSpecified("Usage") || attributes.IsSpecified("Include"); }
        }

        CacheMapping ICacheMappingProvider.GetCacheMapping()
        {
            var mapping = new CacheMapping(attributes.Clone());
            mapping.ContainedEntityType = entityType;

            return mapping;
        }
    }
}