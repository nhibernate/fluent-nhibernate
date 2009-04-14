using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmIdWriter : NullMappingModelVisitor, IXmlWriter<IdMapping>
    {
        private readonly IXmlWriter<ColumnMapping> _columnWriter;
        private readonly IXmlWriter<IdGeneratorMapping> _generatorWriter;

        private HbmId _hbm;

        public HbmIdWriter(IXmlWriter<ColumnMapping> columnWriter, IXmlWriter<IdGeneratorMapping> generatorWriter)
        {
            _columnWriter = columnWriter;
            _generatorWriter = generatorWriter;
        }

        public object Write(IdMapping mappingModel)
        {
            _hbm = null; 
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessId(IdMapping idMapping)
        {
            _hbm = new HbmId();

            if(idMapping.Attributes.IsSpecified(x => x.Name))
                _hbm.name = idMapping.Name;
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            var columnHbm = (HbmColumn) _columnWriter.Write(columnMapping);
            columnHbm.AddTo(ref _hbm.column);
        }

        public override void Visit(IdGeneratorMapping generatorMapping)
        {
            var generatorHbm = (HbmGenerator) _generatorWriter.Write(generatorMapping);
            _hbm.generator = generatorHbm;
        }
    }
}
