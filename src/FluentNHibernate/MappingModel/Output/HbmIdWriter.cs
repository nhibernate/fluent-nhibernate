using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmIdWriter : MappingModelVisitorBase, IHbmWriter<IdMapping>
    {
        private readonly IHbmWriter<ColumnMapping> _columnWriter;
        private readonly IHbmWriter<IdGeneratorMapping> _generatorWriter;

        private HbmId _hbmId;

        public HbmIdWriter(IHbmWriter<ColumnMapping> columnWriter, IHbmWriter<IdGeneratorMapping> generatorWriter)
        {
            _columnWriter = columnWriter;
            _generatorWriter = generatorWriter;
        }

        public object Write(IdMapping mappingModel)
        {
            mappingModel.AcceptVisitor(this);
            return _hbmId;
        }

        public override void ProcessId(IdMapping idMapping)
        {
            _hbmId = new HbmId();

            if(idMapping.Attributes.IsSpecified(x => x.Name))
                _hbmId.name = idMapping.Name;
        }

        public override void ProcessColumn(ColumnMapping columnMapping)
        {
            var columnHbm = (HbmColumn) _columnWriter.Write(columnMapping);
            columnHbm.AddTo(ref _hbmId.column);
        }

        public override void ProcessIdGenerator(IdGeneratorMapping generatorMapping)
        {
            var generatorHbm = (HbmGenerator) _generatorWriter.Write(generatorMapping);
            _hbmId.generator = generatorHbm;
        }
    }
}
