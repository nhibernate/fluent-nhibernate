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
    public class HbmBasicSubclassConverterTester
    {
        private IHbmConverter<SubclassMapping, HbmSubclass> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<SubclassMapping, HbmSubclass>>();
        }

        #region Value field tests

        [Test]
        public void ShouldConvertDiscriminatorValueIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.Subclass);
            subclassMapping.Set(fluent => fluent.DiscriminatorValue, Layer.Conventions, "val");
            var convertedHbmSubclass = converter.Convert(subclassMapping);
            convertedHbmSubclass.discriminatorvalue.ShouldEqual(subclassMapping.DiscriminatorValue.ToString());
        }

        [Test]
        public void ShouldNotConvertDiscriminatorValueIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.Subclass);
            // Don't set anything on the original mapping
            var convertedHbmSubclass = converter.Convert(subclassMapping);
            var blankHbmSubclass = new HbmSubclass();
            convertedHbmSubclass.discriminatorvalue.ShouldEqual(blankHbmSubclass.discriminatorvalue);
        }

        [Test]
        public void ShouldConvertNameIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.Subclass);
            subclassMapping.Set(fluent => fluent.Name, Layer.Conventions, "name");
            var convertedHbmSubclass = converter.Convert(subclassMapping);
            convertedHbmSubclass.name.ShouldEqual(subclassMapping.Name);
        }

        [Test]
        public void ShouldNotConvertNameIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.Subclass);
            // Don't set anything on the original mapping
            var convertedHbmSubclass = converter.Convert(subclassMapping);
            var blankHbmSubclass = new HbmSubclass();
            convertedHbmSubclass.name.ShouldEqual(blankHbmSubclass.name);
        }

        [Test]
        public void ShouldConvertProxyIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.Subclass);
            subclassMapping.Set(fluent => fluent.Proxy, Layer.Conventions, "p");
            var convertedHbmSubclass = converter.Convert(subclassMapping);
            convertedHbmSubclass.proxy.ShouldEqual(subclassMapping.Proxy);
        }

        [Test]
        public void ShouldNotConvertProxyIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.Subclass);
            // Don't set anything on the original mapping
            var convertedHbmSubclass = converter.Convert(subclassMapping);
            var blankHbmSubclass = new HbmSubclass();
            convertedHbmSubclass.proxy.ShouldEqual(blankHbmSubclass.proxy);
        }

        [Test]
        public void ShouldConvertLazyIfPopulated_True()
        {
            var subclassMapping = new SubclassMapping(SubclassType.Subclass);
            subclassMapping.Set(fluent => fluent.Lazy, Layer.Conventions, true);
            var convertedHbmSubclass = converter.Convert(subclassMapping);
            convertedHbmSubclass.lazy.ShouldEqual(subclassMapping.Lazy);
            Assert.That(convertedHbmSubclass.lazySpecified.Equals(true), "Lazy was not marked as specified");
        }

        [Test]
        public void ShouldConvertLazyIfPopulated_False()
        {
            var subclassMapping = new SubclassMapping(SubclassType.Subclass);
            subclassMapping.Set(fluent => fluent.Lazy, Layer.Conventions, false);
            var convertedHbmSubclass = converter.Convert(subclassMapping);
            convertedHbmSubclass.lazy.ShouldEqual(subclassMapping.Lazy);
            Assert.That(convertedHbmSubclass.lazySpecified.Equals(true), "Lazy was not marked as specified");
        }

        [Test]
        public void ShouldNotConvertLazyIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.Subclass);
            // Don't set anything on the original mapping
            var convertedHbmSubclass = converter.Convert(subclassMapping);
            var blankHbmSubclass = new HbmSubclass();
            convertedHbmSubclass.lazy.ShouldEqual(blankHbmSubclass.lazy);
            Assert.That(convertedHbmSubclass.lazySpecified.Equals(false), "Batch size was marked as specified");
        }

        [Test]
        public void ShouldConvertDynamicUpdateIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.Subclass);
            subclassMapping.Set(fluent => fluent.DynamicUpdate, Layer.Conventions, true); // Defaults to false, so we have to set it true here in order to tell if it actually changed
            var convertedHbmSubclass = converter.Convert(subclassMapping);
            convertedHbmSubclass.dynamicupdate.ShouldEqual(subclassMapping.DynamicUpdate);
        }

        [Test]
        public void ShouldNotConvertDynamicUpdateIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.Subclass);
            // Don't set anything on the original mapping
            var convertedHbmSubclass = converter.Convert(subclassMapping);
            var blankHbmSubclass = new HbmSubclass();
            convertedHbmSubclass.dynamicupdate.ShouldEqual(blankHbmSubclass.dynamicupdate);
        }

        [Test]
        public void ShouldConvertDynamicInsertIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.Subclass);
            subclassMapping.Set(fluent => fluent.DynamicInsert, Layer.Conventions, true); // Defaults to false, so we have to set it true here in order to tell if it actually changed
            var convertedHbmSubclass = converter.Convert(subclassMapping);
            convertedHbmSubclass.dynamicinsert.ShouldEqual(subclassMapping.DynamicInsert);
        }

        [Test]
        public void ShouldNotConvertDynamicInsertIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.Subclass);
            // Don't set anything on the original mapping
            var convertedHbmSubclass = converter.Convert(subclassMapping);
            var blankHbmSubclass = new HbmSubclass();
            convertedHbmSubclass.dynamicinsert.ShouldEqual(blankHbmSubclass.dynamicinsert);
        }

        [Test]
        public void ShouldConvertSelectBeforeUpdateIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.Subclass);
            subclassMapping.Set(fluent => fluent.SelectBeforeUpdate, Layer.Conventions, true); // Defaults to false, so we have to set it true here in order to tell if it actually changed
            var convertedHbmSubclass = converter.Convert(subclassMapping);
            convertedHbmSubclass.selectbeforeupdate.ShouldEqual(subclassMapping.SelectBeforeUpdate);
        }

        [Test]
        public void ShouldNotConvertSelectBeforeUpdateIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.Subclass);
            // Don't set anything on the original mapping
            var convertedHbmSubclass = converter.Convert(subclassMapping);
            var blankHbmSubclass = new HbmSubclass();
            convertedHbmSubclass.selectbeforeupdate.ShouldEqual(blankHbmSubclass.selectbeforeupdate);
        }

        [Test]
        public void ShouldConvertAbstractIfPopulated_True()
        {
            var subclassMapping = new SubclassMapping(SubclassType.Subclass);
            subclassMapping.Set(fluent => fluent.Abstract, Layer.Conventions, true);
            var convertedHbmSubclass = converter.Convert(subclassMapping);
            convertedHbmSubclass.@abstract.ShouldEqual(subclassMapping.Abstract);
            Assert.That(convertedHbmSubclass.abstractSpecified.Equals(true), "Abstract was not marked as specified");
        }

        [Test]
        public void ShouldConvertAbstractIfPopulated_False()
        {
            var subclassMapping = new SubclassMapping(SubclassType.Subclass);
            subclassMapping.Set(fluent => fluent.Abstract, Layer.Conventions, false);
            var convertedHbmSubclass = converter.Convert(subclassMapping);
            convertedHbmSubclass.@abstract.ShouldEqual(subclassMapping.Abstract);
            Assert.That(convertedHbmSubclass.abstractSpecified.Equals(true), "Abstract was not marked as specified");
        }

        [Test]
        public void ShouldNotConvertAbstractIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.Subclass);
            // Don't set anything on the original mapping
            var convertedHbmSubclass = converter.Convert(subclassMapping);
            var blankHbmSubclass = new HbmSubclass();
            convertedHbmSubclass.@abstract.ShouldEqual(blankHbmSubclass.@abstract);
            Assert.That(convertedHbmSubclass.abstractSpecified.Equals(false), "Abstract was marked as specified");
        }

        [Test]
        public void ShouldConvertEntityNameIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.Subclass);
            subclassMapping.Set(fluent => fluent.EntityName, Layer.Conventions, "entity1");
            var convertedHbmSubclass = converter.Convert(subclassMapping);
            convertedHbmSubclass.entityname.ShouldEqual(subclassMapping.EntityName);
        }

        [Test]
        public void ShouldNotConvertEntityNameIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.Subclass);
            // Don't set anything on the original mapping
            var convertedHbmSubclass = converter.Convert(subclassMapping);
            var blankHbmSubclass = new HbmSubclass();
            convertedHbmSubclass.entityname.ShouldEqual(blankHbmSubclass.entityname);
        }

        [Test]
        public void ShouldConvertBatchSizeIfPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.Subclass);
            subclassMapping.Set(fluent => fluent.BatchSize, Layer.Conventions, 10);
            var convertedHbmSubclass = converter.Convert(subclassMapping);
            convertedHbmSubclass.batchsize.ShouldEqual(subclassMapping.BatchSize.ToString());
        }

        [Test]
        public void ShouldNotConvertBatchSizeIfNotPopulated()
        {
            var subclassMapping = new SubclassMapping(SubclassType.Subclass);
            // Don't set anything on the original mapping
            var convertedHbmSubclass = converter.Convert(subclassMapping);
            var blankHbmSubclass = new HbmSubclass();
            convertedHbmSubclass.batchsize.ShouldEqual(blankHbmSubclass.batchsize);
        }

        #endregion Value field tests

        #region Converter-based subobject tests

        [Test]
        public void ShouldConvertProperties()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, PropertyMapping, HbmSubclass, HbmProperty, object>(
                () => new SubclassMapping(SubclassType.Subclass),
                (subclassMapping, propertyMapping) => subclassMapping.AddProperty(propertyMapping),
                hbmSubclass => hbmSubclass.Items);
        }

        [Test]
        public void ShouldConvertManyToOnes()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, ManyToOneMapping, HbmSubclass, HbmManyToOne, object>(
                () => new SubclassMapping(SubclassType.Subclass),
                (subclassMapping, manyToOneMapping) => subclassMapping.AddReference(manyToOneMapping),
                hbmSubclass => hbmSubclass.Items);
        }

        [Test]
        public void ShouldConvertOneToOnes()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, OneToOneMapping, HbmSubclass, HbmOneToOne, object>(
                () => new SubclassMapping(SubclassType.Subclass),
                (subclassMapping, oneToOneMapping) => subclassMapping.AddOneToOne(oneToOneMapping),
                hbmSubclass => hbmSubclass.Items);
        }

        [Test]
        public void ShouldConvertComponents_Component()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, IComponentMapping, HbmSubclass, object, object>(
                () => new SubclassMapping(SubclassType.Subclass),
                () => new ComponentMapping(ComponentType.Component),
                (subclassMapping, componentMapping) => subclassMapping.AddComponent(componentMapping),
                hbmSubclass => hbmSubclass.Items);
        }

        [Test]
        public void ShouldConvertComponents_DynamicComponent()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, IComponentMapping, HbmSubclass, object, object>(
                () => new SubclassMapping(SubclassType.Subclass),
                () => new ComponentMapping(ComponentType.DynamicComponent),
                (subclassMapping, componentMapping) => subclassMapping.AddComponent(componentMapping),
                hbmSubclass => hbmSubclass.Items);
        }

        [Test]
        public void ShouldConvertAnys()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, AnyMapping, HbmSubclass, HbmAny, object>(
                () => new SubclassMapping(SubclassType.Subclass),
                (subclassMapping, anyMapping) => subclassMapping.AddAny(anyMapping),
                hbmSubclass => hbmSubclass.Items);
        }

        [Test]
        public void ShouldConvertCollections_Map()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, CollectionMapping, HbmSubclass, object, object>(
                () => new SubclassMapping(SubclassType.Subclass),
                () => CollectionMapping.Map(),
                (subclassMapping, mapMapping) => subclassMapping.AddCollection(mapMapping),
                hbmSubclass => hbmSubclass.Items);
        }

        [Test]
        public void ShouldConvertCollections_Set()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, CollectionMapping, HbmSubclass, object, object>(
                () => new SubclassMapping(SubclassType.Subclass),
                () => CollectionMapping.Set(),
                (subclassMapping, setMapping) => subclassMapping.AddCollection(setMapping),
                hbmSubclass => hbmSubclass.Items);
        }

        [Test]
        public void ShouldConvertCollections_List()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, CollectionMapping, HbmSubclass, object, object>(
                () => new SubclassMapping(SubclassType.Subclass),
                () => CollectionMapping.List(),
                (subclassMapping, listMapping) => subclassMapping.AddCollection(listMapping),
                hbmSubclass => hbmSubclass.Items);
        }

        [Test]
        public void ShouldConvertCollections_Bag()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, CollectionMapping, HbmSubclass, object, object>(
                () => new SubclassMapping(SubclassType.Subclass),
                () => CollectionMapping.Bag(),
                (subclassMapping, bagMapping) => subclassMapping.AddCollection(bagMapping),
                hbmSubclass => hbmSubclass.Items);
        }

        [Test, Ignore("ShouldConvertCollections_IdBag")]
        public void ShouldConvertCollections_IdBag()
        {
            Assert.Fail("Target logic not yet available");
        }

        [Test]
        public void ShouldConvertCollections_Array()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, CollectionMapping, HbmSubclass, object, object>(
                () => new SubclassMapping(SubclassType.Subclass),
                () => CollectionMapping.Array(),
                (subclassMapping, bagMapping) => subclassMapping.AddCollection(bagMapping),
                hbmSubclass => hbmSubclass.Items);
        }

        [Test, Ignore("ShouldConvertCollections_PrimitiveArray")]
        public void ShouldConvertCollections_PrimitiveArray()
        {
            Assert.Fail("Target logic not yet available");
        }

        [Test]
        public void ShouldConvertJoins()
        {
            ShouldConvertSubobjectsAsStrictlyTypedArray<SubclassMapping, JoinMapping, HbmSubclass, HbmJoin>(
                () => new SubclassMapping(SubclassType.Subclass),
                (subclassMapping, joinMapping) => subclassMapping.AddJoin(joinMapping),
                hbmSubclass => hbmSubclass.join);
        }

        [Test]
        public void ShouldConvertSubclasses_Subclass()
        {
            ShouldConvertSubobjectsAsStrictlyTypedArray<SubclassMapping, SubclassMapping, HbmSubclass, HbmSubclass>(
                () => new SubclassMapping(SubclassType.Subclass),
                () => new SubclassMapping(SubclassType.Subclass),
                (subclassMapping1, subclassMapping2) => subclassMapping1.AddSubclass(subclassMapping2),
                hbmSubclass => hbmSubclass.subclass1);
        }

        [Test]
        public void ShouldConvertSubclasses_JoinedSubclass()
        {
            Assert.Throws<NotSupportedException>(() => 
                ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, SubclassMapping, HbmSubclass, HbmJoinedSubclass, object>(
                    () => new SubclassMapping(SubclassType.Subclass),
                    () => new SubclassMapping(SubclassType.JoinedSubclass),
                    (subclassMapping, joinedSubclassMapping) => subclassMapping.AddSubclass(joinedSubclassMapping),
                    hbmSubclass => hbmSubclass.subclass1)
            );
        }

        [Test]
        public void ShouldConvertSubclasses_UnionSubclass()
        {
            Assert.Throws<NotSupportedException>(() =>
                ShouldConvertSubobjectsAsLooselyTypedArray<SubclassMapping, SubclassMapping, HbmSubclass, HbmUnionSubclass, object>(
                    () => new SubclassMapping(SubclassType.Subclass),
                    () => new SubclassMapping(SubclassType.UnionSubclass),
                    (subclassMapping, unionSubclassMapping) => subclassMapping.AddSubclass(unionSubclassMapping),
                    hbmSubclass => hbmSubclass.subclass1)
            );
        }

        [Test]
        public void ShouldConvertStoredProcedure_SqlInsert()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<SubclassMapping, StoredProcedureMapping, HbmSubclass, HbmCustomSQL>(
                () => new SubclassMapping(SubclassType.Subclass),
                () => new StoredProcedureMapping("sql-insert", ""),
                (subclassMapping, storedProcedureMapping) => subclassMapping.AddStoredProcedure(storedProcedureMapping),
                hbmSubclass => hbmSubclass.sqlinsert);
        }

        [Test]
        public void ShouldConvertStoredProcedure_SqlUpdate()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<SubclassMapping, StoredProcedureMapping, HbmSubclass, HbmCustomSQL>(
                () => new SubclassMapping(SubclassType.Subclass),
                () => new StoredProcedureMapping("sql-update", ""),
                (subclassMapping, storedProcedureMapping) => subclassMapping.AddStoredProcedure(storedProcedureMapping),
                hbmSubclass => hbmSubclass.sqlupdate);
        }

        [Test]
        public void ShouldConvertStoredProcedure_SqlDelete()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<SubclassMapping, StoredProcedureMapping, HbmSubclass, HbmCustomSQL>(
                () => new SubclassMapping(SubclassType.Subclass),
                () => new StoredProcedureMapping("sql-delete", ""),
                (subclassMapping, storedProcedureMapping) => subclassMapping.AddStoredProcedure(storedProcedureMapping),
                hbmSubclass => hbmSubclass.sqldelete);
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
            converter = container.Resolve<IHbmConverter<SubclassMapping, HbmSubclass>>();
            container.Register<IHbmConverter<StoredProcedureMapping, HbmCustomSQL>>(cnvrt => fakeConverter);

            // Allocate the subclass mapping and a stored procedure submapping with an unsupported sptype
            var subclassMapping = new SubclassMapping(SubclassType.Subclass);
            subclassMapping.AddStoredProcedure(new StoredProcedureMapping(unsupportedSPType, ""));

            // This should throw
            Assert.Throws<NotSupportedException>(() => converter.Convert(subclassMapping));

            // We don't care if it made a call to the subobject conversion logic or not (it is low enough cost that it doesn't
            // really matter in the case of failure, and some implementation approaches that uses this may be simpler).
        }

        #endregion Converter-based subobject tests
    }
}
