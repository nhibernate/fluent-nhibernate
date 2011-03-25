using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlSetWriter : BaseXmlCollectionWriter, IXmlWriter<CollectionMapping>
    {
        public XmlSetWriter(IXmlWriterServiceLocator serviceLocator)
            : base(serviceLocator)
        { }

        public XmlDocument Write(CollectionMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessCollection(CollectionMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("set");

            WriteBaseCollectionAttributes(element, mapping);

            if (mapping.HasValue(x => x.OrderBy))
                element.WithAtt("order-by", mapping.OrderBy);

            if (mapping.HasValue(x => x.Sort))
                element.WithAtt("sort", mapping.Sort);
        }
    }
}