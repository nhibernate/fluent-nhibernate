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

        public override void ProcessImport(ImportMapping mapping)
        {
            document = new XmlDocument();

            var element = document.CreateElement("import");

            if (mapping.HasValue(x => x.Class))
                element.WithAtt("class", mapping.Class);

            if (mapping.HasValue(x => x.Rename))
                element.WithAtt("rename", mapping.Rename);

            document.AppendChild(element);
        }
    }
}