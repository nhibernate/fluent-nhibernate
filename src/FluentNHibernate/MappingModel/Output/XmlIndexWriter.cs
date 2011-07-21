using System;
using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlIndexWriter : NullMappingModelVisitor, IXmlWriter<IndexMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        private XmlDocument document;

        public XmlIndexWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(IndexMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessIndex(IndexMapping mapping)
        {
            document = new XmlDocument();

            if (mapping.IsManyToMany)
                WriteManyToManyIndex(mapping);
            else
                if (mapping.HasValue(x => x.Offset))
                    WriteListIndex(mapping);
                else
                    WriteIndex(mapping);
        }

        void WriteIndex(IndexMapping mapping)
        {
            var element = document.AddElement("index");

            if (mapping.HasValue(x => x.Type))
                element.WithAtt("type", mapping.Type);
        }

        void WriteListIndex(IndexMapping mapping)
        {
            var element = document.AddElement("list-index");

            element.WithAtt("base", mapping.Offset);
        }

        void WriteManyToManyIndex(IndexMapping mapping)
        {
            var element = document.AddElement("index-many-to-many");

            if (mapping.HasValue(x => x.Type))
                element.WithAtt("class", mapping.Type);

            if (mapping.HasValue(x => x.EntityName))
                element.WithAtt("entity-name", mapping.EntityName);

            if (mapping.HasValue(x => x.ForeignKey))
                element.WithAtt("foreign-key", mapping.ForeignKey);
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            var writer = serviceLocator.GetWriter<ColumnMapping>();
            var xml = writer.Write(columnMapping);

            document.ImportAndAppendChild(xml);
        }
    }
}