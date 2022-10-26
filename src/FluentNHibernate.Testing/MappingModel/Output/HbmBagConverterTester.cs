using System;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmBagConverterTester
    {
        private IHbmConverter<CollectionMapping, HbmBag> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<CollectionMapping, HbmBag>>();
        }

        #region Base collection attribute value field tests

        [Test]
        public void ShouldConvertAccessIfPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            bagMapping.Set(fluent => fluent.Access, Layer.Conventions, "acc");
            var convertedHbmBag = converter.Convert(bagMapping);
            convertedHbmBag.access.ShouldEqual(bagMapping.Access);
        }

        [Test]
        public void ShouldNotConvertAccessIfNotPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            // Don't set anything on the original mapping
            var convertedHbmBag = converter.Convert(bagMapping);
            var blankHbmBag = new HbmBag();
            convertedHbmBag.access.ShouldEqual(blankHbmBag.access);
        }

        [Test]
        public void ShouldConvertBatchSizeIfPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            bagMapping.Set(fluent => fluent.BatchSize, Layer.Conventions, 10);
            var convertedHbmBag = converter.Convert(bagMapping);
            convertedHbmBag.batchsize.ShouldEqual(bagMapping.BatchSize);
            Assert.That(convertedHbmBag.batchsizeSpecified.Equals(true), "Batch size was not marked as specified");
        }

        [Test]
        public void ShouldNotConvertBatchSizeIfNotPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            // Don't set anything on the original mapping
            var convertedHbmBag = converter.Convert(bagMapping);
            var blankHbmBag = new HbmBag();
            convertedHbmBag.batchsize.ShouldEqual(blankHbmBag.batchsize);
            Assert.That(convertedHbmBag.batchsizeSpecified.Equals(false), "Batch size was marked as specified");
        }

        [Test]
        public void ShouldConvertCascadeIfPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            bagMapping.Set(fluent => fluent.Cascade, Layer.Conventions, "all");
            var convertedHbmBag = converter.Convert(bagMapping);
            convertedHbmBag.cascade.ShouldEqual(bagMapping.Cascade);
        }

        [Test]
        public void ShouldNotConvertCascadeIfNotPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            // Don't set anything on the original mapping
            var convertedHbmBag = converter.Convert(bagMapping);
            var blankHbmBag = new HbmBag();
            convertedHbmBag.cascade.ShouldEqual(blankHbmBag.cascade);
        }

        [Test]
        public void ShouldConvertCheckIfPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            bagMapping.Set(fluent => fluent.Check, Layer.Conventions, "chk");
            var convertedHbmBag = converter.Convert(bagMapping);
            convertedHbmBag.check.ShouldEqual(bagMapping.Check);
        }

        [Test]
        public void ShouldNotConvertCheckIfNotPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            // Don't set anything on the original mapping
            var convertedHbmBag = converter.Convert(bagMapping);
            var blankHbmBag = new HbmBag();
            convertedHbmBag.check.ShouldEqual(blankHbmBag.check);
        }

        [Test]
        public void ShouldConvertCollectionTypeIfPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            bagMapping.Set(fluent => fluent.CollectionType, Layer.Conventions, new TypeReference("type"));
            var convertedHbmBag = converter.Convert(bagMapping);
            convertedHbmBag.collectiontype.ShouldEqual(bagMapping.CollectionType.ToString());
        }

        [Test]
        public void ShouldNotConvertCollectionTypeIfEmpty()
        {
            var bagMapping = CollectionMapping.Bag();
            // Set an explicitly empty type reference
            bagMapping.Set(fluent => fluent.CollectionType, Layer.Conventions, TypeReference.Empty);
            var convertedHbmBag = converter.Convert(bagMapping);
            var blankHbmBag = new HbmBag();
            convertedHbmBag.collectiontype.ShouldEqual(blankHbmBag.collectiontype);
        }

        [Test]
        public void ShouldNotConvertCollectionTypeIfNotPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            // Don't set anything on the original mapping
            var convertedHbmBag = converter.Convert(bagMapping);
            var blankHbmBag = new HbmBag();
            convertedHbmBag.collectiontype.ShouldEqual(blankHbmBag.collectiontype);
        }

        [Test]
        public void ShouldConvertFetchIfPopulatedWithValidValue()
        {
            var fetch = HbmCollectionFetchMode.Subselect; // Defaults to Select, so use something else to properly detect that it changes

            var bagMapping = CollectionMapping.Bag();
            var fetchDict = new XmlLinkedEnumBiDictionary<HbmCollectionFetchMode>();
            bagMapping.Set(fluent => fluent.Fetch, Layer.Conventions, fetchDict[fetch]);
            var convertedHbmBag = converter.Convert(bagMapping);
            convertedHbmBag.fetch.ShouldEqual(fetch);
            Assert.That(convertedHbmBag.fetchSpecified.Equals(true), "Fetch was not marked as specified");
        }

        [Test]
        public void ShouldFailToConvertFetchIfPopulatedWithInvalidValue()
        {
            var bagMapping = CollectionMapping.Bag();
            bagMapping.Set(fluent => fluent.Fetch, Layer.Conventions, "invalid_value");
            Assert.Throws<NotSupportedException>(() => converter.Convert(bagMapping));
        }

        [Test]
        public void ShouldNotConvertFetchIfNotPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            // Don't set anything on the original mapping
            var convertedHbmBag = converter.Convert(bagMapping);
            var blankHbmBag = new HbmBag();
            convertedHbmBag.fetch.ShouldEqual(blankHbmBag.fetch);
            Assert.That(convertedHbmBag.fetchSpecified.Equals(false), "Fetch was marked as specified");
        }

        [Test]
        public void ShouldConvertGenericIfPopulated_True()
        {
            var bagMapping = CollectionMapping.Bag();
            bagMapping.Set(fluent => fluent.Generic, Layer.Conventions, true);
            var convertedHbmBag = converter.Convert(bagMapping);
            convertedHbmBag.generic.ShouldEqual(bagMapping.Generic);
            Assert.That(convertedHbmBag.genericSpecified.Equals(true), "Generic was not marked as specified");
        }

        [Test]
        public void ShouldConvertGenericIfPopulated_False()
        {
            var bagMapping = CollectionMapping.Bag();
            bagMapping.Set(fluent => fluent.Generic, Layer.Conventions, false);
            var convertedHbmBag = converter.Convert(bagMapping);
            convertedHbmBag.generic.ShouldEqual(bagMapping.Generic);
            Assert.That(convertedHbmBag.genericSpecified.Equals(true), "Generic was not marked as specified");
        }

        [Test]
        public void ShouldNotConvertGenericIfNotPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            // Don't set anything on the original mapping
            var convertedHbmBag = converter.Convert(bagMapping);
            var blankHbmBag = new HbmBag();
            convertedHbmBag.generic.ShouldEqual(blankHbmBag.generic);
            Assert.That(convertedHbmBag.genericSpecified.Equals(false), "Generic was marked as specified");
        }

        [Test]
        public void ShouldConvertInverseIfPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            bagMapping.Set(fluent => fluent.Inverse, Layer.Conventions, true); // Defaults to false, so use this to ensure that we can detect changes
            var convertedHbmBag = converter.Convert(bagMapping);
            convertedHbmBag.inverse.ShouldEqual(bagMapping.Inverse);
        }

        [Test]
        public void ShouldNotConvertInverseIfNotPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            // Don't set anything on the original mapping
            var convertedHbmBag = converter.Convert(bagMapping);
            var blankHbmBag = new HbmBag();
            convertedHbmBag.inverse.ShouldEqual(blankHbmBag.inverse);
        }

        [Test]
        public void ShouldConvertLazyIfPopulated()
        {
            var hbmLazy = HbmCollectionLazy.False; // Defaults to True, so use something else to properly detect that it changes

            var bagMapping = CollectionMapping.Bag();
            bagMapping.Set(fluent => fluent.Lazy, Layer.Conventions, HbmCollectionConverter.FluentHbmLazyBiDict[hbmLazy]);
            var convertedHbmBag = converter.Convert(bagMapping);
            convertedHbmBag.lazy.ShouldEqual(hbmLazy);
            Assert.That(convertedHbmBag.lazySpecified.Equals(true), "Lazy was not marked as specified");
        }

        // Since it is enum-based, Lazy cannot contain any invalid values, so no need to test for that here

        [Test]
        public void ShouldNotConvertLazyIfNotPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            // Don't set anything on the original mapping
            var convertedHbmBag = converter.Convert(bagMapping);
            var blankHbmBag = new HbmBag();
            convertedHbmBag.lazy.ShouldEqual(blankHbmBag.lazy);
            Assert.That(convertedHbmBag.lazySpecified.Equals(false), "Lazy was marked as specified");
        }

        [Test]
        public void ShouldConvertNameIfPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            bagMapping.Set(fluent => fluent.Name, Layer.Conventions, "name");
            var convertedHbmBag = converter.Convert(bagMapping);
            convertedHbmBag.name.ShouldEqual(bagMapping.Name);
        }

        [Test]
        public void ShouldNotConvertNameIfNotPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            // Don't set anything on the original mapping
            var convertedHbmBag = converter.Convert(bagMapping);
            var blankHbmBag = new HbmBag();
            convertedHbmBag.name.ShouldEqual(blankHbmBag.name);
        }

        [Test]
        public void ShouldConvertOptimisticLockIfPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            bagMapping.Set(fluent => fluent.OptimisticLock, Layer.Conventions, false); // Defaults to true, so use this to ensure that we can detect changes
            var convertedHbmBag = converter.Convert(bagMapping);
            convertedHbmBag.optimisticlock.ShouldEqual(bagMapping.OptimisticLock);
        }

        [Test]
        public void ShouldNotConvertOptimisticLockIfNotPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            // Don't set anything on the original mapping
            var convertedHbmBag = converter.Convert(bagMapping);
            var blankHbmBag = new HbmBag();
            convertedHbmBag.optimisticlock.ShouldEqual(blankHbmBag.optimisticlock);
        }

        [Test]
        public void ShouldConvertPersisterIfPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            bagMapping.Set(fluent => fluent.Persister, Layer.Conventions, new TypeReference(typeof(string)));
            var convertedHbmBag = converter.Convert(bagMapping);
            convertedHbmBag.persister.ShouldEqual(bagMapping.Persister.ToString());
        }

        [Test]
        public void ShouldNotConvertPersisterIfNotPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            // Don't set anything on the original mapping
            var convertedHbmBag = converter.Convert(bagMapping);
            var blankHbmBag = new HbmBag();
            convertedHbmBag.persister.ShouldEqual(blankHbmBag.persister);
        }

        [Test]
        public void ShouldConvertSchemaIfPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            bagMapping.Set(fluent => fluent.Schema, Layer.Conventions, "dbo");
            var convertedHbmBag = converter.Convert(bagMapping);
            convertedHbmBag.schema.ShouldEqual(bagMapping.Schema);
        }

        [Test]
        public void ShouldNotConvertSchemaIfNotPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            // Don't set anything on the original mapping
            var convertedHbmBag = converter.Convert(bagMapping);
            var blankHbmBag = new HbmBag();
            convertedHbmBag.schema.ShouldEqual(blankHbmBag.schema);
        }

        [Test]
        public void ShouldConvertTableNameIfPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            bagMapping.Set(fluent => fluent.TableName, Layer.Conventions, "tbl");
            var convertedHbmBag = converter.Convert(bagMapping);
            convertedHbmBag.table.ShouldEqual(bagMapping.TableName);
        }

        [Test]
        public void ShouldNotConvertTableNameIfNotPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            // Don't set anything on the original mapping
            var convertedHbmBag = converter.Convert(bagMapping);
            var blankHbmBag = new HbmBag();
            convertedHbmBag.table.ShouldEqual(blankHbmBag.table);
        }

        [Test]
        public void ShouldConvertWhereIfPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            bagMapping.Set(fluent => fluent.Where, Layer.Conventions, "x = 1");
            var convertedHbmBag = converter.Convert(bagMapping);
            convertedHbmBag.where.ShouldEqual(bagMapping.Where);
        }

        [Test]
        public void ShouldNotConvertWhereIfNotPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            // Don't set anything on the original mapping
            var convertedHbmBag = converter.Convert(bagMapping);
            var blankHbmBag = new HbmBag();
            convertedHbmBag.where.ShouldEqual(blankHbmBag.where);
        }

        [Test]
        public void ShouldConvertSubselectIfPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            bagMapping.Set(fluent => fluent.Subselect, Layer.Conventions, "val");
            var convertedHbmBag = converter.Convert(bagMapping);
            convertedHbmBag.subselect.Text.ShouldEqual(new string[] { bagMapping.Subselect });
        }

        [Test]
        public void ShouldNotConvertSubselectIfNotPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            // Don't set anything on the original mapping
            var convertedHbmBag = converter.Convert(bagMapping);
            var blankHbmBag = new HbmBag();
            convertedHbmBag.subselect.ShouldEqual(blankHbmBag.subselect);
        }

        [Test]
        public void ShouldConvertMutableIfPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            bagMapping.Set(fluent => fluent.Mutable, Layer.Conventions, false); // Defaults to true, so use this to ensure that we can detect changes
            var convertedHbmBag = converter.Convert(bagMapping);
            convertedHbmBag.mutable.ShouldEqual(bagMapping.Mutable);
        }

        [Test]
        public void ShouldNotConvertMutableIfNotPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            // Don't set anything on the original mapping
            var convertedHbmBag = converter.Convert(bagMapping);
            var blankHbmBag = new HbmBag();
            convertedHbmBag.mutable.ShouldEqual(blankHbmBag.mutable);
        }

        #endregion Base collection attribute value field tests

        #region Type-specific collection attribute value field tests

        [Test]
        public void ShouldConvertOrderByIfPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            bagMapping.Set(fluent => fluent.OrderBy, Layer.Conventions, "ord");
            var convertedHbmBag = converter.Convert(bagMapping);
            convertedHbmBag.orderby.ShouldEqual(bagMapping.OrderBy);
        }

        [Test]
        public void ShouldNotConvertOrderByIfNotPopulated()
        {
            var bagMapping = CollectionMapping.Bag();
            // Don't set anything on the original mapping
            var convertedHbmBag = converter.Convert(bagMapping);
            var blankHbmBag = new HbmBag();
            convertedHbmBag.orderby.ShouldEqual(blankHbmBag.orderby);
        }

        #endregion Type-specific collection attribute value field tests

        #region Base collection converter-based subobject tests

        [Test]
        public void ShouldConvertKey()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<CollectionMapping, KeyMapping, HbmBag, HbmKey>(
                () => CollectionMapping.Bag(),
                (bagMapping, keyMapping) => bagMapping.Set(fluent => fluent.Key, Layer.Defaults, keyMapping),
                hbmBag => hbmBag.key);
        }

        [Test]
        public void ShouldConvertICollectionRelationship_OneToMany()
        {
            ShouldConvertSubobjectAsLooselyTypedField<CollectionMapping, ICollectionRelationshipMapping, HbmBag, object, object>(
                () => CollectionMapping.Bag(),
                () => new OneToManyMapping(),
                (bagMapping, icrMapping) => bagMapping.Set(fluent => fluent.Relationship, Layer.Defaults, icrMapping),
                hbmBag => hbmBag.Item);
        }

        [Test]
        public void ShouldConvertICollectionRelationship_ManyToMany()
        {
            ShouldConvertSubobjectAsLooselyTypedField<CollectionMapping, ICollectionRelationshipMapping, HbmBag, object, object>(
                () => CollectionMapping.Bag(),
                () => new ManyToManyMapping(),
                (bagMapping, icrMapping) => bagMapping.Set(fluent => fluent.Relationship, Layer.Defaults, icrMapping),
                hbmBag => hbmBag.Item);
        }

        [Test]
        public void ShouldConvertCache()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<CollectionMapping, CacheMapping, HbmBag, HbmCache>(
                () => CollectionMapping.Bag(),
                (bagMapping, cacheMapping) => bagMapping.Set(fluent => fluent.Cache, Layer.Defaults, cacheMapping),
                hbmBag => hbmBag.cache);
        }

        [Test]
        public void ShouldConvertCompositeElement()
        {
            ShouldConvertSubobjectAsLooselyTypedField<CollectionMapping, CompositeElementMapping, HbmBag, HbmCompositeElement, object>(
                () => CollectionMapping.Bag(),
                (bagMapping, compositeElementMapping) => bagMapping.Set(fluent => fluent.CompositeElement, Layer.Defaults, compositeElementMapping),
                hbmBag => hbmBag.Item);
        }

        [Test]
        public void ShouldConvertElement()
        {
            ShouldConvertSubobjectAsLooselyTypedField<CollectionMapping, ElementMapping, HbmBag, HbmElement, object>(
                () => CollectionMapping.Bag(),
                (bagMapping, elementMapping) => bagMapping.Set(fluent => fluent.Element, Layer.Defaults, elementMapping),
                hbmBag => hbmBag.Item);
        }

        [Test]
        public void ShouldConvertFilters()
        {
            ShouldConvertSubobjectsAsStrictlyTypedArray<CollectionMapping, FilterMapping, HbmBag, HbmFilter>(
                () => CollectionMapping.Bag(),
                (bagMapping, filterMapping) => bagMapping.AddFilter(filterMapping),
                hbmBag => hbmBag.filter);
        }

        #endregion Base collection converter-based subobject tests

        #region Type-specific collection converter-based subobject tests

        // No tests for this type

        #endregion Type-specific collection converter-based subobject tests
    }
}
