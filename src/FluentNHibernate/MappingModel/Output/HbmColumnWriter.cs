using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmColumnWriter : NullMappingModelVisitor, IHbmWriter<ColumnMapping>
    {
        private HbmColumn _hbm;

        public object Write(ColumnMapping mappingModel)
        {
            _hbm = null;
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessColumn(ColumnMapping columnMapping)
        {
            _hbm = new HbmColumn();
            _hbm.name = columnMapping.Name;
            
            if(columnMapping.Attributes.IsSpecified(x => x.IsNotNullable))
            {
                _hbm.notnull = columnMapping.IsNotNullable;
                _hbm.notnullSpecified = true;
            }

            if (columnMapping.Attributes.IsSpecified(x => x.Length))
                _hbm.length = columnMapping.Length.ToString();

            if(columnMapping.Attributes.IsSpecified(x => x.IsUnique))
            {
                _hbm.unique = columnMapping.IsUnique;
                _hbm.uniqueSpecified = true;
            }

            if (columnMapping.Attributes.IsSpecified(x => x.UniqueKey))
                _hbm.uniquekey = columnMapping.UniqueKey;

            if (columnMapping.Attributes.IsSpecified(x => x.SqlType))
                _hbm.sqltype = columnMapping.SqlType;

            if (columnMapping.Attributes.IsSpecified(x => x.Index))
                _hbm.index = columnMapping.Index;

            if (columnMapping.Attributes.IsSpecified(x => x.Check))
                _hbm.check = columnMapping.Check;
        }
    }
}
