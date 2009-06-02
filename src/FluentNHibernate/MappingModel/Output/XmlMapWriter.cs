using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlMapWriter : BaseXmlCollectionWriter, IXmlWriter<MapMapping>
    {
        public XmlMapWriter(IXmlWriterServiceLocator serviceLocator)
            : base(serviceLocator)
        {}

        public XmlDocument Write(MapMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessMap(MapMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("map");

            WriteBaseCollectionAttributes(element, mapping);

            if (mapping.Attributes.IsSpecified(x => x.OrderBy))
                element.WithAtt("order-by", mapping.OrderBy);

            if (mapping.Attributes.IsSpecified(x => x.Sort))
                element.WithAtt("sort", mapping.Sort);
        }
    }
}