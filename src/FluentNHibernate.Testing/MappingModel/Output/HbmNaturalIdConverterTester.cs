using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;
using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmNaturalIdConverterTester
    {
        private IHbmConverter<NaturalIdMapping, HbmNaturalId> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<NaturalIdMapping, HbmNaturalId>>();
        }

        [Test]
        public void ShouldConvertMutableIfPopulated()
        {
            var naturalIdMapping = new NaturalIdMapping();
            naturalIdMapping.Set(fluent => fluent.Mutable, Layer.Conventions, true); // Defaults to false, so use this to ensure that we can detect changes
            var convertedHbmNaturalId = converter.Convert(naturalIdMapping);
            convertedHbmNaturalId.mutable.ShouldEqual(naturalIdMapping.Mutable);
        }

        [Test]
        public void ShouldNotConvertMutableIfNotPopulated()
        {
            var naturalIdMapping = new NaturalIdMapping();
            // Don't set anything on the original mapping
            var convertedHbmNaturalId = converter.Convert(naturalIdMapping);
            var blankHbmNaturalId = new HbmNaturalId();
            convertedHbmNaturalId.mutable.ShouldEqual(blankHbmNaturalId.mutable);
        }

        [Test]
        public void ShouldConvertProperties()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<NaturalIdMapping, PropertyMapping, HbmNaturalId, HbmProperty, object>(
                (naturalIdMapping, propertyMapping) => naturalIdMapping.AddProperty(propertyMapping),
                hbmNaturalId => hbmNaturalId.Items);
        }

        [Test]
        public void ShouldConvertManyToOnes()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<NaturalIdMapping, ManyToOneMapping, HbmNaturalId, HbmManyToOne, object>(
                (naturalIdMapping, manyToOneMapping) => naturalIdMapping.AddReference(manyToOneMapping),
                hbmNaturalId => hbmNaturalId.Items);
        }
    }
}