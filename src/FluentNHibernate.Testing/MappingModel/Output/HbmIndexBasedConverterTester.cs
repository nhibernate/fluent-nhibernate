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
        private IHbmConverter<IIndexMapping, object> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<IIndexMapping, object>>();
        }

        [Test]
        public void ShouldConvertIndexForIndexMapping()
        {
            ShouldConvertSpecificHbmForMappingChild<IIndexMapping, IndexMapping, object, HbmIndex>();
        }

        [Test]
        public void ShouldConvertIndexManyToManyForIndexManyToManyMapping()
        {
            ShouldConvertSpecificHbmForMappingChild<IIndexMapping, IndexManyToManyMapping, object, HbmIndexManyToMany>();
        }
    }
}