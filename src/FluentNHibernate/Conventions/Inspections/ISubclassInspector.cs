using System.Collections.Generic;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface ISubclassInspector : ISubclassInspectorBase
    {
        object DiscriminatorValue { get; }
        new IEnumerable<ISubclassInspector> Subclasses { get; }
    }
}