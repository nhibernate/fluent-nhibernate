using System.Collections.Generic;
using System.Diagnostics;
using NHibernate.Cache;
using NHibEnvironment = NHibernate.Cfg.Environment;

namespace FluentNHibernate.Cfg.Db
{
    public class CacheSettingsBuilder
    {
        protected const string ProviderClassKey = NHibEnvironment.CacheProvider;
        protected const string CacheUseMininmalPutsKey = NHibEnvironment.UseMinimalPuts;
        protected const string CacheUseQueryCacheKey = NHibEnvironment.UseQueryCache;
        protected const string CacheUseSecondLevelCacheKey = NHibEnvironment.UseSecondLevelCache;
        protected const string CacheQueryCacheFactoryKey = NHibEnvironment.QueryCacheFactory;
        protected const string CacheRegionPrefixKey = NHibEnvironment.CacheRegionPrefix;

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

        public CacheSettingsBuilder UseSecondLevelCache()
        {
            settings.Add(CacheUseSecondLevelCacheKey, nextBool.ToString().ToLowerInvariant());
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