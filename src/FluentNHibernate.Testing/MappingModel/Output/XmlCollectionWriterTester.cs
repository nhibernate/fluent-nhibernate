using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlCollectionWriterTester
    {
        private XmlCollectionWriter writer;

        [Test]
        public void ShouldWriteBagForBagMapping()
        {
            var mapping = new BagMapping();

            writer = new XmlCollectionWriter(new XmlBagWriter(null, null), null, null, null);
            writer.VerifyXml(mapping)
                .RootElement.HasName("bag");
        }

        [Test]
        public void ShouldWriteListForListMapping()
        {
            var mapping = new ListMapping();

            writer = new XmlCollectionWriter(null, null, new XmlListWriter(null, null), null);
            writer.VerifyXml(mapping)
                .RootElement.HasName("list");
        }

        [Test]
        public void ShouldWriteSetForSetMapping()
        {
            var mapping = new SetMapping();

            writer = new XmlCollectionWriter(null, new XmlSetWriter(null, null), null, null);
            writer.VerifyXml(mapping)
                .RootElement.HasName("set");
        }

        [Test]
        public void ShouldWriteMapForMapMapping()
        {
            var mapping = new MapMapping();

            writer = new XmlCollectionWriter(null, null, null, new XmlMapWriter(null, null));
            writer.VerifyXml(mapping)
                .RootElement.HasName("map");
        }
    }
}