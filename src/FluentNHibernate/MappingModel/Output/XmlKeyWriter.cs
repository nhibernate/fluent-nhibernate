using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlKeyWriter : NullMappingModelVisitor, IXmlWriter<KeyMapping>
    {
        private XmlDocument document;

        public XmlDocument Write(KeyMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessKey(KeyMapping keyMapping)
        {
            document = new XmlDocument();

            var keyElement = document.AddElement("key");

            if (keyMapping.Attributes.IsSpecified(x => x.Column))
                keyElement.WithAtt("column", keyMapping.Column);
        }
    }
}