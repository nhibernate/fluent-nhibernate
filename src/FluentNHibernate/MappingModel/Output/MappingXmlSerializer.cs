using System.Xml;

namespace FluentNHibernate.MappingModel.Output
{
    public class MappingXmlSerializer
    {
        public XmlDocument Serialize(HibernateMapping mapping)
        {
            return BuildXml(mapping);
        }

        private static XmlDocument BuildXml(HibernateMapping rootMapping)
        {
            var xmlWriter = XmlWriterFactory.CreateHibernateMappingWriter();

            return xmlWriter.Write(rootMapping);
        }
    }
}