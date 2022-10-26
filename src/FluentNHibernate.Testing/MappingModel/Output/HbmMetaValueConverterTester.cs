using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmMetaValueConverterTester
    {
        private IHbmConverter<MetaValueMapping, HbmMetaValue> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<MetaValueMapping, HbmMetaValue>>();
        }

        [Test]
        public void ShouldConvertClassIfPopulated()
        {
            var metaValueMapping = new MetaValueMapping();
            metaValueMapping.Set(fluent => fluent.Class, Layer.Conventions, new TypeReference("type"));
            var convertedHbmMetaValue = converter.Convert(metaValueMapping);
            convertedHbmMetaValue.@class.ShouldEqual(metaValueMapping.Class.ToString());
        }

        [Test]
        public void ShouldNotConvertClassIfNotPopulated()
        {
            var metaValueMapping = new MetaValueMapping();
            // Don't set anything on the original mapping
            var convertedHbmMetaValue = converter.Convert(metaValueMapping);
            var blankHbmMetaValue = new HbmMetaValue();
            convertedHbmMetaValue.@class.ShouldEqual(blankHbmMetaValue.@class);
        }

        [Test]
        public void ShouldConvertValueIfPopulated()
        {
            var metaValueMapping = new MetaValueMapping();
            metaValueMapping.Set(fluent => fluent.Value, Layer.Conventions, "val");
            var convertedHbmMetaValue = converter.Convert(metaValueMapping);
            convertedHbmMetaValue.value.ShouldEqual(metaValueMapping.Value);
        }

        [Test]
        public void ShouldNotConvertValueIfNotPopulated()
        {
            var metaValueMapping = new MetaValueMapping();
            // Don't set metaValuething on the original mapping
            var convertedHbmMetaValue = converter.Convert(metaValueMapping);
            var blankHbmMetaValue = new HbmMetaValue();
            convertedHbmMetaValue.value.ShouldEqual(blankHbmMetaValue.value);
        }
    }
}