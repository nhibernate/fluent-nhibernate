using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmCompositeIdWriter : NullMappingModelVisitor, IHbmWriter<CompositeIdMapping>
    {
        private HbmCompositeId _hbmCompositeId;

        public object Write(CompositeIdMapping mappingModel)
        {
            mappingModel.AcceptVisitor(this);
            return _hbmCompositeId;
        }

        public override void ProcessCompositeId(CompositeIdMapping idMapping)
        {
            _hbmCompositeId = new HbmCompositeId();
        }
    }
}
