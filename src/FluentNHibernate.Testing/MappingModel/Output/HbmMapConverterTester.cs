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
    public class HbmMapConverterTester
    {
        private IHbmConverter<CollectionMapping, HbmMap> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<CollectionMapping, HbmMap>>();
        }

        #region Base collection attribute value field tests

        [Test]
        public void ShouldConvertAccessIfPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            mapMapping.Set(fluent => fluent.Access, Layer.Conventions, "acc");
            var convertedHbmMap = converter.Convert(mapMapping);
            convertedHbmMap.access.ShouldEqual(mapMapping.Access);
        }

        [Test]
        public void ShouldNotConvertAccessIfNotPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            // Don't map anything on the original mapping
            var convertedHbmMap = converter.Convert(mapMapping);
            var blankHbmMap = new HbmMap();
            convertedHbmMap.access.ShouldEqual(blankHbmMap.access);
        }

        [Test]
        public void ShouldConvertBatchSizeIfPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            mapMapping.Set(fluent => fluent.BatchSize, Layer.Conventions, 10);
            var convertedHbmMap = converter.Convert(mapMapping);
            convertedHbmMap.batchsize.ShouldEqual(mapMapping.BatchSize);
            Assert.That(convertedHbmMap.batchsizeSpecified.Equals(true), "Batch size was not marked as specified");
        }

        [Test]
        public void ShouldNotConvertBatchSizeIfNotPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            // Don't map anything on the original mapping
            var convertedHbmMap = converter.Convert(mapMapping);
            var blankHbmMap = new HbmMap();
            convertedHbmMap.batchsize.ShouldEqual(blankHbmMap.batchsize);
            Assert.That(convertedHbmMap.batchsizeSpecified.Equals(false), "Batch size was marked as specified");
        }

        [Test]
        public void ShouldConvertCascadeIfPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            mapMapping.Set(fluent => fluent.Cascade, Layer.Conventions, "all");
            var convertedHbmMap = converter.Convert(mapMapping);
            convertedHbmMap.cascade.ShouldEqual(mapMapping.Cascade);
        }

        [Test]
        public void ShouldNotConvertCascadeIfNotPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            // Don't map anything on the original mapping
            var convertedHbmMap = converter.Convert(mapMapping);
            var blankHbmMap = new HbmMap();
            convertedHbmMap.cascade.ShouldEqual(blankHbmMap.cascade);
        }

        [Test]
        public void ShouldConvertCheckIfPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            mapMapping.Set(fluent => fluent.Check, Layer.Conventions, "chk");
            var convertedHbmMap = converter.Convert(mapMapping);
            convertedHbmMap.check.ShouldEqual(mapMapping.Check);
        }

        [Test]
        public void ShouldNotConvertCheckIfNotPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            // Don't map anything on the original mapping
            var convertedHbmMap = converter.Convert(mapMapping);
            var blankHbmMap = new HbmMap();
            convertedHbmMap.check.ShouldEqual(blankHbmMap.check);
        }

        [Test]
        public void ShouldConvertCollectionTypeIfPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            mapMapping.Set(fluent => fluent.CollectionType, Layer.Conventions, new TypeReference("type"));
            var convertedHbmMap = converter.Convert(mapMapping);
            convertedHbmMap.collectiontype.ShouldEqual(mapMapping.CollectionType.ToString());
        }

        [Test]
        public void ShouldNotConvertCollectionTypeIfEmpty()
        {
            var mapMapping = CollectionMapping.Map();
            // Map an explicitly empty type reference
            mapMapping.Set(fluent => fluent.CollectionType, Layer.Conventions, TypeReference.Empty);
            var convertedHbmMap = converter.Convert(mapMapping);
            var blankHbmMap = new HbmMap();
            convertedHbmMap.collectiontype.ShouldEqual(blankHbmMap.collectiontype);
        }

        [Test]
        public void ShouldNotConvertCollectionTypeIfNotPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            // Don't map anything on the original mapping
            var convertedHbmMap = converter.Convert(mapMapping);
            var blankHbmMap = new HbmMap();
            convertedHbmMap.collectiontype.ShouldEqual(blankHbmMap.collectiontype);
        }

        [Test]
        public void ShouldConvertFetchIfPopulatedWithValidValue()
        {
            var fetch = HbmCollectionFetchMode.Subselect; // Defaults to Select, so use something else to properly detect that it changes

            var mapMapping = CollectionMapping.Map();
            var fetchDict = new XmlLinkedEnumBiDictionary<HbmCollectionFetchMode>();
            mapMapping.Set(fluent => fluent.Fetch, Layer.Conventions, fetchDict[fetch]);
            var convertedHbmMap = converter.Convert(mapMapping);
            convertedHbmMap.fetch.ShouldEqual(fetch);
            Assert.That(convertedHbmMap.fetchSpecified.Equals(true), "Fetch was not marked as specified");
        }

        [Test]
        public void ShouldFailToConvertFetchIfPopulatedWithInvalidValue()
        {
            var mapMapping = CollectionMapping.Map();
            mapMapping.Set(fluent => fluent.Fetch, Layer.Conventions, "invalid_value");
            Assert.Throws<NotSupportedException>(() => converter.Convert(mapMapping));
        }

        [Test]
        public void ShouldNotConvertFetchIfNotPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            // Don't map anything on the original mapping
            var convertedHbmMap = converter.Convert(mapMapping);
            var blankHbmMap = new HbmMap();
            convertedHbmMap.fetch.ShouldEqual(blankHbmMap.fetch);
            Assert.That(convertedHbmMap.fetchSpecified.Equals(false), "Fetch was marked as specified");
        }

        [Test]
        public void ShouldConvertGenericIfPopulated_True()
        {
            var mapMapping = CollectionMapping.Map();
            mapMapping.Set(fluent => fluent.Generic, Layer.Conventions, true);
            var convertedHbmMap = converter.Convert(mapMapping);
            convertedHbmMap.generic.ShouldEqual(mapMapping.Generic);
            Assert.That(convertedHbmMap.genericSpecified.Equals(true), "Generic was not marked as specified");
        }

        [Test]
        public void ShouldConvertGenericIfPopulated_False()
        {
            var mapMapping = CollectionMapping.Map();
            mapMapping.Set(fluent => fluent.Generic, Layer.Conventions, false);
            var convertedHbmMap = converter.Convert(mapMapping);
            convertedHbmMap.generic.ShouldEqual(mapMapping.Generic);
            Assert.That(convertedHbmMap.genericSpecified.Equals(true), "Generic was not marked as specified");
        }

        [Test]
        public void ShouldNotConvertGenericIfNotPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            // Don't map anything on the original mapping
            var convertedHbmMap = converter.Convert(mapMapping);
            var blankHbmMap = new HbmMap();
            convertedHbmMap.generic.ShouldEqual(blankHbmMap.generic);
            Assert.That(convertedHbmMap.genericSpecified.Equals(false), "Generic was marked as specified");
        }

        [Test]
        public void ShouldConvertInverseIfPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            mapMapping.Set(fluent => fluent.Inverse, Layer.Conventions, true); // Defaults to false, so use this to ensure that we can detect changes
            var convertedHbmMap = converter.Convert(mapMapping);
            convertedHbmMap.inverse.ShouldEqual(mapMapping.Inverse);
        }

        [Test]
        public void ShouldNotConvertInverseIfNotPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            // Don't map anything on the original mapping
            var convertedHbmMap = converter.Convert(mapMapping);
            var blankHbmMap = new HbmMap();
            convertedHbmMap.inverse.ShouldEqual(blankHbmMap.inverse);
        }

        [Test]
        public void ShouldConvertLazyIfPopulated()
        {
            var hbmLazy = HbmCollectionLazy.False; // Defaults to True, so use something else to properly detect that it changes

            var mapMapping = CollectionMapping.Map();
            mapMapping.Set(fluent => fluent.Lazy, Layer.Conventions, HbmCollectionConverter.FluentHbmLazyBiDict[hbmLazy]);
            var convertedHbmMap = converter.Convert(mapMapping);
            convertedHbmMap.lazy.ShouldEqual(hbmLazy);
            Assert.That(convertedHbmMap.lazySpecified.Equals(true), "Lazy was not marked as specified");
        }

        // Since it is enum-based, Lazy cannot contain any invalid values, so no need to test for that here

        [Test]
        public void ShouldNotConvertLazyIfNotPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            // Don't map anything on the original mapping
            var convertedHbmMap = converter.Convert(mapMapping);
            var blankHbmMap = new HbmMap();
            convertedHbmMap.lazy.ShouldEqual(blankHbmMap.lazy);
            Assert.That(convertedHbmMap.lazySpecified.Equals(false), "Lazy was marked as specified");
        }

        [Test]
        public void ShouldConvertNameIfPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            mapMapping.Set(fluent => fluent.Name, Layer.Conventions, "name");
            var convertedHbmMap = converter.Convert(mapMapping);
            convertedHbmMap.name.ShouldEqual(mapMapping.Name);
        }

        [Test]
        public void ShouldNotConvertNameIfNotPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            // Don't map anything on the original mapping
            var convertedHbmMap = converter.Convert(mapMapping);
            var blankHbmMap = new HbmMap();
            convertedHbmMap.name.ShouldEqual(blankHbmMap.name);
        }

        [Test]
        public void ShouldConvertOptimisticLockIfPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            mapMapping.Set(fluent => fluent.OptimisticLock, Layer.Conventions, false); // Defaults to true, so use this to ensure that we can detect changes
            var convertedHbmMap = converter.Convert(mapMapping);
            convertedHbmMap.optimisticlock.ShouldEqual(mapMapping.OptimisticLock);
        }

        [Test]
        public void ShouldNotConvertOptimisticLockIfNotPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            // Don't map anything on the original mapping
            var convertedHbmMap = converter.Convert(mapMapping);
            var blankHbmMap = new HbmMap();
            convertedHbmMap.optimisticlock.ShouldEqual(blankHbmMap.optimisticlock);
        }

        [Test]
        public void ShouldConvertPersisterIfPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            mapMapping.Set(fluent => fluent.Persister, Layer.Conventions, new TypeReference(typeof(string)));
            var convertedHbmMap = converter.Convert(mapMapping);
            convertedHbmMap.persister.ShouldEqual(mapMapping.Persister.ToString());
        }

        [Test]
        public void ShouldNotConvertPersisterIfNotPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            // Don't map anything on the original mapping
            var convertedHbmMap = converter.Convert(mapMapping);
            var blankHbmMap = new HbmMap();
            convertedHbmMap.persister.ShouldEqual(blankHbmMap.persister);
        }

        [Test]
        public void ShouldConvertSchemaIfPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            mapMapping.Set(fluent => fluent.Schema, Layer.Conventions, "dbo");
            var convertedHbmMap = converter.Convert(mapMapping);
            convertedHbmMap.schema.ShouldEqual(mapMapping.Schema);
        }

        [Test]
        public void ShouldNotConvertSchemaIfNotPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            // Don't map anything on the original mapping
            var convertedHbmMap = converter.Convert(mapMapping);
            var blankHbmMap = new HbmMap();
            convertedHbmMap.schema.ShouldEqual(blankHbmMap.schema);
        }

        [Test]
        public void ShouldConvertTableNameIfPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            mapMapping.Set(fluent => fluent.TableName, Layer.Conventions, "tbl");
            var convertedHbmMap = converter.Convert(mapMapping);
            convertedHbmMap.table.ShouldEqual(mapMapping.TableName);
        }

        [Test]
        public void ShouldNotConvertTableNameIfNotPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            // Don't map anything on the original mapping
            var convertedHbmMap = converter.Convert(mapMapping);
            var blankHbmMap = new HbmMap();
            convertedHbmMap.table.ShouldEqual(blankHbmMap.table);
        }

        [Test]
        public void ShouldConvertWhereIfPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            mapMapping.Set(fluent => fluent.Where, Layer.Conventions, "x = 1");
            var convertedHbmMap = converter.Convert(mapMapping);
            convertedHbmMap.where.ShouldEqual(mapMapping.Where);
        }

        [Test]
        public void ShouldNotConvertWhereIfNotPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            // Don't map anything on the original mapping
            var convertedHbmMap = converter.Convert(mapMapping);
            var blankHbmMap = new HbmMap();
            convertedHbmMap.where.ShouldEqual(blankHbmMap.where);
        }

        [Test]
        public void ShouldConvertSubselectIfPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            mapMapping.Set(fluent => fluent.Subselect, Layer.Conventions, "val");
            var convertedHbmMap = converter.Convert(mapMapping);
            convertedHbmMap.subselect.Text.ShouldEqual(new string[] { mapMapping.Subselect });
        }

        [Test]
        public void ShouldNotConvertSubselectIfNotPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            // Don't map anything on the original mapping
            var convertedHbmMap = converter.Convert(mapMapping);
            var blankHbmMap = new HbmMap();
            convertedHbmMap.subselect.ShouldEqual(blankHbmMap.subselect);
        }

        #endregion Base collection attribute value field tests

        #region Type-specific collection attribute value field tests

        [Test]
        public void ShouldConvertOrderByIfPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            mapMapping.Set(fluent => fluent.OrderBy, Layer.Conventions, "ord");
            var convertedHbmMap = converter.Convert(mapMapping);
            convertedHbmMap.orderby.ShouldEqual(mapMapping.OrderBy);
        }

        [Test]
        public void ShouldNotConvertOrderByIfNotPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            // Don't map anything on the original mapping
            var convertedHbmMap = converter.Convert(mapMapping);
            var blankHbmMap = new HbmMap();
            convertedHbmMap.orderby.ShouldEqual(blankHbmMap.orderby);
        }

        [Test]
        public void ShouldConvertSortIfPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            mapMapping.Set(fluent => fluent.Sort, Layer.Conventions, "asc");
            var convertedHbmMap = converter.Convert(mapMapping);
            convertedHbmMap.sort.ShouldEqual(mapMapping.Sort);
        }

        [Test]
        public void ShouldNotConvertSortIfNotPopulated()
        {
            var mapMapping = CollectionMapping.Map();
            // Don't map anything on the original mapping
            var convertedHbmMap = converter.Convert(mapMapping);
            var blankHbmMap = new HbmMap();
            convertedHbmMap.sort.ShouldEqual(blankHbmMap.sort);
        }

        #endregion Type-specific collection attribute value field tests

        #region Base collection converter-based subobject tests

        [Test]
        public void ShouldConvertKey()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<CollectionMapping, KeyMapping, HbmMap, HbmKey>(
                () => CollectionMapping.Map(),
                (mapMapping, keyMapping) => mapMapping.Set(fluent => fluent.Key, Layer.Defaults, keyMapping),
                hbmMap => hbmMap.key);
        }

        [Test]
        public void ShouldConvertICollectionRelationship_OneToMany()
        {
            ShouldConvertSubobjectAsLooselyTypedField<CollectionMapping, ICollectionRelationshipMapping, HbmMap, object, object>(
                () => CollectionMapping.Map(),
                () => new OneToManyMapping(),
                (mapMapping, icrMapping) => mapMapping.Set(fluent => fluent.Relationship, Layer.Defaults, icrMapping),
                hbmMap => hbmMap.Item1);
        }

        [Test]
        public void ShouldConvertICollectionRelationship_ManyToMany()
        {
            ShouldConvertSubobjectAsLooselyTypedField<CollectionMapping, ICollectionRelationshipMapping, HbmMap, object, object>(
                () => CollectionMapping.Map(),
                () => new ManyToManyMapping(),
                (mapMapping, icrMapping) => mapMapping.Set(fluent => fluent.Relationship, Layer.Defaults, icrMapping),
                hbmMap => hbmMap.Item1);
        }

        [Test]
        public void ShouldConvertCache()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<CollectionMapping, CacheMapping, HbmMap, HbmCache>(
                () => CollectionMapping.Map(),
                (mapMapping, cacheMapping) => mapMapping.Set(fluent => fluent.Cache, Layer.Defaults, cacheMapping),
                hbmMap => hbmMap.cache);
        }

        [Test]
        public void ShouldConvertCompositeElement()
        {
            ShouldConvertSubobjectAsLooselyTypedField<CollectionMapping, CompositeElementMapping, HbmMap, HbmCompositeElement, object>(
                () => CollectionMapping.Map(),
                (mapMapping, compositeElementMapping) => mapMapping.Set(fluent => fluent.CompositeElement, Layer.Defaults, compositeElementMapping),
                hbmMap => hbmMap.Item1);
        }

        [Test]
        public void ShouldConvertElement()
        {
            ShouldConvertSubobjectAsLooselyTypedField<CollectionMapping, ElementMapping, HbmMap, HbmElement, object>(
                () => CollectionMapping.Map(),
                (mapMapping, elementMapping) => mapMapping.Set(fluent => fluent.Element, Layer.Defaults, elementMapping),
                hbmMap => hbmMap.Item1);
        }

        [Test]
        public void ShouldConvertFilters()
        {
            ShouldConvertSubobjectsAsStrictlyTypedArray<CollectionMapping, FilterMapping, HbmMap, HbmFilter>(
                () => CollectionMapping.Map(),
                (mapMapping, filterMapping) => mapMapping.AddFilter(filterMapping),
                hbmMap => hbmMap.filter);
        }

        #endregion Base collection converter-based subobject tests

        #region Type-specific collection converter-based subobject tests

        [Test]
        public void ShouldConvertIIndex_Index()
        {
            ShouldConvertSubobjectAsLooselyTypedField<CollectionMapping, IIndexMapping, HbmMap, object, object>(
                () => CollectionMapping.Map(),
                () => new IndexMapping(),
                (mapMapping, indexMapping) => mapMapping.Set(fluent => fluent.Index, Layer.Defaults, indexMapping),
                hbmMap => hbmMap.Item);
        }

        [Test]
        public void ShouldConvertIIndex_IndexManyToMany()
        {
            ShouldConvertSubobjectAsLooselyTypedField<CollectionMapping, IIndexMapping, HbmMap, object, object>(
                () => CollectionMapping.Map(),
                () => new IndexManyToManyMapping(),
                (mapMapping, indexMapping) => mapMapping.Set(fluent => fluent.Index, Layer.Defaults, indexMapping),
                hbmMap => hbmMap.Item);
        }

        #endregion Type-specific collection converter-based subobject tests
    }
}
