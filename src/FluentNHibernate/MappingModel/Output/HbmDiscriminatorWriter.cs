using System;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmDiscriminatorWriter : NullMappingModelVisitor, IHbmWriter<DiscriminatorMapping>
    {
        private readonly IHbmWriter<ColumnMapping> _columnWriter;
        private HbmDiscriminator _hbm;

        public HbmDiscriminatorWriter(IHbmWriter<ColumnMapping> columnWriter)
        {
            _columnWriter = columnWriter;
        }

        public object Write(DiscriminatorMapping mappingModel)
        {
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessDiscriminator(DiscriminatorMapping discriminatorMapping)
        {
            _hbm = new HbmDiscriminator();

            if (discriminatorMapping.Attributes.IsSpecified(x => x.ColumnName))
                _hbm.column1 = discriminatorMapping.ColumnName;

            if (discriminatorMapping.Attributes.IsSpecified(x => x.DiscriminatorType))
                _hbm.type = discriminatorMapping.DiscriminatorType.ToString();

            if (discriminatorMapping.Attributes.IsSpecified(x => x.Force))
                _hbm.force = discriminatorMapping.Force;

            if (discriminatorMapping.Attributes.IsSpecified(x => x.Formula))
                _hbm.formula = discriminatorMapping.Formula;

            if (discriminatorMapping.Attributes.IsSpecified(x => x.Insert))
            {
                _hbm.insertSpecified = true;
                _hbm.insert = discriminatorMapping.Insert;
            }

            if (discriminatorMapping.Attributes.IsSpecified(x => x.IsNotNullable))
                _hbm.notnull = discriminatorMapping.IsNotNullable;

            if (discriminatorMapping.Attributes.IsSpecified(x => x.Length))
                _hbm.length = discriminatorMapping.Length.ToString();
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            _hbm.column = (HbmColumn) _columnWriter.Write(columnMapping);
        }
    }
}