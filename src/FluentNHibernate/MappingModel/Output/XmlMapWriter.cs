using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlMapWriter : BaseXmlCollectionWriter, IXmlWriter<MapMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;

        public XmlMapWriter(IXmlWriterServiceLocator serviceLocator)
            : base(serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

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

            if (mapping.HasValue(x => x.OrderBy))
                element.WithAtt("order-by", mapping.OrderBy);

            if (mapping.HasValue(x => x.Sort))
                element.WithAtt("sort", mapping.Sort);
        }

        public override void Visit(IIndexMapping indexMapping)
        {
            var writer = serviceLocator.GetWriter<IIndexMapping>();
            var xml = writer.Write(indexMapping);

            document.ImportAndAppendChild(xml);
        }
    }
}