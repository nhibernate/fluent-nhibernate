using System.Linq;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlCompositeElementWriterTester
    {
        private IXmlWriter<CompositeElementMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<CompositeElementMapping>>();
        }

        [Test]
        public void ShouldWriteTheAttributes()
        {
            var compositeElementMapping = new CompositeElementMapping { Type = typeof(object) };

            writer.VerifyXml(compositeElementMapping)
                .HasAttribute("class", typeof(object).AssemblyQualifiedName);
        }

        [Test]
        public void ShouldWriteTheProperties()
        {
            var compositeElementMapping = new CompositeElementMapping();
            compositeElementMapping.AddProperty(new PropertyMapping());

            writer.VerifyXml(compositeElementMapping)
                .Element("property").Exists();
        }
    }
}
