using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlListWriter : BaseXmlCollectionWriter, IXmlWriter<ListMapping>
    {
        public XmlListWriter(IXmlWriter<KeyMapping> keyWriter, IXmlWriter<ICollectionRelationshipMapping> relationshipWriter, IXmlWriter<CacheMapping> cacheWriter)
            : base(keyWriter, relationshipWriter, cacheWriter)
        {}

        public XmlDocument Write(ListMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessList(ListMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("list");

            WriteBaseCollectionAttributes(element, mapping);
        }
    }
}