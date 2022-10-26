using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmCacheConverterTester
    {
        private IHbmConverter<CacheMapping, HbmCache> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<CacheMapping, HbmCache>>();
        }

        [Test]
        public void ShouldConvertRegionIfPopulatedWithValidValue()
        {
            var cacheMapping = new CacheMapping();
            cacheMapping.Set(fluent => fluent.Region, Layer.Conventions, "region");
            var convertedHbmCache = converter.Convert(cacheMapping);
            convertedHbmCache.region.ShouldEqual(cacheMapping.Region);
        }

        [Test]
        public void ShouldNotConvertRegionIfNotPopulated()
        {
            var cacheMapping = new CacheMapping();
            // Don't set the anything on the original mapping
            var convertedHbmCache = converter.Convert(cacheMapping);
            var blankHbmCache = new HbmCache();
            convertedHbmCache.region.ShouldEqual(blankHbmCache.region);
        }

        [Test]
        public void ShouldConvertUsageIfPopulatedWithValidValue()
        {
            var usage = HbmCacheUsage.Transactional; // Defaults to ReadOnly, so use this to ensure that we can detect changes

            var cacheMapping = new CacheMapping();
            var usageDict = new XmlLinkedEnumBiDictionary<HbmCacheUsage>();
            cacheMapping.Set(fluent => fluent.Usage, Layer.Conventions, usageDict[usage]);
            var convertedHbmCache = converter.Convert(cacheMapping);
            convertedHbmCache.usage.ShouldEqual(usage);
        }

        [Test]
        public void ShouldFailToConvertUsageIfPopulatedWithInvalidValue()
        {
            var cacheMapping = new CacheMapping();
            cacheMapping.Set(fluent => fluent.Usage, Layer.Conventions, "invalid_value");
            Assert.Throws<NotSupportedException>(() => converter.Convert(cacheMapping));
        }

        [Test]
        public void ShouldNotConvertUsageIfNotPopulated()
        {
            var cacheMapping = new CacheMapping();
            // Don't set the anything on the original mapping
            var convertedHbmCache = converter.Convert(cacheMapping);
            var blankHbmCache = new HbmCache();
            convertedHbmCache.usage.ShouldEqual(blankHbmCache.usage);
        }

        [Test]
        public void ShouldConvertIncludeIfPopulatedWithValidValue()
        {
            var include = HbmCacheInclude.NonLazy; // Defaults to All, so use this to ensure that we can detect changes

            var cacheMapping = new CacheMapping();
            var includeDict = new XmlLinkedEnumBiDictionary<HbmCacheInclude>();
            cacheMapping.Set(fluent => fluent.Include, Layer.Conventions, includeDict[include]);
            var convertedHbmCache = converter.Convert(cacheMapping);
            convertedHbmCache.include.ShouldEqual(include);
        }

        [Test]
        public void ShouldFailToConvertIncludeIfPopulatedWithInvalidValue()
        {
            var cacheMapping = new CacheMapping();
            cacheMapping.Set(fluent => fluent.Include, Layer.Conventions, "invalid_value");
            Assert.Throws<NotSupportedException>(() => converter.Convert(cacheMapping));
        }

        [Test]
        public void ShouldNotConvertIncludeIfNotPopulated()
        {
            var cacheMapping = new CacheMapping();
            // Don't set the anything on the original mapping
            var convertedHbmCache = converter.Convert(cacheMapping);
            var blankHbmCache = new HbmCache();
            convertedHbmCache.include.ShouldEqual(blankHbmCache.include);
        }
    }
}
