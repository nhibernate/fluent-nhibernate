using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlDiscriminatorWriterTester
    {
        private IXmlWriter<DiscriminatorMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<DiscriminatorMapping>>();
        }

        [Test]
        public void ShouldWriteForceAttribute()
        {
            var testHelper = new XmlWriterTestHelper<DiscriminatorMapping>();
            testHelper.Check(x => x.Force, true).MapsToAttribute("force");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteInsertAttribute()
        {
            var testHelper = new XmlWriterTestHelper<DiscriminatorMapping>();
            testHelper.Check(x => x.Insert, true).MapsToAttribute("insert");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteFormulaAttribute()
        {
            var testHelper = new XmlWriterTestHelper<DiscriminatorMapping>();
            testHelper.Check(x => x.Formula, "f").MapsToAttribute("formula");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteColumns()
        {
            var mapping = new DiscriminatorMapping();

            mapping.AddColumn(new ColumnMapping { Name = "Column1" });

            writer.VerifyXml(mapping)
                .Element("column").Exists();
        }
    }
}