using System.Collections.Generic;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IJoinedSubclassInspector : ISubclassInspectorBase
    {
        string Check { get; }
        string TableName { get; }
        new IEnumerable<IJoinedSubclassInspector> Subclasses { get; }
    }
}