using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using FluentNHibernate.Mapping;

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

            document.AppendChild(element);
        }

        public override void Visit(PropertyMapping propertyMapping)
        {
            var propertyXml = propertyWriter.Write(propertyMapping);
            var propertyNode = document.ImportNode(propertyXml.DocumentElement, true);

            document.DocumentElement.AppendChild(propertyNode);
        }

    }
}
