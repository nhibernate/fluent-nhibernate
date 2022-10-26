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
    public class HbmKeyManyToOneConverterTester
    {
        private IHbmConverter<KeyManyToOneMapping, HbmKeyManyToOne> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<KeyManyToOneMapping, HbmKeyManyToOne>>();
        }

        [Test]
        public void ShouldConvertAccessIfPopulated()
        {
            var keyManyToOneMapping = new KeyManyToOneMapping();
            keyManyToOneMapping.Set(fluent => fluent.Access, Layer.Conventions, "access");
            var convertedHbmKeyManyToOne = converter.Convert(keyManyToOneMapping);
            convertedHbmKeyManyToOne.access.ShouldEqual(keyManyToOneMapping.Access);
        }

        [Test]
        public void ShouldNotConvertAccessIfNotPopulated()
        {
            var keyManyToOneMapping = new KeyManyToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmKeyManyToOne = converter.Convert(keyManyToOneMapping);
            var blankHbmKeyManyToOne = new HbmKeyManyToOne();
            convertedHbmKeyManyToOne.access.ShouldEqual(blankHbmKeyManyToOne.access);
        }

        [Test]
        public void ShouldConvertNameIfPopulated()
        {
            var keyManyToOneMapping = new KeyManyToOneMapping();
            keyManyToOneMapping.Set(fluent => fluent.Name, Layer.Conventions, "name");
            var convertedHbmKeyManyToOne = converter.Convert(keyManyToOneMapping);
            convertedHbmKeyManyToOne.name.ShouldEqual(keyManyToOneMapping.Name);
        }

        [Test]
        public void ShouldNotConvertNameIfNotPopulated()
        {
            var keyManyToOneMapping = new KeyManyToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmKeyManyToOne = converter.Convert(keyManyToOneMapping);
            var blankHbmKeyManyToOne = new HbmKeyManyToOne();
            convertedHbmKeyManyToOne.name.ShouldEqual(blankHbmKeyManyToOne.name);
        }

        [Test]
        public void ShouldConvertClassIfPopulated()
        {
            var keyManyToOneMapping = new KeyManyToOneMapping();
            keyManyToOneMapping.Set(fluent => fluent.Class, Layer.Conventions, new TypeReference(typeof(HbmKeyManyToOneConverterTester))); // Can be any class, this one is just guaranteed to exist
            var convertedHbmKeyManyToOne = converter.Convert(keyManyToOneMapping);
            convertedHbmKeyManyToOne.@class.ShouldEqual(keyManyToOneMapping.Class.ToString());
        }

        [Test]
        public void ShouldNotConvertClassIfNotPopulated()
        {
            var keyManyToOneMapping = new KeyManyToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmKeyManyToOne = converter.Convert(keyManyToOneMapping);
            var blankHbmKeyManyToOne = new HbmKeyManyToOne();
            convertedHbmKeyManyToOne.@class.ShouldEqual(blankHbmKeyManyToOne.@class);
        }

        [Test]
        public void ShouldConvertForeignKeyIfPopulated()
        {
            var keyManyToOneMapping = new KeyManyToOneMapping();
            keyManyToOneMapping.Set(fluent => fluent.ForeignKey, Layer.Conventions, "fk");
            var convertedHbmKeyManyToOne = converter.Convert(keyManyToOneMapping);
            convertedHbmKeyManyToOne.foreignkey.ShouldEqual(keyManyToOneMapping.ForeignKey);
        }

        [Test]
        public void ShouldNotConvertForeignKeyIfNotPopulated()
        {
            var keyManyToOneMapping = new KeyManyToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmKeyManyToOne = converter.Convert(keyManyToOneMapping);
            var blankHbmKeyManyToOne = new HbmKeyManyToOne();
            convertedHbmKeyManyToOne.foreignkey.ShouldEqual(blankHbmKeyManyToOne.foreignkey);
        }

        [Test]
        public void ShouldConvertLazyIfPopulated_True()
        {
            var lazyBool = true;
            var lazyEnum = HbmRestrictedLaziness.Proxy; // true maps to proxy, false maps to False

            var keyManyToOneMapping = new KeyManyToOneMapping();
            keyManyToOneMapping.Set(fluent => fluent.Lazy, Layer.Conventions, lazyBool);
            var convertedHbmKeyManyToOne = converter.Convert(keyManyToOneMapping);
            convertedHbmKeyManyToOne.lazy.ShouldEqual(lazyEnum);
            Assert.That(convertedHbmKeyManyToOne.lazySpecified.Equals(true), "Lazy was not marked as specified");
        }

        [Test]
        public void ShouldConvertLazyIfPopulated_False()
        {
            var lazyBool = false;
            var lazyEnum = HbmRestrictedLaziness.False; // true maps to proxy, false maps to False

            var keyManyToOneMapping = new KeyManyToOneMapping();
            keyManyToOneMapping.Set(fluent => fluent.Lazy, Layer.Conventions, lazyBool);
            var convertedHbmKeyManyToOne = converter.Convert(keyManyToOneMapping);
            convertedHbmKeyManyToOne.lazy.ShouldEqual(lazyEnum);
            Assert.That(convertedHbmKeyManyToOne.lazySpecified.Equals(true), "Lazy was not marked as specified");
        }

        [Test]
        public void ShouldNotConvertLazyIfNotPopulated()
        {
            var keyManyToOneMapping = new KeyManyToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmKeyManyToOne = converter.Convert(keyManyToOneMapping);
            var blankHbmKeyManyToOne = new HbmKeyManyToOne();
            convertedHbmKeyManyToOne.lazy.ShouldEqual(blankHbmKeyManyToOne.lazy);
            Assert.That(convertedHbmKeyManyToOne.lazySpecified.Equals(false), "Lazy was marked as specified");
        }

        [Test]
        public void ShouldConvertNotFoundIfPopulatedWithValidValue()
        {
            var notFound = HbmNotFoundMode.Ignore; // Defaults to Exception, so use this to ensure that we can detect changes

            var keyManyToOneMapping = new KeyManyToOneMapping();
            var notFoundDict = new XmlLinkedEnumBiDictionary<HbmNotFoundMode>();
            keyManyToOneMapping.Set(fluent => fluent.NotFound, Layer.Conventions, notFoundDict[notFound]);
            var convertedHbmKeyManyToOne = converter.Convert(keyManyToOneMapping);
            convertedHbmKeyManyToOne.notfound.ShouldEqual(notFound);
        }

        [Test]
        public void ShouldFailToConvertNotFoundIfPopulatedWithInvalidValue()
        {
            var keyManyToOneMapping = new KeyManyToOneMapping();
            keyManyToOneMapping.Set(fluent => fluent.NotFound, Layer.Conventions, "invalid_value");
            Assert.Throws<NotSupportedException>(() => converter.Convert(keyManyToOneMapping));
        }

        [Test]
        public void ShouldNotConvertNotFoundIfNotPopulated()
        {
            var keyManyToOneMapping = new KeyManyToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmKeyManyToOne = converter.Convert(keyManyToOneMapping);
            var blankHbmKeyManyToOne = new HbmKeyManyToOne();
            convertedHbmKeyManyToOne.notfound.ShouldEqual(blankHbmKeyManyToOne.notfound);
        }

        [Test]
        public void ShouldConvertEntityNameIfPopulated()
        {
            var keyManyToOneMapping = new KeyManyToOneMapping();
            keyManyToOneMapping.Set(fluent => fluent.EntityName, Layer.Conventions, "name1");
            var convertedHbmKeyManyToOne = converter.Convert(keyManyToOneMapping);
            convertedHbmKeyManyToOne.entityname.ShouldEqual(keyManyToOneMapping.EntityName);
        }

        [Test]
        public void ShouldNotConvertEntityNameIfNotPopulated()
        {
            var keyManyToOneMapping = new KeyManyToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmKeyManyToOne = converter.Convert(keyManyToOneMapping);
            var blankHbmKeyManyToOne = new HbmKeyManyToOne();
            convertedHbmKeyManyToOne.entityname.ShouldEqual(blankHbmKeyManyToOne.entityname);
        }

        [Test]
        public void ShouldConvertColumns()
        {
            ShouldConvertSubobjectsAsStrictlyTypedArray<KeyManyToOneMapping, ColumnMapping, HbmKeyManyToOne, HbmColumn>(
                (keyManyToOneMapping, columnMapping) => keyManyToOneMapping.AddColumn(columnMapping),
                hbmKeyManyToOne => hbmKeyManyToOne.column);
        }
    }
}