using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlBagWriter : BaseXmlCollectionWriter, IXmlWriter<BagMapping>
    {
        public XmlBagWriter(IXmlWriter<KeyMapping> keyWriter, IXmlWriter<ICollectionRelationshipMapping> relationshipWriter, IXmlWriter<CacheMapping> cacheWriter)
            : base(keyWriter, relationshipWriter, cacheWriter)
        {}

        public XmlDocument Write(BagMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessBag(BagMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("bag");

            WriteBaseCollectionAttributes(element, mapping);

            if (mapping.Attributes.IsSpecified(x => x.OrderBy))
                element.WithAtt("order-by", mapping.OrderBy);
        }
    }
}