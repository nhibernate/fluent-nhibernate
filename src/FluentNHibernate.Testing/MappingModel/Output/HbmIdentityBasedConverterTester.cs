using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmIdentityBasedConverterTester
    {
        private IHbmConverter<IIdentityMapping, object> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<IIdentityMapping, object>>();
        }

        [Test]
        public void ShouldConvertIdForIdMapping()
        {
            ShouldConvertSpecificHbmForMappingChild<IIdentityMapping, IdMapping, object, HbmId>();
        }

        [Test]
        public void ShouldConvertCompositeIdForCompositeIdMapping()
        {
            ShouldConvertSpecificHbmForMappingChild<IIdentityMapping, CompositeIdMapping, object, HbmCompositeId>();
        }
    }
}