using System.Xml;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlIdentityBasedWriter : NullMappingModelVisitor, IXmlWriter<IIdentityMapping>
    {
        private XmlDocument document;
        private readonly IXmlWriter<IdMapping> idWriter;

        public XmlIdentityBasedWriter(IXmlWriter<IdMapping> idWriter)
        {
            this.idWriter = idWriter;
        }

        public XmlDocument Write(IIdentityMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessId(IdMapping mapping)
        {
            document = idWriter.Write(mapping);
        }

        public override void ProcessCompositeId(CompositeIdMapping idMapping)
        {
            base.ProcessCompositeId(idMapping);
        }
    }
}