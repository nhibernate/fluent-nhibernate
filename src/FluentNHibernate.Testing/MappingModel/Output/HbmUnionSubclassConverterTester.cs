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
    public class HbmUnionSubclassConverterTester
    {
        private IHbmConverter<SubclassMapping, HbmUnionSubclass> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<SubclassMapping, HbmUnionSubclass>>();
        }

        #region Value field tests

        [Test]
        public void ShouldConvertTableNameIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            subclassMapping.Set(fluent => fluent.TableName, Layer.Conventions, "tbl");
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            convertedHbmUnionSubclass.table.ShouldEqual(subclassMapping.TableName);
        }

        [Test]
        public void ShouldNotConvertTableNameIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            // Don't set anything on the original mapping
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            var blankHbmUnionSubclass = new HbmUnionSubclass();
            convertedHbmUnionSubclass.table.ShouldEqual(blankHbmUnionSubclass.table);
        }

        [Test]
        public void ShouldConvertSchemaIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            subclassMapping.Set(fluent => fluent.Schema, Layer.Conventions, "dbo");
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            convertedHbmUnionSubclass.schema.ShouldEqual(subclassMapping.Schema);
        }

        [Test]
        public void ShouldNotConvertSchemaIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            // Don't set anything on the original mapping
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            var blankHbmUnionSubclass = new HbmUnionSubclass();
            convertedHbmUnionSubclass.schema.ShouldEqual(blankHbmUnionSubclass.schema);
        }

        [Test]
        public void ShouldConvertCheckIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            subclassMapping.Set(fluent => fluent.Check, Layer.Conventions, "chk");
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            convertedHbmUnionSubclass.check.ShouldEqual(subclassMapping.Check);
        }

        [Test]
        public void ShouldNotConvertCheckIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            // Don't set anything on the original mapping
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            var blankHbmUnionSubclass = new HbmUnionSubclass();
            convertedHbmUnionSubclass.check.ShouldEqual(blankHbmUnionSubclass.check);
        }

        [Test]
        public void ShouldConvertPersisterIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            subclassMapping.Set(fluent => fluent.Persister, Layer.Conventions, new TypeReference(typeof(HbmUnionSubclassConverterTester))); // Not really representative, but the class is guaranteed to exist
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            convertedHbmUnionSubclass.persister.ShouldEqual(subclassMapping.Persister.ToString());
        }

        [Test]
        public void ShouldNotConvertPersisterIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            // Don't set anything on the original mapping
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            var blankHbmUnionSubclass = new HbmUnionSubclass();
            convertedHbmUnionSubclass.persister.ShouldEqual(blankHbmUnionSubclass.persister);
        }

        [Test]
        public void ShouldConvertNameIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            subclassMapping.Set(fluent => fluent.Name, Layer.Conventions, "name");
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            convertedHbmUnionSubclass.name.ShouldEqual(subclassMapping.Name);
        }

        [Test]
        public void ShouldNotConvertNameIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            // Don't set anything on the original mapping
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            var blankHbmUnionSubclass = new HbmUnionSubclass();
            convertedHbmUnionSubclass.name.ShouldEqual(blankHbmUnionSubclass.name);
        }

        [Test]
        public void ShouldConvertProxyIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            subclassMapping.Set(fluent => fluent.Proxy, Layer.Conventions, "p");
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            convertedHbmUnionSubclass.proxy.ShouldEqual(subclassMapping.Proxy);
        }

        [Test]
        public void ShouldNotConvertProxyIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            // Don't set anything on the original mapping
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            var blankHbmUnionSubclass = new HbmUnionSubclass();
            convertedHbmUnionSubclass.proxy.ShouldEqual(blankHbmUnionSubclass.proxy);
        }

        [Test]
        public void ShouldConvertLazyIfPopulated_True()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            subclassMapping.Set(fluent => fluent.Lazy, Layer.Conventions, true);
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            convertedHbmUnionSubclass.lazy.ShouldEqual(subclassMapping.Lazy);
            Assert.That(convertedHbmUnionSubclass.lazySpecified.Equals(true), "Lazy was not marked as specified");
        }

        [Test]
        public void ShouldConvertLazyIfPopulated_False()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            subclassMapping.Set(fluent => fluent.Lazy, Layer.Conventions, false);
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            convertedHbmUnionSubclass.lazy.ShouldEqual(subclassMapping.Lazy);
            Assert.That(convertedHbmUnionSubclass.lazySpecified.Equals(true), "Lazy was not marked as specified");
        }

        [Test]
        public void ShouldNotConvertLazyIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            // Don't set anything on the original mapping
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            var blankHbmUnionSubclass = new HbmUnionSubclass();
            convertedHbmUnionSubclass.lazy.ShouldEqual(blankHbmUnionSubclass.lazy);
            Assert.That(convertedHbmUnionSubclass.lazySpecified.Equals(false), "Batch size was marked as specified");
        }

        [Test]
        public void ShouldConvertDynamicUpdateIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            subclassMapping.Set(fluent => fluent.DynamicUpdate, Layer.Conventions, true); // Defaults to false, so we have to set it true here in order to tell if it actually changed
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            convertedHbmUnionSubclass.dynamicupdate.ShouldEqual(subclassMapping.DynamicUpdate);
        }

        [Test]
        public void ShouldNotConvertDynamicUpdateIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            // Don't set anything on the original mapping
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            var blankHbmUnionSubclass = new HbmUnionSubclass();
            convertedHbmUnionSubclass.dynamicupdate.ShouldEqual(blankHbmUnionSubclass.dynamicupdate);
        }

        [Test]
        public void ShouldConvertDynamicInsertIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            subclassMapping.Set(fluent => fluent.DynamicInsert, Layer.Conventions, true); // Defaults to false, so we have to set it true here in order to tell if it actually changed
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            convertedHbmUnionSubclass.dynamicinsert.ShouldEqual(subclassMapping.DynamicInsert);
        }

        [Test]
        public void ShouldNotConvertDynamicInsertIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            // Don't set anything on the original mapping
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            var blankHbmUnionSubclass = new HbmUnionSubclass();
            convertedHbmUnionSubclass.dynamicinsert.ShouldEqual(blankHbmUnionSubclass.dynamicinsert);
        }

        [Test]
        public void ShouldConvertSelectBeforeUpdateIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            subclassMapping.Set(fluent => fluent.SelectBeforeUpdate, Layer.Conventions, true); // Defaults to false, so we have to set it true here in order to tell if it actually changed
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            convertedHbmUnionSubclass.selectbeforeupdate.ShouldEqual(subclassMapping.SelectBeforeUpdate);
        }

        [Test]
        public void ShouldNotConvertSelectBeforeUpdateIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            // Don't set anything on the original mapping
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            var blankHbmUnionSubclass = new HbmUnionSubclass();
            convertedHbmUnionSubclass.selectbeforeupdate.ShouldEqual(blankHbmUnionSubclass.selectbeforeupdate);
        }

        [Test]
        public void ShouldConvertAbstractIfPopulated_True()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            subclassMapping.Set(fluent => fluent.Abstract, Layer.Conventions, true);
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            convertedHbmUnionSubclass.@abstract.ShouldEqual(subclassMapping.Abstract);
            Assert.That(convertedHbmUnionSubclass.abstractSpecified.Equals(true), "Abstract was not marked as specified");
        }

        [Test]
        public void ShouldConvertAbstractIfPopulated_False()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            subclassMapping.Set(fluent => fluent.Abstract, Layer.Conventions, false);
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            convertedHbmUnionSubclass.@abstract.ShouldEqual(subclassMapping.Abstract);
            Assert.That(convertedHbmUnionSubclass.abstractSpecified.Equals(true), "Abstract was not marked as specified");
        }

        [Test]
        public void ShouldNotConvertAbstractIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            // Don't set anything on the original mapping
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            var blankHbmUnionSubclass = new HbmUnionSubclass();
            convertedHbmUnionSubclass.@abstract.ShouldEqual(blankHbmUnionSubclass.@abstract);
            Assert.That(convertedHbmUnionSubclass.abstractSpecified.Equals(false), "Abstract was marked as specified");
        }

        [Test]
        public void ShouldConvertEntityNameIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            subclassMapping.Set(fluent => fluent.EntityName, Layer.Conventions, "entity1");
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            convertedHbmUnionSubclass.entityname.ShouldEqual(subclassMapping.EntityName);
        }

        [Test]
        public void ShouldNotConvertEntityNameIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            // Don't set anything on the original mapping
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            var blankHbmUnionSubclass = new HbmUnionSubclass();
            convertedHbmUnionSubclass.entityname.ShouldEqual(blankHbmUnionSubclass.entityname);
        }

        [Test]
        public void ShouldConvertBatchSizeIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            subclassMapping.Set(fluent => fluent.BatchSize, Layer.Conventions, 10);
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            convertedHbmUnionSubclass.batchsize.ShouldEqual(subclassMapping.BatchSize.ToString());
        }

        [Test]
        public void ShouldNotConvertBatchSizeIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            // Don't set anything on the original mapping
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            var blankHbmUnionSubclass = new HbmUnionSubclass();
            convertedHbmUnionSubclass.batchsize.ShouldEqual(blankHbmUnionSubclass.batchsize);
        }

        #endregion Value field tests

        #region Non-converter-based subobject tests

        [Test]
        public void ShouldConvertSubselectIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            subclassMapping.Set(fluent => fluent.Subselect, Layer.Conventions, "val");
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            convertedHbmUnionSubclass.subselect.Text.ShouldEqual(new string[] { subclassMapping.Subselect });
        }

        [Test]
        public void ShouldNotConvertSubselectIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            // Don't set anything on the original mapping
            var convertedHbmUnionSubclass = converter.Convert(subclassMapping);
            var blankHbmUnionSubclass = new HbmUnionSubclass();
            convertedHbmUnionSubclass.subselect.ShouldEqual(blankHbmUnionSubclass.subselect);
        }

        #endregion Non-converter-based subobject tests

        #region Converter-based subobject tests

        [Test]
        public void ShouldConvertProperties()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, PropertyMapping, HbmUnionSubclass, HbmProperty, object>(
                () => new SubclassMapping(SubclassType.UnionSubclass),
                (subclassMapping, propertyMapping) => subclassMapping.AddProperty(propertyMapping),
                hbmUnionSubclass => hbmUnionSubclass.Items);
        }

        [Test]
        public void ShouldConvertManyToOnes()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, ManyToOneMapping, HbmUnionSubclass, HbmManyToOne, object>(
                () => new SubclassMapping(SubclassType.UnionSubclass),
                (subclassMapping, manyToOneMapping) => subclassMapping.AddReference(manyToOneMapping),
                hbmUnionSubclass => hbmUnionSubclass.Items);
        }

        [Test]
        public void ShouldConvertOneToOnes()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, OneToOneMapping, HbmUnionSubclass, HbmOneToOne, object>(
                () => new SubclassMapping(SubclassType.UnionSubclass),
                (subclassMapping, oneToOneMapping) => subclassMapping.AddOneToOne(oneToOneMapping),
                hbmUnionSubclass => hbmUnionSubclass.Items);
        }

        [Test]
        public void ShouldConvertComponents_Component()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, IComponentMapping, HbmUnionSubclass, object, object>(
                () => new SubclassMapping(SubclassType.UnionSubclass),
                () => new ComponentMapping(ComponentType.Component),
                (subclassMapping, componentMapping) => subclassMapping.AddComponent(componentMapping),
                hbmUnionSubclass => hbmUnionSubclass.Items);
        }

        [Test]
        public void ShouldConvertComponents_DynamicComponent()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, IComponentMapping, HbmUnionSubclass, object, object>(
                () => new SubclassMapping(SubclassType.UnionSubclass),
                () => new ComponentMapping(ComponentType.DynamicComponent),
                (subclassMapping, componentMapping) => subclassMapping.AddComponent(componentMapping),
                hbmUnionSubclass => hbmUnionSubclass.Items);
        }

        [Test]
        public void ShouldConvertAnys()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, AnyMapping, HbmUnionSubclass, HbmAny, object>(
                () => new SubclassMapping(SubclassType.UnionSubclass),
                (subclassMapping, anyMapping) => subclassMapping.AddAny(anyMapping),
                hbmUnionSubclass => hbmUnionSubclass.Items);
        }

        [Test]
        public void ShouldConvertCollections_Map()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, CollectionMapping, HbmUnionSubclass, object, object>(
                () => new SubclassMapping(SubclassType.UnionSubclass),
                () => CollectionMapping.Map(),
                (subclassMapping, mapMapping) => subclassMapping.AddCollection(mapMapping),
                hbmUnionSubclass => hbmUnionSubclass.Items);
        }

        [Test]
        public void ShouldConvertCollections_Set()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, CollectionMapping, HbmUnionSubclass, object, object>(
                () => new SubclassMapping(SubclassType.UnionSubclass),
                () => CollectionMapping.Set(),
                (subclassMapping, setMapping) => subclassMapping.AddCollection(setMapping),
                hbmUnionSubclass => hbmUnionSubclass.Items);
        }

        [Test]
        public void ShouldConvertCollections_List()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, CollectionMapping, HbmUnionSubclass, object, object>(
                () => new SubclassMapping(SubclassType.UnionSubclass),
                () => CollectionMapping.List(),
                (subclassMapping, listMapping) => subclassMapping.AddCollection(listMapping),
                hbmUnionSubclass => hbmUnionSubclass.Items);
        }

        [Test]
        public void ShouldConvertCollections_Bag()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, CollectionMapping, HbmUnionSubclass, object, object>(
                () => new SubclassMapping(SubclassType.UnionSubclass),
                () => CollectionMapping.Bag(),
                (subclassMapping, bagMapping) => subclassMapping.AddCollection(bagMapping),
                hbmUnionSubclass => hbmUnionSubclass.Items);
        }

        [Test, Ignore("ShouldConvertCollections_IdBag")]
        public void ShouldConvertCollections_IdBag()
        {
            Assert.Fail("Target logic not yet available");
        }

        [Test]
        public void ShouldConvertCollections_Array()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, CollectionMapping, HbmUnionSubclass, object, object>(
                () => new SubclassMapping(SubclassType.UnionSubclass),
                () => CollectionMapping.Array(),
                (subclassMapping, bagMapping) => subclassMapping.AddCollection(bagMapping),
                hbmUnionSubclass => hbmUnionSubclass.Items);
        }

        [Test, Ignore("ShouldConvertCollections_PrimitiveArray")]
        public void ShouldConvertCollections_PrimitiveArray()
        {
            Assert.Fail("Target logic not yet available");
        }

        [Test]
        public void ShouldConvertSubclasses_Subclass()
        {
            Assert.Throws<NotSupportedException>(() =>
                ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, SubclassMapping, HbmUnionSubclass, HbmSubclass, object>(
                    () => new SubclassMapping(SubclassType.UnionSubclass),
                    () => new SubclassMapping(SubclassType.Subclass),
                    (unionSubclassMapping, subclassMapping) => unionSubclassMapping.AddSubclass(subclassMapping),
                    hbmUnionSubclass => hbmUnionSubclass.unionsubclass1)
            );
        }

        [Test]
        public void ShouldConvertSubclasses_JoinedSubclass()
        {
            Assert.Throws<NotSupportedException>(() =>
                ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, SubclassMapping, HbmUnionSubclass, HbmJoinedSubclass, object>(
                    () => new SubclassMapping(SubclassType.UnionSubclass),
                    () => new SubclassMapping(SubclassType.JoinedSubclass),
                    (unionSubclassMapping, joinedSubclassMapping) => unionSubclassMapping.AddSubclass(joinedSubclassMapping),
                    hbmUnionSubclass => hbmUnionSubclass.unionsubclass1)
            );
        }

        [Test]
        public void ShouldConvertSubclasses_UnionSubclass()
        {
            ShouldConvertSubobjectsAsStrictlyTypedArray<SubclassMapping, SubclassMapping, HbmUnionSubclass, HbmUnionSubclass>(
                () => new SubclassMapping(SubclassType.UnionSubclass),
                () => new SubclassMapping(SubclassType.UnionSubclass),
                (unionSubclassMapping1, unionSubclassMapping2) => unionSubclassMapping1.AddSubclass(unionSubclassMapping2),
                hbmUnionSubclass => hbmUnionSubclass.unionsubclass1);
        }

        [Test]
        public void ShouldConvertStoredProcedure_SqlInsert()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<SubclassMapping, StoredProcedureMapping, HbmUnionSubclass, HbmCustomSQL>(
                () => new SubclassMapping(SubclassType.UnionSubclass),
                () => new StoredProcedureMapping("sql-insert", ""),
                (subclassMapping, storedProcedureMapping) => subclassMapping.AddStoredProcedure(storedProcedureMapping),
                hbmUnionSubclass => hbmUnionSubclass.sqlinsert);
        }

        [Test]
        public void ShouldConvertStoredProcedure_SqlUpdate()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<SubclassMapping, StoredProcedureMapping, HbmUnionSubclass, HbmCustomSQL>(
                () => new SubclassMapping(SubclassType.UnionSubclass),
                () => new StoredProcedureMapping("sql-update", ""),
                (subclassMapping, storedProcedureMapping) => subclassMapping.AddStoredProcedure(storedProcedureMapping),
                hbmUnionSubclass => hbmUnionSubclass.sqlupdate);
        }

        [Test]
        public void ShouldConvertStoredProcedure_SqlDelete()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<SubclassMapping, StoredProcedureMapping, HbmUnionSubclass, HbmCustomSQL>(
                () => new SubclassMapping(SubclassType.UnionSubclass),
                () => new StoredProcedureMapping("sql-delete", ""),
                (subclassMapping, storedProcedureMapping) => subclassMapping.AddStoredProcedure(storedProcedureMapping),
                hbmUnionSubclass => hbmUnionSubclass.sqldelete);
        }

        [Test]
        public void ShouldConvertStoredProcedure_Unsupported()
        {
            var unsupportedSPType = "invalid";

            // Set up a fake converter
            var fakeConverter = A.Fake<IHbmConverter<StoredProcedureMapping, HbmCustomSQL>>();

            // Set up a custom container with the fake FSub->HSub converter registered, and obtain our main converter from it (so
            // that it will use the fake implementation). Note that we do the resolution _before_ we register the fake, so that
            // in cases where we are doing recursive types and FMain == FSub + HMain == HSub (e.g., subclasses-of-subclasses) we
            // get the real converter for the "outer" call but the fake for any "inner" calls.
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<SubclassMapping, HbmUnionSubclass>>();
            container.Register<IHbmConverter<StoredProcedureMapping, HbmCustomSQL>>(cnvrt => fakeConverter);

            // Allocate the subclass mapping and a stored procedure submapping with an unsupported sptype
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            subclassMapping.AddStoredProcedure(new StoredProcedureMapping(unsupportedSPType, ""));

            // This should throw
            Assert.Throws<NotSupportedException>(() => converter.Convert(subclassMapping));

            // We don't care if it made a call to the subobject conversion logic or not (it is low enough cost that it doesn't
            // really matter in the case of failure, and some implementation approaches that uses this may be simpler).
        }

        #endregion Converter-based subobject tests
    }
}
