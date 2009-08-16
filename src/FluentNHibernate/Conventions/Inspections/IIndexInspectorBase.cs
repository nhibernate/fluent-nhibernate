using System.Collections.Generic;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IIndexInspectorBase :IInspector
    {
        IEnumerable<IColumnInspector> Columns { get; }
    }
}
