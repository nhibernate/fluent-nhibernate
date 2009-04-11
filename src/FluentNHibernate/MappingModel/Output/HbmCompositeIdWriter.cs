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
        private HbmCompositeId _hbm;

        public object Write(CompositeIdMapping mappingModel)
        {
            _hbm = null;
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessCompositeId(CompositeIdMapping idMapping)
        {
            _hbm = new HbmCompositeId();
        }
    }
}
