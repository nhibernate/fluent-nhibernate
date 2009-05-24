using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Testing.Testing;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using FluentNHibernate.MappingModel.Output;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlJoinWriterTester
    {
        private XmlJoinWriter _writer;

        [Test]
        public void ShouldWriteTheAttributes()
        {
            _writer = new XmlJoinWriter(null, null);
            var testHelper = new XmlWriterTestHelper<JoinMapping>();
            testHelper.Check(x => x.TableName, "Table1").MapsToAttribute("table");

            testHelper.VerifyAll(_writer);
        }

        [Test]
        public void ShouldWriteTheKey()
        {
            var joinMapping = new JoinMapping();
            joinMapping.Key = new KeyMapping();
            joinMapping.Key.AddColumn(new ColumnMapping { Name = "Column1" });
            
            _writer = new XmlJoinWriter(null, null);

            _writer.VerifyXml(joinMapping)
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

            _writer = new XmlJoinWriter(propertyWriter, null);

            _writer.VerifyXml(joinMapping)
                .Element("property").Exists();
        }
    }
}
