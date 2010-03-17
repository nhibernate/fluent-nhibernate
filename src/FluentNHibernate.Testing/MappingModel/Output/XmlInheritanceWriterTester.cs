using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlInheritanceWriterTester
    {
        private IXmlWriter<SubclassMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<SubclassMapping>>();
        }

        [Test]
        public void ShouldWriteSubclassForSubclassMapping()
        {
            var mapping = new SubclassMapping(SubclassType.Subclass);

            writer.VerifyXml(mapping)
                .RootElement.HasName("subclass");
        }

        [Test]
        public void ShouldWriteJoinedSubclassForJoinedSubclassMapping()
        {
            var mapping = new SubclassMapping(SubclassType.JoinedSubclass);

            writer.VerifyXml(mapping)
                .RootElement.HasName("joined-subclass");
        }
    }
}