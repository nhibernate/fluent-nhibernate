using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmColumnConverter : HbmConverterBase<ColumnMapping, HbmColumn>
    {
        private HbmColumn hbmColumn;

        public HbmColumnConverter() : base(null)
        {
        }

        public override HbmColumn Convert(ColumnMapping columnMapping)
        {
            columnMapping.AcceptVisitor(this);
            return hbmColumn;
        }

        public override void ProcessColumn(ColumnMapping columnMapping)
        {
            hbmColumn = new HbmColumn();

            if (columnMapping.IsSpecified("Name"))
                hbmColumn.name = columnMapping.Name;

            if (columnMapping.IsSpecified("Check"))
                hbmColumn.check = columnMapping.Check;

            if (columnMapping.IsSpecified("Length"))
                hbmColumn.length = columnMapping.Length.ToString();

            if (columnMapping.IsSpecified("Index"))
                hbmColumn.index = columnMapping.Index;

            bool notNullSpecified = columnMapping.IsSpecified("NotNull");
            hbmColumn.notnullSpecified = notNullSpecified;
            if (notNullSpecified)
                hbmColumn.notnull = columnMapping.NotNull;

            if (columnMapping.IsSpecified("SqlType"))
                hbmColumn.sqltype = columnMapping.SqlType;

            bool uniqueSpecified = columnMapping.IsSpecified("Unique");
            hbmColumn.uniqueSpecified = uniqueSpecified;
            if (uniqueSpecified)
                hbmColumn.unique = columnMapping.Unique;

            if (columnMapping.IsSpecified("UniqueKey"))
                hbmColumn.uniquekey = columnMapping.UniqueKey;

            if (columnMapping.IsSpecified("Precision"))
                hbmColumn.precision = columnMapping.Precision.ToString();

            if (columnMapping.IsSpecified("Scale"))
                hbmColumn.scale = columnMapping.Scale.ToString();

            if (columnMapping.IsSpecified("Default"))
                hbmColumn.@default = columnMapping.Default;
        }
    }
}