using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlBagWriter : BaseXmlCollectionWriter, IXmlWriter<CollectionMapping>
    {
        public XmlBagWriter(IXmlWriterServiceLocator serviceLocator)
            : base(serviceLocator)
        {}

        public XmlDocument Write(CollectionMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessCollection(CollectionMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("bag");

            WriteBaseCollectionAttributes(element, mapping);

            if (mapping.IsSpecified("OrderBy"))
                element.WithAtt("order-by", mapping.OrderBy);
        }
    }
}