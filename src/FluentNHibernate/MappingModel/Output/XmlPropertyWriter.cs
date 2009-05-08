using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlPropertyWriter : NullMappingModelVisitor, IXmlWriter<PropertyMapping>
    {
        private XmlDocument document;
        private readonly IXmlWriter<ColumnMapping> columnWriter;

        public XmlPropertyWriter(IXmlWriter<ColumnMapping> columnWriter)
        {
            this.columnWriter = columnWriter;
        }

        public XmlDocument Write(PropertyMapping property)
        {
            document = null;
            property.AcceptVisitor(this);
            return document;
        }

        public override void ProcessProperty(PropertyMapping propertyMapping)
        {
            document = new XmlDocument();

            var element = document.CreateElement("property");

            element.WithAtt("name", propertyMapping.Name);

            if (propertyMapping.Attributes.IsSpecified(x => x.Insert))
                element.WithAtt("insert", propertyMapping.Insert.ToString().ToLowerInvariant());

            if (propertyMapping.Attributes.IsSpecified(x => x.Update))
                element.WithAtt("update", propertyMapping.Update.ToString().ToLowerInvariant());

            if (propertyMapping.Attributes.IsSpecified(x => x.UniqueKey))
                element.WithAtt("unique-key", propertyMapping.UniqueKey);

            if (propertyMapping.Attributes.IsSpecified(x => x.Formula))
                element.WithAtt("formula", propertyMapping.Formula);

            if (propertyMapping.Attributes.IsSpecified(x => x.Type))
                element.WithAtt("type", propertyMapping.Type);

            foreach (var attribute in propertyMapping.UnmigratedAttributes)
                element.WithAtt(attribute.Key, attribute.Value);

            document.AppendChild(element);
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            var columnXml = columnWriter.Write(columnMapping);
            var columnNode = document.ImportNode(columnXml.DocumentElement, true);

            document.DocumentElement.AppendChild(columnNode);
        }
    }
}