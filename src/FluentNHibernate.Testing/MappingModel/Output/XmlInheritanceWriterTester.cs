using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlInheritanceWriterTester
    {
        private IXmlWriter<ISubclassMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<ISubclassMapping>>();
        }

        [Test]
        public void ShouldWriteSubclassForSubclassMapping()
        {
            var mapping = new SubclassMapping();

            writer.VerifyXml(mapping)
                .RootElement.HasName("subclass");
        }

        [Test]
        public void ShouldWriteSetForSetMapping()
        {
            var mapping = new JoinedSubclassMapping();

            writer.VerifyXml(mapping)
                .RootElement.HasName("joined-subclass");
        }
    }
}