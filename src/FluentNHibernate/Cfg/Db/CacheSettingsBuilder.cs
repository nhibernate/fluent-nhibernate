using System.Collections.Generic;
using System.Diagnostics;
using NHibernate.Cache;

namespace FluentNHibernate.Cfg.Db
{
    public class CacheSettingsBuilder
    {
        protected const string ProviderClassKey = "cache.provider_class";
        protected const string CacheUseMininmalPutsKey = "cache.use_minimal_puts";
        protected const string CacheUseQueryCacheKey = "cache.use_query_cache";
        protected const string CacheQueryCacheFactoryKey = "cache.query_cache_factory";
        protected const string CacheRegionPrefixKey = "cache.region_prefix";

        private readonly IDictionary<string, string> settings = new Dictionary<string, string>();
        private bool nextBool = true;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public CacheSettingsBuilder Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        public CacheSettingsBuilder ProviderClass(string providerclass)
        {
            settings.Add(ProviderClassKey, providerclass);
            IsDirty = true;
            return this;
        }

        public CacheSettingsBuilder ProviderClass<T>()
            where T : ICacheProvider
        {
            return ProviderClass(typeof(T).AssemblyQualifiedName);
        }

        public CacheSettingsBuilder UseMinimalPuts()
        {
            settings.Add(CacheUseMininmalPutsKey, nextBool.ToString().ToLowerInvariant());
            nextBool = true;
            IsDirty = true;
            return this;
        }

        public CacheSettingsBuilder UseQueryCache()
        {
            settings.Add(CacheUseQueryCacheKey, nextBool.ToString().ToLowerInvariant());
            nextBool = true;
            IsDirty = true;
            return this;
        }

        public CacheSettingsBuilder QueryCacheFactory(string factory)
        {
            settings.Add(CacheQueryCacheFactoryKey, factory);
            IsDirty = true;
            return this;
        }

        public CacheSettingsBuilder QueryCacheFactory<T>()
            where T : IQueryCacheFactory
        {
            return QueryCacheFactory(typeof(T).AssemblyQualifiedName);
        }

        public CacheSettingsBuilder RegionPrefix(string prefix)
        {
            settings.Add(CacheRegionPrefixKey, prefix);
            IsDirty = true;
            return this;
        }

        protected internal IDictionary<string, string> Create()
        {
            return settings;
        }

        internal bool IsDirty { get; set; }
    }
}