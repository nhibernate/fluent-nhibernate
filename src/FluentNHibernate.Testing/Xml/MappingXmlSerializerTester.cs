using System.Linq;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Xml;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using System.Xml.Schema;

namespace FluentNHibernate.Testing.Xml
{
    [TestFixture]
    public class MappingXmlSerializerTester
    {
        [Test]
        public void CanWriteXmlDocument()
        {
            var mapping = new HibernateMapping();
            var serializer = new MappingXmlSerializer();
            XmlDocument document = serializer.Serialize(mapping);
            Assert.IsNotNull(document);
        }

        [Test]
        public void CanSerializeHbmGraphWithOneClass()
        {
            var mapping = new HbmMapping();
            mapping.Items = new object[] { new HbmClass() };
            var serializer = new MappingXmlSerializer();
            XmlDocument document = serializer.Serialize(mapping);
            Assert.IsNotNull(document);
        }

        [Test]
        public void AssignsNHibernateMappingSchema()
        {
            var mapping = new HbmMapping();
            var serializer = new MappingXmlSerializer();
            XmlDocument document = serializer.Serialize(mapping);            
            Assert.That(document.Schemas.Contains("urn:nhibernate-mapping-2.2"));
        }



    }
}