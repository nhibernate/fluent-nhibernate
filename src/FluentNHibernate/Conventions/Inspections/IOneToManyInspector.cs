using System.Collections.Generic;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IOneToManyInspector : IInspector
    {
        IEnumerable<IColumnInspector> KeyColumns { get; }
    }
}