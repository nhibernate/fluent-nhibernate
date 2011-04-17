using System.Xml;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

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

            if (propertyMapping.IsSpecified("Access"))
                element.WithAtt("access", propertyMapping.Access);

            if (propertyMapping.IsSpecified("Generated"))
                element.WithAtt("generated", propertyMapping.Generated);

            if (propertyMapping.IsSpecified("Name"))
                element.WithAtt("name", propertyMapping.Name);

            if (propertyMapping.IsSpecified("OptimisticLock"))
                element.WithAtt("optimistic-lock", propertyMapping.OptimisticLock);

            if (propertyMapping.IsSpecified("Insert"))
                element.WithAtt("insert", propertyMapping.Insert);

            if (propertyMapping.IsSpecified("Update"))
                element.WithAtt("update", propertyMapping.Update);

            if (propertyMapping.IsSpecified("Formula"))
                element.WithAtt("formula", propertyMapping.Formula);

            if (propertyMapping.IsSpecified("Type"))
                element.WithAtt("type", propertyMapping.Type);

            if (propertyMapping.IsSpecified("Lazy"))
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