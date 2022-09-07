using System;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.DomainModel;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;
using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmManyToOneConverterTester
    {
        private IHbmConverter<ManyToOneMapping, HbmManyToOne> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<ManyToOneMapping, HbmManyToOne>>();
        }

        [Test]
        public void ShouldConvertAccessIfPopulated()
        {
            var manyToOneMapping = new ManyToOneMapping();
            manyToOneMapping.Set(fluent => fluent.Access, Layer.Conventions, "access");
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            convertedHbmManyToOne.access.ShouldEqual(manyToOneMapping.Access);
        }

        [Test]
        public void ShouldNotConvertAccessIfNotPopulated()
        {
            var manyToOneMapping = new ManyToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            var blankHbmManyToOne = new HbmManyToOne();
            convertedHbmManyToOne.access.ShouldEqual(blankHbmManyToOne.access);
        }

        [Test]
        public void ShouldConvertCascadeIfPopulated()
        {
            var manyToOneMapping = new ManyToOneMapping();
            manyToOneMapping.Set(fluent => fluent.Cascade, Layer.Conventions, "all");
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            convertedHbmManyToOne.cascade.ShouldEqual(manyToOneMapping.Cascade);
        }

        [Test]
        public void ShouldNotConvertCascadeIfNotPopulated()
        {
            var manyToOneMapping = new ManyToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            var blankHbmManyToOne = new HbmManyToOne();
            convertedHbmManyToOne.cascade.ShouldEqual(blankHbmManyToOne.cascade);
        }

        [Test]
        public void ShouldConvertClassIfPopulated()
        {
            var manyToOneMapping = new ManyToOneMapping();
            manyToOneMapping.Set(fluent => fluent.Class, Layer.Conventions, new TypeReference(typeof(Record)));
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            convertedHbmManyToOne.@class.ShouldEqual(manyToOneMapping.Class.ToString());
        }

        [Test]
        public void ShouldNotConvertClassIfNotPopulated()
        {
            var manyToOneMapping = new ManyToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            var blankHbmManyToOne = new HbmManyToOne();
            convertedHbmManyToOne.@class.ShouldEqual(blankHbmManyToOne.@class);
        }

        [Test]
        public void ShouldConvertFetchIfPopulatedWithValidValue()
        {
            var fetch = HbmFetchMode.Join; // Defaults to Select, so use this to ensure that we can spot changes

            var manyToOneMapping = new ManyToOneMapping();
            var fetchDict = new XmlLinkedEnumBiDictionary<HbmFetchMode>();
            manyToOneMapping.Set(fluent => fluent.Fetch, Layer.Conventions, fetchDict[fetch]);
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            convertedHbmManyToOne.fetch.ShouldEqual(fetch);
            Assert.That(convertedHbmManyToOne.fetchSpecified.Equals(true), "Fetch was not marked as specified");
        }

        [Test]
        public void ShouldFailToConvertFetchIfPopulatedWithInvalidValue()
        {
            var manyToOneMapping = new ManyToOneMapping();
            manyToOneMapping.Set(fluent => fluent.Fetch, Layer.Conventions, "invalid_value");
            Assert.Throws<NotSupportedException>(() => converter.Convert(manyToOneMapping));
        }

        [Test]
        public void ShouldNotConvertFetchIfNotPopulated()
        {
            var manyToOneMapping = new ManyToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            var blankHbmManyToOne = new HbmManyToOne();
            convertedHbmManyToOne.fetch.ShouldEqual(blankHbmManyToOne.fetch);
            Assert.That(convertedHbmManyToOne.fetchSpecified.Equals(false), "Fetch was marked as specified");
        }

        [Test]
        public void ShouldConvertForeignKeyIfPopulated()
        {
            var manyToOneMapping = new ManyToOneMapping();
            manyToOneMapping.Set(fluent => fluent.ForeignKey, Layer.Conventions, "fk");
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            convertedHbmManyToOne.foreignkey.ShouldEqual(manyToOneMapping.ForeignKey);
        }

        [Test]
        public void ShouldNotConvertForeignKeyIfNotPopulated()
        {
            var manyToOneMapping = new ManyToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            var blankHbmManyToOne = new HbmManyToOne();
            convertedHbmManyToOne.foreignkey.ShouldEqual(blankHbmManyToOne.foreignkey);
        }

        [Test]
        public void ShouldConvertInsertIfPopulated()
        {
            var manyToOneMapping = new ManyToOneMapping();
            manyToOneMapping.Set(fluent => fluent.Insert, Layer.Conventions, false); // Defaults to true, so use this to ensure that we can detect changes
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            convertedHbmManyToOne.insert.ShouldEqual(manyToOneMapping.Insert);
        }

        [Test]
        public void ShouldNotConvertInsertIfNotPopulated()
        {
            var manyToOneMapping = new ManyToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            var blankHbmManyToOne = new HbmManyToOne();
            convertedHbmManyToOne.insert.ShouldEqual(blankHbmManyToOne.insert);
        }

        [Test]
        public void ShouldConvertLazyIfPopulatedWithValidValue()
        {
            var lazy = HbmLaziness.Proxy; // Defaults to False, so use this to ensure that we can detect changes

            var manyToOneMapping = new ManyToOneMapping();
            var lazyDict = new XmlLinkedEnumBiDictionary<HbmLaziness>();
            manyToOneMapping.Set(fluent => fluent.Lazy, Layer.Conventions, lazyDict[lazy]);
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            convertedHbmManyToOne.lazy.ShouldEqual(lazy);
            Assert.That(convertedHbmManyToOne.lazySpecified.Equals(true), "Lazy was not marked as specified");
        }

        [Test]
        public void ShouldFailToConvertLazyIfPopulatedWithInvalidValue()
        {
            var manyToOneMapping = new ManyToOneMapping();
            manyToOneMapping.Set(fluent => fluent.Lazy, Layer.Conventions, "invalid_value");
            Assert.Throws<NotSupportedException>(() => converter.Convert(manyToOneMapping));
        }

        [Test]
        public void ShouldNotConvertLazyIfNotPopulated()
        {
            var manyToOneMapping = new ManyToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            var blankHbmManyToOne = new HbmManyToOne();
            convertedHbmManyToOne.lazy.ShouldEqual(blankHbmManyToOne.lazy);
            Assert.That(convertedHbmManyToOne.lazySpecified.Equals(false), "Lazy was marked as specified");
        }

        [Test]
        public void ShouldConvertNameIfPopulated()
        {
            var manyToOneMapping = new ManyToOneMapping();
            manyToOneMapping.Set(fluent => fluent.Name, Layer.Conventions, "nm");
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            convertedHbmManyToOne.name.ShouldEqual(manyToOneMapping.Name);
        }

        [Test]
        public void ShouldNotConvertNameIfNotPopulated()
        {
            var manyToOneMapping = new ManyToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            var blankHbmManyToOne = new HbmManyToOne();
            convertedHbmManyToOne.name.ShouldEqual(blankHbmManyToOne.name);
        }

        [Test]
        public void ShouldConvertNotFoundIfPopulatedWithValidValue()
        {
            var notFound = HbmNotFoundMode.Ignore; // Defaults to Exception, so use this to ensure that we can detect changes

            var manyToOneMapping = new ManyToOneMapping();
            var notFoundDict = new XmlLinkedEnumBiDictionary<HbmNotFoundMode>();
            manyToOneMapping.Set(fluent => fluent.NotFound, Layer.Conventions, notFoundDict[notFound]);
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            convertedHbmManyToOne.notfound.ShouldEqual(notFound);
        }

        [Test]
        public void ShouldFailToConvertNotFoundIfPopulatedWithInvalidValue()
        {
            var manyToOneMapping = new ManyToOneMapping();
            manyToOneMapping.Set(fluent => fluent.NotFound, Layer.Conventions, "invalid_value");
            Assert.Throws<NotSupportedException>(() => converter.Convert(manyToOneMapping));
        }

        [Test]
        public void ShouldNotConvertNotFoundIfNotPopulated()
        {
            var manyToOneMapping = new ManyToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            var blankHbmManyToOne = new HbmManyToOne();
            convertedHbmManyToOne.notfound.ShouldEqual(blankHbmManyToOne.notfound);
        }

        [Test]
        public void ShouldConvertPropertyRefIfPopulated()
        {
            var manyToOneMapping = new ManyToOneMapping();
            manyToOneMapping.Set(fluent => fluent.PropertyRef, Layer.Conventions, "pr");
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            convertedHbmManyToOne.propertyref.ShouldEqual(manyToOneMapping.PropertyRef);
        }

        [Test]
        public void ShouldNotConvertPropertyRefIfNotPopulated()
        {
            var manyToOneMapping = new ManyToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            var blankHbmManyToOne = new HbmManyToOne();
            convertedHbmManyToOne.propertyref.ShouldEqual(blankHbmManyToOne.propertyref);
        }

        [Test]
        public void ShouldConvertUpdateIfPopulated()
        {
            var manyToOneMapping = new ManyToOneMapping();
            manyToOneMapping.Set(fluent => fluent.Update, Layer.Conventions, false); // Defaults to true, so use this to ensure that we can detect changes
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            convertedHbmManyToOne.update.ShouldEqual(manyToOneMapping.Update);
        }

        [Test]
        public void ShouldNotConvertUpdateIfNotPopulated()
        {
            var manyToOneMapping = new ManyToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            var blankHbmManyToOne = new HbmManyToOne();
            convertedHbmManyToOne.update.ShouldEqual(blankHbmManyToOne.update);
        }

        [Test]
        public void ShouldConvertEntityNameIfPopulated()
        {
            var manyToOneMapping = new ManyToOneMapping();
            manyToOneMapping.Set(fluent => fluent.EntityName, Layer.Conventions, "name1");
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            convertedHbmManyToOne.entityname.ShouldEqual(manyToOneMapping.EntityName);
        }

        [Test]
        public void ShouldNotConvertEntityNameIfNotPopulated()
        {
            var manyToOneMapping = new ManyToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            var blankHbmManyToOne = new HbmManyToOne();
            convertedHbmManyToOne.entityname.ShouldEqual(blankHbmManyToOne.entityname);
        }

        [Test]
        public void ShouldConvertFormulaIfPopulated()
        {
            var manyToOneMapping = new ManyToOneMapping();
            manyToOneMapping.Set(fluent => fluent.Formula, Layer.Conventions, "form");
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            convertedHbmManyToOne.formula.ShouldEqual(manyToOneMapping.Formula);
        }

        [Test]
        public void ShouldNotConvertFormulaIfNotPopulated()
        {
            var manyToOneMapping = new ManyToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            var blankHbmManyToOne = new HbmManyToOne();
            convertedHbmManyToOne.formula.ShouldEqual(blankHbmManyToOne.formula);
        }

        [Test]
        public void ShouldConvertOptimisticLockIfPopulated()
        {
            var manyToOneMapping = new ManyToOneMapping();
            manyToOneMapping.Set(fluent => fluent.OptimisticLock, Layer.Conventions, false); // Defaults to true, so use this to ensure that we can detect changes
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            convertedHbmManyToOne.optimisticlock.ShouldEqual(manyToOneMapping.OptimisticLock);
        }

        [Test]
        public void ShouldNotConvertOptimisticLockIfNotPopulated()
        {
            var manyToOneMapping = new ManyToOneMapping();
            // Don't set anything on the original mapping
            var convertedHbmManyToOne = converter.Convert(manyToOneMapping);
            var blankHbmManyToOne = new HbmManyToOne();
            convertedHbmManyToOne.optimisticlock.ShouldEqual(blankHbmManyToOne.optimisticlock);
        }

        [Test]
        public void ShouldConvertColumns()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<ManyToOneMapping, ColumnMapping, HbmManyToOne, HbmColumn, object>(
                (manyToOneMapping, columnMapping) => manyToOneMapping.AddColumn(Layer.Conventions, columnMapping),
                hbmManyToOne => hbmManyToOne.Items);
        }
    }
}