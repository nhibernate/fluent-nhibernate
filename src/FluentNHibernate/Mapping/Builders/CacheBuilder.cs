using System;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping.Builders
{
    public class CacheBuilder
    {
        readonly CacheMapping mapping;

        public CacheBuilder(CacheMapping mapping, Type entityType)
        {
            this.mapping = mapping;
            this.mapping.ContainedEntityType = entityType;
        }

        /// <summary>
        /// Sets caching to read-write
        /// </summary>
        public CacheBuilder ReadWrite()
        {
            mapping.Usage = "read-write";
            return this;
        }

        /// <summary>
        /// Sets caching to non-strict read-write
        /// </summary>
        public CacheBuilder NonStrictReadWrite()
        {
            mapping.Usage = "nonstrict-read-write";
            return this;
        }

        /// <summary>
        /// Sets caching to read-only
        /// </summary>
        public CacheBuilder ReadOnly()
        {
            mapping.Usage = "read-only";
            return this;
        }

        /// <summary>
        /// Sets caching to transactional
        /// </summary>
        public CacheBuilder Transactional()
        {
            mapping.Usage = "transactional";
            return this;
        }

        /// <summary>
        /// Specifies a custom cache behaviour
        /// </summary>
        /// <param name="custom">Custom behaviour</param>
        public CacheBuilder CustomUsage(string custom)
        {
            mapping.Usage = custom;
            return this;
        }

        /// <summary>
        /// Specifies the cache region
        /// </summary>
        /// <returns></returns>
        public CacheBuilder Region(string name)
        {
            mapping.Region = name;
            return this;
        }

        /// <summary>
        /// Include all properties for caching
        /// </summary>
        /// <returns></returns>
        public CacheBuilder IncludeAll()
        {
            mapping.Include = "all";
            return this;
        }

        /// <summary>
        /// Include only non-lazy properties for caching
        /// </summary>
        public CacheBuilder IncludeNonLazy()
        {
            mapping.Include = "non-lazy";
            return this;
        }

        /// <summary>
        /// Specify a custom property inclusion strategy
        /// </summary>
        /// <param name="custom">Inclusion strategy</param>
        public CacheBuilder CustomInclude(string custom)
        {
            mapping.Include = custom;
            return this;
        }
    }
}