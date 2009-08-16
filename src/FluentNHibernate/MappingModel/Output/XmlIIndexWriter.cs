using System.Xml;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlIIndexWriter : NullMappingModelVisitor, IXmlWriter<IIndexMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        private XmlDocument document;

        public XmlIIndexWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(IIndexMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessIndex(IndexMapping mapping)
        {
            var writer = serviceLocator.GetWriter<IndexMapping>();
            document = writer.Write(mapping);
        }

        public override void ProcessIndex(IndexManyToManyMapping mapping)
        {
            var writer = serviceLocator.GetWriter<IndexManyToManyMapping>();
            document = writer.Write(mapping);
        }
    }
}