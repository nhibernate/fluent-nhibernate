using System.Collections.Generic;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IJoinedSubclassInspector : ISubclassInspectorBase
    {
        IKeyInspector Key { get; }
        string Check { get; }
        string TableName { get; }
        new IEnumerable<IJoinedSubclassInspector> Subclasses { get; }
    }
}