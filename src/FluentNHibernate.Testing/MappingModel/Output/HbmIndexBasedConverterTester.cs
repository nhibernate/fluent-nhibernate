using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmIndexBasedConverterTester
    {
        [Test]
        public void ShouldConvertIndexForIndexMappingWithNoOffset()
        {
            ShouldConvertSpecificHbmForMapping<IIndexMapping, IndexMapping, object, HbmIndex>(
                () => NewIndexMappingWithNoOffset()
            );
        }

        private static IndexMapping NewIndexMappingWithNoOffset()
        {
            var indexMapping = new IndexMapping();
            // Don't apply a value to IndexMapping.Offset
            return indexMapping;
        }

        [Test]
        public void ShouldConvertListIndexForIndexMappingWithOffset()
        {
            ShouldConvertSpecificHbmForMapping<IIndexMapping, IndexMapping, object, HbmListIndex>(
                () => NewIndexMappingWithOffset()
            );
        }

        private static IndexMapping NewIndexMappingWithOffset()
        {
            var indexMapping = new IndexMapping();
            indexMapping.Set(fluent => fluent.Offset, Layer.Conventions, 31);
            return indexMapping;
        }

        [Test]
        public void ShouldConvertIndexManyToManyForIndexManyToManyMapping()
        {
            ShouldConvertSpecificHbmForMappingChild<IIndexMapping, IndexManyToManyMapping, object, HbmIndexManyToMany>();
        }
    }
}