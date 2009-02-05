using System;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmDiscriminatorWriter : NullMappingModelVisitor, IHbmWriter<DiscriminatorMapping>
    {
        private HbmDiscriminator _hbm;

        public object Write(DiscriminatorMapping mappingModel)
        {
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessDiscriminator(DiscriminatorMapping discriminatorMapping)
        {
            _hbm = new HbmDiscriminator();
        }
    }
}