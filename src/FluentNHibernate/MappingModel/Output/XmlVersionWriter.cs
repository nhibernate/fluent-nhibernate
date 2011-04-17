using System.Xml;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlVersionWriter : NullMappingModelVisitor, IXmlWriter<VersionMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        private XmlDocument document;

        public XmlVersionWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(VersionMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessVersion(VersionMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("version");

            if (mapping.IsSpecified("Access"))
                element.WithAtt("access", mapping.Access);

            if (mapping.IsSpecified("Generated"))
                element.WithAtt("generated", mapping.Generated);

            if (mapping.IsSpecified("Name"))
                element.WithAtt("name", mapping.Name);

            if (mapping.IsSpecified("Type"))
                element.WithAtt("type", mapping.Type);

            if (mapping.IsSpecified("UnsavedValue"))
                element.WithAtt("unsaved-value", mapping.UnsavedValue);
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            var writer = serviceLocator.GetWriter<ColumnMapping>();
            var columnXml = writer.Write(columnMapping);

            document.ImportAndAppendChild(columnXml);
        }
    }
}