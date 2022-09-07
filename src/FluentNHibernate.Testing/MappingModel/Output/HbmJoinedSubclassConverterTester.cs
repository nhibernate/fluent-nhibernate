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
    public class HbmJoinedSubclassConverterTester
    {
        private IHbmConverter<SubclassMapping, HbmJoinedSubclass> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<SubclassMapping, HbmJoinedSubclass>>();
        }

        #region Value field tests

        [Test]
        public void ShouldConvertTableNameIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            subclassMapping.Set(fluent => fluent.TableName, Layer.Conventions, "tbl");
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            convertedHbmJoinedSubclass.table.ShouldEqual(subclassMapping.TableName);
        }

        [Test]
        public void ShouldNotConvertTableNameIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            // Don't set anything on the original mapping
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            var blankHbmJoinedSubclass = new HbmJoinedSubclass();
            convertedHbmJoinedSubclass.table.ShouldEqual(blankHbmJoinedSubclass.table);
        }

        [Test]
        public void ShouldConvertSchemaIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            subclassMapping.Set(fluent => fluent.Schema, Layer.Conventions, "dbo");
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            convertedHbmJoinedSubclass.schema.ShouldEqual(subclassMapping.Schema);
        }

        [Test]
        public void ShouldNotConvertSchemaIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            // Don't set anything on the original mapping
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            var blankHbmJoinedSubclass = new HbmJoinedSubclass();
            convertedHbmJoinedSubclass.schema.ShouldEqual(blankHbmJoinedSubclass.schema);
        }

        [Test]
        public void ShouldConvertCheckIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            subclassMapping.Set(fluent => fluent.Check, Layer.Conventions, "chk");
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            convertedHbmJoinedSubclass.check.ShouldEqual(subclassMapping.Check);
        }

        [Test]
        public void ShouldNotConvertCheckIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            // Don't set anything on the original mapping
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            var blankHbmJoinedSubclass = new HbmJoinedSubclass();
            convertedHbmJoinedSubclass.check.ShouldEqual(blankHbmJoinedSubclass.check);
        }

        [Test]
        public void ShouldConvertPersisterIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            subclassMapping.Set(fluent => fluent.Persister, Layer.Conventions, new TypeReference(typeof(HbmJoinedSubclassConverterTester))); // Not really representative, but the class is guaranteed to exist
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            convertedHbmJoinedSubclass.persister.ShouldEqual(subclassMapping.Persister.ToString());
        }

        [Test]
        public void ShouldNotConvertPersisterIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            // Don't set anything on the original mapping
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            var blankHbmJoinedSubclass = new HbmJoinedSubclass();
            convertedHbmJoinedSubclass.persister.ShouldEqual(blankHbmJoinedSubclass.persister);
        }

        [Test]
        public void ShouldConvertNameIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            subclassMapping.Set(fluent => fluent.Name, Layer.Conventions, "name");
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            convertedHbmJoinedSubclass.name.ShouldEqual(subclassMapping.Name);
        }

        [Test]
        public void ShouldNotConvertNameIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            // Don't set anything on the original mapping
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            var blankHbmJoinedSubclass = new HbmJoinedSubclass();
            convertedHbmJoinedSubclass.name.ShouldEqual(blankHbmJoinedSubclass.name);
        }

        [Test]
        public void ShouldConvertProxyIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            subclassMapping.Set(fluent => fluent.Proxy, Layer.Conventions, "p");
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            convertedHbmJoinedSubclass.proxy.ShouldEqual(subclassMapping.Proxy);
        }

        [Test]
        public void ShouldNotConvertProxyIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            // Don't set anything on the original mapping
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            var blankHbmJoinedSubclass = new HbmJoinedSubclass();
            convertedHbmJoinedSubclass.proxy.ShouldEqual(blankHbmJoinedSubclass.proxy);
        }

        [Test]
        public void ShouldConvertLazyIfPopulated_True()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            subclassMapping.Set(fluent => fluent.Lazy, Layer.Conventions, true);
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            convertedHbmJoinedSubclass.lazy.ShouldEqual(subclassMapping.Lazy);
            Assert.That(convertedHbmJoinedSubclass.lazySpecified.Equals(true), "Lazy was not marked as specified");
        }

        [Test]
        public void ShouldConvertLazyIfPopulated_False()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            subclassMapping.Set(fluent => fluent.Lazy, Layer.Conventions, false);
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            convertedHbmJoinedSubclass.lazy.ShouldEqual(subclassMapping.Lazy);
            Assert.That(convertedHbmJoinedSubclass.lazySpecified.Equals(true), "Lazy was not marked as specified");
        }

        [Test]
        public void ShouldNotConvertLazyIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            // Don't set anything on the original mapping
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            var blankHbmJoinedSubclass = new HbmJoinedSubclass();
            convertedHbmJoinedSubclass.lazy.ShouldEqual(blankHbmJoinedSubclass.lazy);
            Assert.That(convertedHbmJoinedSubclass.lazySpecified.Equals(false), "Batch size was marked as specified");
        }

        [Test]
        public void ShouldConvertDynamicUpdateIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            subclassMapping.Set(fluent => fluent.DynamicUpdate, Layer.Conventions, true); // Defaults to false, so we have to set it true here in order to tell if it actually changed
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            convertedHbmJoinedSubclass.dynamicupdate.ShouldEqual(subclassMapping.DynamicUpdate);
        }

        [Test]
        public void ShouldNotConvertDynamicUpdateIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            // Don't set anything on the original mapping
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            var blankHbmJoinedSubclass = new HbmJoinedSubclass();
            convertedHbmJoinedSubclass.dynamicupdate.ShouldEqual(blankHbmJoinedSubclass.dynamicupdate);
        }

        [Test]
        public void ShouldConvertDynamicInsertIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            subclassMapping.Set(fluent => fluent.DynamicInsert, Layer.Conventions, true); // Defaults to false, so we have to set it true here in order to tell if it actually changed
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            convertedHbmJoinedSubclass.dynamicinsert.ShouldEqual(subclassMapping.DynamicInsert);
        }

        [Test]
        public void ShouldNotConvertDynamicInsertIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            // Don't set anything on the original mapping
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            var blankHbmJoinedSubclass = new HbmJoinedSubclass();
            convertedHbmJoinedSubclass.dynamicinsert.ShouldEqual(blankHbmJoinedSubclass.dynamicinsert);
        }

        [Test]
        public void ShouldConvertSelectBeforeUpdateIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            subclassMapping.Set(fluent => fluent.SelectBeforeUpdate, Layer.Conventions, true); // Defaults to false, so we have to set it true here in order to tell if it actually changed
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            convertedHbmJoinedSubclass.selectbeforeupdate.ShouldEqual(subclassMapping.SelectBeforeUpdate);
        }

        [Test]
        public void ShouldNotConvertSelectBeforeUpdateIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            // Don't set anything on the original mapping
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            var blankHbmJoinedSubclass = new HbmJoinedSubclass();
            convertedHbmJoinedSubclass.selectbeforeupdate.ShouldEqual(blankHbmJoinedSubclass.selectbeforeupdate);
        }

        [Test]
        public void ShouldConvertAbstractIfPopulated_True()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            subclassMapping.Set(fluent => fluent.Abstract, Layer.Conventions, true);
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            convertedHbmJoinedSubclass.@abstract.ShouldEqual(subclassMapping.Abstract);
            Assert.That(convertedHbmJoinedSubclass.abstractSpecified.Equals(true), "Abstract was not marked as specified");
        }

        [Test]
        public void ShouldConvertAbstractIfPopulated_False()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            subclassMapping.Set(fluent => fluent.Abstract, Layer.Conventions, false);
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            convertedHbmJoinedSubclass.@abstract.ShouldEqual(subclassMapping.Abstract);
            Assert.That(convertedHbmJoinedSubclass.abstractSpecified.Equals(true), "Abstract was not marked as specified");
        }

        [Test]
        public void ShouldNotConvertAbstractIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            // Don't set anything on the original mapping
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            var blankHbmJoinedSubclass = new HbmJoinedSubclass();
            convertedHbmJoinedSubclass.@abstract.ShouldEqual(blankHbmJoinedSubclass.@abstract);
            Assert.That(convertedHbmJoinedSubclass.abstractSpecified.Equals(false), "Abstract was marked as specified");
        }

        [Test]
        public void ShouldConvertEntityNameIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            subclassMapping.Set(fluent => fluent.EntityName, Layer.Conventions, "entity1");
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            convertedHbmJoinedSubclass.entityname.ShouldEqual(subclassMapping.EntityName);
        }

        [Test]
        public void ShouldNotConvertEntityNameIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            // Don't set anything on the original mapping
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            var blankHbmJoinedSubclass = new HbmJoinedSubclass();
            convertedHbmJoinedSubclass.entityname.ShouldEqual(blankHbmJoinedSubclass.entityname);
        }

        [Test]
        public void ShouldConvertBatchSizeIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            subclassMapping.Set(fluent => fluent.BatchSize, Layer.Conventions, 10);
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            convertedHbmJoinedSubclass.batchsize.ShouldEqual(subclassMapping.BatchSize.ToString());
        }

        [Test]
        public void ShouldNotConvertBatchSizeIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            // Don't set anything on the original mapping
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            var blankHbmJoinedSubclass = new HbmJoinedSubclass();
            convertedHbmJoinedSubclass.batchsize.ShouldEqual(blankHbmJoinedSubclass.batchsize);
        }

        #endregion Value field tests

        #region Non-converter-based subobject tests

        [Test]
        public void ShouldConvertSubselectIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            subclassMapping.Set(fluent => fluent.Subselect, Layer.Conventions, "val");
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            convertedHbmJoinedSubclass.subselect.Text.ShouldEqual(new string[] { subclassMapping.Subselect });
        }

        [Test]
        public void ShouldNotConvertSubselectIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            // Don't set anything on the original mapping
            var convertedHbmJoinedSubclass = converter.Convert(subclassMapping);
            var blankHbmJoinedSubclass = new HbmJoinedSubclass();
            convertedHbmJoinedSubclass.subselect.ShouldEqual(blankHbmJoinedSubclass.subselect);
        }

        #endregion Non-converter-based subobject tests

        #region Converter-based subobject tests

        [Test]
        public void ShouldConvertProperties()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, PropertyMapping, HbmJoinedSubclass, HbmProperty, object>(
                () => new SubclassMapping(SubclassType.JoinedSubclass),
                (subclassMapping, propertyMapping) => subclassMapping.AddProperty(propertyMapping),
                hbmJoinedSubclass => hbmJoinedSubclass.Items);
        }

        [Test]
        public void ShouldConvertManyToOnes()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, ManyToOneMapping, HbmJoinedSubclass, HbmManyToOne, object>(
                () => new SubclassMapping(SubclassType.JoinedSubclass),
                (subclassMapping, manyToOneMapping) => subclassMapping.AddReference(manyToOneMapping),
                hbmJoinedSubclass => hbmJoinedSubclass.Items);
        }

        [Test]
        public void ShouldConvertOneToOnes()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, OneToOneMapping, HbmJoinedSubclass, HbmOneToOne, object>(
                () => new SubclassMapping(SubclassType.JoinedSubclass),
                (subclassMapping, oneToOneMapping) => subclassMapping.AddOneToOne(oneToOneMapping),
                hbmJoinedSubclass => hbmJoinedSubclass.Items);
        }

        [Test]
        public void ShouldConvertComponents_Component()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, IComponentMapping, HbmJoinedSubclass, object, object>(
                () => new SubclassMapping(SubclassType.JoinedSubclass),
                () => new ComponentMapping(ComponentType.Component),
                (subclassMapping, componentMapping) => subclassMapping.AddComponent(componentMapping),
                hbmJoinedSubclass => hbmJoinedSubclass.Items);
        }

        [Test]
        public void ShouldConvertComponents_DynamicComponent()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, IComponentMapping, HbmJoinedSubclass, object, object>(
                () => new SubclassMapping(SubclassType.JoinedSubclass),
                () => new ComponentMapping(ComponentType.DynamicComponent),
                (subclassMapping, componentMapping) => subclassMapping.AddComponent(componentMapping),
                hbmJoinedSubclass => hbmJoinedSubclass.Items);
        }

        [Test]
        public void ShouldConvertAnys()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, AnyMapping, HbmJoinedSubclass, HbmAny, object>(
                () => new SubclassMapping(SubclassType.JoinedSubclass),
                (subclassMapping, anyMapping) => subclassMapping.AddAny(anyMapping),
                hbmJoinedSubclass => hbmJoinedSubclass.Items);
        }

        [Test]
        public void ShouldConvertCollections_Map()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, CollectionMapping, HbmJoinedSubclass, object, object>(
                () => new SubclassMapping(SubclassType.JoinedSubclass),
                () => CollectionMapping.Map(),
                (subclassMapping, mapMapping) => subclassMapping.AddCollection(mapMapping),
                hbmJoinedSubclass => hbmJoinedSubclass.Items);
        }

        [Test]
        public void ShouldConvertCollections_Set()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, CollectionMapping, HbmJoinedSubclass, object, object>(
                () => new SubclassMapping(SubclassType.JoinedSubclass),
                () => CollectionMapping.Set(),
                (subclassMapping, setMapping) => subclassMapping.AddCollection(setMapping),
                hbmJoinedSubclass => hbmJoinedSubclass.Items);
        }

        [Test]
        public void ShouldConvertCollections_List()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, CollectionMapping, HbmJoinedSubclass, object, object>(
                () => new SubclassMapping(SubclassType.JoinedSubclass),
                () => CollectionMapping.List(),
                (subclassMapping, listMapping) => subclassMapping.AddCollection(listMapping),
                hbmJoinedSubclass => hbmJoinedSubclass.Items);
        }

        [Test]
        public void ShouldConvertCollections_Bag()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, CollectionMapping, HbmJoinedSubclass, object, object>(
                () => new SubclassMapping(SubclassType.JoinedSubclass),
                () => CollectionMapping.Bag(),
                (subclassMapping, bagMapping) => subclassMapping.AddCollection(bagMapping),
                hbmJoinedSubclass => hbmJoinedSubclass.Items);
        }

        [Test, Ignore("ShouldConvertCollections_IdBag")]
        public void ShouldConvertCollections_IdBag()
        {
            Assert.Fail("Target logic not yet available");
        }

        [Test]
        public void ShouldConvertCollections_Array()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, CollectionMapping, HbmJoinedSubclass, object, object>(
                () => new SubclassMapping(SubclassType.JoinedSubclass),
                () => CollectionMapping.Array(),
                (subclassMapping, bagMapping) => subclassMapping.AddCollection(bagMapping),
                hbmJoinedSubclass => hbmJoinedSubclass.Items);
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
                ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, SubclassMapping, HbmJoinedSubclass, HbmSubclass, object>(
                    () => new SubclassMapping(SubclassType.JoinedSubclass),
                    () => new SubclassMapping(SubclassType.Subclass),
                    (joinedSubclassMapping, subclassMapping) => joinedSubclassMapping.AddSubclass(subclassMapping),
                    hbmJoinedSubclass => hbmJoinedSubclass.joinedsubclass1)
            );
        }

        [Test]
        public void ShouldConvertSubclasses_JoinedSubclass()
        {
            ShouldConvertSubobjectsAsStrictlyTypedArray<SubclassMapping, SubclassMapping, HbmJoinedSubclass, HbmJoinedSubclass>(
                () => new SubclassMapping(SubclassType.JoinedSubclass),
                () => new SubclassMapping(SubclassType.JoinedSubclass),
                (joinedSubclassMapping1, joinedSubclassMapping2) => joinedSubclassMapping1.AddSubclass(joinedSubclassMapping2),
                hbmJoinedSubclass => hbmJoinedSubclass.joinedsubclass1);
        }

        [Test]
        public void ShouldConvertSubclasses_UnionSubclass()
        {
            Assert.Throws<NotSupportedException>(() =>
                ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, SubclassMapping, HbmJoinedSubclass, HbmUnionSubclass, object>(
                    () => new SubclassMapping(SubclassType.JoinedSubclass),
                    () => new SubclassMapping(SubclassType.UnionSubclass),
                    (joinedSubclassMapping, unionSubclassMapping) => joinedSubclassMapping.AddSubclass(unionSubclassMapping),
                    hbmJoinedSubclass => hbmJoinedSubclass.joinedsubclass1)
            );
        }

        [Test]
        public void ShouldConvertKeys()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<SubclassMapping, KeyMapping, HbmJoinedSubclass, HbmKey>(
                () => new SubclassMapping(SubclassType.JoinedSubclass),
                (subclassMapping, keyMapping) => subclassMapping.Set(fluent => fluent.Key, Layer.Conventions, keyMapping),
                hbmJoinedSubclass => hbmJoinedSubclass.key);
        }

        [Test]
        public void ShouldConvertStoredProcedure_SqlInsert()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<SubclassMapping, StoredProcedureMapping, HbmJoinedSubclass, HbmCustomSQL>(
                () => new SubclassMapping(SubclassType.JoinedSubclass),
                () => new StoredProcedureMapping("sql-insert", ""),
                (subclassMapping, storedProcedureMapping) => subclassMapping.AddStoredProcedure(storedProcedureMapping),
                hbmJoinedSubclass => hbmJoinedSubclass.sqlinsert);
        }

        [Test]
        public void ShouldConvertStoredProcedure_SqlUpdate()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<SubclassMapping, StoredProcedureMapping, HbmJoinedSubclass, HbmCustomSQL>(
                () => new SubclassMapping(SubclassType.JoinedSubclass),
                () => new StoredProcedureMapping("sql-update", ""),
                (subclassMapping, storedProcedureMapping) => subclassMapping.AddStoredProcedure(storedProcedureMapping),
                hbmJoinedSubclass => hbmJoinedSubclass.sqlupdate);
        }

        [Test]
        public void ShouldConvertStoredProcedure_SqlDelete()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<SubclassMapping, StoredProcedureMapping, HbmJoinedSubclass, HbmCustomSQL>(
                () => new SubclassMapping(SubclassType.JoinedSubclass),
                () => new StoredProcedureMapping("sql-delete", ""),
                (subclassMapping, storedProcedureMapping) => subclassMapping.AddStoredProcedure(storedProcedureMapping),
                hbmJoinedSubclass => hbmJoinedSubclass.sqldelete);
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
            converter = container.Resolve<IHbmConverter<SubclassMapping, HbmJoinedSubclass>>();
            container.Register<IHbmConverter<StoredProcedureMapping, HbmCustomSQL>>(cnvrt => fakeConverter);

            // Allocate the subclass mapping and a stored procedure submapping with an unsupported sptype
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            subclassMapping.AddStoredProcedure(new StoredProcedureMapping(unsupportedSPType, ""));

            // This should throw
            Assert.Throws<NotSupportedException>(() => converter.Convert(subclassMapping));

            // We don't care if it made a call to the subobject conversion logic or not (it is low enough cost that it doesn't
            // really matter in the case of failure, and some implementation approaches that uses this may be simpler).
        }

        #endregion Converter-based subobject tests
    }
}
