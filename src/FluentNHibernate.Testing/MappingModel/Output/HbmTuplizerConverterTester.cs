using System;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmTuplizerConverterTester
    {
        private IHbmConverter<TuplizerMapping, HbmTuplizer> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<TuplizerMapping, HbmTuplizer>>();
        }

        [Test]
        public void ShouldConvertModeIfPopulated()
        {
            var hbmEntityMode = HbmTuplizerEntitymode.DynamicMap; // Defaults to Poco, so use something else to properly detect that it changes

            var tuplizerMapping = new TuplizerMapping();
            tuplizerMapping.Set(fluent => fluent.Mode, Layer.Conventions, HbmTuplizerConverter.FluentHbmEntityModeBiDict[hbmEntityMode]);
            var convertedHbmTuplizer = converter.Convert(tuplizerMapping);
            convertedHbmTuplizer.entitymode.ShouldEqual(hbmEntityMode);
            Assert.That(convertedHbmTuplizer.entitymodeSpecified.Equals(true), "Entity mode was not marked as specified");
        }

        // This test shouldn't really be possible, since it relies on the existence of a value in TuplizerMode that does not have
        // an equivalent in HbmTuplizerEntitymode, which really should not be the case (the value is also not present in the XSD).
        // But for as long as it is defined, it is possible for someone to try to use it, and thus valuable to test that we reject
        // it properly.
        [Test]
        public void ShouldFailToConvertModeIfPopulatedWithInvalidValue()
        {
            var tuplizerMapping = new TuplizerMapping();
            tuplizerMapping.Set(fluent => fluent.Mode, Layer.Conventions, TuplizerMode.Xml);
            Assert.Throws<NotSupportedException>(() => converter.Convert(tuplizerMapping));
        }

        [Test]
        public void ShouldNotConvertModeIfNotPopulated()
        {
            var tuplizerMapping = new TuplizerMapping();
            // Don't set anything on the original mapping
            var convertedHbmTuplizer = converter.Convert(tuplizerMapping);
            var blankHbmTuplizer = new HbmTuplizer();
            convertedHbmTuplizer.entitymode.ShouldEqual(blankHbmTuplizer.entitymode);
            Assert.That(convertedHbmTuplizer.entitymodeSpecified.Equals(false), "Entity mode was marked as specified");
        }

        [Test]
        public void ShouldConvertTypeIfPopulated()
        {
            Type tuplizerType = typeof(NHibernate.Tuple.Entity.PocoEntityTuplizer);

            var tuplizerMapping = new TuplizerMapping();
            tuplizerMapping.Set(fluent => fluent.Type, Layer.Conventions, new TypeReference(tuplizerType));
            var convertedHbmTuplizer = converter.Convert(tuplizerMapping);
            convertedHbmTuplizer.@class.ShouldEqual(tuplizerMapping.Type.ToString());
        }

        [Test]
        public void ShouldNotConvertTypeIfNotPopulated()
        {
            var tuplizerMapping = new TuplizerMapping();
            // Don't set anything on the original mapping
            var convertedHbmTuplizer = converter.Convert(tuplizerMapping);
            var blankHbmTuplizer = new HbmTuplizer();
            convertedHbmTuplizer.@class.ShouldEqual(blankHbmTuplizer.@class);
        }

        // The XML variant tests for EntityName handling, but writes it to an attribute that isn't actually defined in the XSD,
        // nor is it available on HbmTuplizer. Given that fact, we don't support it.
    }
}