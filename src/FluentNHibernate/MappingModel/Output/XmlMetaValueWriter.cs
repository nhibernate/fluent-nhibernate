using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlMetaValueWriter : NullMappingModelVisitor, IXmlWriter<MetaValueMapping>
    {
        private XmlDocument document;

        public XmlDocument Write(MetaValueMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessMetaValue(MetaValueMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("meta-value");

            if (mapping.HasValue(x => x.Value))
                element.WithAtt("value", mapping.Value);

            if (mapping.HasValue(x => x.Class))
                element.WithAtt("class", mapping.Class);
        }
    }
}