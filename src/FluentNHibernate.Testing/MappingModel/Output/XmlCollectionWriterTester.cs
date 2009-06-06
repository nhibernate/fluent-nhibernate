using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlCollectionWriterTester
    {
        private IXmlWriter<ICollectionMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<ICollectionMapping>>();
        }

        [Test]
        public void ShouldWriteBagForBagMapping()
        {
            var mapping = new BagMapping();

            writer.VerifyXml(mapping)
                .RootElement.HasName("bag");
        }

        [Test]
        public void ShouldWriteListForListMapping()
        {
            var mapping = new ListMapping();

            writer.VerifyXml(mapping)
                .RootElement.HasName("list");
        }

        [Test]
        public void ShouldWriteSetForSetMapping()
        {
            var mapping = new SetMapping();

            writer.VerifyXml(mapping)
                .RootElement.HasName("set");
        }

        [Test]
        public void ShouldWriteMapForMapMapping()
        {
            var mapping = new MapMapping();

            writer.VerifyXml(mapping)
                .RootElement.HasName("map");
        }

        [Test]
        public void ShouldWriteArrayForArrayMapping()
        {
            var mapping = new ArrayMapping();

            writer.VerifyXml(mapping)
                .RootElement.HasName("array");
        }
    }
}