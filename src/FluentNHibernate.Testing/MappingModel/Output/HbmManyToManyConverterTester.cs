using System;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.DomainModel;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;
using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmManyToManyConverterTester
    {
        private IHbmConverter<ManyToManyMapping, HbmManyToMany> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<ManyToManyMapping, HbmManyToMany>>();
        }

        [Test]
        public void ShouldConvertClassIfPopulated()
        {
            var manyToManyMapping = new ManyToManyMapping();
            manyToManyMapping.Set(fluent => fluent.Class, Layer.Conventions, new TypeReference("type"));
            var convertedHbmManyToMany = converter.Convert(manyToManyMapping);
            convertedHbmManyToMany.@class.ShouldEqual(manyToManyMapping.Class.ToString());
        }

        [Test]
        public void ShouldNotConvertClassIfNotPopulated()
        {
            var manyToManyMapping = new ManyToManyMapping();
            // Don't set anything on the original mapping
            var convertedHbmManyToMany = converter.Convert(manyToManyMapping);
            var blankHbmManyToMany = new HbmManyToMany();
            convertedHbmManyToMany.@class.ShouldEqual(blankHbmManyToMany.@class);
        }

        [Test]
        public void ShouldConvertFetchIfPopulatedWithValidValue()
        {
            var fetch = HbmFetchMode.Join; // Defaults to Select, so use this to ensure that we can spot changes

            var manyToManyMapping = new ManyToManyMapping();
            var fetchDict = new XmlLinkedEnumBiDictionary<HbmFetchMode>();
            manyToManyMapping.Set(fluent => fluent.Fetch, Layer.Conventions, fetchDict[fetch]);
            var convertedHbmManyToMany = converter.Convert(manyToManyMapping);
            convertedHbmManyToMany.fetch.ShouldEqual(fetch);
            Assert.That(convertedHbmManyToMany.fetchSpecified.Equals(true), "Fetch was not marked as specified");
        }

        [Test]
        public void ShouldFailToConvertFetchIfPopulatedWithInvalidValue()
        {
            var manyToManyMapping = new ManyToManyMapping();
            manyToManyMapping.Set(fluent => fluent.Fetch, Layer.Conventions, "invalid_value");
            Assert.Throws<NotSupportedException>(() => converter.Convert(manyToManyMapping));
        }

        [Test]
        public void ShouldNotConvertFetchIfNotPopulated()
        {
            var manyToManyMapping = new ManyToManyMapping();
            // Don't set anything on the original mapping
            var convertedHbmManyToMany = converter.Convert(manyToManyMapping);
            var blankHbmManyToMany = new HbmManyToMany();
            convertedHbmManyToMany.fetch.ShouldEqual(blankHbmManyToMany.fetch);
            Assert.That(convertedHbmManyToMany.fetchSpecified.Equals(false), "Fetch was marked as specified");
        }

        [Test]
        public void ShouldConvertForeignKeyIfPopulated()
        {
            var manyToManyMapping = new ManyToManyMapping();
            manyToManyMapping.Set(fluent => fluent.ForeignKey, Layer.Conventions, "fk");
            var convertedHbmManyToMany = converter.Convert(manyToManyMapping);
            convertedHbmManyToMany.foreignkey.ShouldEqual(manyToManyMapping.ForeignKey);
        }

        [Test]
        public void ShouldNotConvertForeignKeyIfNotPopulated()
        {
            var manyToManyMapping = new ManyToManyMapping();
            // Don't set anything on the original mapping
            var convertedHbmManyToMany = converter.Convert(manyToManyMapping);
            var blankHbmManyToMany = new HbmManyToMany();
            convertedHbmManyToMany.foreignkey.ShouldEqual(blankHbmManyToMany.foreignkey);
        }

        [Test]
        public void ShouldConvertLazyIfPopulated_True()
        {
            var lazyBool = true;
            var lazyEnum = HbmRestrictedLaziness.Proxy; // true maps to proxy, false maps to False

            var manyToManyMapping = new ManyToManyMapping();
            manyToManyMapping.Set(fluent => fluent.Lazy, Layer.Conventions, lazyBool);
            var convertedHbmManyToMany = converter.Convert(manyToManyMapping);
            convertedHbmManyToMany.lazy.ShouldEqual(lazyEnum);
            Assert.That(convertedHbmManyToMany.lazySpecified.Equals(true), "Lazy was not marked as specified");
        }

        [Test]
        public void ShouldConvertLazyIfPopulated_False()
        {
            var lazyBool = false;
            var lazyEnum = HbmRestrictedLaziness.False; // true maps to proxy, false maps to False

            var manyToManyMapping = new ManyToManyMapping();
            manyToManyMapping.Set(fluent => fluent.Lazy, Layer.Conventions, lazyBool);
            var convertedHbmManyToMany = converter.Convert(manyToManyMapping);
            convertedHbmManyToMany.lazy.ShouldEqual(lazyEnum);
            Assert.That(convertedHbmManyToMany.lazySpecified.Equals(true), "Lazy was not marked as specified");
        }

        [Test]
        public void ShouldNotConvertLazyIfNotPopulated()
        {
            var manyToManyMapping = new ManyToManyMapping();
            // Don't set anything on the original mapping
            var convertedHbmManyToMany = converter.Convert(manyToManyMapping);
            var blankHbmKeyManyToOne = new HbmKeyManyToOne();
            convertedHbmManyToMany.lazy.ShouldEqual(blankHbmKeyManyToOne.lazy);
            Assert.That(convertedHbmManyToMany.lazySpecified.Equals(false), "Lazy was marked as specified");
        }

        [Test]
        public void ShouldConvertNotFoundIfPopulatedWithValidValue()
        {
            var notFound = HbmNotFoundMode.Ignore; // Defaults to Exception, so use this to ensure that we can detect changes

            var manyToManyMapping = new ManyToManyMapping();
            var notFoundDict = new XmlLinkedEnumBiDictionary<HbmNotFoundMode>();
            manyToManyMapping.Set(fluent => fluent.NotFound, Layer.Conventions, notFoundDict[notFound]);
            var convertedHbmManyToMany = converter.Convert(manyToManyMapping);
            convertedHbmManyToMany.notfound.ShouldEqual(notFound);
        }

        [Test]
        public void ShouldFailToConvertNotFoundIfPopulatedWithInvalidValue()
        {
            var manyToManyMapping = new ManyToManyMapping();
            manyToManyMapping.Set(fluent => fluent.NotFound, Layer.Conventions, "invalid_value");
            Assert.Throws<NotSupportedException>(() => converter.Convert(manyToManyMapping));
        }

        [Test]
        public void ShouldNotConvertNotFoundIfNotPopulated()
        {
            var manyToManyMapping = new ManyToManyMapping();
            // Don't set anything on the original mapping
            var convertedHbmManyToMany = converter.Convert(manyToManyMapping);
            var blankHbmManyToMany = new HbmManyToMany();
            convertedHbmManyToMany.notfound.ShouldEqual(blankHbmManyToMany.notfound);
        }

        [Test]
        public void ShouldConvertWhereIfPopulated()
        {
            var manyToManyMapping = new ManyToManyMapping();
            manyToManyMapping.Set(fluent => fluent.Where, Layer.Conventions, "x = 1");
            var convertedHbmManyToMany = converter.Convert(manyToManyMapping);
            convertedHbmManyToMany.where.ShouldEqual(manyToManyMapping.Where);
        }

        [Test]
        public void ShouldNotConvertWhereIfNotPopulated()
        {
            var manyToManyMapping = new ManyToManyMapping();
            // Don't set anything on the original mapping
            var convertedHbmManyToMany = converter.Convert(manyToManyMapping);
            var blankHbmManyToMany = new HbmManyToMany();
            convertedHbmManyToMany.where.ShouldEqual(blankHbmManyToMany.where);
        }

        [Test]
        public void ShouldConvertEntityNameIfPopulated()
        {
            var manyToManyMapping = new ManyToManyMapping();
            manyToManyMapping.Set(fluent => fluent.EntityName, Layer.Conventions, "name1");
            var convertedHbmManyToMany = converter.Convert(manyToManyMapping);
            convertedHbmManyToMany.entityname.ShouldEqual(manyToManyMapping.EntityName);
        }

        [Test]
        public void ShouldNotConvertEntityNameIfNotPopulated()
        {
            var manyToManyMapping = new ManyToManyMapping();
            // Don't set anything on the original mapping
            var convertedHbmManyToMany = converter.Convert(manyToManyMapping);
            var blankHbmManyToMany = new HbmManyToMany();
            convertedHbmManyToMany.entityname.ShouldEqual(blankHbmManyToMany.entityname);
        }

        [Test]
        public void ShouldConvertOrderByIfPopulated()
        {
            var manyToManyMapping = new ManyToManyMapping();
            manyToManyMapping.Set(fluent => fluent.OrderBy, Layer.Conventions, "col1");
            var convertedHbmManyToMany = converter.Convert(manyToManyMapping);
            convertedHbmManyToMany.orderby.ShouldEqual(manyToManyMapping.OrderBy);
        }

        [Test]
        public void ShouldNotConvertOrderByIfNotPopulated()
        {
            var manyToManyMapping = new ManyToManyMapping();
            // Don't set anything on the original mapping
            var convertedHbmManyToMany = converter.Convert(manyToManyMapping);
            var blankHbmManyToMany = new HbmManyToMany();
            convertedHbmManyToMany.orderby.ShouldEqual(blankHbmManyToMany.orderby);
        }

        [Test]
        public void ShouldConvertChildPropertyRefIfPopulated()
        {
            // Actually mapped from ChildPropertyRef -> propertyref
            var manyToManyMapping = new ManyToManyMapping();
            manyToManyMapping.Set(fluent => fluent.ChildPropertyRef, Layer.Conventions, "childprop");
            var convertedHbmManyToMany = converter.Convert(manyToManyMapping);
            convertedHbmManyToMany.propertyref.ShouldEqual(manyToManyMapping.ChildPropertyRef);
        }

        [Test]
        public void ShouldNotConvertChildPropertyRefIfNotPopulated()
        {
            // Actually mapped from ChildPropertyRef -> propertyref
            var manyToManyMapping = new ManyToManyMapping();
            // Don't set anything on the original mapping
            var convertedHbmManyToMany = converter.Convert(manyToManyMapping);
            var blankHbmManyToMany = new HbmManyToMany();
            convertedHbmManyToMany.propertyref.ShouldEqual(blankHbmManyToMany.propertyref);
        }

        [Test]
        public void ShouldConvertColumns()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<ManyToManyMapping, ColumnMapping, HbmManyToMany, HbmColumn, object>(
                (manyToManyMapping, columnMapping) => manyToManyMapping.AddColumn(Layer.Conventions, columnMapping),
                hbmManyToMany => hbmManyToMany.Items);
        }
    }
}