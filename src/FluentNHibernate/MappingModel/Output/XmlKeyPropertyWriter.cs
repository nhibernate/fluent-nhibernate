using System;
using System.Xml;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlKeyPropertyWriter : NullMappingModelVisitor, IXmlWriter<KeyPropertyMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        private XmlDocument document;

        public XmlKeyPropertyWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(KeyPropertyMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessKeyProperty(KeyPropertyMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("key-property");

            if (mapping.HasValue(x => x.Name))
                element.WithAtt("name", mapping.Name);

            if (mapping.HasValue(x => x.Access))
                element.WithAtt("access", mapping.Access);

            if (mapping.HasValue(x => x.Type))
                element.WithAtt("type", mapping.Type);
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            var writer = serviceLocator.GetWriter<ColumnMapping>();
            var xml = writer.Write(columnMapping);

            document.ImportAndAppendChild(xml);
        }
    }
}