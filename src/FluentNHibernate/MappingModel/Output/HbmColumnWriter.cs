using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmColumnWriter : NullMappingModelVisitor, IHbmWriter<ColumnMapping>
    {
        private HbmColumn _hbmColumn;

        public object Write(ColumnMapping mappingModel)
        {
            mappingModel.AcceptVisitor(this);
            return _hbmColumn;
        }

        public override void ProcessColumn(ColumnMapping columnMapping)
        {
            _hbmColumn = new HbmColumn();
            _hbmColumn.name = columnMapping.Name;
            
            if(columnMapping.Attributes.IsSpecified(x => x.IsNotNullable))
            {
                _hbmColumn.notnull = columnMapping.IsNotNullable;
                _hbmColumn.notnullSpecified = true;
            }

            if (columnMapping.Attributes.IsSpecified(x => x.Length))
                _hbmColumn.length = columnMapping.Length.ToString();

            if(columnMapping.Attributes.IsSpecified(x => x.IsUnique))
            {
                _hbmColumn.unique = columnMapping.IsUnique;
                _hbmColumn.uniqueSpecified = true;
            }

            if (columnMapping.Attributes.IsSpecified(x => x.UniqueKey))
                _hbmColumn.uniquekey = columnMapping.UniqueKey;

            if (columnMapping.Attributes.IsSpecified(x => x.SqlType))
                _hbmColumn.sqltype = columnMapping.SqlType;

            if (columnMapping.Attributes.IsSpecified(x => x.Index))
                _hbmColumn.index = columnMapping.Index;

            if (columnMapping.Attributes.IsSpecified(x => x.Check))
                _hbmColumn.check = columnMapping.Check;
        }
    }
}
