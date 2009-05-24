using System;
using System.Xml;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlIdWriter : NullMappingModelVisitor, IXmlWriter<IdMapping>
    {
        private readonly XmlGeneratorWriter generatorWriter;
        private readonly XmlColumnWriter columnWriter;
        private XmlDocument document;

        public XmlIdWriter(XmlGeneratorWriter generatorWriter, XmlColumnWriter columnWriter)
        {
            this.generatorWriter = generatorWriter;
            this.columnWriter = columnWriter;
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

            if (mapping.Attributes.IsSpecified(x => x.Access))
                element.WithAtt("access", mapping.Access);

            if (mapping.Attributes.IsSpecified(x => x.Name))
                element.WithAtt("name", mapping.Name);

            if (mapping.Attributes.IsSpecified(x => x.Type))
                element.WithAtt("type", mapping.Type);

            if (mapping.Attributes.IsSpecified(x => x.UnsavedValue))
                element.WithAtt("unsaved-value", mapping.UnsavedValue);
        }

        public override void Visit(GeneratorMapping mapping)
        {
            var generatorXml = generatorWriter.Write(mapping);

            document.ImportAndAppendChild(generatorXml);
        }

        public override void Visit(ColumnMapping mapping)
        {
            var columnXml = columnWriter.Write(mapping);

            document.ImportAndAppendChild(columnXml);
        }
    }
}