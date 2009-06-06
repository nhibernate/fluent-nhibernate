using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlCollectionRelationshipWriterTester
    {
        private IXmlWriter<ICollectionRelationshipMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<ICollectionRelationshipMapping>>();
        }

        [Test]
        public void ShouldWriteManyToManyForManyToManyMapping()
        {
            var mapping = new ManyToManyMapping();

            writer.VerifyXml(mapping)
                .RootElement.HasName("many-to-many");
        }

        [Test]
        public void ShouldWriteOneToManyForOneToManyMapping()
        {
            var mapping = new OneToManyMapping();

            writer.VerifyXml(mapping)
                .RootElement.HasName("one-to-many");
        }
    }
}