using System.Collections.Generic;
using FluentNHibernate.MappingModel.Collections;

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
        IEnumerable<ICollectionInspector> Collections { get;}
        string Schema { get; }
        string TableName { get; }
        string Catalog { get; }
        string Subselect { get; }
    }
}