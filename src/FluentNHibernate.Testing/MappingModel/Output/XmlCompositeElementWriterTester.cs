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
        private XmlCompositeElementWriter xmlCompositeElementWriter;

        [Test]
        public void ShouldWriteTheAttributes()
        {
            var compositeElementMapping = new CompositeElementMapping { Type = typeof(object) };

            xmlCompositeElementWriter = new XmlCompositeElementWriter(null);
            xmlCompositeElementWriter.VerifyXml(compositeElementMapping).HasAttribute("class", typeof(object).AssemblyQualifiedName);
        }

        [Test]
        public void ShouldWriteTheProperties()
        {
            var compositeElementMapping = new CompositeElementMapping();
            compositeElementMapping.AddProperty(new PropertyMapping());

            var propertyDocument = new XmlDocument();
            propertyDocument.AppendChild(propertyDocument.CreateElement("property"));

            var propertyWriter = MockRepository.GenerateMock<IXmlWriter<PropertyMapping>>();
            propertyWriter
                .Expect(x => x.Write(compositeElementMapping.Properties.First()))
                .Return(propertyDocument);

            xmlCompositeElementWriter = new XmlCompositeElementWriter(propertyWriter);

            xmlCompositeElementWriter.VerifyXml(compositeElementMapping)
                .Element("property").Exists();
        }
    }
}
