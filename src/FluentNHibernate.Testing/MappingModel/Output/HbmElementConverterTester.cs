using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;
using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmElementConverterTester
    {
        private IHbmConverter<ElementMapping, HbmElement> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<ElementMapping, HbmElement>>();
        }

        [Test]
        public void ShouldConvertTypeIfPopulated()
        {
            var elementMapping = new ElementMapping();
            elementMapping.Set(fluent => fluent.Type, Layer.Conventions, new TypeReference("type"));
            var convertedHbmElement = converter.Convert(elementMapping);
            convertedHbmElement.type1.ShouldEqual(elementMapping.Type.ToString());
        }

        [Test]
        public void ShouldNotConvertTypeIfNotPopulated()
        {
            var elementMapping = new ElementMapping();
            // Don't set anything on the original mapping
            var convertedHbmElement = converter.Convert(elementMapping);
            var blankHbmElement = new HbmElement();
            convertedHbmElement.type1.ShouldEqual(blankHbmElement.type1);
        }

        [Test]
        public void ShouldConvertFormulaIfPopulated()
        {
            var elementMapping = new ElementMapping();
            elementMapping.Set(fluent => fluent.Formula, Layer.Conventions, "formula");
            var convertedHbmElement = converter.Convert(elementMapping);
            convertedHbmElement.formula.ShouldEqual(elementMapping.Formula);
        }

        [Test]
        public void ShouldNotConvertFormulaIfNotPopulated()
        {
            var elementMapping = new ElementMapping();
            // Don't set anything on the original mapping
            var convertedHbmElement = converter.Convert(elementMapping);
            var blankHbmElement = new HbmElement();
            convertedHbmElement.formula.ShouldEqual(blankHbmElement.formula);
        }

        [Test]
        public void ShouldConvertColumns()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<ElementMapping, ColumnMapping, HbmElement, HbmColumn, object>(
                (elementMapping, columnMapping) => elementMapping.AddColumn(Layer.Conventions, columnMapping),
                hbmElement => hbmElement.Items);
        }
    }
}