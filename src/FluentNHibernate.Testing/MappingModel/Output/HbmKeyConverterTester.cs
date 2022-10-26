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
    public class HbmKeyConverterTester
    {
        private IHbmConverter<KeyMapping, HbmKey> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<KeyMapping, HbmKey>>();
        }

        [Test]
        public void ShouldConvertForeignKeyIfPopulated()
        {
            var keyMapping = new KeyMapping();
            keyMapping.Set(fluent => fluent.ForeignKey, Layer.Conventions, "fk");
            var convertedHbmKey = converter.Convert(keyMapping);
            convertedHbmKey.foreignkey.ShouldEqual(keyMapping.ForeignKey);
        }

        [Test]
        public void ShouldNotConvertForeignKeyIfNotPopulated()
        {
            var keyMapping = new KeyMapping();
            // Don't set anything on the original mapping
            var convertedHbmKey = converter.Convert(keyMapping);
            var blankHbmKey = new HbmKey();
            convertedHbmKey.foreignkey.ShouldEqual(blankHbmKey.foreignkey);
        }

        [Test]
        public void ShouldConvertPropertyRefIfPopulated()
        {
            var keyMapping = new KeyMapping();
            keyMapping.Set(fluent => fluent.PropertyRef, Layer.Conventions, "prop");
            var convertedHbmKey = converter.Convert(keyMapping);
            convertedHbmKey.propertyref.ShouldEqual(keyMapping.PropertyRef);
        }

        [Test]
        public void ShouldNotConvertPropertyRefIfNotPopulated()
        {
            var keyMapping = new KeyMapping();
            // Don't set anything on the original mapping
            var convertedHbmKey = converter.Convert(keyMapping);
            var blankHbmKey = new HbmKey();
            convertedHbmKey.propertyref.ShouldEqual(blankHbmKey.propertyref);
        }

        [Test]
        public void ShouldConvertOnDeleteIfPopulatedWithValidValue()
        {
            var ondelete = HbmOndelete.Cascade; // Defaults to Noaction, so use this to ensure that we can detect changes

            var keyMapping = new KeyMapping();
            var ondeleteDict = new XmlLinkedEnumBiDictionary<HbmOndelete>();
            keyMapping.Set(fluent => fluent.OnDelete, Layer.Conventions, ondeleteDict[ondelete]);
            var convertedHbmKey = converter.Convert(keyMapping);
            convertedHbmKey.ondelete.ShouldEqual(ondelete);
        }

        [Test]
        public void ShouldFailToConvertOnDeleteIfPopulatedWithInvalidValue()
        {
            var keyMapping = new KeyMapping();
            keyMapping.Set(fluent => fluent.OnDelete, Layer.Conventions, "invalid_value");
            Assert.Throws<NotSupportedException>(() => converter.Convert(keyMapping));
        }

        [Test]
        public void ShouldNotConvertOnDeleteIfNotPopulated()
        {
            var keyMapping = new KeyMapping();
            // Don't set anything on the original mapping
            var convertedHbmKey = converter.Convert(keyMapping);
            var blankHbmKey = new HbmKey();
            convertedHbmKey.ondelete.ShouldEqual(blankHbmKey.ondelete);
        }

        [Test]
        public void ShouldConvertNotNullIfPopulated()
        {
            var keyMapping = new KeyMapping();
            keyMapping.Set(fluent => fluent.NotNull, Layer.Conventions, true); // Defaults to false, so use true so that we can detect changes
            var convertedHbmKey = converter.Convert(keyMapping);
            convertedHbmKey.notnull.ShouldEqual(keyMapping.NotNull);
            Assert.That(convertedHbmKey.notnullSpecified.Equals(true), "NotNull was not marked as specified");
        }

        [Test]
        public void ShouldNotConvertNotNullIfNotPopulated()
        {
            var keyMapping = new KeyMapping();
            // Don't set anything on the original mapping
            var convertedHbmKey = converter.Convert(keyMapping);
            var blankHbmKey = new HbmKey();
            convertedHbmKey.notnull.ShouldEqual(blankHbmKey.notnull);
            Assert.That(convertedHbmKey.notnullSpecified.Equals(false), "NotNull was marked as specified");
        }

        [Test]
        public void ShouldConvertUpdateIfPopulated()
        {
            var keyMapping = new KeyMapping();
            keyMapping.Set(fluent => fluent.Update, Layer.Conventions, true); // Defaults to false, so use true so that we can detect changes
            var convertedHbmKey = converter.Convert(keyMapping);
            convertedHbmKey.update.ShouldEqual(keyMapping.Update);
            Assert.That(convertedHbmKey.updateSpecified.Equals(true), "Update was not marked as specified");
        }

        [Test]
        public void ShouldNotConvertUpdateIfNotPopulated()
        {
            var keyMapping = new KeyMapping();
            // Don't set anything on the original mapping
            var convertedHbmKey = converter.Convert(keyMapping);
            var blankHbmKey = new HbmKey();
            convertedHbmKey.update.ShouldEqual(blankHbmKey.update);
            Assert.That(convertedHbmKey.updateSpecified.Equals(false), "Update was marked as specified");
        }

        [Test]
        public void ShouldConvertUniqueIfPopulated()
        {
            var keyMapping = new KeyMapping();
            keyMapping.Set(fluent => fluent.Unique, Layer.Conventions, true); // Defaults to false, so use true so that we can detect changes
            var convertedHbmKey = converter.Convert(keyMapping);
            convertedHbmKey.unique.ShouldEqual(keyMapping.Unique);
            Assert.That(convertedHbmKey.uniqueSpecified.Equals(true), "Unique was not marked as specified");
        }

        [Test]
        public void ShouldNotConvertUniqueIfNotPopulated()
        {
            var keyMapping = new KeyMapping();
            // Don't set anything on the original mapping
            var convertedHbmKey = converter.Convert(keyMapping);
            var blankHbmKey = new HbmKey();
            convertedHbmKey.unique.ShouldEqual(blankHbmKey.unique);
            Assert.That(convertedHbmKey.uniqueSpecified.Equals(false), "Unique was marked as specified");
        }

        [Test]
        public void ShouldConvertColumns()
        {
            ShouldConvertSubobjectsAsStrictlyTypedArray<KeyMapping, ColumnMapping, HbmKey, HbmColumn>(
                (keyMapping, columnMapping) => keyMapping.AddColumn(Layer.Defaults, columnMapping),
                hbmKey => hbmKey.column);
        }
    }
}