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
    public class HbmComponentConverterTester
    {
        private IHbmConverter<ComponentMapping, HbmComponent> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<ComponentMapping, HbmComponent>>();
        }

        [Test]
        public void ShouldConvertNameIfPopulated()
        {
            var componentMapping = new ComponentMapping(ComponentType.Component);
            componentMapping.Set(fluent => fluent.Name, Layer.Conventions, "name");
            var convertedHbmComponent = converter.Convert(componentMapping);
            convertedHbmComponent.name.ShouldEqual(componentMapping.Name);
        }

        [Test]
        public void ShouldNotConvertNameIfNotPopulated()
        {
            var componentMapping = new ComponentMapping(ComponentType.Component);
            // Don't set anything on the original mapping
            var convertedHbmComponent = converter.Convert(componentMapping);
            var blankHbmComponent = new HbmComponent();
            convertedHbmComponent.name.ShouldEqual(blankHbmComponent.name);
        }

        [Test]
        public void ShouldConvertAccessIfPopulated()
        {
            var componentMapping = new ComponentMapping(ComponentType.Component);
            componentMapping.Set(fluent => fluent.Access, Layer.Conventions, "acc");
            var convertedHbmComponent = converter.Convert(componentMapping);
            convertedHbmComponent.access.ShouldEqual(componentMapping.Access);
        }

        [Test]
        public void ShouldNotConvertAccessIfNotPopulated()
        {
            var componentMapping = new ComponentMapping(ComponentType.Component);
            // Don't set anything on the original mapping
            var convertedHbmComponent = converter.Convert(componentMapping);
            var blankHbmComponent = new HbmComponent();
            convertedHbmComponent.access.ShouldEqual(blankHbmComponent.access);
        }

        [Test]
        public void ShouldConvertClassIfPopulated()
        {
            var componentMapping = new ComponentMapping(ComponentType.Component);
            componentMapping.Set(fluent => fluent.Class, Layer.Conventions, new TypeReference("class"));
            var convertedHbmComponent = converter.Convert(componentMapping);
            convertedHbmComponent.@class.ShouldEqual(componentMapping.Class);
        }

        [Test]
        public void ShouldNotConvertClassIfNotPopulated()
        {
            var componentMapping = new ComponentMapping(ComponentType.Component);
            // Don't set anything on the original mapping
            var convertedHbmComponent = converter.Convert(componentMapping);
            var blankHbmComponent = new HbmComponent();
            convertedHbmComponent.@class.ShouldEqual(blankHbmComponent.@class);
        }

        [Test]
        public void ShouldConvertUpdateIfPopulated()
        {
            var componentMapping = new ComponentMapping(ComponentType.Component);
            componentMapping.Set(fluent => fluent.Update, Layer.Conventions, false); // Defaults to true, so use this to ensure that we can detect changes
            var convertedHbmComponent = converter.Convert(componentMapping);
            convertedHbmComponent.update.ShouldEqual(componentMapping.Update);
        }

        [Test]
        public void ShouldNotConvertUpdateIfNotPopulated()
        {
            var componentMapping = new ComponentMapping(ComponentType.Component);
            // Don't set anything on the original mapping
            var convertedHbmComponent = converter.Convert(componentMapping);
            var blankHbmComponent = new HbmComponent();
            convertedHbmComponent.update.ShouldEqual(blankHbmComponent.update);
        }

        [Test]
        public void ShouldConvertInsertIfPopulated()
        {
            var componentMapping = new ComponentMapping(ComponentType.Component);
            componentMapping.Set(fluent => fluent.Insert, Layer.Conventions, false); // Defaults to true, so use this to ensure that we can detect changes
            var convertedHbmComponent = converter.Convert(componentMapping);
            convertedHbmComponent.insert.ShouldEqual(componentMapping.Insert);
        }

        [Test]
        public void ShouldNotConvertInsertIfNotPopulated()
        {
            var componentMapping = new ComponentMapping(ComponentType.Component);
            // Don't set anything on the original mapping
            var convertedHbmComponent = converter.Convert(componentMapping);
            var blankHbmComponent = new HbmComponent();
            convertedHbmComponent.insert.ShouldEqual(blankHbmComponent.insert);
        }

        [Test]
        public void ShouldConvertLazyIfPopulated()
        {
            var componentMapping = new ComponentMapping(ComponentType.Component);
            componentMapping.Set(fluent => fluent.Lazy, Layer.Conventions, true); // Defaults to false, so use this to ensure that we can detect changes
            var convertedHbmComponent = converter.Convert(componentMapping);
            convertedHbmComponent.lazy.ShouldEqual(componentMapping.Lazy);
        }

        [Test]
        public void ShouldNotConvertLazyIfNotPopulated()
        {
            var componentMapping = new ComponentMapping(ComponentType.Component);
            // Don't set anything on the original mapping
            var convertedHbmComponent = converter.Convert(componentMapping);
            var blankHbmComponent = new HbmComponent();
            convertedHbmComponent.lazy.ShouldEqual(blankHbmComponent.lazy);
        }

        [Test]
        public void ShouldConvertOptimisticLockIfPopulated()
        {
            var componentMapping = new ComponentMapping(ComponentType.Component);
            componentMapping.Set(fluent => fluent.OptimisticLock, Layer.Conventions, false); // Defaults to true, so use this to ensure that we can detect changes
            var convertedHbmComponent = converter.Convert(componentMapping);
            convertedHbmComponent.optimisticlock.ShouldEqual(componentMapping.OptimisticLock);
        }

        [Test]
        public void ShouldNotConvertOptimisticLockIfNotPopulated()
        {
            var componentMapping = new ComponentMapping(ComponentType.Component);
            // Don't set anything on the original mapping
            var convertedHbmComponent = converter.Convert(componentMapping);
            var blankHbmComponent = new HbmComponent();
            convertedHbmComponent.optimisticlock.ShouldEqual(blankHbmComponent.optimisticlock);
        }

        [Test]
        public void ShouldConvertComponents_Component()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<ComponentMapping, IComponentMapping, HbmComponent, object, object>(
                () => new ComponentMapping(ComponentType.Component),
                () => new ComponentMapping(ComponentType.Component),
                (componentMapping1, componentMapping2) => componentMapping1.AddComponent(componentMapping2),
                hbmComponent => hbmComponent.Items);
        }

        [Test]
        public void ShouldConvertComponents_DynamicComponent()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<ComponentMapping, IComponentMapping, HbmComponent, object, object>(
                () => new ComponentMapping(ComponentType.Component),
                () => new ComponentMapping(ComponentType.DynamicComponent),
                (componentMapping, dynamicComponentMapping) => componentMapping.AddComponent(dynamicComponentMapping),
                hbmComponent => hbmComponent.Items);
        }

        [Test]
        public void ShouldConvertParent()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<ComponentMapping, ParentMapping, HbmComponent, HbmParent>(
                () => new ComponentMapping(ComponentType.Component),
                (componentMapping, parentMapping) => componentMapping.Set(fluent => fluent.Parent, Layer.Conventions, parentMapping),
                hbmComponent => hbmComponent.parent);
        }

        [Test]
        public void ShouldConvertProperties()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<ComponentMapping, PropertyMapping, HbmComponent, HbmProperty, object>(
                () => new ComponentMapping(ComponentType.Component),
                (componentMapping, propertyMapping) => componentMapping.AddProperty(propertyMapping),
                hbmComponent => hbmComponent.Items);
        }

        [Test]
        public void ShouldConvertManyToOnes()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<ComponentMapping, ManyToOneMapping, HbmComponent, HbmManyToOne, object>(
                () => new ComponentMapping(ComponentType.Component),
                (componentMapping, manyToOneMapping) => componentMapping.AddReference(manyToOneMapping),
                hbmComponent => hbmComponent.Items);
        }

        [Test]
        public void ShouldConvertOneToOnes()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<ComponentMapping, OneToOneMapping, HbmComponent, HbmOneToOne, object>(
                () => new ComponentMapping(ComponentType.Component),
                (componentMapping, oneToOneMapping) => componentMapping.AddOneToOne(oneToOneMapping),
                hbmComponent => hbmComponent.Items);
        }

        [Test]
        public void ShouldConvertAnys()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<ComponentMapping, AnyMapping, HbmComponent, HbmAny, object>(
                () => new ComponentMapping(ComponentType.Component),
                (componentMapping, anyMapping) => componentMapping.AddAny(anyMapping),
                hbmComponent => hbmComponent.Items);
        }

        [Test]
        public void ShouldConvertCollections_Map()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<ComponentMapping, CollectionMapping, HbmComponent, object, object>(
                () => new ComponentMapping(ComponentType.Component),
                () => CollectionMapping.Map(),
                (componentMapping, mapMapping) => componentMapping.AddCollection(mapMapping),
                hbmComponent => hbmComponent.Items);
        }

        [Test]
        public void ShouldConvertCollections_Set()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<ComponentMapping, CollectionMapping, HbmComponent, object, object>(
                () => new ComponentMapping(ComponentType.Component),
                () => CollectionMapping.Set(),
                (componentMapping, setMapping) => componentMapping.AddCollection(setMapping),
                hbmComponent => hbmComponent.Items);
        }

        [Test]
        public void ShouldConvertCollections_Bag()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<ComponentMapping, CollectionMapping, HbmComponent, object, object>(
                () => new ComponentMapping(ComponentType.Component),
                () => CollectionMapping.Bag(),
                (componentMapping, bagMapping) => componentMapping.AddCollection(bagMapping),
                hbmComponent => hbmComponent.Items);
        }

        [Test]
        public void ShouldConvertCollections_List()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<ComponentMapping, CollectionMapping, HbmComponent, object, object>(
                () => new ComponentMapping(ComponentType.Component),
                () => CollectionMapping.List(),
                (componentMapping, listMapping) => componentMapping.AddCollection(listMapping),
                hbmComponent => hbmComponent.Items);
        }

        [Test, Ignore("ShouldConvertCollections_IdBag")]
        public void ShouldConvertCollections_IdBag()
        {
            Assert.Fail("Target logic not yet available");
        }

        [Test]
        public void ShouldConvertCollections_Array()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<ComponentMapping, CollectionMapping, HbmComponent, object, object>(
                () => new ComponentMapping(ComponentType.Component),
                () => CollectionMapping.Array(),
                (componentMapping, bagMapping) => componentMapping.AddCollection(bagMapping),
                hbmComponent => hbmComponent.Items);
        }

        [Test, Ignore("ShouldConvertCollections_PrimitiveArray")]
        public void ShouldConvertCollections_PrimitiveArray()
        {
            Assert.Fail("Not yet fully reviewed");
        }
    }
}