using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlIndexWriterTester
    {
        private IXmlWriter<IndexMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<IndexMapping>>();
        }

        [Test]
        public void ShouldWriteTypeAttribute()
        {
            var testHelper = new XmlWriterTestHelper<IndexMapping>();

            testHelper.Check(x => x.Type, new TypeReference("type")).MapsToAttribute("type");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteColumns()
        {
            var mapping = new IndexMapping();

            mapping.AddColumn(new ColumnMapping());

            writer.VerifyXml(mapping)
                .Element("column").Exists();
        }
    }
}