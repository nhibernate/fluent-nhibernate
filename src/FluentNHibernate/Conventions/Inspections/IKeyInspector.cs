using System.Collections.Generic;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IKeyInspector : IInspector
    {
        IEnumerable<IColumnInspector> Columns { get; }
        string ForeignKey { get; }
        string OnDelete { get; }
        string PropertyRef { get; }
    }
}