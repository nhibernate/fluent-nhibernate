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
    public class HbmDynamicComponentConverterTester
    {
        private IHbmConverter<ComponentMapping, HbmDynamicComponent> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<ComponentMapping, HbmDynamicComponent>>();
        }

        [Test]
        public void ShouldConvertNameIfPopulated()
        {
            var dynamicComponentMapping = new ComponentMapping(ComponentType.DynamicComponent);
            dynamicComponentMapping.Set(fluent => fluent.Name, Layer.Conventions, "name");
            var convertedHbmDynamicComponent = converter.Convert(dynamicComponentMapping);
            convertedHbmDynamicComponent.name.ShouldEqual(dynamicComponentMapping.Name);
        }

        [Test]
        public void ShouldNotConvertNameIfNotPopulated()
        {
            var dynamicComponentMapping = new ComponentMapping(ComponentType.DynamicComponent);
            // Don't set anything on the original mapping
            var convertedHbmDynamicComponent = converter.Convert(dynamicComponentMapping);
            var blankHbmDynamicComponent = new HbmDynamicComponent();
            convertedHbmDynamicComponent.name.ShouldEqual(blankHbmDynamicComponent.name);
        }

        [Test]
        public void ShouldConvertAccessIfPopulated()
        {
            var dynamicComponentMapping = new ComponentMapping(ComponentType.DynamicComponent);
            dynamicComponentMapping.Set(fluent => fluent.Access, Layer.Conventions, "acc");
            var convertedHbmDynamicComponent = converter.Convert(dynamicComponentMapping);
            convertedHbmDynamicComponent.access.ShouldEqual(dynamicComponentMapping.Access);
        }

        [Test]
        public void ShouldNotConvertAccessIfNotPopulated()
        {
            var dynamicComponentMapping = new ComponentMapping(ComponentType.DynamicComponent);
            // Don't set anything on the original mapping
            var convertedHbmDynamicComponent = converter.Convert(dynamicComponentMapping);
            var blankHbmDynamicComponent = new HbmDynamicComponent();
            convertedHbmDynamicComponent.access.ShouldEqual(blankHbmDynamicComponent.access);
        }

        [Test]
        public void ShouldConvertUpdateIfPopulated()
        {
            var dynamicComponentMapping = new ComponentMapping(ComponentType.DynamicComponent);
            dynamicComponentMapping.Set(fluent => fluent.Update, Layer.Conventions, false); // Defaults to true, so use this to ensure that we can detect changes
            var convertedHbmDynamicComponent = converter.Convert(dynamicComponentMapping);
            convertedHbmDynamicComponent.update.ShouldEqual(dynamicComponentMapping.Update);
        }

        [Test]
        public void ShouldNotConvertUpdateIfNotPopulated()
        {
            var dynamicComponentMapping = new ComponentMapping(ComponentType.DynamicComponent);
            // Don't set anything on the original mapping
            var convertedHbmDynamicComponent = converter.Convert(dynamicComponentMapping);
            var blankHbmDynamicComponent = new HbmDynamicComponent();
            convertedHbmDynamicComponent.update.ShouldEqual(blankHbmDynamicComponent.update);
        }

        [Test]
        public void ShouldConvertInsertIfPopulated()
        {
            var dynamicComponentMapping = new ComponentMapping(ComponentType.DynamicComponent);
            dynamicComponentMapping.Set(fluent => fluent.Insert, Layer.Conventions, false); // Defaults to true, so use this to ensure that we can detect changes
            var convertedHbmDynamicComponent = converter.Convert(dynamicComponentMapping);
            convertedHbmDynamicComponent.insert.ShouldEqual(dynamicComponentMapping.Insert);
        }

        [Test]
        public void ShouldNotConvertInsertIfNotPopulated()
        {
            var dynamicComponentMapping = new ComponentMapping(ComponentType.DynamicComponent);
            // Don't set anything on the original mapping
            var convertedHbmDynamicComponent = converter.Convert(dynamicComponentMapping);
            var blankHbmDynamicComponent = new HbmDynamicComponent();
            convertedHbmDynamicComponent.insert.ShouldEqual(blankHbmDynamicComponent.insert);
        }

        [Test]
        public void ShouldConvertOptimisticLockIfPopulated()
        {
            var dynamicComponentMapping = new ComponentMapping(ComponentType.DynamicComponent);
            dynamicComponentMapping.Set(fluent => fluent.OptimisticLock, Layer.Conventions, false); // Defaults to true, so use this to ensure that we can detect changes
            var convertedHbmDynamicComponent = converter.Convert(dynamicComponentMapping);
            convertedHbmDynamicComponent.optimisticlock.ShouldEqual(dynamicComponentMapping.OptimisticLock);
        }

        [Test]
        public void ShouldNotConvertOptimisticLockIfNotPopulated()
        {
            var dynamicComponentMapping = new ComponentMapping(ComponentType.DynamicComponent);
            // Don't set anything on the original mapping
            var convertedHbmDynamicComponent = converter.Convert(dynamicComponentMapping);
            var blankHbmDynamicComponent = new HbmDynamicComponent();
            convertedHbmDynamicComponent.optimisticlock.ShouldEqual(blankHbmDynamicComponent.optimisticlock);
        }

        [Test]
        public void ShouldConvertComponents_Component()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<ComponentMapping, IComponentMapping, HbmDynamicComponent, object, object>(
                () => new ComponentMapping(ComponentType.DynamicComponent),
                () => new ComponentMapping(ComponentType.Component),
                (dynamicComponentMapping, componentMapping) => dynamicComponentMapping.AddComponent(componentMapping),
                hbmDynamicComponent => hbmDynamicComponent.Items);
        }

        [Test]
        public void ShouldConvertComponents_DynamicComponent()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<ComponentMapping, IComponentMapping, HbmDynamicComponent, object, object>(
                () => new ComponentMapping(ComponentType.DynamicComponent),
                () => new ComponentMapping(ComponentType.DynamicComponent),
                (dynamicComponentMapping1, dynamicComponentMapping2) => dynamicComponentMapping1.AddComponent(dynamicComponentMapping2),
                hbmDynamicComponent => hbmDynamicComponent.Items);
        }

        [Test]
        public void ShouldConvertProperties()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<ComponentMapping, PropertyMapping, HbmDynamicComponent, HbmProperty, object>(
                () => new ComponentMapping(ComponentType.DynamicComponent),
                (dynamicComponentMapping, propertyMapping) => dynamicComponentMapping.AddProperty(propertyMapping),
                hbmDynamicComponent => hbmDynamicComponent.Items);
        }

        [Test]
        public void ShouldConvertManyToOnes()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<ComponentMapping, ManyToOneMapping, HbmDynamicComponent, HbmManyToOne, object>(
                () => new ComponentMapping(ComponentType.DynamicComponent),
                (dynamicComponentMapping, manyToOneMapping) => dynamicComponentMapping.AddReference(manyToOneMapping),
                hbmDynamicComponent => hbmDynamicComponent.Items);
        }

        [Test]
        public void ShouldConvertOneToOnes()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<ComponentMapping, OneToOneMapping, HbmDynamicComponent, HbmOneToOne, object>(
                () => new ComponentMapping(ComponentType.DynamicComponent),
                (dynamicComponentMapping, oneToOneMapping) => dynamicComponentMapping.AddOneToOne(oneToOneMapping),
                hbmDynamicComponent => hbmDynamicComponent.Items);
        }

        [Test]
        public void ShouldConvertAnys()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<ComponentMapping, AnyMapping, HbmDynamicComponent, HbmAny, object>(
                () => new ComponentMapping(ComponentType.DynamicComponent),
                (dynamicComponentMapping, anyMapping) => dynamicComponentMapping.AddAny(anyMapping),
                hbmDynamicComponent => hbmDynamicComponent.Items);
        }

        [Test]
        public void ShouldConvertCollections_Map()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<ComponentMapping, CollectionMapping, HbmDynamicComponent, object, object>(
                () => new ComponentMapping(ComponentType.DynamicComponent),
                () => CollectionMapping.Map(),
                (dynamicComponentMapping, mapMapping) => dynamicComponentMapping.AddCollection(mapMapping),
                hbmDynamicComponent => hbmDynamicComponent.Items);
        }

        [Test]
        public void ShouldConvertCollections_Set()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<ComponentMapping, CollectionMapping, HbmDynamicComponent, object, object>(
                () => new ComponentMapping(ComponentType.DynamicComponent),
                () => CollectionMapping.Set(),
                (dynamicComponentMapping, setMapping) => dynamicComponentMapping.AddCollection(setMapping),
                hbmDynamicComponent => hbmDynamicComponent.Items);
        }

        [Test]
        public void ShouldConvertCollections_Bag()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<ComponentMapping, CollectionMapping, HbmDynamicComponent, object, object>(
                () => new ComponentMapping(ComponentType.DynamicComponent),
                () => CollectionMapping.Bag(),
                (dynamicComponentMapping, bagMapping) => dynamicComponentMapping.AddCollection(bagMapping),
                hbmDynamicComponent => hbmDynamicComponent.Items);
        }

        [Test]
        public void ShouldConvertCollections_List()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<ComponentMapping, CollectionMapping, HbmDynamicComponent, object, object>(
                () => new ComponentMapping(ComponentType.DynamicComponent),
                () => CollectionMapping.List(),
                (dynamicComponentMapping, listMapping) => dynamicComponentMapping.AddCollection(listMapping),
                hbmDynamicComponent => hbmDynamicComponent.Items);
        }

        [Test, Ignore("ShouldConvertCollections_IdBag")]
        public void ShouldConvertCollections_IdBag()
        {
            Assert.Fail("Target logic not yet available");
        }

        [Test]
        public void ShouldConvertCollections_Array()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<ComponentMapping, CollectionMapping, HbmDynamicComponent, object, object>(
                () => new ComponentMapping(ComponentType.DynamicComponent),
                () => CollectionMapping.Array(),
                (dynamicComponentMapping, bagMapping) => dynamicComponentMapping.AddCollection(bagMapping),
                hbmDynamicComponent => hbmDynamicComponent.Items);
        }

        [Test, Ignore("ShouldConvertCollections_PrimitiveArray")]
        public void ShouldConvertCollections_PrimitiveArray()
        {
            Assert.Fail("Not yet fully reviewed");
        }
    }
}