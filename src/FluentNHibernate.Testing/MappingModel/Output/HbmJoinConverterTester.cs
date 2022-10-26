using System;
using FakeItEasy;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;
using IComponentMapping = FluentNHibernate.MappingModel.ClassBased.IComponentMapping;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmJoinConverterTester
    {
        private IHbmConverter<JoinMapping, HbmJoin> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<JoinMapping, HbmJoin>>();
        }

        #region Value field tests

        [Test]
        public void ShouldConvertTableNameIfPopulated()
        {
            var joinMapping = new JoinMapping();
            joinMapping.Set(fluent => fluent.TableName, Layer.Conventions, "tbl");
            var convertedHbmJoin = converter.Convert(joinMapping);
            convertedHbmJoin.table.ShouldEqual(joinMapping.TableName);
        }

        [Test]
        public void ShouldNotConvertTableNameIfNotPopulated()
        {
            var joinMapping = new JoinMapping();
            // Don't set anything on the original mapping
            var convertedHbmJoin = converter.Convert(joinMapping);
            var blankHbmJoin = new HbmJoin();
            convertedHbmJoin.table.ShouldEqual(blankHbmJoin.table);
        }

        [Test]
        public void ShouldConvertSchemaIfPopulated()
        {
            var joinMapping = new JoinMapping();
            joinMapping.Set(fluent => fluent.Schema, Layer.Conventions, "dbo");
            var convertedHbmJoin = converter.Convert(joinMapping);
            convertedHbmJoin.schema.ShouldEqual(joinMapping.Schema);
        }

        [Test]
        public void ShouldNotConvertSchemaIfNotPopulated()
        {
            var joinMapping = new JoinMapping();
            // Don't set anything on the original mapping
            var convertedHbmJoin = converter.Convert(joinMapping);
            var blankHbmJoin = new HbmJoin();
            convertedHbmJoin.schema.ShouldEqual(blankHbmJoin.schema);
        }

        [Test]
        public void ShouldConvertFetchIfPopulatedWithValidValue()
        {
            var fetch = HbmJoinFetch.Select; // Defaults to Join, so use this to ensure that we can detect changes

            var joinMapping = new JoinMapping();
            var fetchDict = new XmlLinkedEnumBiDictionary<HbmJoinFetch>();
            joinMapping.Set(fluent => fluent.Fetch, Layer.Conventions, fetchDict[fetch]);
            var convertedHbmJoin = converter.Convert(joinMapping);
            convertedHbmJoin.fetch.ShouldEqual(fetch);
        }

        [Test]
        public void ShouldFailToConvertFetchIfPopulatedWithInvalidValue()
        {
            var joinMapping = new JoinMapping();
            joinMapping.Set(fluent => fluent.Fetch, Layer.Conventions, "invalid_value");
            Assert.Throws<NotSupportedException>(() => converter.Convert(joinMapping));
        }

        [Test]
        public void ShouldNotConvertFetchIfNotPopulated()
        {
            var joinMapping = new JoinMapping();
            // Don't set anything on the original mapping
            var convertedHbmJoin = converter.Convert(joinMapping);
            var blankHbmJoin = new HbmJoin();
            convertedHbmJoin.fetch.ShouldEqual(blankHbmJoin.fetch);
        }

        [Test]
        public void ShouldConvertInverseIfPopulated()
        {
            var joinMapping = new JoinMapping();
            joinMapping.Set(fluent => fluent.Inverse, Layer.Conventions, true); // Defaults to false, so use this in order to tell if it actually changed
            var convertedHbmJoin = converter.Convert(joinMapping);
            convertedHbmJoin.inverse.ShouldEqual(joinMapping.Inverse);
        }

        [Test]
        public void ShouldNotConvertInverseIfNotPopulated()
        {
            var joinMapping = new JoinMapping();
            // Don't set anything on the original mapping
            var convertedHbmJoin = converter.Convert(joinMapping);
            var blankHbmJoin = new HbmJoin();
            convertedHbmJoin.inverse.ShouldEqual(blankHbmJoin.inverse);
        }

        [Test]
        public void ShouldConvertOptionalIfPopulated()
        {
            var joinMapping = new JoinMapping();
            joinMapping.Set(fluent => fluent.Optional, Layer.Conventions, true); // Defaults to false, so use this in order to tell if it actually changed
            var convertedHbmJoin = converter.Convert(joinMapping);
            convertedHbmJoin.optional.ShouldEqual(joinMapping.Optional);
        }

        [Test]
        public void ShouldNotConvertOptionalIfNotPopulated()
        {
            var joinMapping = new JoinMapping();
            // Don't set anything on the original mapping
            var convertedHbmJoin = converter.Convert(joinMapping);
            var blankHbmJoin = new HbmJoin();
            convertedHbmJoin.optional.ShouldEqual(blankHbmJoin.optional);
        }

        [Test]
        public void ShouldConvertCatalogIfPopulated()
        {
            var joinMapping = new JoinMapping();
            joinMapping.Set(fluent => fluent.Catalog, Layer.Conventions, "catalog");
            var convertedHbmJoin = converter.Convert(joinMapping);
            convertedHbmJoin.catalog.ShouldEqual(joinMapping.Catalog);
        }

        [Test]
        public void ShouldNotConvertCatalogIfNotPopulated()
        {
            var joinMapping = new JoinMapping();
            // Don't set anything on the original mapping
            var convertedHbmJoin = converter.Convert(joinMapping);
            var blankHbmJoin = new HbmJoin();
            convertedHbmJoin.catalog.ShouldEqual(blankHbmJoin.catalog);
        }

        #endregion Value field tests

        #region Non-converter-based subobject tests

        [Test]
        public void ShouldConvertSubselectIfPopulated()
        {
            var joinMapping = new JoinMapping();
            joinMapping.Set(fluent => fluent.Subselect, Layer.Conventions, "val");
            var convertedHbmJoin = converter.Convert(joinMapping);
            convertedHbmJoin.subselect.Text.ShouldEqual(new string[] { joinMapping.Subselect });
        }

        [Test]
        public void ShouldNotConvertSubselectIfNotPopulated()
        {
            var joinMapping = new JoinMapping();
            // Don't set anything on the original mapping
            var convertedHbmJoin = converter.Convert(joinMapping);
            var blankHbmJoin = new HbmJoin();
            convertedHbmJoin.subselect.ShouldEqual(blankHbmJoin.subselect);
        }

        #endregion Non-converter-based subobject tests

        #region Converter-based subobject tests

        [Test]
        public void ShouldConvertKey()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<JoinMapping, KeyMapping, HbmJoin, HbmKey>(
                (joinMapping, keyMapping) => joinMapping.Set(fluent => fluent.Key, Layer.Conventions, keyMapping),
                hbmJoin => hbmJoin.key);
        }

        [Test]
        public void ShouldConvertProperties()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<JoinMapping, PropertyMapping, HbmJoin, HbmProperty, object>(
                (joinMapping, propertyMapping) => joinMapping.AddProperty(propertyMapping),
                hbmJoin => hbmJoin.Items);
        }

        [Test]
        public void ShouldConvertManyToOnes()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<JoinMapping, ManyToOneMapping, HbmJoin, HbmManyToOne, object>(
                (joinMapping, manyToOneMapping) => joinMapping.AddReference(manyToOneMapping),
                hbmJoin => hbmJoin.Items);
        }

        [Test]
        public void ShouldConvertComponents_Component()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<JoinMapping, IComponentMapping, HbmJoin, object, object>(
                () => new ComponentMapping(ComponentType.Component),
                (joinMapping, componentMapping) => joinMapping.AddComponent(componentMapping),
                hbmJoin => hbmJoin.Items);
        }

        [Test]
        public void ShouldConvertComponents_DynamicComponent()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<JoinMapping, IComponentMapping, HbmJoin, object, object>(
                () => new ComponentMapping(ComponentType.DynamicComponent),
                (joinMapping, componentMapping) => joinMapping.AddComponent(componentMapping),
                hbmJoin => hbmJoin.Items);
        }

        [Test]
        public void ShouldConvertAnys()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<JoinMapping, AnyMapping, HbmJoin, HbmAny, object>(
                (joinMapping, anyMapping) => joinMapping.AddAny(anyMapping),
                hbmJoin => hbmJoin.Items);
        }

        [Test]
        public void ShouldConvertCollections_Map()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<JoinMapping, CollectionMapping, HbmJoin, object, object>(
                () => CollectionMapping.Map(),
                (joinMapping, mapMapping) => joinMapping.AddCollection(mapMapping),
                hbmJoin => hbmJoin.Items);
        }

        [Test]
        public void ShouldConvertCollections_Set()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<JoinMapping, CollectionMapping, HbmJoin, object, object>(
                () => CollectionMapping.Set(),
                (joinMapping, setMapping) => joinMapping.AddCollection(setMapping),
                hbmJoin => hbmJoin.Items);
        }

        [Test]
        public void ShouldConvertCollections_List()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<JoinMapping, CollectionMapping, HbmJoin, object, object>(
                () => CollectionMapping.List(),
                (joinMapping, listMapping) => joinMapping.AddCollection(listMapping),
                hbmJoin => hbmJoin.Items);
        }

        [Test]
        public void ShouldConvertCollections_Bag()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<JoinMapping, CollectionMapping, HbmJoin, object, object>(
                () => CollectionMapping.Bag(),
                (joinMapping, bagMapping) => joinMapping.AddCollection(bagMapping),
                hbmJoin => hbmJoin.Items);
        }

        [Test, Ignore("ShouldConvertCollections_IdBag")]
        public void ShouldConvertCollections_IdBag()
        {
            Assert.Fail("Target logic not yet available");
        }

        [Test]
        public void ShouldConvertCollections_Array()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<JoinMapping, CollectionMapping, HbmJoin, object, object>(
                () => CollectionMapping.Array(),
                (joinMapping, bagMapping) => joinMapping.AddCollection(bagMapping),
                hbmJoin => hbmJoin.Items);
        }

        [Test, Ignore("ShouldConvertCollections_PrimitiveArray")]
        public void ShouldConvertCollections_PrimitiveArray()
        {
            Assert.Fail("Target logic not yet available");
        }

        [Test]
        public void ShouldConvertStoredProcedure_SqlInsert()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<JoinMapping, StoredProcedureMapping, HbmJoin, HbmCustomSQL>(
                () => new StoredProcedureMapping("sql-insert", ""),
                (joinMapping, storedProcedureMapping) => joinMapping.AddStoredProcedure(storedProcedureMapping),
                hbmJoin => hbmJoin.sqlinsert);
        }

        [Test]
        public void ShouldConvertStoredProcedure_SqlUpdate()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<JoinMapping, StoredProcedureMapping, HbmJoin, HbmCustomSQL>(
                () => new StoredProcedureMapping("sql-update", ""),
                (joinMapping, storedProcedureMapping) => joinMapping.AddStoredProcedure(storedProcedureMapping),
                hbmJoin => hbmJoin.sqlupdate);
        }

        [Test]
        public void ShouldConvertStoredProcedure_SqlDelete()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<JoinMapping, StoredProcedureMapping, HbmJoin, HbmCustomSQL>(
                () => new StoredProcedureMapping("sql-delete", ""),
                (joinMapping, storedProcedureMapping) => joinMapping.AddStoredProcedure(storedProcedureMapping),
                hbmJoin => hbmJoin.sqldelete);
        }

        [Test]
        public void ShouldConvertStoredProcedure_Unsupported()
        {
            var unsupportedSPType = "invalid";

            // Set up a fake converter
            var fakeConverter = A.Fake<IHbmConverter<StoredProcedureMapping, HbmCustomSQL>>();

            // Set up a custom container with the fake FSub->HSub converter registered, and obtain our main converter from it (so that it will use the fake implementation)
            var container = new HbmConverterContainer();
            container.Register<IHbmConverter<StoredProcedureMapping, HbmCustomSQL>>(cnvrt => fakeConverter);
            converter = container.Resolve<IHbmConverter<JoinMapping, HbmJoin>>();

            // Allocate the join mapping and a stored procedure submapping with an unsupported sptype
            var joinMapping = new JoinMapping();
            joinMapping.AddStoredProcedure(new StoredProcedureMapping(unsupportedSPType, ""));

            // This should throw
            Assert.Throws<NotSupportedException>(() => converter.Convert(joinMapping));

            // We don't care if it made a call to the subobject conversion logic or not (it is low enough cost that it doesn't
            // really matter in the case of failure, and some implementation approaches that uses this may be simpler).
        }

        #endregion Converter-based subobject tests
    }
}
