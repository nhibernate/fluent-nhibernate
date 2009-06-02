using System.Linq;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;
using FluentNHibernate.MappingModel.Output;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlJoinWriterTester
    {
        private IXmlWriter<JoinMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<JoinMapping>>();
        }

        [Test]
        public void ShouldWriteTheAttributes()
        {
            var testHelper = new XmlWriterTestHelper<JoinMapping>();
            testHelper.Check(x => x.TableName, "Table1").MapsToAttribute("table");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteTheKey()
        {
            var joinMapping = new JoinMapping();
            joinMapping.Key = new KeyMapping();
            joinMapping.Key.AddColumn(new ColumnMapping { Name = "Column1" });
            
            writer.VerifyXml(joinMapping)
                .Element("key/column")
                    .Exists()
                    .HasAttribute("name", "Column1");
        }

        [Test]
        public void ShouldWriteTheProperties()
        {
            var joinMapping = new JoinMapping();
            joinMapping.AddProperty(new PropertyMapping());

            var propertyDocument = new XmlDocument();
            propertyDocument.AppendChild(propertyDocument.CreateElement("property"));

            var propertyWriter = MockRepository.GenerateMock<IXmlWriter<PropertyMapping>>();
            propertyWriter
                .Expect(x => x.Write(joinMapping.Properties.First()))
                .Return(propertyDocument);

            writer.VerifyXml(joinMapping)
                .Element("property").Exists();
        }
    }
}
