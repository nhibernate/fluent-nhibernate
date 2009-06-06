using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlListWriter : BaseXmlCollectionWriter, IXmlWriter<ListMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;

        public XmlListWriter(IXmlWriterServiceLocator serviceLocator)
            : base(serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

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

        public override void Visit(IIndexMapping indexMapping)
        {
            var writer = serviceLocator.GetWriter<IIndexMapping>();
            var xml = writer.Write(indexMapping);

            document.ImportAndAppendChild(xml);
        }
    }
}