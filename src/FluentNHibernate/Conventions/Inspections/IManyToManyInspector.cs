using System.Collections.Generic;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IManyToManyInspector : IInspector
    {
        IEnumerable<IColumnInspector> ParentKeyColumns { get; }
        IEnumerable<IColumnInspector> ChildKeyColumns { get; }
    }
}