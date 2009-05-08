using System.Collections.Generic;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cache;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Cfg.Db
{
    [TestFixture]
    public class CacheSettingsBuilderTester
    {
        private CacheSettingsBuilderDouble cache;

        [SetUp]
        public void CreateCache()
        {
            cache = new CacheSettingsBuilderDouble();
        }

        [Test]
        public void CanUseQueryCache()
        {
            cache.UseQueryCache();

            HasProperty("cache.use_query_cache", "true");
        }

        [Test]
        public void CanNotUseQueryCache()
        {
            cache.Not.UseQueryCache();

            HasProperty("cache.use_query_cache", "false");
        }

        [Test]
        public void CanUseMinimalPuts()
        {
            cache.UseMinimalPuts();

            HasProperty("cache.use_minimal_puts", "true");
        }

        [Test]
        public void CanNotUseMinimalPuts()
        {
            cache.Not.UseMinimalPuts();

            HasProperty("cache.use_minimal_puts", "false");
        }

        [Test]
        public void CanSetProviderClass()
        {
            cache.ProviderClass("provider");

            HasProperty("cache.provider_class", "provider");
        }

        [Test]
        public void CanSetProviderGenerically()
        {
            cache.ProviderClass<NoCacheProvider>();

            HasProperty("cache.provider_class", typeof(NoCacheProvider).AssemblyQualifiedName);
        }

        [Test]
        public void CanSetQueryCacheFactory()
        {
            cache.QueryCacheFactory("factory");

            HasProperty("cache.query_cache_factory", "factory");
        }

        [Test]
        public void CanSetQueryCacheFactoryGenerically()
        {
            cache.QueryCacheFactory<StandardQueryCacheFactory>();

            HasProperty("cache.query_cache_factory", typeof(StandardQueryCacheFactory).AssemblyQualifiedName);
        }

        [Test]
        public void CanSetRegionPrefix()
        {
            cache.RegionPrefix("prefix");

            HasProperty("cache.region_prefix", "prefix");
        }

        [Test]
        public void CanSetThemAll()
        {
            cache.ProviderClass("provider")
                .QueryCacheFactory("factory")
                .RegionPrefix("prefix")
                .UseMinimalPuts()
                .UseQueryCache();

            HasProperty("cache.provider_class", "provider");
            HasProperty("cache.query_cache_factory", "factory");
            HasProperty("cache.region_prefix", "prefix");
            HasProperty("cache.use_minimal_puts", "true");
            HasProperty("cache.use_query_cache", "true");
        }

        private void HasProperty(string key, string value)
        {
            cache.Properties.ShouldContain(key, value);
        }

        private class CacheSettingsBuilderDouble : CacheSettingsBuilder
        {
            public IDictionary<string, string> Properties
            {
                get { return Create(); }
            }
        }
    }
}