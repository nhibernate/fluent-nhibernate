using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlVersionWriter : NullMappingModelVisitor, IXmlWriter<VersionMapping>
    {
        private XmlDocument document;

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

            if (mapping.IsSpecified(x => x.Access))
                element.WithAtt("access", mapping.Access);

            if (mapping.IsSpecified(x => x.Column))
                element.WithAtt("column", mapping.Column);

            if (mapping.IsSpecified(x => x.Generated))
                element.WithAtt("generated", mapping.Generated);

            if (mapping.IsSpecified(x => x.Name))
                element.WithAtt("name", mapping.Name);

            if (mapping.IsSpecified(x => x.Type))
                element.WithAtt("type", mapping.Type);

            if (mapping.IsSpecified(x => x.UnsavedValue))
                element.WithAtt("unsaved-value", mapping.UnsavedValue);
        }
    }
}