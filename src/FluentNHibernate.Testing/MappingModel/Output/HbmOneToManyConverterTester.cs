using System;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;
using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmOneToManyConverterTester
    {
        private IHbmConverter<OneToManyMapping, HbmOneToMany> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<OneToManyMapping, HbmOneToMany>>();
        }

        [Test]
        public void ShouldConvertClassIfPopulated()
        {
            var oneToManyMapping = new OneToManyMapping();
            oneToManyMapping.Set(fluent => fluent.Class, Layer.Conventions, new TypeReference("type"));
            var convertedHbmOneToMany = converter.Convert(oneToManyMapping);
            convertedHbmOneToMany.@class.ShouldEqual(oneToManyMapping.Class.ToString());
        }

        [Test]
        public void ShouldNotConvertClassIfNotPopulated()
        {
            var oneToManyMapping = new OneToManyMapping();
            // Don't set anything on the original mapping
            var convertedHbmOneToMany = converter.Convert(oneToManyMapping);
            var blankHbmOneToMany = new HbmOneToMany();
            convertedHbmOneToMany.@class.ShouldEqual(blankHbmOneToMany.@class);
        }

        [Test]
        public void ShouldConvertNotFoundIfPopulatedWithValidValue()
        {
            var notFound = HbmNotFoundMode.Ignore; // Defaults to Exception, so use this to ensure that we can detect changes

            var oneToManyMapping = new OneToManyMapping();
            var notFoundDict = new XmlLinkedEnumBiDictionary<HbmNotFoundMode>();
            oneToManyMapping.Set(fluent => fluent.NotFound, Layer.Conventions, notFoundDict[notFound]);
            var convertedHbmOneToMany = converter.Convert(oneToManyMapping);
            convertedHbmOneToMany.notfound.ShouldEqual(notFound);
        }

        [Test]
        public void ShouldFailToConvertNotFoundIfPopulatedWithInvalidValue()
        {
            var oneToManyMapping = new OneToManyMapping();
            oneToManyMapping.Set(fluent => fluent.NotFound, Layer.Conventions, "invalid_value");
            Assert.Throws<NotSupportedException>(() => converter.Convert(oneToManyMapping));
        }

        [Test]
        public void ShouldNotConvertNotFoundIfNotPopulated()
        {
            var oneToManyMapping = new OneToManyMapping();
            // Don't set anything on the original mapping
            var convertedHbmOneToMany = converter.Convert(oneToManyMapping);
            var blankHbmOneToMany = new HbmOneToMany();
            convertedHbmOneToMany.notfound.ShouldEqual(blankHbmOneToMany.notfound);
        }

        [Test]
        public void ShouldConvertEntityNameIfPopulated()
        {
            var oneToManyMapping = new OneToManyMapping();
            oneToManyMapping.Set(fluent => fluent.EntityName, Layer.Conventions, "name1");
            var convertedHbmOneToMany = converter.Convert(oneToManyMapping);
            convertedHbmOneToMany.entityname.ShouldEqual(oneToManyMapping.EntityName);
        }

        [Test]
        public void ShouldNotConvertEntityNameIfNotPopulated()
        {
            var oneToManyMapping = new OneToManyMapping();
            // Don't set anything on the original mapping
            var convertedHbmOneToMany = converter.Convert(oneToManyMapping);
            var blankHbmOneToMany = new HbmOneToMany();
            convertedHbmOneToMany.entityname.ShouldEqual(blankHbmOneToMany.entityname);
        }
    }
}