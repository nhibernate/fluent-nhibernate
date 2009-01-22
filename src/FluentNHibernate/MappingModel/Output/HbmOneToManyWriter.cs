using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;
using FluentNHibernate.Versioning.HbmExtensions;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmOneToManyWriter : MappingModelVisitorBase, IHbmWriter<OneToManyMapping>
    {
        private HbmOneToMany _hbmOneToMany;

        public object Write(OneToManyMapping mappingModel)
        {
            mappingModel.AcceptVisitor(this);
            return _hbmOneToMany;
        }

        public override void ProcessOneToMany(OneToManyMapping oneToManyMapping)
        {
            _hbmOneToMany = new HbmOneToMany();
            _hbmOneToMany.@class = oneToManyMapping.ClassName;

            if(oneToManyMapping.Attributes.IsSpecified(x => x.ExceptionOnNotFound))
            {
                _hbmOneToMany.SetNotFound(oneToManyMapping.ExceptionOnNotFound);
            }
        }
    }
}
