using FluentNHibernate.MappingModel.Identity;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmIdentityWriter : NullMappingModelVisitor, IXmlWriter<IIdentityMapping>
    {
        private readonly IXmlWriter<IdMapping> _idWriter;
        private readonly IXmlWriter<CompositeIdMapping> _compositeIdWriter;

        private object _hbm;

        public HbmIdentityWriter(IXmlWriter<IdMapping> idWriter, IXmlWriter<CompositeIdMapping> compositeIdWriter)
        {
            _idWriter = idWriter;
            _compositeIdWriter = compositeIdWriter;
        }

        public object Write(IIdentityMapping mappingModel)
        {
            _hbm = null;
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessId(IdMapping idMapping)
        {
            _hbm = _idWriter.Write(idMapping);
        }

        public override void ProcessCompositeId(CompositeIdMapping compositeIdMapping)
        {
            _hbm = _compositeIdWriter.Write(compositeIdMapping);
        }

    }
}