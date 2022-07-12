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
    public class HbmSetConverterTester
    {
        private IHbmConverter<CollectionMapping, HbmSet> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<CollectionMapping, HbmSet>>();
        }

        #region Base collection attribute value field tests

        [Test]
        public void ShouldConvertAccessIfPopulated()
        {
            var setMapping = CollectionMapping.Set();
            setMapping.Set(fluent => fluent.Access, Layer.Conventions, "acc");
            var convertedHbmSet = converter.Convert(setMapping);
            convertedHbmSet.access.ShouldEqual(setMapping.Access);
        }

        [Test]
        public void ShouldNotConvertAccessIfNotPopulated()
        {
            var setMapping = CollectionMapping.Set();
            // Don't set anything on the original mapping
            var convertedHbmSet = converter.Convert(setMapping);
            var blankHbmSet = new HbmSet();
            convertedHbmSet.access.ShouldEqual(blankHbmSet.access);
        }

        [Test]
        public void ShouldConvertBatchSizeIfPopulated()
        {
            var setMapping = CollectionMapping.Set();
            setMapping.Set(fluent => fluent.BatchSize, Layer.Conventions, 10);
            var convertedHbmSet = converter.Convert(setMapping);
            convertedHbmSet.batchsize.ShouldEqual(setMapping.BatchSize);
            Assert.That(convertedHbmSet.batchsizeSpecified.Equals(true), "Batch size was not marked as specified");
        }

        [Test]
        public void ShouldNotConvertBatchSizeIfNotPopulated()
        {
            var setMapping = CollectionMapping.Set();
            // Don't set anything on the original mapping
            var convertedHbmSet = converter.Convert(setMapping);
            var blankHbmSet = new HbmSet();
            convertedHbmSet.batchsize.ShouldEqual(blankHbmSet.batchsize);
            Assert.That(convertedHbmSet.batchsizeSpecified.Equals(false), "Batch size was marked as specified");
        }

        [Test]
        public void ShouldConvertCascadeIfPopulated()
        {
            var setMapping = CollectionMapping.Set();
            setMapping.Set(fluent => fluent.Cascade, Layer.Conventions, "all");
            var convertedHbmSet = converter.Convert(setMapping);
            convertedHbmSet.cascade.ShouldEqual(setMapping.Cascade);
        }

        [Test]
        public void ShouldNotConvertCascadeIfNotPopulated()
        {
            var setMapping = CollectionMapping.Set();
            // Don't set anything on the original mapping
            var convertedHbmSet = converter.Convert(setMapping);
            var blankHbmSet = new HbmSet();
            convertedHbmSet.cascade.ShouldEqual(blankHbmSet.cascade);
        }

        [Test]
        public void ShouldConvertCheckIfPopulated()
        {
            var setMapping = CollectionMapping.Set();
            setMapping.Set(fluent => fluent.Check, Layer.Conventions, "chk");
            var convertedHbmSet = converter.Convert(setMapping);
            convertedHbmSet.check.ShouldEqual(setMapping.Check);
        }

        [Test]
        public void ShouldNotConvertCheckIfNotPopulated()
        {
            var setMapping = CollectionMapping.Set();
            // Don't set anything on the original mapping
            var convertedHbmSet = converter.Convert(setMapping);
            var blankHbmSet = new HbmSet();
            convertedHbmSet.check.ShouldEqual(blankHbmSet.check);
        }

        [Test]
        public void ShouldConvertCollectionTypeIfPopulated()
        {
            var setMapping = CollectionMapping.Set();
            setMapping.Set(fluent => fluent.CollectionType, Layer.Conventions, new TypeReference("type"));
            var convertedHbmSet = converter.Convert(setMapping);
            convertedHbmSet.collectiontype.ShouldEqual(setMapping.CollectionType.ToString());
        }

        [Test]
        public void ShouldNotConvertCollectionTypeIfEmpty()
        {
            var setMapping = CollectionMapping.Set();
            // Set an explicitly empty type reference
            setMapping.Set(fluent => fluent.CollectionType, Layer.Conventions, TypeReference.Empty);
            var convertedHbmSet = converter.Convert(setMapping);
            var blankHbmSet = new HbmSet();
            convertedHbmSet.collectiontype.ShouldEqual(blankHbmSet.collectiontype);
        }

        [Test]
        public void ShouldNotConvertCollectionTypeIfNotPopulated()
        {
            var setMapping = CollectionMapping.Set();
            // Don't set anything on the original mapping
            var convertedHbmSet = converter.Convert(setMapping);
            var blankHbmSet = new HbmSet();
            convertedHbmSet.collectiontype.ShouldEqual(blankHbmSet.collectiontype);
        }

        [Test]
        public void ShouldConvertFetchIfPopulatedWithValidValue()
        {
            var fetch = HbmCollectionFetchMode.Subselect; // Defaults to Select, so use something else to properly detect that it changes

            var setMapping = CollectionMapping.Set();
            var fetchDict = new XmlLinkedEnumBiDictionary<HbmCollectionFetchMode>();
            setMapping.Set(fluent => fluent.Fetch, Layer.Conventions, fetchDict[fetch]);
            var convertedHbmSet = converter.Convert(setMapping);
            convertedHbmSet.fetch.ShouldEqual(fetch);
            Assert.That(convertedHbmSet.fetchSpecified.Equals(true), "Fetch was not marked as specified");
        }

        [Test]
        public void ShouldFailToConvertFetchIfPopulatedWithInvalidValue()
        {
            var setMapping = CollectionMapping.Set();
            setMapping.Set(fluent => fluent.Fetch, Layer.Conventions, "invalid_value");
            Assert.Throws<NotSupportedException>(() => converter.Convert(setMapping));
        }

        [Test]
        public void ShouldNotConvertFetchIfNotPopulated()
        {
            var setMapping = CollectionMapping.Set();
            // Don't set anything on the original mapping
            var convertedHbmSet = converter.Convert(setMapping);
            var blankHbmSet = new HbmSet();
            convertedHbmSet.fetch.ShouldEqual(blankHbmSet.fetch);
            Assert.That(convertedHbmSet.fetchSpecified.Equals(false), "Fetch was marked as specified");
        }

        [Test]
        public void ShouldConvertGenericIfPopulated_True()
        {
            var setMapping = CollectionMapping.Set();
            setMapping.Set(fluent => fluent.Generic, Layer.Conventions, true);
            var convertedHbmSet = converter.Convert(setMapping);
            convertedHbmSet.generic.ShouldEqual(setMapping.Generic);
            Assert.That(convertedHbmSet.genericSpecified.Equals(true), "Generic was not marked as specified");
        }

        [Test]
        public void ShouldConvertGenericIfPopulated_False()
        {
            var setMapping = CollectionMapping.Set();
            setMapping.Set(fluent => fluent.Generic, Layer.Conventions, false);
            var convertedHbmSet = converter.Convert(setMapping);
            convertedHbmSet.generic.ShouldEqual(setMapping.Generic);
            Assert.That(convertedHbmSet.genericSpecified.Equals(true), "Generic was not marked as specified");
        }

        [Test]
        public void ShouldNotConvertGenericIfNotPopulated()
        {
            var setMapping = CollectionMapping.Set();
            // Don't set anything on the original mapping
            var convertedHbmSet = converter.Convert(setMapping);
            var blankHbmSet = new HbmSet();
            convertedHbmSet.generic.ShouldEqual(blankHbmSet.generic);
            Assert.That(convertedHbmSet.genericSpecified.Equals(false), "Generic was marked as specified");
        }

        [Test]
        public void ShouldConvertInverseIfPopulated()
        {
            var setMapping = CollectionMapping.Set();
            setMapping.Set(fluent => fluent.Inverse, Layer.Conventions, true); // Defaults to false, so use this to ensure that we can detect changes
            var convertedHbmSet = converter.Convert(setMapping);
            convertedHbmSet.inverse.ShouldEqual(setMapping.Inverse);
        }

        [Test]
        public void ShouldNotConvertInverseIfNotPopulated()
        {
            var setMapping = CollectionMapping.Set();
            // Don't set anything on the original mapping
            var convertedHbmSet = converter.Convert(setMapping);
            var blankHbmSet = new HbmSet();
            convertedHbmSet.inverse.ShouldEqual(blankHbmSet.inverse);
        }

        [Test]
        public void ShouldConvertLazyIfPopulated()
        {
            var hbmLazy = HbmCollectionLazy.False; // Defaults to True, so use something else to properly detect that it changes

            var setMapping = CollectionMapping.Set();
            setMapping.Set(fluent => fluent.Lazy, Layer.Conventions, HbmCollectionConverter.FluentHbmLazyBiDict[hbmLazy]);
            var convertedHbmSet = converter.Convert(setMapping);
            convertedHbmSet.lazy.ShouldEqual(hbmLazy);
            Assert.That(convertedHbmSet.lazySpecified.Equals(true), "Lazy was not marked as specified");
        }

        // Since it is enum-based, Lazy cannot contain any invalid values, so no need to test for that here

        [Test]
        public void ShouldNotConvertLazyIfNotPopulated()
        {
            var setMapping = CollectionMapping.Set();
            // Don't set anything on the original mapping
            var convertedHbmSet = converter.Convert(setMapping);
            var blankHbmSet = new HbmSet();
            convertedHbmSet.lazy.ShouldEqual(blankHbmSet.lazy);
            Assert.That(convertedHbmSet.lazySpecified.Equals(false), "Lazy was marked as specified");
        }

        [Test]
        public void ShouldConvertNameIfPopulated()
        {
            var setMapping = CollectionMapping.Set();
            setMapping.Set(fluent => fluent.Name, Layer.Conventions, "name");
            var convertedHbmSet = converter.Convert(setMapping);
            convertedHbmSet.name.ShouldEqual(setMapping.Name);
        }

        [Test]
        public void ShouldNotConvertNameIfNotPopulated()
        {
            var setMapping = CollectionMapping.Set();
            // Don't set anything on the original mapping
            var convertedHbmSet = converter.Convert(setMapping);
            var blankHbmSet = new HbmSet();
            convertedHbmSet.name.ShouldEqual(blankHbmSet.name);
        }

        [Test]
        public void ShouldConvertOptimisticLockIfPopulated()
        {
            var setMapping = CollectionMapping.Set();
            setMapping.Set(fluent => fluent.OptimisticLock, Layer.Conventions, false); // Defaults to true, so use this to ensure that we can detect changes
            var convertedHbmSet = converter.Convert(setMapping);
            convertedHbmSet.optimisticlock.ShouldEqual(setMapping.OptimisticLock);
        }

        [Test]
        public void ShouldNotConvertOptimisticLockIfNotPopulated()
        {
            var setMapping = CollectionMapping.Set();
            // Don't set anything on the original mapping
            var convertedHbmSet = converter.Convert(setMapping);
            var blankHbmSet = new HbmSet();
            convertedHbmSet.optimisticlock.ShouldEqual(blankHbmSet.optimisticlock);
        }

        [Test]
        public void ShouldConvertPersisterIfPopulated()
        {
            var setMapping = CollectionMapping.Set();
            setMapping.Set(fluent => fluent.Persister, Layer.Conventions, new TypeReference(typeof(string)));
            var convertedHbmSet = converter.Convert(setMapping);
            convertedHbmSet.persister.ShouldEqual(setMapping.Persister.ToString());
        }

        [Test]
        public void ShouldNotConvertPersisterIfNotPopulated()
        {
            var setMapping = CollectionMapping.Set();
            // Don't set anything on the original mapping
            var convertedHbmSet = converter.Convert(setMapping);
            var blankHbmSet = new HbmSet();
            convertedHbmSet.persister.ShouldEqual(blankHbmSet.persister);
        }

        [Test]
        public void ShouldConvertSchemaIfPopulated()
        {
            var setMapping = CollectionMapping.Set();
            setMapping.Set(fluent => fluent.Schema, Layer.Conventions, "dbo");
            var convertedHbmSet = converter.Convert(setMapping);
            convertedHbmSet.schema.ShouldEqual(setMapping.Schema);
        }

        [Test]
        public void ShouldNotConvertSchemaIfNotPopulated()
        {
            var setMapping = CollectionMapping.Set();
            // Don't set anything on the original mapping
            var convertedHbmSet = converter.Convert(setMapping);
            var blankHbmSet = new HbmSet();
            convertedHbmSet.schema.ShouldEqual(blankHbmSet.schema);
        }

        [Test]
        public void ShouldConvertTableNameIfPopulated()
        {
            var setMapping = CollectionMapping.Set();
            setMapping.Set(fluent => fluent.TableName, Layer.Conventions, "tbl");
            var convertedHbmSet = converter.Convert(setMapping);
            convertedHbmSet.table.ShouldEqual(setMapping.TableName);
        }

        [Test]
        public void ShouldNotConvertTableNameIfNotPopulated()
        {
            var setMapping = CollectionMapping.Set();
            // Don't set anything on the original mapping
            var convertedHbmSet = converter.Convert(setMapping);
            var blankHbmSet = new HbmSet();
            convertedHbmSet.table.ShouldEqual(blankHbmSet.table);
        }

        [Test]
        public void ShouldConvertWhereIfPopulated()
        {
            var setMapping = CollectionMapping.Set();
            setMapping.Set(fluent => fluent.Where, Layer.Conventions, "x = 1");
            var convertedHbmSet = converter.Convert(setMapping);
            convertedHbmSet.where.ShouldEqual(setMapping.Where);
        }

        [Test]
        public void ShouldNotConvertWhereIfNotPopulated()
        {
            var setMapping = CollectionMapping.Set();
            // Don't set anything on the original mapping
            var convertedHbmSet = converter.Convert(setMapping);
            var blankHbmSet = new HbmSet();
            convertedHbmSet.where.ShouldEqual(blankHbmSet.where);
        }

        [Test]
        public void ShouldConvertSubselectIfPopulated()
        {
            var setMapping = CollectionMapping.Set();
            setMapping.Set(fluent => fluent.Subselect, Layer.Conventions, "val");
            var convertedHbmSet = converter.Convert(setMapping);
            convertedHbmSet.subselect.Text.ShouldEqual(new string[] { setMapping.Subselect });
        }

        [Test]
        public void ShouldNotConvertSubselectIfNotPopulated()
        {
            var setMapping = CollectionMapping.Set();
            // Don't set anything on the original mapping
            var convertedHbmSet = converter.Convert(setMapping);
            var blankHbmSet = new HbmSet();
            convertedHbmSet.subselect.ShouldEqual(blankHbmSet.subselect);
        }

        #endregion Base collection attribute value field tests

        #region Type-specific collection attribute value field tests

        [Test]
        public void ShouldConvertOrderByIfPopulated()
        {
            var setMapping = CollectionMapping.Set();
            setMapping.Set(fluent => fluent.OrderBy, Layer.Conventions, "ord");
            var convertedHbmSet = converter.Convert(setMapping);
            convertedHbmSet.orderby.ShouldEqual(setMapping.OrderBy);
        }

        [Test]
        public void ShouldNotConvertOrderByIfNotPopulated()
        {
            var setMapping = CollectionMapping.Set();
            // Don't set anything on the original mapping
            var convertedHbmSet = converter.Convert(setMapping);
            var blankHbmSet = new HbmSet();
            convertedHbmSet.orderby.ShouldEqual(blankHbmSet.orderby);
        }

        [Test]
        public void ShouldConvertSortIfPopulated()
        {
            var setMapping = CollectionMapping.Set();
            setMapping.Set(fluent => fluent.Sort, Layer.Conventions, "asc");
            var convertedHbmSet = converter.Convert(setMapping);
            convertedHbmSet.sort.ShouldEqual(setMapping.Sort);
        }

        [Test]
        public void ShouldNotConvertSortIfNotPopulated()
        {
            var setMapping = CollectionMapping.Set();
            // Don't set anything on the original mapping
            var convertedHbmSet = converter.Convert(setMapping);
            var blankHbmSet = new HbmSet();
            convertedHbmSet.sort.ShouldEqual(blankHbmSet.sort);
        }

        #endregion Type-specific collection attribute value field tests

        #region Base collection converter-based subobject tests

        [Test]
        public void ShouldConvertKey()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<CollectionMapping, KeyMapping, HbmSet, HbmKey>(
                () => CollectionMapping.Set(),
                (setMapping, keyMapping) => setMapping.Set(fluent => fluent.Key, Layer.Defaults, keyMapping),
                hbmSet => hbmSet.key);
        }

        [Test]
        public void ShouldConvertICollectionRelationship_OneToMany()
        {
            ShouldConvertSubobjectAsLooselyTypedField<CollectionMapping, ICollectionRelationshipMapping, HbmSet, object, object>(
                () => CollectionMapping.Set(),
                () => new OneToManyMapping(),
                (setMapping, icrMapping) => setMapping.Set(fluent => fluent.Relationship, Layer.Defaults, icrMapping),
                hbmSet => hbmSet.Item);
        }

        [Test]
        public void ShouldConvertICollectionRelationship_ManyToMany()
        {
            ShouldConvertSubobjectAsLooselyTypedField<CollectionMapping, ICollectionRelationshipMapping, HbmSet, object, object>(
                () => CollectionMapping.Set(),
                () => new ManyToManyMapping(),
                (setMapping, icrMapping) => setMapping.Set(fluent => fluent.Relationship, Layer.Defaults, icrMapping),
                hbmSet => hbmSet.Item);
        }

        [Test]
        public void ShouldConvertCache()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<CollectionMapping, CacheMapping, HbmSet, HbmCache>(
                () => CollectionMapping.Set(),
                (setMapping, cacheMapping) => setMapping.Set(fluent => fluent.Cache, Layer.Defaults, cacheMapping),
                hbmSet => hbmSet.cache);
        }

        [Test]
        public void ShouldConvertCompositeElement()
        {
            ShouldConvertSubobjectAsLooselyTypedField<CollectionMapping, CompositeElementMapping, HbmSet, HbmCompositeElement, object>(
                () => CollectionMapping.Set(),
                (setMapping, compositeElementMapping) => setMapping.Set(fluent => fluent.CompositeElement, Layer.Defaults, compositeElementMapping),
                hbmSet => hbmSet.Item);
        }

        [Test]
        public void ShouldConvertElement()
        {
            ShouldConvertSubobjectAsLooselyTypedField<CollectionMapping, ElementMapping, HbmSet, HbmElement, object>(
                () => CollectionMapping.Set(),
                (setMapping, elementMapping) => setMapping.Set(fluent => fluent.Element, Layer.Defaults, elementMapping),
                hbmSet => hbmSet.Item);
        }

        [Test]
        public void ShouldConvertFilters()
        {
            ShouldConvertSubobjectsAsStrictlyTypedArray<CollectionMapping, FilterMapping, HbmSet, HbmFilter>(
                () => CollectionMapping.Set(),
                (setMapping, filterMapping) => setMapping.AddFilter(filterMapping),
                hbmSet => hbmSet.filter);
        }

        #endregion Base collection converter-based subobject tests

        #region Type-specific collection converter-based subobject tests

        // No tests for this type

        #endregion Type-specific collection converter-based subobject tests
    }
}
