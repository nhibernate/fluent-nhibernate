using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlListWriter : BaseXmlCollectionWriter, IXmlWriter<CollectionMapping>
    {
        readonly IXmlWriterServiceLocator serviceLocator;

        public XmlListWriter(IXmlWriterServiceLocator serviceLocator)
            : base(serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(CollectionMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessCollection(CollectionMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("list");

            WriteBaseCollectionAttributes(element, mapping);
        }

        public override void Visit(IIndexMapping indexMapping)
        {
            var writer = serviceLocator.GetWriter<IIndexMapping>();
            var xml = writer.Write(indexMapping);

            document.ImportAndAppendChild(xml);
        }
    }
}