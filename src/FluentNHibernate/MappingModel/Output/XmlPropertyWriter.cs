using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlPropertyWriter : NullMappingModelVisitor, IXmlWriter<PropertyMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        private XmlDocument document;

        public XmlPropertyWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
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

            if (propertyMapping.HasValue("Access"))
                element.WithAtt("access", propertyMapping.Access);

            if (propertyMapping.HasValue("Generated"))
                element.WithAtt("generated", propertyMapping.Generated);

            if (propertyMapping.HasValue("Name"))
                element.WithAtt("name", propertyMapping.Name);

            if (propertyMapping.HasValue("OptimisticLock"))
                element.WithAtt("optimistic-lock", propertyMapping.OptimisticLock);

            if (propertyMapping.HasValue("Insert"))
                element.WithAtt("insert", propertyMapping.Insert);

            if (propertyMapping.HasValue("Update"))
                element.WithAtt("update", propertyMapping.Update);

            if (propertyMapping.HasValue("Formula"))
                element.WithAtt("formula", propertyMapping.Formula);

            if (propertyMapping.HasValue("Type"))
                element.WithAtt("type", propertyMapping.Type);

            if (propertyMapping.HasValue("Lazy"))
                element.WithAtt("lazy", propertyMapping.Lazy);

            document.AppendChild(element);
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            var writer = serviceLocator.GetWriter<ColumnMapping>();
            var columnXml = writer.Write(columnMapping);
            
            document.ImportAndAppendChild(columnXml);
        }
    }
}