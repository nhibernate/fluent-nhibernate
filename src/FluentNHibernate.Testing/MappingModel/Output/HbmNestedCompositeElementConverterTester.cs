using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;
using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmNestedCompositeElementConverterTester
    {
        private IHbmConverter<NestedCompositeElementMapping, HbmNestedCompositeElement> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<NestedCompositeElementMapping, HbmNestedCompositeElement>>();
        }

        [Test]
        public void ShouldConvertClassIfPopulated()
        {
            var nestedCompositeElementMapping = new NestedCompositeElementMapping();
            nestedCompositeElementMapping.Set(fluent => fluent.Class, Layer.Conventions, new TypeReference("t"));
            var convertedHbmNestedCompositeElement = converter.Convert(nestedCompositeElementMapping);
            convertedHbmNestedCompositeElement.@class.ShouldEqual(nestedCompositeElementMapping.Class);
        }

        [Test]
        public void ShouldNotConvertClassIfNotPopulated()
        {
            var nestedCompositeElementMapping = new NestedCompositeElementMapping();
            // Don't set anything on the original mapping
            var convertedHbmNestedCompositeElement = converter.Convert(nestedCompositeElementMapping);
            var blankHbmNestedCompositeElement = new HbmNestedCompositeElement();
            convertedHbmNestedCompositeElement.@class.ShouldEqual(blankHbmNestedCompositeElement.@class);
        }

        [Test]
        public void ShouldConvertNameIfPopulated()
        {
            var nestedCompositeElementMapping = new NestedCompositeElementMapping();
            nestedCompositeElementMapping.Set(fluent => fluent.Name, Layer.Conventions, "testName");
            var convertedHbmNestedCompositeElement = converter.Convert(nestedCompositeElementMapping);
            convertedHbmNestedCompositeElement.name.ShouldEqual(nestedCompositeElementMapping.Name);
        }

        [Test]
        public void ShouldConvertNameIfNotPopulated()
        {
            var nestedCompositeElementMapping = new NestedCompositeElementMapping();
            // Don't set anything on the original mapping
            var convertedHbmNestedCompositeElement = converter.Convert(nestedCompositeElementMapping);
            convertedHbmNestedCompositeElement.name.ShouldEqual(nestedCompositeElementMapping.Name);
        }

        [Test]
        public void ShouldConvertAccessIfPopulated()
        {
            var nestedCompositeElementMapping = new NestedCompositeElementMapping();
            nestedCompositeElementMapping.Set(fluent => fluent.Access, Layer.Conventions, "acc");
            var convertedHbmNestedCompositeElement = converter.Convert(nestedCompositeElementMapping);
            convertedHbmNestedCompositeElement.access.ShouldEqual(nestedCompositeElementMapping.Access);
        }

        [Test]
        public void ShouldNotConvertAccessIfNotPopulated()
        {
            var nestedCompositeElementMapping = new NestedCompositeElementMapping();
            // Don't set anything on the original mapping
            var convertedHbmNestedCompositeElement = converter.Convert(nestedCompositeElementMapping);
            var blankHbmNestedCompositeElement = new HbmNestedCompositeElement();
            convertedHbmNestedCompositeElement.access.ShouldEqual(blankHbmNestedCompositeElement.access);
        }

        [Test]
        public void ShouldConvertProperties()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<NestedCompositeElementMapping, PropertyMapping, HbmNestedCompositeElement, HbmProperty, object>(
                (nestedCompositeElementMapping, propertyMapping) => nestedCompositeElementMapping.AddProperty(propertyMapping),
                hbmNestedCompositeElement => hbmNestedCompositeElement.Items);
        }

        [Test]
        public void ShouldConvertManyToOnes()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<NestedCompositeElementMapping, ManyToOneMapping, HbmNestedCompositeElement, HbmManyToOne, object>(
                (nestedCompositeElementMapping, manyToOneMapping) => nestedCompositeElementMapping.AddReference(manyToOneMapping),
                hbmNestedCompositeElement => hbmNestedCompositeElement.Items);
        }

        [Test]
        public void ShouldConvertNestedCompositeElements()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<NestedCompositeElementMapping, NestedCompositeElementMapping, HbmNestedCompositeElement, HbmNestedCompositeElement, object>(
                (nestedCompositeElementMapping1, nestedCompositeElementMapping2) => nestedCompositeElementMapping1.AddCompositeElement(nestedCompositeElementMapping2),
                hbmNestedCompositeElement => hbmNestedCompositeElement.Items);
        }

        [Test]
        public void ShouldConvertParent()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<NestedCompositeElementMapping, ParentMapping, HbmNestedCompositeElement, HbmParent>(
                (nestedCompositeElementMapping, parentMapping) => nestedCompositeElementMapping.Set(fluent => fluent.Parent, Layer.Defaults, parentMapping),
                hbmNestedCompositeElement => hbmNestedCompositeElement.parent);
        }
    }
}