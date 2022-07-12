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
    public class HbmArrayConverterTester
    {
        private IHbmConverter<CollectionMapping, HbmArray> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<CollectionMapping, HbmArray>>();
        }

        #region Base collection attribute value field tests

        [Test]
        public void ShouldConvertAccessIfPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            arrayMapping.Set(fluent => fluent.Access, Layer.Conventions, "acc");
            var convertedHbmArray = converter.Convert(arrayMapping);
            convertedHbmArray.access.ShouldEqual(arrayMapping.Access);
        }

        [Test]
        public void ShouldNotConvertAccessIfNotPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            // Don't array anything on the original mapping
            var convertedHbmArray = converter.Convert(arrayMapping);
            var blankHbmArray = new HbmArray();
            convertedHbmArray.access.ShouldEqual(blankHbmArray.access);
        }

        [Test]
        public void ShouldConvertBatchSizeIfPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            arrayMapping.Set(fluent => fluent.BatchSize, Layer.Conventions, 10);
            var convertedHbmArray = converter.Convert(arrayMapping);
            convertedHbmArray.batchsize.ShouldEqual(arrayMapping.BatchSize);
            Assert.That(convertedHbmArray.batchsizeSpecified.Equals(true), "Batch size was not marked as specified");
        }

        [Test]
        public void ShouldNotConvertBatchSizeIfNotPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            // Don't array anything on the original mapping
            var convertedHbmArray = converter.Convert(arrayMapping);
            var blankHbmArray = new HbmArray();
            convertedHbmArray.batchsize.ShouldEqual(blankHbmArray.batchsize);
            Assert.That(convertedHbmArray.batchsizeSpecified.Equals(false), "Batch size was marked as specified");
        }

        [Test]
        public void ShouldConvertCascadeIfPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            arrayMapping.Set(fluent => fluent.Cascade, Layer.Conventions, "all");
            var convertedHbmArray = converter.Convert(arrayMapping);
            convertedHbmArray.cascade.ShouldEqual(arrayMapping.Cascade);
        }

        [Test]
        public void ShouldNotConvertCascadeIfNotPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            // Don't array anything on the original mapping
            var convertedHbmArray = converter.Convert(arrayMapping);
            var blankHbmArray = new HbmArray();
            convertedHbmArray.cascade.ShouldEqual(blankHbmArray.cascade);
        }

        [Test]
        public void ShouldConvertCheckIfPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            arrayMapping.Set(fluent => fluent.Check, Layer.Conventions, "chk");
            var convertedHbmArray = converter.Convert(arrayMapping);
            convertedHbmArray.check.ShouldEqual(arrayMapping.Check);
        }

        [Test]
        public void ShouldNotConvertCheckIfNotPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            // Don't array anything on the original mapping
            var convertedHbmArray = converter.Convert(arrayMapping);
            var blankHbmArray = new HbmArray();
            convertedHbmArray.check.ShouldEqual(blankHbmArray.check);
        }

        [Test]
        public void ShouldConvertCollectionTypeIfPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            arrayMapping.Set(fluent => fluent.CollectionType, Layer.Conventions, new TypeReference("type"));
            var convertedHbmArray = converter.Convert(arrayMapping);
            convertedHbmArray.collectiontype.ShouldEqual(arrayMapping.CollectionType.ToString());
        }

        [Test]
        public void ShouldNotConvertCollectionTypeIfEmpty()
        {
            var arrayMapping = CollectionMapping.Array();
            // Array an explicitly empty type reference
            arrayMapping.Set(fluent => fluent.CollectionType, Layer.Conventions, TypeReference.Empty);
            var convertedHbmArray = converter.Convert(arrayMapping);
            var blankHbmArray = new HbmArray();
            convertedHbmArray.collectiontype.ShouldEqual(blankHbmArray.collectiontype);
        }

        [Test]
        public void ShouldNotConvertCollectionTypeIfNotPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            // Don't array anything on the original mapping
            var convertedHbmArray = converter.Convert(arrayMapping);
            var blankHbmArray = new HbmArray();
            convertedHbmArray.collectiontype.ShouldEqual(blankHbmArray.collectiontype);
        }

        [Test]
        public void ShouldConvertFetchIfPopulatedWithValidValue()
        {
            var fetch = HbmCollectionFetchMode.Subselect; // Defaults to Select, so use something else to properly detect that it changes

            var arrayMapping = CollectionMapping.Array();
            var fetchDict = new XmlLinkedEnumBiDictionary<HbmCollectionFetchMode>();
            arrayMapping.Set(fluent => fluent.Fetch, Layer.Conventions, fetchDict[fetch]);
            var convertedHbmArray = converter.Convert(arrayMapping);
            convertedHbmArray.fetch.ShouldEqual(fetch);
            Assert.That(convertedHbmArray.fetchSpecified.Equals(true), "Fetch was not marked as specified");
        }

        [Test]
        public void ShouldFailToConvertFetchIfPopulatedWithInvalidValue()
        {
            var arrayMapping = CollectionMapping.Array();
            arrayMapping.Set(fluent => fluent.Fetch, Layer.Conventions, "invalid_value");
            Assert.Throws<NotSupportedException>(() => converter.Convert(arrayMapping));
        }

        [Test]
        public void ShouldNotConvertFetchIfNotPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            // Don't array anything on the original mapping
            var convertedHbmArray = converter.Convert(arrayMapping);
            var blankHbmArray = new HbmArray();
            convertedHbmArray.fetch.ShouldEqual(blankHbmArray.fetch);
            Assert.That(convertedHbmArray.fetchSpecified.Equals(false), "Fetch was marked as specified");
        }

        // HbmArray, unlike HbmList, doesn't support the generic attribute
        /*
        [Test]
        public void ShouldConvertGenericIfPopulated_True()
        {
            var arrayMapping = CollectionMapping.Array();
            arrayMapping.Set(fluent => fluent.Generic, Layer.Conventions, true);
            var convertedHbmArray = converter.Convert(arrayMapping);
            convertedHbmArray.generic.ShouldEqual(arrayMapping.Generic);
            Assert.That(convertedHbmArray.genericSpecified.Equals(true), "Generic was not marked as specified");
        }

        [Test]
        public void ShouldConvertGenericIfPopulated_False()
        {
            var arrayMapping = CollectionMapping.Array();
            arrayMapping.Set(fluent => fluent.Generic, Layer.Conventions, false);
            var convertedHbmArray = converter.Convert(arrayMapping);
            convertedHbmArray.generic.ShouldEqual(arrayMapping.Generic);
            Assert.That(convertedHbmArray.genericSpecified.Equals(true), "Generic was not marked as specified");
        }

        [Test]
        public void ShouldNotConvertGenericIfNotPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            // Don't array anything on the original mapping
            var convertedHbmArray = converter.Convert(arrayMapping);
            var blankHbmArray = new HbmArray();
            convertedHbmArray.generic.ShouldEqual(blankHbmArray.generic);
            Assert.That(convertedHbmArray.genericSpecified.Equals(false), "Generic was marked as specified");
        }
        */

        [Test]
        public void ShouldConvertInverseIfPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            arrayMapping.Set(fluent => fluent.Inverse, Layer.Conventions, true); // Defaults to false, so use this to ensure that we can detect changes
            var convertedHbmArray = converter.Convert(arrayMapping);
            convertedHbmArray.inverse.ShouldEqual(arrayMapping.Inverse);
        }

        [Test]
        public void ShouldNotConvertInverseIfNotPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            // Don't array anything on the original mapping
            var convertedHbmArray = converter.Convert(arrayMapping);
            var blankHbmArray = new HbmArray();
            convertedHbmArray.inverse.ShouldEqual(blankHbmArray.inverse);
        }

        // HbmArray, unlike HbmList, doesn't support the lazy attribute
        /*
        [Test]
        public void ShouldConvertLazyIfPopulated()
        {
            var hbmLazy = HbmCollectionLazy.False; // Defaults to True, so use something else to properly detect that it changes

            var arrayMapping = CollectionMapping.Array();
            arrayMapping.Set(fluent => fluent.Lazy, Layer.Conventions, HbmCollectionConverter.FluentHbmLazyBiDict[hbmLazy]);
            var convertedHbmArray = converter.Convert(arrayMapping);
            convertedHbmArray.lazy.ShouldEqual(hbmLazy);
            Assert.That(convertedHbmArray.lazySpecified.Equals(true), "Lazy was not marked as specified");
        }

        // Since it is enum-based, Lazy cannot contain any invalid values, so no need to test for that here

        [Test]
        public void ShouldNotConvertLazyIfNotPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            // Don't array anything on the original mapping
            var convertedHbmArray = converter.Convert(arrayMapping);
            var blankHbmArray = new HbmArray();
            convertedHbmArray.lazy.ShouldEqual(blankHbmArray.lazy);
            Assert.That(convertedHbmArray.lazySpecified.Equals(false), "Lazy was marked as specified");
        }
        */

        [Test]
        public void ShouldConvertNameIfPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            arrayMapping.Set(fluent => fluent.Name, Layer.Conventions, "name");
            var convertedHbmArray = converter.Convert(arrayMapping);
            convertedHbmArray.name.ShouldEqual(arrayMapping.Name);
        }

        [Test]
        public void ShouldNotConvertNameIfNotPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            // Don't array anything on the original mapping
            var convertedHbmArray = converter.Convert(arrayMapping);
            var blankHbmArray = new HbmArray();
            convertedHbmArray.name.ShouldEqual(blankHbmArray.name);
        }

        [Test]
        public void ShouldConvertOptimisticLockIfPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            arrayMapping.Set(fluent => fluent.OptimisticLock, Layer.Conventions, false); // Defaults to true, so use this to ensure that we can detect changes
            var convertedHbmArray = converter.Convert(arrayMapping);
            convertedHbmArray.optimisticlock.ShouldEqual(arrayMapping.OptimisticLock);
        }

        [Test]
        public void ShouldNotConvertOptimisticLockIfNotPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            // Don't array anything on the original mapping
            var convertedHbmArray = converter.Convert(arrayMapping);
            var blankHbmArray = new HbmArray();
            convertedHbmArray.optimisticlock.ShouldEqual(blankHbmArray.optimisticlock);
        }

        [Test]
        public void ShouldConvertPersisterIfPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            arrayMapping.Set(fluent => fluent.Persister, Layer.Conventions, new TypeReference(typeof(string)));
            var convertedHbmArray = converter.Convert(arrayMapping);
            convertedHbmArray.persister.ShouldEqual(arrayMapping.Persister.ToString());
        }

        [Test]
        public void ShouldNotConvertPersisterIfNotPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            // Don't array anything on the original mapping
            var convertedHbmArray = converter.Convert(arrayMapping);
            var blankHbmArray = new HbmArray();
            convertedHbmArray.persister.ShouldEqual(blankHbmArray.persister);
        }

        [Test]
        public void ShouldConvertSchemaIfPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            arrayMapping.Set(fluent => fluent.Schema, Layer.Conventions, "dbo");
            var convertedHbmArray = converter.Convert(arrayMapping);
            convertedHbmArray.schema.ShouldEqual(arrayMapping.Schema);
        }

        [Test]
        public void ShouldNotConvertSchemaIfNotPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            // Don't array anything on the original mapping
            var convertedHbmArray = converter.Convert(arrayMapping);
            var blankHbmArray = new HbmArray();
            convertedHbmArray.schema.ShouldEqual(blankHbmArray.schema);
        }

        [Test]
        public void ShouldConvertTableNameIfPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            arrayMapping.Set(fluent => fluent.TableName, Layer.Conventions, "tbl");
            var convertedHbmArray = converter.Convert(arrayMapping);
            convertedHbmArray.table.ShouldEqual(arrayMapping.TableName);
        }

        [Test]
        public void ShouldNotConvertTableNameIfNotPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            // Don't array anything on the original mapping
            var convertedHbmArray = converter.Convert(arrayMapping);
            var blankHbmArray = new HbmArray();
            convertedHbmArray.table.ShouldEqual(blankHbmArray.table);
        }

        [Test]
        public void ShouldConvertWhereIfPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            arrayMapping.Set(fluent => fluent.Where, Layer.Conventions, "x = 1");
            var convertedHbmArray = converter.Convert(arrayMapping);
            convertedHbmArray.where.ShouldEqual(arrayMapping.Where);
        }

        [Test]
        public void ShouldNotConvertWhereIfNotPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            // Don't array anything on the original mapping
            var convertedHbmArray = converter.Convert(arrayMapping);
            var blankHbmArray = new HbmArray();
            convertedHbmArray.where.ShouldEqual(blankHbmArray.where);
        }

        [Test]
        public void ShouldConvertSubselectIfPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            arrayMapping.Set(fluent => fluent.Subselect, Layer.Conventions, "val");
            var convertedHbmArray = converter.Convert(arrayMapping);
            convertedHbmArray.subselect.Text.ShouldEqual(new string[] { arrayMapping.Subselect });
        }

        [Test]
        public void ShouldNotConvertSubselectIfNotPopulated()
        {
            var arrayMapping = CollectionMapping.Array();
            // Don't array anything on the original mapping
            var convertedHbmArray = converter.Convert(arrayMapping);
            var blankHbmArray = new HbmArray();
            convertedHbmArray.subselect.ShouldEqual(blankHbmArray.subselect);
        }

        #endregion Base collection attribute value field tests

        #region Type-specific collection attribute value field tests

        // No tests for this type

        #endregion Type-specific collection attribute value field tests

        #region Base collection converter-based subobject tests

        [Test]
        public void ShouldConvertKey()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<CollectionMapping, KeyMapping, HbmArray, HbmKey>(
                () => CollectionMapping.Array(),
                (arrayMapping, keyMapping) => arrayMapping.Set(fluent => fluent.Key, Layer.Defaults, keyMapping),
                hbmArray => hbmArray.key);
        }

        [Test]
        public void ShouldConvertICollectionRelationship_OneToMany()
        {
            ShouldConvertSubobjectAsLooselyTypedField<CollectionMapping, ICollectionRelationshipMapping, HbmArray, object, object>(
                () => CollectionMapping.Array(),
                () => new OneToManyMapping(),
                (arrayMapping, icrMapping) => arrayMapping.Set(fluent => fluent.Relationship, Layer.Defaults, icrMapping),
                hbmArray => hbmArray.Item1);
        }

        [Test]
        public void ShouldConvertICollectionRelationship_ManyToMany()
        {
            ShouldConvertSubobjectAsLooselyTypedField<CollectionMapping, ICollectionRelationshipMapping, HbmArray, object, object>(
                () => CollectionMapping.Array(),
                () => new ManyToManyMapping(),
                (arrayMapping, icrMapping) => arrayMapping.Set(fluent => fluent.Relationship, Layer.Defaults, icrMapping),
                hbmArray => hbmArray.Item1);
        }

        [Test]
        public void ShouldConvertCache()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<CollectionMapping, CacheMapping, HbmArray, HbmCache>(
                () => CollectionMapping.Array(),
                (arrayMapping, cacheMapping) => arrayMapping.Set(fluent => fluent.Cache, Layer.Defaults, cacheMapping),
                hbmArray => hbmArray.cache);
        }

        [Test]
        public void ShouldConvertCompositeElement()
        {
            ShouldConvertSubobjectAsLooselyTypedField<CollectionMapping, CompositeElementMapping, HbmArray, HbmCompositeElement, object>(
                () => CollectionMapping.Array(),
                (arrayMapping, compositeElementMapping) => arrayMapping.Set(fluent => fluent.CompositeElement, Layer.Defaults, compositeElementMapping),
                hbmArray => hbmArray.Item1);
        }

        [Test]
        public void ShouldConvertElement()
        {
            ShouldConvertSubobjectAsLooselyTypedField<CollectionMapping, ElementMapping, HbmArray, HbmElement, object>(
                () => CollectionMapping.Array(),
                (arrayMapping, elementMapping) => arrayMapping.Set(fluent => fluent.Element, Layer.Defaults, elementMapping),
                hbmArray => hbmArray.Item1);
        }

        // HbmArray, unlike HbmList, doesn't support filters
        /*
        [Test]
        public void ShouldConvertFilters()
        {
            ShouldConvertSubobjectsAsStrictlyTypedArray<CollectionMapping, FilterMapping, HbmArray, HbmFilter>(
                () => CollectionMapping.Array(),
                (arrayMapping, filterMapping) => arrayMapping.AddFilter(filterMapping),
                hbmArray => hbmArray.filter);
        }
        */

        #endregion Base collection converter-based subobject tests

        #region Type-specific collection converter-based subobject tests

        [Test]
        public void ShouldConvertIIndex_Index()
        {
            ShouldConvertSubobjectAsLooselyTypedField<CollectionMapping, IIndexMapping, HbmArray, object, object>(
                () => CollectionMapping.Array(),
                () => new IndexMapping(),
                (arrayMapping, indexMapping) => arrayMapping.Set(fluent => fluent.Index, Layer.Defaults, indexMapping),
                hbmArray => hbmArray.Item);
        }

        // No other index type allowed by HbmArray has a fluent mapping at this point

        #endregion Type-specific collection converter-based subobject tests
    }
}
