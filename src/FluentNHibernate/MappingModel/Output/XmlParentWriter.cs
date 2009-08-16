using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlParentWriter : NullMappingModelVisitor, IXmlWriter<ParentMapping>
    {
        private XmlDocument document;

        public XmlDocument Write(ParentMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessParent(ParentMapping parentMapping)
        {
            document = new XmlDocument();

            var parentElement = document.AddElement("parent");

            if (parentMapping.HasValue(x => x.Name))
                parentElement.WithAtt("name", parentMapping.Name);
        }
    }
}