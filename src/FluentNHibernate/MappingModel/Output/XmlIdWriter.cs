using System;
using System.Xml;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlIdWriter : NullMappingModelVisitor, IXmlWriter<IdMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        private XmlDocument document;

        public XmlIdWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(IdMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessId(IdMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("id");

            if (mapping.HasValue("Access"))
                element.WithAtt("access", mapping.Access);

            if (mapping.HasValue("Name"))
                element.WithAtt("name", mapping.Name);

            if (mapping.HasValue("Type"))
                element.WithAtt("type", mapping.Type);

            if (mapping.HasValue("UnsavedValue"))
                element.WithAtt("unsaved-value", mapping.UnsavedValue);
        }

        public override void Visit(GeneratorMapping mapping)
        {
            var writer = serviceLocator.GetWriter<GeneratorMapping>();
            var generatorXml = writer.Write(mapping);

            document.ImportAndAppendChild(generatorXml);
        }

        public override void Visit(ColumnMapping mapping)
        {
            var writer = serviceLocator.GetWriter<ColumnMapping>();
            var columnXml = writer.Write(mapping);

            document.ImportAndAppendChild(columnXml);
        }
    }
}