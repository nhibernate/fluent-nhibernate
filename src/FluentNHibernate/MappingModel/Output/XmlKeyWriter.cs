using System.Xml;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlKeyWriter : NullMappingModelVisitor, IXmlWriter<KeyMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        private XmlDocument document;

        public XmlKeyWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
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

            if (mapping.IsSpecified("ForeignKey"))
                element.WithAtt("foreign-key", mapping.ForeignKey);

            if (mapping.IsSpecified("OnDelete"))
                element.WithAtt("on-delete", mapping.OnDelete);

            if (mapping.IsSpecified("PropertyRef"))
                element.WithAtt("property-ref", mapping.PropertyRef);

            if (mapping.IsSpecified("NotNull"))
                element.WithAtt("not-null", mapping.NotNull);

            if (mapping.IsSpecified("Update"))
                element.WithAtt("update", mapping.Update);

            if (mapping.IsSpecified("Unique"))
                element.WithAtt("unique", mapping.Unique);

        }

        public override void Visit(ColumnMapping mapping)
        {
            var writer = serviceLocator.GetWriter<ColumnMapping>();
            var columnXml = writer.Write(mapping);

            document.ImportAndAppendChild(columnXml);
        }
    }
}