using System.Xml;
using FluentNHibernate.Utils;

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

        public override void ProcessImport(ImportMapping importMapping)
        {
            document = new XmlDocument();

            var element = document.CreateElement("import");

            element.WithAtt("class", importMapping.Type.AssemblyQualifiedName);

            if (importMapping.Attributes.IsSpecified(x => x.Rename))
                element.WithAtt("rename", importMapping.Rename);

            document.AppendChild(element);
        }
    }
}