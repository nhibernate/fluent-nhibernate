using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlComponentBaseWriterTester
    {
        private IXmlWriter<IComponentMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<IComponentMapping>>();
        }

        [Test]
        public void ShouldWriteComponentForComponentMapping()
        {
            var mapping = new ComponentMapping();

            writer.VerifyXml(mapping)
                .RootElement.HasName("component");
        }

        [Test]
        public void ShouldWriteDynamicComponentForDynamicComponentMapping()
        {
            var mapping = new DynamicComponentMapping();

            writer.VerifyXml(mapping)
                .RootElement.HasName("dynamic-component");
        }
    }
}