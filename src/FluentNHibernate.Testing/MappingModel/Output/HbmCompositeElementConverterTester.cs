using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;
using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmCompositeElementConverterTester
    {
        private IHbmConverter<CompositeElementMapping, HbmCompositeElement> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<CompositeElementMapping, HbmCompositeElement>>();
        }

        [Test]
        public void ShouldConvertClassIfPopulated()
        {
            var compositeElementMapping = new CompositeElementMapping();
            compositeElementMapping.Set(fluent => fluent.Class, Layer.Conventions, new TypeReference("t"));
            var convertedHbmCompositeElement = converter.Convert(compositeElementMapping);
            convertedHbmCompositeElement.@class.ShouldEqual(compositeElementMapping.Class);
        }

        [Test]
        public void ShouldNotConvertClassIfNotPopulated()
        {
            var compositeElementMapping = new CompositeElementMapping();
            // Don't set anything on the original mapping
            var convertedHbmCompositeElement = converter.Convert(compositeElementMapping);
            var blankHbmCompositeElement = new HbmCompositeElement();
            convertedHbmCompositeElement.@class.ShouldEqual(blankHbmCompositeElement.@class);
        }

        [Test]
        public void ShouldConvertProperties()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<CompositeElementMapping, PropertyMapping, HbmCompositeElement, HbmProperty, object>(
                (compositeElementMapping, propertyMapping) => compositeElementMapping.AddProperty(propertyMapping),
                hbmCompositeElement => hbmCompositeElement.Items);
        }

        [Test]
        public void ShouldConvertManyToOnes()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<CompositeElementMapping, ManyToOneMapping, HbmCompositeElement, HbmManyToOne, object>(
                (compositeElementMapping, manyToOneMapping) => compositeElementMapping.AddReference(manyToOneMapping),
                hbmCompositeElement => hbmCompositeElement.Items);
        }

        [Test]
        public void ShouldConvertNestedCompositeElements()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<CompositeElementMapping, NestedCompositeElementMapping, HbmCompositeElement, HbmNestedCompositeElement, object>(
                (compositeElementMapping, nestedCompositeElementMapping) => compositeElementMapping.AddCompositeElement(nestedCompositeElementMapping),
                hbmCompositeElement => hbmCompositeElement.Items);
        }

        [Test]
        public void ShouldConvertParent()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<CompositeElementMapping, ParentMapping, HbmCompositeElement, HbmParent>(
                (compositeElementMapping, parentMapping) => compositeElementMapping.Set(fluent => fluent.Parent, Layer.Defaults, parentMapping),
                hbmCompositeElement => hbmCompositeElement.parent);
        }
    }
}