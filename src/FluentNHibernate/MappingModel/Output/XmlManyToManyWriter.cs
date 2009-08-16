using System;
using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlManyToManyWriter : NullMappingModelVisitor, IXmlWriter<ManyToManyMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        private XmlDocument document;

        public XmlManyToManyWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(ManyToManyMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessManyToMany(ManyToManyMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("many-to-many");

            if (mapping.HasValue(x => x.Class))
                element.WithAtt("class", mapping.Class);

            if (mapping.HasValue(x => x.Fetch))
                element.WithAtt("fetch", mapping.Fetch);

            if (mapping.HasValue(x => x.ForeignKey))
                element.WithAtt("foreign-key", mapping.ForeignKey);

            if (mapping.HasValue(x => x.Lazy))
                element.WithAtt("lazy", mapping.Lazy);

            if (mapping.HasValue(x => x.NotFound))
                element.WithAtt("not-found", mapping.NotFound);

            if (mapping.HasValue(x => x.Where))
                element.WithAtt("where", mapping.Where);
        }

        public override void Visit(ColumnMapping mapping)
        {
            var writer = serviceLocator.GetWriter<ColumnMapping>();
            var columnXml = writer.Write(mapping);

            document.ImportAndAppendChild(columnXml);
        }
    }
}