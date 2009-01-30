using FluentNHibernate.MappingModel.Identity;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmIdentityWriter : NullMappingModelVisitor, IHbmWriter<IIdentityMapping>
    {
        private readonly IHbmWriter<IdMapping> _idWriter;
        private readonly IHbmWriter<CompositeIdMapping> _compositeIdWriter;

        private object _hbm;

        public HbmIdentityWriter(IHbmWriter<IdMapping> idWriter, IHbmWriter<CompositeIdMapping> compositeIdWriter)
        {
            _idWriter = idWriter;
            _compositeIdWriter = compositeIdWriter;
        }

        public object Write(IIdentityMapping mappingModel)
        {
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