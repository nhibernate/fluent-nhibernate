using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlElementWriterTester
    {
        private IXmlWriter<ElementMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<ElementMapping>>();
        }

        [Test]
        public void ShouldWriteTypeAttribute()
        {
            var testHelper = new XmlWriterTestHelper<ElementMapping>();

            testHelper.Check(x => x.Type, new TypeReference("type")).MapsToAttribute("type");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteColumns()
        {
            var mapping = new ElementMapping();

            mapping.AddColumn(new ColumnMapping());

            writer.VerifyXml(mapping)
                .Element("column").Exists();
        }

        [Test]
        public void ShouldWriteLength()
        {
            var testHelper = new XmlWriterTestHelper<ElementMapping>();
            testHelper.Check(x => x.Length, 50).MapsToAttribute("length");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteFormula()
        {
            var testHelper = new XmlWriterTestHelper<ElementMapping>();
            testHelper.Check(x => x.Formula, "formula").MapsToAttribute("formula");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWritePrecision()
        {
            var testHelper = new XmlWriterTestHelper<ElementMapping>();
            testHelper.Check(x => x.Precision, 10).MapsToAttribute("precision");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteScale()
        {
            var testHelper = new XmlWriterTestHelper<ElementMapping>();
            testHelper.Check(x => x.Scale, 10).MapsToAttribute("scale");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNotNull()
        {
            var testHelper = new XmlWriterTestHelper<ElementMapping>();
            testHelper.Check(x => x.NotNull, true).MapsToAttribute("not-null");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteUnique()
        {
            var testHelper = new XmlWriterTestHelper<ElementMapping>();
            testHelper.Check(x => x.Unique, true).MapsToAttribute("unique");

            testHelper.VerifyAll(writer);
        }
    }
}