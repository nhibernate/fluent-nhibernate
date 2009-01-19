using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmIdGeneratorWriter : MappingModelVisitorBase, IHbmWriter<IdGeneratorMapping>
    {
        private HbmGenerator _hbmGenerator;

        public object Write(IdGeneratorMapping mappingModel)
        {
            mappingModel.AcceptVisitor(this);
            return _hbmGenerator;
        }

        public override void ProcessIdGenerator(IdGeneratorMapping generatorMapping)
        {
            _hbmGenerator = new HbmGenerator();
            _hbmGenerator.@class = generatorMapping.ClassName;
        }
    }
}
