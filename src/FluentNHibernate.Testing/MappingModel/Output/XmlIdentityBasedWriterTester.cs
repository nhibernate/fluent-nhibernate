using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlIdentityBasedWriterTester
    {
        private IXmlWriter<IIdentityMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<IIdentityMapping>>();
        }

        [Test]
        public void ShouldWriteIdForIdMapping()
        {
            writer.VerifyXml(new IdMapping())
                .RootElement.HasName("id");
        }

        [Test]
        public void ShouldWriteCompositeIdForCompositeIdMapping()
        {
            writer.VerifyXml(new CompositeIdMapping())
                .RootElement.HasName("composite-id");
        }
    }
}