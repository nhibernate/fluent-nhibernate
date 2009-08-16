using System.Collections.Generic;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IJoinInspector : IInspector
    {
        IEnumerable<IAnyInspector> Anys { get; }
        Fetch Fetch { get; }
        bool Inverse { get; }
        IKeyInspector Key { get; }
        bool Optional { get; }
        IEnumerable<IPropertyInspector> Properties { get; }
        IEnumerable<IManyToOneInspector> References { get; }
        string Schema { get; }
        string TableName { get; }
        string Catalog { get; }
        string Subselect { get; }
    }
}