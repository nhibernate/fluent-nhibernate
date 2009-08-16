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

            if (propertyMapping.HasValue(x => x.Access))
                element.WithAtt("access", propertyMapping.Access);

            if (propertyMapping.HasValue(x => x.Generated))
                element.WithAtt("generated", propertyMapping.Generated);

            if (propertyMapping.HasValue(x => x.Name))
                element.WithAtt("name", propertyMapping.Name);

            if (propertyMapping.HasValue(x => x.OptimisticLock))
                element.WithAtt("optimistic-lock", propertyMapping.OptimisticLock);

            if (propertyMapping.HasValue(x => x.Insert))
                element.WithAtt("insert", propertyMapping.Insert);

            if (propertyMapping.HasValue(x => x.Update))
                element.WithAtt("update", propertyMapping.Update);

            if (propertyMapping.HasValue(x => x.Formula))
                element.WithAtt("formula", propertyMapping.Formula);

            if (propertyMapping.HasValue(x => x.Type))
                element.WithAtt("type", propertyMapping.Type);

            if (propertyMapping.HasValue(x => x.Lazy))
                element.WithAtt("lazy", propertyMapping.Lazy);

            if (propertyMapping.HasValue(x => x.Index))
                element.WithAtt("index", propertyMapping.Index);

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