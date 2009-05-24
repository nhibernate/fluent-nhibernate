using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlJoinWriter : NullMappingModelVisitor, IXmlWriter<JoinMapping>
    {
        private XmlDocument document;
        private readonly IXmlWriter<PropertyMapping> propertyWriter;
        private readonly IXmlWriter<KeyMapping> keyWriter;

        public XmlJoinWriter(IXmlWriter<PropertyMapping> propertyWriter, IXmlWriter<KeyMapping> keyWriter)
        {
            this.propertyWriter = propertyWriter;
            this.keyWriter = keyWriter;
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
            var propertyXml = propertyWriter.Write(mapping);

            document.ImportAndAppendChild(propertyXml);
        }

        public override void Visit(KeyMapping mapping)
        {
            var keyXml = keyWriter.Write(mapping);

            document.ImportAndAppendChild(keyXml);
        }
    }
}
