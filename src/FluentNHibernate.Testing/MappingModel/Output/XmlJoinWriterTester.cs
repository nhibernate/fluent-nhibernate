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
        public void Should_write_the_attributes()
        {
            _writer = new XmlJoinWriter(null);
            var testHelper = new XmlWriterTestHelper<JoinMapping>();
            testHelper.Check(x => x.TableName, "Table1").MapsToAttribute("table");

            testHelper.VerifyAll(_writer);
        }

        [Test]
        public void Should_write_the_properties()
        {
            var joinMapping = new JoinMapping();
            joinMapping.AddProperty(new PropertyMapping());

            var propertyDocument = new XmlDocument();
            propertyDocument.AppendChild(propertyDocument.CreateElement("property"));

            var propertyWriter = MockRepository.GenerateMock<IXmlWriter<PropertyMapping>>();
            propertyWriter
                .Expect(x => x.Write(joinMapping.Properties.First()))
                .Return(propertyDocument);

            _writer = new XmlJoinWriter(propertyWriter);

            _writer.VerifyXml(joinMapping)
                .Element("property").Exists();
        }
    }
}
