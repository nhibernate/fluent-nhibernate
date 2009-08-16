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

            if (mapping.HasValue(x => x.Access))
                element.WithAtt("access", mapping.Access);

            if (mapping.HasValue(x => x.Name))
                element.WithAtt("name", mapping.Name);

            if (mapping.HasValue(x => x.Type))
                element.WithAtt("type", mapping.Type);

            if (mapping.HasValue(x => x.Length))
                element.WithAtt("length", mapping.Length);

            if (mapping.HasValue(x => x.UnsavedValue))
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