using System;
using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlManyToManyWriter : NullMappingModelVisitor, IXmlWriter<ManyToManyMapping>
    {
        private readonly IXmlWriter<ColumnMapping> columnWriter;
        private XmlDocument document;

        public XmlManyToManyWriter(IXmlWriter<ColumnMapping> columnWriter)
        {
            this.columnWriter = columnWriter;
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

            if (mapping.Attributes.IsSpecified(x => x.Class))
                element.WithAtt("class", mapping.Class);

            if (mapping.Attributes.IsSpecified(x => x.Fetch))
                element.WithAtt("fetch", mapping.Fetch);

            if (mapping.Attributes.IsSpecified(x => x.ForeignKey))
                element.WithAtt("foreign-key", mapping.ForeignKey);

            if (mapping.Attributes.IsSpecified(x => x.Lazy))
                element.WithAtt("lazy", mapping.Lazy);

            if (mapping.Attributes.IsSpecified(x => x.NotFound))
                element.WithAtt("not-found", mapping.NotFound);

            if (mapping.Attributes.IsSpecified(x => x.OuterJoin))
                element.WithAtt("outer-join", mapping.OuterJoin);

            if (mapping.Attributes.IsSpecified(x => x.Where))
                element.WithAtt("where", mapping.Where);
        }

        public override void Visit(ColumnMapping mapping)
        {
            var columnXml = columnWriter.Write(mapping);

            document.ImportAndAppendChild(columnXml);
        }
    }
}