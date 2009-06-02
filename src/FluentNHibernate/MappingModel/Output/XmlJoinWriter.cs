using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlJoinWriter : NullMappingModelVisitor, IXmlWriter<JoinMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        private XmlDocument document;

        public XmlJoinWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(JoinMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessJoin(JoinMapping joinMapping)
        {
            document = new XmlDocument();

            var element = document.CreateElement("join");
            element.WithAtt("table", joinMapping.TableName);
            
            document.AppendChild(element);

            var sortedUnmigratedParts = new List<IMappingPart>(joinMapping.UnmigratedParts);
            sortedUnmigratedParts.Sort(new MappingPartComparer(joinMapping.UnmigratedParts));

            foreach (var part in sortedUnmigratedParts)
                part.Write(element, null);

            foreach (var attribute in joinMapping.UnmigratedAttributes)
                element.WithAtt(attribute.Key, attribute.Value);

        }

        public override void Visit(PropertyMapping mapping)
        {
            var writer = serviceLocator.GetWriter<PropertyMapping>();
            var propertyXml = writer.Write(mapping);

            document.ImportAndAppendChild(propertyXml);
        }

        public override void Visit(KeyMapping mapping)
        {
            var writer = serviceLocator.GetWriter<KeyMapping>();
            var keyXml = writer.Write(mapping);

            document.ImportAndAppendChild(keyXml);
        }
    }
}
