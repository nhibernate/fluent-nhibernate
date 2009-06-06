using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlIIndexWriterTester
    {
        private IXmlWriter<IIndexMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<IIndexMapping>>();
        }

        [Test]
        public void ShouldWriteIndexForIndexMapping()
        {
            writer.VerifyXml(new IndexMapping())
                .RootElement.HasName("index");
        }

        [Test]
        public void ShouldWriteIndexManyToManyForIndexManyToManyMapping()
        {
            writer.VerifyXml(new IndexManyToManyMapping())
                .RootElement.HasName("index-many-to-many");
        }
    }
}