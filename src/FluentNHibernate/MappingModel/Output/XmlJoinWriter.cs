using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlJoinWriter : NullMappingModelVisitor, IXmlWriter<JoinMapping>
    {
        private XmlDocument document;
        private readonly IXmlWriter<PropertyMapping> propertyWriter;

        public XmlJoinWriter(IXmlWriter<PropertyMapping> propertyWriter)
        {
            this.propertyWriter = propertyWriter;
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
            
            if(joinMapping.Key != null)
                element.AddElement("key").SetAttribute("column", joinMapping.Key.Column);

            document.AppendChild(element);

            var sortedUnmigratedParts = new List<IMappingPart>(joinMapping.UnmigratedParts);
            sortedUnmigratedParts.Sort(new MappingPartComparer(joinMapping.UnmigratedParts));

            foreach (var part in sortedUnmigratedParts)
                part.Write(element, null);

            foreach (var attribute in joinMapping.UnmigratedAttributes)
                element.WithAtt(attribute.Key, attribute.Value);

        }

        public override void Visit(PropertyMapping propertyMapping)
        {
            var propertyXml = propertyWriter.Write(propertyMapping);
            var propertyNode = document.ImportNode(propertyXml.DocumentElement, true);

            document.DocumentElement.AppendChild(propertyNode);
        }

    }
}
