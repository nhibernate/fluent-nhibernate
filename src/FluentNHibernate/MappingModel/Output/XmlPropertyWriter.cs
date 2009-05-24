using System.Xml;
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

            if (propertyMapping.Attributes.IsSpecified(x => x.Access))
                element.WithAtt("access", propertyMapping.Access);

            if (propertyMapping.Attributes.IsSpecified(x => x.Generated))
                element.WithAtt("generated", propertyMapping.Generated);

            if (propertyMapping.Attributes.IsSpecified(x => x.Name))
                element.WithAtt("name", propertyMapping.Name);

            if (propertyMapping.Attributes.IsSpecified(x => x.OptimisticLock))
                element.WithAtt("optimistic-lock", propertyMapping.OptimisticLock);

            if (propertyMapping.Attributes.IsSpecified(x => x.Insert))
                element.WithAtt("insert", propertyMapping.Insert);

            if (propertyMapping.Attributes.IsSpecified(x => x.Update))
                element.WithAtt("update", propertyMapping.Update);

            if (propertyMapping.Attributes.IsSpecified(x => x.Formula))
                element.WithAtt("formula", propertyMapping.Formula);

            if (propertyMapping.Attributes.IsSpecified(x => x.Type))
                element.WithAtt("type", propertyMapping.Type);

            document.AppendChild(element);
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            var columnXml = columnWriter.Write(columnMapping);
            
            document.ImportAndAppendChild(columnXml);
        }
    }
}