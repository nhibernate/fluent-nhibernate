using System.Xml;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlImportWriter : NullMappingModelVisitor, IXmlWriter<ImportMapping>
    {
        private XmlDocument document;

        public XmlDocument Write(ImportMapping mappingModel)
        {
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessImport(ImportMapping mapping)
        {
            document = new XmlDocument();

            var element = document.CreateElement("import");

            if (mapping.IsSpecified("Class"))
                element.WithAtt("class", mapping.Class);

            if (mapping.IsSpecified("Rename"))
                element.WithAtt("rename", mapping.Rename);

            document.AppendChild(element);
        }
    }
}