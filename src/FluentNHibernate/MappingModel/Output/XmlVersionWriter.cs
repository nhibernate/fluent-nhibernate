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

            if (mapping.Attributes.IsSpecified(x => x.Access))
                element.WithAtt("access", mapping.Access);

            if (mapping.Attributes.IsSpecified(x => x.Column))
                element.WithAtt("column", mapping.Column);

            if (mapping.Attributes.IsSpecified(x => x.Generated))
                element.WithAtt("generated", mapping.Generated.ToString());

            if (mapping.Attributes.IsSpecified(x => x.Name))
                element.WithAtt("name", mapping.Name);

            if (mapping.Attributes.IsSpecified(x => x.Type))
                element.WithAtt("type", mapping.Type);

            if (mapping.Attributes.IsSpecified(x => x.UnsavedValue))
                element.WithAtt("unsaved-value", mapping.UnsavedValue);
        }
    }
}