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

            writer = new XmlCollectionWriter();
            writer.VerifyXml(mapping)
                .Element("bag").Exists();
        }

        [Test]
        public void ShouldWriteListForListMapping()
        {
            var mapping = new ListMapping();

            writer = new XmlCollectionWriter();
            writer.VerifyXml(mapping)
                .Element("list").Exists();
        }

        [Test]
        public void ShouldWriteSetForSetMapping()
        {
            var mapping = new SetMapping();

            writer = new XmlCollectionWriter();
            writer.VerifyXml(mapping)
                .Element("set").Exists();
        }

        [Test]
        public void ShouldWriteMapForMapMapping()
        {
            var mapping = new MapMapping();

            writer = new XmlCollectionWriter();
            writer.VerifyXml(mapping)
                .Element("map").Exists();
        }
    }
}