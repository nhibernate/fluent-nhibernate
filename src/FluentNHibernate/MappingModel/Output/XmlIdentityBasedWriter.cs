using System.Xml;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlIdentityBasedWriter : NullMappingModelVisitor, IXmlWriter<IIdentityMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        private XmlDocument document;

        public XmlIdentityBasedWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(IIdentityMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessId(IdMapping mapping)
        {
            var writer = serviceLocator.GetWriter<IdMapping>();
            document = writer.Write(mapping);
        }

        public override void ProcessCompositeId(CompositeIdMapping idMapping)
        {
            var writer = serviceLocator.GetWriter<CompositeIdMapping>();
            document = writer.Write(idMapping);
        }
    }
}