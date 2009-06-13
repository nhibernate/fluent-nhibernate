namespace FluentNHibernate.MappingModel
{
    public interface IHasColumnMappings
    {
        IDefaultableEnumerable<ColumnMapping> Columns { get; }
        void AddColumn(ColumnMapping column);
        void AddDefaultColumn(ColumnMapping column);
        void ClearColumns();
    }
}