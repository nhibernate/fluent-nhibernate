using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IOneToManyCollectionInstance : IOneToManyCollectionInspector, ICollectionInstance
    {
        new IOneToManyInstance Relationship { get; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        new IOneToManyCollectionInstance Not { get; }
    }
}