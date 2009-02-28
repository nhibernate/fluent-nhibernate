using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;
using FluentNHibernate.Versioning.HbmExtensions;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmOneToManyWriter : NullMappingModelVisitor, IHbmWriter<OneToManyMapping>
    {
        private HbmOneToMany _hbm;

        public object Write(OneToManyMapping mappingModel)
        {
            _hbm = null;
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessOneToMany(OneToManyMapping oneToManyMapping)
        {
            _hbm = new HbmOneToMany();
            _hbm.@class = oneToManyMapping.ClassName;

            if(oneToManyMapping.Attributes.IsSpecified(x => x.ExceptionOnNotFound))
            {
                _hbm.SetNotFound(oneToManyMapping.ExceptionOnNotFound);
            }
        }
    }
}
