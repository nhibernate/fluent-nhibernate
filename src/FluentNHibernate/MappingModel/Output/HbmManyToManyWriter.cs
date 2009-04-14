using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmManyToManyWriter : NullMappingModelVisitor, IXmlWriter<ManyToManyMapping>
    {
        private HbmManyToMany _hbm;

        public object Write(ManyToManyMapping mappingModel)
        {
            _hbm = null;
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessManyToMany(ManyToManyMapping manyToManyMapping)
        {
            _hbm = new HbmManyToMany();
            _hbm.@class = manyToManyMapping.ClassName;
        }
    }
}
