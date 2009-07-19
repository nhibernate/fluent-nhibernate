using System.Collections.Generic;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IKeyInstance : IKeyInspector
    {
        void ColumnName(string columnName);
        void ForeignKey(string constraint);
        new IEnumerable<IColumnInspector> Columns { get; }
    }
}