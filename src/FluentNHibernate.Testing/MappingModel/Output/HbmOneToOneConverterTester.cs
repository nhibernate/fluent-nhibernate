using System;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.DomainModel;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmOneToOneConverterTester
    {
        private IHbmConverter<OneToOneMapping, HbmOneToOne> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<OneToOneMapping, HbmOneToOne>>();
        }

        [Test]
        public void ShouldConvertAccessIfPopulated()
        {
            var oneToOneMapping = new OneToOneMapping();
            oneToOneMapping.Set(fluent => fluent.Access, Layer.Conventions, "access");
            var convertedHbmOneToOne = converter.Convert(oneToOneMapping);
            convertedHbmOneToOne.access.ShouldEqual(oneToOneMapping.Access);
        }

        [Test]
        public void ShouldNotConvertAccessIfNotPopulated()
        {
            var oneToOneMapping = new OneToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmOneToOne = converter.Convert(oneToOneMapping);
            var blankHbmOneToOne = new HbmOneToOne();
            convertedHbmOneToOne.access.ShouldEqual(blankHbmOneToOne.access);
        }

        [Test]
        public void ShouldConvertCascadeIfPopulated()
        {
            var oneToOneMapping = new OneToOneMapping();
            oneToOneMapping.Set(fluent => fluent.Cascade, Layer.Conventions, "cascade");
            var convertedHbmOneToOne = converter.Convert(oneToOneMapping);
            convertedHbmOneToOne.cascade.ShouldEqual(oneToOneMapping.Cascade);
        }

        [Test]
        public void ShouldNotConvertCascadeIfNotPopulated()
        {
            var oneToOneMapping = new OneToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmOneToOne = converter.Convert(oneToOneMapping);
            var blankHbmOneToOne = new HbmOneToOne();
            convertedHbmOneToOne.cascade.ShouldEqual(blankHbmOneToOne.cascade);
        }

        [Test]
        public void ShouldConvertClassIfPopulated()
        {
            var oneToOneMapping = new OneToOneMapping();
            oneToOneMapping.Set(fluent => fluent.Class, Layer.Conventions, new TypeReference(typeof(Record)));
            var convertedHbmOneToOne = converter.Convert(oneToOneMapping);
            convertedHbmOneToOne.@class.ShouldEqual(oneToOneMapping.Class.ToString());
        }

        [Test]
        public void ShouldNotConvertClassIfNotPopulated()
        {
            var oneToOneMapping = new OneToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmOneToOne = converter.Convert(oneToOneMapping);
            var blankHbmOneToOne = new HbmOneToOne();
            convertedHbmOneToOne.@class.ShouldEqual(blankHbmOneToOne.@class);
        }

        [Test]
        public void ShouldConvertConstrainedIfPopulated()
        {
            var oneToOneMapping = new OneToOneMapping();
            oneToOneMapping.Set(fluent => fluent.Constrained, Layer.Conventions, true); // Defaults to false, so use this to ensure that we can detect changes
            var convertedHbmOneToOne = converter.Convert(oneToOneMapping);
            convertedHbmOneToOne.constrained.ShouldEqual(oneToOneMapping.Constrained);
        }

        [Test]
        public void ShouldNotConvertConstrainedIfNotPopulated()
        {
            var oneToOneMapping = new OneToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmOneToOne = converter.Convert(oneToOneMapping);
            var blankHbmOneToOne = new HbmOneToOne();
            convertedHbmOneToOne.constrained.ShouldEqual(blankHbmOneToOne.constrained);
        }

        [Test]
        public void ShouldConvertFetchIfPopulatedWithValidValue()
        {
            var fetch = HbmFetchMode.Join; // Defaults to Select, so use this to ensure that we can spot changes

            var oneToOneMapping = new OneToOneMapping();
            var fetchDict = new XmlLinkedEnumBiDictionary<HbmFetchMode>();
            oneToOneMapping.Set(fluent => fluent.Fetch, Layer.Conventions, fetchDict[fetch]);
            var convertedHbmOneToOne = converter.Convert(oneToOneMapping);
            convertedHbmOneToOne.fetch.ShouldEqual(fetch);
            Assert.That(convertedHbmOneToOne.fetchSpecified.Equals(true), "Fetch was not marked as specified");
        }

        [Test]
        public void ShouldFailToConvertFetchIfPopulatedWithInvalidValue()
        {
            var oneToOneMapping = new OneToOneMapping();
            oneToOneMapping.Set(fluent => fluent.Fetch, Layer.Conventions, "invalid_value");
            Assert.Throws<NotSupportedException>(() => converter.Convert(oneToOneMapping));
        }

        [Test]
        public void ShouldNotConvertFetchIfNotPopulated()
        {
            var oneToOneMapping = new OneToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmOneToOne = converter.Convert(oneToOneMapping);
            var blankHbmOneToOne = new HbmOneToOne();
            convertedHbmOneToOne.fetch.ShouldEqual(blankHbmOneToOne.fetch);
            Assert.That(convertedHbmOneToOne.fetchSpecified.Equals(false), "Fetch was marked as specified");
        }

        [Test]
        public void ShouldConvertForeignKeyIfPopulated()
        {
            var oneToOneMapping = new OneToOneMapping();
            oneToOneMapping.Set(fluent => fluent.ForeignKey, Layer.Conventions, "fk");
            var convertedHbmOneToOne = converter.Convert(oneToOneMapping);
            convertedHbmOneToOne.foreignkey.ShouldEqual(oneToOneMapping.ForeignKey);
        }

        [Test]
        public void ShouldNotConvertForeignKeyIfNotPopulated()
        {
            var oneToOneMapping = new OneToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmOneToOne = converter.Convert(oneToOneMapping);
            var blankHbmOneToOne = new HbmOneToOne();
            convertedHbmOneToOne.foreignkey.ShouldEqual(blankHbmOneToOne.foreignkey);
        }

        [Test]
        public void ShouldConvertLazyIfPopulatedWithValidValue()
        {
            var lazy = HbmLaziness.Proxy; // Defaults to False, so use this to ensure that we can detect changes

            var oneToOneMapping = new OneToOneMapping();
            var lazyDict = new XmlLinkedEnumBiDictionary<HbmLaziness>();
            oneToOneMapping.Set(fluent => fluent.Lazy, Layer.Conventions, lazyDict[lazy]);
            var convertedHbmOneToOne = converter.Convert(oneToOneMapping);
            convertedHbmOneToOne.lazy.ShouldEqual(lazy);
            Assert.That(convertedHbmOneToOne.lazySpecified.Equals(true), "Lazy was not marked as specified");
        }

        [Test]
        public void ShouldFailToConvertLazyIfPopulatedWithInvalidValue()
        {
            var oneToOneMapping = new OneToOneMapping();
            oneToOneMapping.Set(fluent => fluent.Lazy, Layer.Conventions, "invalid_value");
            Assert.Throws<NotSupportedException>(() => converter.Convert(oneToOneMapping));
        }

        [Test]
        public void ShouldNotConvertLazyIfNotPopulated()
        {
            var oneToOneMapping = new OneToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmOneToOne = converter.Convert(oneToOneMapping);
            var blankHbmOneToOne = new HbmOneToOne();
            convertedHbmOneToOne.lazy.ShouldEqual(blankHbmOneToOne.lazy);
            Assert.That(convertedHbmOneToOne.lazySpecified.Equals(false), "Lazy was marked as specified");
        }

        [Test]
        public void ShouldConvertNameIfPopulated()
        {
            var oneToOneMapping = new OneToOneMapping();
            oneToOneMapping.Set(fluent => fluent.Name, Layer.Conventions, "nm");
            var convertedHbmOneToOne = converter.Convert(oneToOneMapping);
            convertedHbmOneToOne.name.ShouldEqual(oneToOneMapping.Name);
        }

        [Test]
        public void ShouldNotConvertNameIfNotPopulated()
        {
            var oneToOneMapping = new OneToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmOneToOne = converter.Convert(oneToOneMapping);
            var blankHbmOneToOne = new HbmOneToOne();
            convertedHbmOneToOne.name.ShouldEqual(blankHbmOneToOne.name);
        }

        [Test]
        public void ShouldConvertPropertyRefIfPopulated()
        {
            var oneToOneMapping = new OneToOneMapping();
            oneToOneMapping.Set(fluent => fluent.PropertyRef, Layer.Conventions, "pr");
            var convertedHbmOneToOne = converter.Convert(oneToOneMapping);
            convertedHbmOneToOne.propertyref.ShouldEqual(oneToOneMapping.PropertyRef);
        }

        [Test]
        public void ShouldNotConvertPropertyRefIfNotPopulated()
        {
            var oneToOneMapping = new OneToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmOneToOne = converter.Convert(oneToOneMapping);
            var blankHbmOneToOne = new HbmOneToOne();
            convertedHbmOneToOne.propertyref.ShouldEqual(blankHbmOneToOne.propertyref);
        }

        [Test]
        public void ShouldConvertEntityNameIfPopulated()
        {
            var oneToOneMapping = new OneToOneMapping();
            oneToOneMapping.Set(fluent => fluent.EntityName, Layer.Conventions, "name1");
            var convertedHbmOneToOne = converter.Convert(oneToOneMapping);
            convertedHbmOneToOne.entityname.ShouldEqual(oneToOneMapping.EntityName);
        }

        [Test]
        public void ShouldNotConvertEntityNameIfNotPopulated()
        {
            var oneToOneMapping = new OneToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmOneToOne = converter.Convert(oneToOneMapping);
            var blankHbmOneToOne = new HbmOneToOne();
            convertedHbmOneToOne.entityname.ShouldEqual(blankHbmOneToOne.entityname);
        }
    }
}