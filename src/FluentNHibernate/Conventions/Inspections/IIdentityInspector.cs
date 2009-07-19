using System.Collections.Generic;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IIdentityInspector : IExposedThroughPropertyInspector
    {
        IEnumerable<IColumnInspector> Columns { get; }
        Generator Generator { get; }
        object UnsavedValue { get; }
        string Name { get; }
    }
}