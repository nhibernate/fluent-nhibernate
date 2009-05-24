using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlKeyWriter : NullMappingModelVisitor, IXmlWriter<KeyMapping>
    {
        private readonly IXmlWriter<ColumnMapping> columnWriter;
        private XmlDocument document;

        public XmlKeyWriter(IXmlWriter<ColumnMapping> columnWriter)
        {
            this.columnWriter = columnWriter;
        }

        public XmlDocument Write(KeyMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessKey(KeyMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("key");

            if (mapping.Attributes.IsSpecified(x => x.ForeignKey))
                element.WithAtt("foreign-key", mapping.ForeignKey);

            if (mapping.Attributes.IsSpecified(x => x.OnDelete))
                element.WithAtt("on-delete", mapping.OnDelete);

            if (mapping.Attributes.IsSpecified(x => x.PropertyRef))
                element.WithAtt("property-ref", mapping.PropertyRef);
        }

        public override void Visit(ColumnMapping mapping)
        {
            var columnXml = columnWriter.Write(mapping);

            document.ImportAndAppendChild(columnXml);
        }
    }
}