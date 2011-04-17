using System.Collections.Generic;

namespace FluentNHibernate.MappingModel
{
    public interface IHasColumnMappings
    {
        IEnumerable<ColumnMapping> Columns { get; }
        void AddColumn(int layer, ColumnMapping column);
        void MakeColumnsEmpty(int layer);
    }
}