using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmIdGeneratorWriter : NullMappingModelVisitor, IHbmWriter<IdGeneratorMapping>
    {
        private HbmGenerator _hbm;

        public object Write(IdGeneratorMapping mappingModel)
        {
            _hbm = null;
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessIdGenerator(IdGeneratorMapping generatorMapping)
        {
            _hbm = new HbmGenerator();
            _hbm.@class = generatorMapping.ClassName;
        }
    }
}
