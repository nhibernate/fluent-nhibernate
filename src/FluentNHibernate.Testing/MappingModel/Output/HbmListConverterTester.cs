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
    public class HbmListConverterTester
    {
        private IHbmConverter<CollectionMapping, HbmList> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<CollectionMapping, HbmList>>();
        }

        #region Base collection attribute value field tests

        [Test]
        public void ShouldConvertAccessIfPopulated()
        {
            var listMapping = CollectionMapping.List();
            listMapping.Set(fluent => fluent.Access, Layer.Conventions, "acc");
            var convertedHbmList = converter.Convert(listMapping);
            convertedHbmList.access.ShouldEqual(listMapping.Access);
        }

        [Test]
        public void ShouldNotConvertAccessIfNotPopulated()
        {
            var listMapping = CollectionMapping.List();
            // Don't list anything on the original mapping
            var convertedHbmList = converter.Convert(listMapping);
            var blankHbmList = new HbmList();
            convertedHbmList.access.ShouldEqual(blankHbmList.access);
        }

        [Test]
        public void ShouldConvertBatchSizeIfPopulated()
        {
            var listMapping = CollectionMapping.List();
            listMapping.Set(fluent => fluent.BatchSize, Layer.Conventions, 10);
            var convertedHbmList = converter.Convert(listMapping);
            convertedHbmList.batchsize.ShouldEqual(listMapping.BatchSize);
            Assert.That(convertedHbmList.batchsizeSpecified.Equals(true), "Batch size was not marked as specified");
        }

        [Test]
        public void ShouldNotConvertBatchSizeIfNotPopulated()
        {
            var listMapping = CollectionMapping.List();
            // Don't list anything on the original mapping
            var convertedHbmList = converter.Convert(listMapping);
            var blankHbmList = new HbmList();
            convertedHbmList.batchsize.ShouldEqual(blankHbmList.batchsize);
            Assert.That(convertedHbmList.batchsizeSpecified.Equals(false), "Batch size was marked as specified");
        }

        [Test]
        public void ShouldConvertCascadeIfPopulated()
        {
            var listMapping = CollectionMapping.List();
            listMapping.Set(fluent => fluent.Cascade, Layer.Conventions, "all");
            var convertedHbmList = converter.Convert(listMapping);
            convertedHbmList.cascade.ShouldEqual(listMapping.Cascade);
        }

        [Test]
        public void ShouldNotConvertCascadeIfNotPopulated()
        {
            var listMapping = CollectionMapping.List();
            // Don't list anything on the original mapping
            var convertedHbmList = converter.Convert(listMapping);
            var blankHbmList = new HbmList();
            convertedHbmList.cascade.ShouldEqual(blankHbmList.cascade);
        }

        [Test]
        public void ShouldConvertCheckIfPopulated()
        {
            var listMapping = CollectionMapping.List();
            listMapping.Set(fluent => fluent.Check, Layer.Conventions, "chk");
            var convertedHbmList = converter.Convert(listMapping);
            convertedHbmList.check.ShouldEqual(listMapping.Check);
        }

        [Test]
        public void ShouldNotConvertCheckIfNotPopulated()
        {
            var listMapping = CollectionMapping.List();
            // Don't list anything on the original mapping
            var convertedHbmList = converter.Convert(listMapping);
            var blankHbmList = new HbmList();
            convertedHbmList.check.ShouldEqual(blankHbmList.check);
        }

        [Test]
        public void ShouldConvertCollectionTypeIfPopulated()
        {
            var listMapping = CollectionMapping.List();
            listMapping.Set(fluent => fluent.CollectionType, Layer.Conventions, new TypeReference("type"));
            var convertedHbmList = converter.Convert(listMapping);
            convertedHbmList.collectiontype.ShouldEqual(listMapping.CollectionType.ToString());
        }

        [Test]
        public void ShouldNotConvertCollectionTypeIfEmpty()
        {
            var listMapping = CollectionMapping.List();
            // List an explicitly empty type reference
            listMapping.Set(fluent => fluent.CollectionType, Layer.Conventions, TypeReference.Empty);
            var convertedHbmList = converter.Convert(listMapping);
            var blankHbmList = new HbmList();
            convertedHbmList.collectiontype.ShouldEqual(blankHbmList.collectiontype);
        }

        [Test]
        public void ShouldNotConvertCollectionTypeIfNotPopulated()
        {
            var listMapping = CollectionMapping.List();
            // Don't list anything on the original mapping
            var convertedHbmList = converter.Convert(listMapping);
            var blankHbmList = new HbmList();
            convertedHbmList.collectiontype.ShouldEqual(blankHbmList.collectiontype);
        }

        [Test]
        public void ShouldConvertFetchIfPopulatedWithValidValue()
        {
            var fetch = HbmCollectionFetchMode.Subselect; // Defaults to Select, so use something else to properly detect that it changes

            var listMapping = CollectionMapping.List();
            var fetchDict = new XmlLinkedEnumBiDictionary<HbmCollectionFetchMode>();
            listMapping.Set(fluent => fluent.Fetch, Layer.Conventions, fetchDict[fetch]);
            var convertedHbmList = converter.Convert(listMapping);
            convertedHbmList.fetch.ShouldEqual(fetch);
            Assert.That(convertedHbmList.fetchSpecified.Equals(true), "Fetch was not marked as specified");
        }

        [Test]
        public void ShouldFailToConvertFetchIfPopulatedWithInvalidValue()
        {
            var listMapping = CollectionMapping.List();
            listMapping.Set(fluent => fluent.Fetch, Layer.Conventions, "invalid_value");
            Assert.Throws<NotSupportedException>(() => converter.Convert(listMapping));
        }

        [Test]
        public void ShouldNotConvertFetchIfNotPopulated()
        {
            var listMapping = CollectionMapping.List();
            // Don't list anything on the original mapping
            var convertedHbmList = converter.Convert(listMapping);
            var blankHbmList = new HbmList();
            convertedHbmList.fetch.ShouldEqual(blankHbmList.fetch);
            Assert.That(convertedHbmList.fetchSpecified.Equals(false), "Fetch was marked as specified");
        }

        [Test]
        public void ShouldConvertGenericIfPopulated_True()
        {
            var listMapping = CollectionMapping.List();
            listMapping.Set(fluent => fluent.Generic, Layer.Conventions, true);
            var convertedHbmList = converter.Convert(listMapping);
            convertedHbmList.generic.ShouldEqual(listMapping.Generic);
            Assert.That(convertedHbmList.genericSpecified.Equals(true), "Generic was not marked as specified");
        }

        [Test]
        public void ShouldConvertGenericIfPopulated_False()
        {
            var listMapping = CollectionMapping.List();
            listMapping.Set(fluent => fluent.Generic, Layer.Conventions, false);
            var convertedHbmList = converter.Convert(listMapping);
            convertedHbmList.generic.ShouldEqual(listMapping.Generic);
            Assert.That(convertedHbmList.genericSpecified.Equals(true), "Generic was not marked as specified");
        }

        [Test]
        public void ShouldNotConvertGenericIfNotPopulated()
        {
            var listMapping = CollectionMapping.List();
            // Don't list anything on the original mapping
            var convertedHbmList = converter.Convert(listMapping);
            var blankHbmList = new HbmList();
            convertedHbmList.generic.ShouldEqual(blankHbmList.generic);
            Assert.That(convertedHbmList.genericSpecified.Equals(false), "Generic was marked as specified");
        }

        [Test]
        public void ShouldConvertInverseIfPopulated()
        {
            var listMapping = CollectionMapping.List();
            listMapping.Set(fluent => fluent.Inverse, Layer.Conventions, true); // Defaults to false, so use this to ensure that we can detect changes
            var convertedHbmList = converter.Convert(listMapping);
            convertedHbmList.inverse.ShouldEqual(listMapping.Inverse);
        }

        [Test]
        public void ShouldNotConvertInverseIfNotPopulated()
        {
            var listMapping = CollectionMapping.List();
            // Don't list anything on the original mapping
            var convertedHbmList = converter.Convert(listMapping);
            var blankHbmList = new HbmList();
            convertedHbmList.inverse.ShouldEqual(blankHbmList.inverse);
        }

        [Test]
        public void ShouldConvertLazyIfPopulated()
        {
            var hbmLazy = HbmCollectionLazy.False; // Defaults to True, so use something else to properly detect that it changes

            var listMapping = CollectionMapping.List();
            listMapping.Set(fluent => fluent.Lazy, Layer.Conventions, HbmCollectionConverter.FluentHbmLazyBiDict[hbmLazy]);
            var convertedHbmList = converter.Convert(listMapping);
            convertedHbmList.lazy.ShouldEqual(hbmLazy);
            Assert.That(convertedHbmList.lazySpecified.Equals(true), "Lazy was not marked as specified");
        }

        // Since it is enum-based, Lazy cannot contain any invalid values, so no need to test for that here

        [Test]
        public void ShouldNotConvertLazyIfNotPopulated()
        {
            var listMapping = CollectionMapping.List();
            // Don't list anything on the original mapping
            var convertedHbmList = converter.Convert(listMapping);
            var blankHbmList = new HbmList();
            convertedHbmList.lazy.ShouldEqual(blankHbmList.lazy);
            Assert.That(convertedHbmList.lazySpecified.Equals(false), "Lazy was marked as specified");
        }

        [Test]
        public void ShouldConvertNameIfPopulated()
        {
            var listMapping = CollectionMapping.List();
            listMapping.Set(fluent => fluent.Name, Layer.Conventions, "name");
            var convertedHbmList = converter.Convert(listMapping);
            convertedHbmList.name.ShouldEqual(listMapping.Name);
        }

        [Test]
        public void ShouldNotConvertNameIfNotPopulated()
        {
            var listMapping = CollectionMapping.List();
            // Don't list anything on the original mapping
            var convertedHbmList = converter.Convert(listMapping);
            var blankHbmList = new HbmList();
            convertedHbmList.name.ShouldEqual(blankHbmList.name);
        }

        [Test]
        public void ShouldConvertOptimisticLockIfPopulated()
        {
            var listMapping = CollectionMapping.List();
            listMapping.Set(fluent => fluent.OptimisticLock, Layer.Conventions, false); // Defaults to true, so use this to ensure that we can detect changes
            var convertedHbmList = converter.Convert(listMapping);
            convertedHbmList.optimisticlock.ShouldEqual(listMapping.OptimisticLock);
        }

        [Test]
        public void ShouldNotConvertOptimisticLockIfNotPopulated()
        {
            var listMapping = CollectionMapping.List();
            // Don't list anything on the original mapping
            var convertedHbmList = converter.Convert(listMapping);
            var blankHbmList = new HbmList();
            convertedHbmList.optimisticlock.ShouldEqual(blankHbmList.optimisticlock);
        }

        [Test]
        public void ShouldConvertPersisterIfPopulated()
        {
            var listMapping = CollectionMapping.List();
            listMapping.Set(fluent => fluent.Persister, Layer.Conventions, new TypeReference(typeof(string)));
            var convertedHbmList = converter.Convert(listMapping);
            convertedHbmList.persister.ShouldEqual(listMapping.Persister.ToString());
        }

        [Test]
        public void ShouldNotConvertPersisterIfNotPopulated()
        {
            var listMapping = CollectionMapping.List();
            // Don't list anything on the original mapping
            var convertedHbmList = converter.Convert(listMapping);
            var blankHbmList = new HbmList();
            convertedHbmList.persister.ShouldEqual(blankHbmList.persister);
        }

        [Test]
        public void ShouldConvertSchemaIfPopulated()
        {
            var listMapping = CollectionMapping.List();
            listMapping.Set(fluent => fluent.Schema, Layer.Conventions, "dbo");
            var convertedHbmList = converter.Convert(listMapping);
            convertedHbmList.schema.ShouldEqual(listMapping.Schema);
        }

        [Test]
        public void ShouldNotConvertSchemaIfNotPopulated()
        {
            var listMapping = CollectionMapping.List();
            // Don't list anything on the original mapping
            var convertedHbmList = converter.Convert(listMapping);
            var blankHbmList = new HbmList();
            convertedHbmList.schema.ShouldEqual(blankHbmList.schema);
        }

        [Test]
        public void ShouldConvertTableNameIfPopulated()
        {
            var listMapping = CollectionMapping.List();
            listMapping.Set(fluent => fluent.TableName, Layer.Conventions, "tbl");
            var convertedHbmList = converter.Convert(listMapping);
            convertedHbmList.table.ShouldEqual(listMapping.TableName);
        }

        [Test]
        public void ShouldNotConvertTableNameIfNotPopulated()
        {
            var listMapping = CollectionMapping.List();
            // Don't list anything on the original mapping
            var convertedHbmList = converter.Convert(listMapping);
            var blankHbmList = new HbmList();
            convertedHbmList.table.ShouldEqual(blankHbmList.table);
        }

        [Test]
        public void ShouldConvertWhereIfPopulated()
        {
            var listMapping = CollectionMapping.List();
            listMapping.Set(fluent => fluent.Where, Layer.Conventions, "x = 1");
            var convertedHbmList = converter.Convert(listMapping);
            convertedHbmList.where.ShouldEqual(listMapping.Where);
        }

        [Test]
        public void ShouldNotConvertWhereIfNotPopulated()
        {
            var listMapping = CollectionMapping.List();
            // Don't list anything on the original mapping
            var convertedHbmList = converter.Convert(listMapping);
            var blankHbmList = new HbmList();
            convertedHbmList.where.ShouldEqual(blankHbmList.where);
        }

        [Test]
        public void ShouldConvertSubselectIfPopulated()
        {
            var listMapping = CollectionMapping.List();
            listMapping.Set(fluent => fluent.Subselect, Layer.Conventions, "val");
            var convertedHbmList = converter.Convert(listMapping);
            convertedHbmList.subselect.Text.ShouldEqual(new string[] { listMapping.Subselect });
        }

        [Test]
        public void ShouldNotConvertSubselectIfNotPopulated()
        {
            var listMapping = CollectionMapping.List();
            // Don't list anything on the original mapping
            var convertedHbmList = converter.Convert(listMapping);
            var blankHbmList = new HbmList();
            convertedHbmList.subselect.ShouldEqual(blankHbmList.subselect);
        }

        #endregion Base collection attribute value field tests

        #region Type-specific collection attribute value field tests

        // No tests for this type

        #endregion Type-specific collection attribute value field tests

        #region Base collection converter-based subobject tests

        [Test]
        public void ShouldConvertKey()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<CollectionMapping, KeyMapping, HbmList, HbmKey>(
                () => CollectionMapping.List(),
                (listMapping, keyMapping) => listMapping.Set(fluent => fluent.Key, Layer.Defaults, keyMapping),
                hbmList => hbmList.key);
        }

        [Test]
        public void ShouldConvertICollectionRelationship_OneToMany()
        {
            ShouldConvertSubobjectAsLooselyTypedField<CollectionMapping, ICollectionRelationshipMapping, HbmList, object, object>(
                () => CollectionMapping.List(),
                () => new OneToManyMapping(),
                (listMapping, icrMapping) => listMapping.Set(fluent => fluent.Relationship, Layer.Defaults, icrMapping),
                hbmList => hbmList.Item1);
        }

        [Test]
        public void ShouldConvertICollectionRelationship_ManyToMany()
        {
            ShouldConvertSubobjectAsLooselyTypedField<CollectionMapping, ICollectionRelationshipMapping, HbmList, object, object>(
                () => CollectionMapping.List(),
                () => new ManyToManyMapping(),
                (listMapping, icrMapping) => listMapping.Set(fluent => fluent.Relationship, Layer.Defaults, icrMapping),
                hbmList => hbmList.Item1);
        }

        [Test]
        public void ShouldConvertCache()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<CollectionMapping, CacheMapping, HbmList, HbmCache>(
                () => CollectionMapping.List(),
                (listMapping, cacheMapping) => listMapping.Set(fluent => fluent.Cache, Layer.Defaults, cacheMapping),
                hbmList => hbmList.cache);
        }

        [Test]
        public void ShouldConvertCompositeElement()
        {
            ShouldConvertSubobjectAsLooselyTypedField<CollectionMapping, CompositeElementMapping, HbmList, HbmCompositeElement, object>(
                () => CollectionMapping.List(),
                (listMapping, compositeElementMapping) => listMapping.Set(fluent => fluent.CompositeElement, Layer.Defaults, compositeElementMapping),
                hbmList => hbmList.Item1);
        }

        [Test]
        public void ShouldConvertElement()
        {
            ShouldConvertSubobjectAsLooselyTypedField<CollectionMapping, ElementMapping, HbmList, HbmElement, object>(
                () => CollectionMapping.List(),
                (listMapping, elementMapping) => listMapping.Set(fluent => fluent.Element, Layer.Defaults, elementMapping),
                hbmList => hbmList.Item1);
        }

        [Test]
        public void ShouldConvertFilters()
        {
            ShouldConvertSubobjectsAsStrictlyTypedArray<CollectionMapping, FilterMapping, HbmList, HbmFilter>(
                () => CollectionMapping.List(),
                (listMapping, filterMapping) => listMapping.AddFilter(filterMapping),
                hbmList => hbmList.filter);
        }

        #endregion Base collection converter-based subobject tests

        #region Type-specific collection converter-based subobject tests

        [Test]
        public void ShouldConvertIIndex_Index()
        {
            ShouldConvertSubobjectAsLooselyTypedField<CollectionMapping, IIndexMapping, HbmList, object, object>(
                () => CollectionMapping.List(),
                () => new IndexMapping(),
                (listMapping, indexMapping) => listMapping.Set(fluent => fluent.Index, Layer.Defaults, indexMapping),
                hbmList => hbmList.Item);
        }

        // No other index type allowed by HbmList has a fluent mapping at this point

        #endregion Type-specific collection converter-based subobject tests
    }
}
