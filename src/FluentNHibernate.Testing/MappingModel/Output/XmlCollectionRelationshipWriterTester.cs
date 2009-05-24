using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlCollectionRelationshipWriterTester
    {
        private XmlCollectionRelationshipWriter writer;

        [Test]
        public void ShouldWriteManyToManyForManyToManyMapping()
        {
            var mapping = new ManyToManyMapping();

            writer = new XmlCollectionRelationshipWriter(null, new XmlManyToManyWriter(null));
            writer.VerifyXml(mapping)
                .RootElement.HasName("many-to-many");
        }

        [Test]
        public void ShouldWriteOneToManyForOneToManyMapping()
        {
            var mapping = new OneToManyMapping();

            writer = new XmlCollectionRelationshipWriter(new XmlOneToManyWriter(), null);
            writer.VerifyXml(mapping)
                .RootElement.HasName("one-to-many");
        }
    }
}