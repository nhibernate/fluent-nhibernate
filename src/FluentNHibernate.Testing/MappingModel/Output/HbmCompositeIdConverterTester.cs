using System;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;
using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmCompositeIdConverterTester
    {
        private IHbmConverter<CompositeIdMapping, HbmCompositeId> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<CompositeIdMapping, HbmCompositeId>>();
        }

        [Test]
        public void ShouldConvertAccessIfPopulated()
        {
            var compositeIdMapping = new CompositeIdMapping();
            compositeIdMapping.Set(fluent => fluent.Access, Layer.Conventions, "access");
            var convertedHbmCompositeId = converter.Convert(compositeIdMapping);
            convertedHbmCompositeId.access.ShouldEqual(compositeIdMapping.Access);
        }

        [Test]
        public void ShouldNotConvertAccessIfNotPopulated()
        {
            var compositeIdMapping = new CompositeIdMapping();
            // Don't set anything on the original mapping
            var convertedHbmCompositeId = converter.Convert(compositeIdMapping);
            var blankHbmCompositeId = new HbmCompositeId();
            convertedHbmCompositeId.access.ShouldEqual(blankHbmCompositeId.access);
        }

        [Test]
        public void ShouldConvertNameIfPopulated()
        {
            var compositeIdMapping = new CompositeIdMapping();
            compositeIdMapping.Set(fluent => fluent.Name, Layer.Conventions, "name");
            var convertedHbmCompositeId = converter.Convert(compositeIdMapping);
            convertedHbmCompositeId.name.ShouldEqual(compositeIdMapping.Name);
        }

        [Test]
        public void ShouldNotConvertNameIfNotPopulated()
        {
            var compositeIdMapping = new CompositeIdMapping();
            // Don't set anything on the original mapping
            var convertedHbmCompositeId = converter.Convert(compositeIdMapping);
            var blankHbmCompositeId = new HbmCompositeId();
            convertedHbmCompositeId.name.ShouldEqual(blankHbmCompositeId.name);
        }

        [Test]
        public void ShouldConvertClassIfPopulated()
        {
            var compositeIdMapping = new CompositeIdMapping();
            compositeIdMapping.Set(fluent => fluent.Class, Layer.Conventions, new TypeReference(typeof(HbmCompositeIdConverterTester))); // Can be any class, this one is just guaranteed to exist
            var convertedHbmCompositeId = converter.Convert(compositeIdMapping);
            convertedHbmCompositeId.@class.ShouldEqual(compositeIdMapping.Class.ToString());
        }

        [Test]
        public void ShouldNotConvertClassIfNotPopulated()
        {
            var compositeIdMapping = new CompositeIdMapping();
            // Don't set anything on the original mapping
            var convertedHbmCompositeId = converter.Convert(compositeIdMapping);
            var blankHbmCompositeId = new HbmCompositeId();
            convertedHbmCompositeId.@class.ShouldEqual(blankHbmCompositeId.@class);
        }

        [Test]
        public void ShouldConvertMappedIfPopulated()
        {
            var compositeIdMapping = new CompositeIdMapping();
            compositeIdMapping.Set(fluent => fluent.Mapped, Layer.Conventions, true); // Defaults to false, so we set it true in order to ensure that we can detect that it changed
            var convertedHbmCompositeId = converter.Convert(compositeIdMapping);
            convertedHbmCompositeId.mapped.ShouldEqual(compositeIdMapping.Mapped);
        }

        [Test]
        public void ShouldNotConvertMappedIfNotPopulated()
        {
            var compositeIdMapping = new CompositeIdMapping();
            // Don't set anything on the original mapping
            var convertedHbmCompositeId = converter.Convert(compositeIdMapping);
            var blankHbmCompositeId = new HbmCompositeId();
            convertedHbmCompositeId.mapped.ShouldEqual(blankHbmCompositeId.mapped);
        }

        [Test]
        public void ShouldConvertUnsavedValueIfPopulatedWithValidValue()
        {
            var unsavedValue = HbmUnsavedValueType.Any; // Defaults to Undefined, so use something else to properly detect that it changes

            var compositeIdMapping = new CompositeIdMapping();
            var unsavedDict = new XmlLinkedEnumBiDictionary<HbmUnsavedValueType>();
            compositeIdMapping.Set(fluent => fluent.UnsavedValue, Layer.Conventions, unsavedDict[unsavedValue]);
            var convertedHbmCompositeId = converter.Convert(compositeIdMapping);
            convertedHbmCompositeId.unsavedvalue.ShouldEqual(unsavedValue);
        }

        [Test]
        public void ShouldFailToConvertUnsavedValueIfPopulatedWithInvalidValue()
        {
            var compositeIdMapping = new CompositeIdMapping();
            compositeIdMapping.Set(fluent => fluent.UnsavedValue, Layer.Conventions, "invalid_value");
            Assert.Throws<NotSupportedException>(() => converter.Convert(compositeIdMapping));
        }

        [Test]
        public void ShouldNotConvertUnsavedValueIfNotPopulated()
        {
            var compositeIdMapping = new CompositeIdMapping();
            // Don't set anything on the original mapping
            var convertedHbmCompositeId = converter.Convert(compositeIdMapping);
            var blankHbmCompositeId = new HbmCompositeId();
            convertedHbmCompositeId.unsavedvalue.ShouldEqual(blankHbmCompositeId.unsavedvalue);
        }

        [Test]
        public void ShouldConvertKeyProperties()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<CompositeIdMapping, KeyPropertyMapping, HbmCompositeId, HbmKeyProperty, object>(
                (compositeIdMapping, keyPropertyMapping) => compositeIdMapping.AddKey(keyPropertyMapping),
                hbmCompositeId => hbmCompositeId.Items);
        }

        [Test]
        public void ShouldConvertKeyManyToOnes()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<CompositeIdMapping, KeyManyToOneMapping, HbmCompositeId, HbmKeyManyToOne, object>(
                (compositeIdMapping, keyManyToOneMapping) => compositeIdMapping.AddKey(keyManyToOneMapping),
                hbmCompositeId => hbmCompositeId.Items);
        }
    }
}