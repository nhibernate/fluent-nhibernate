using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IManyToManyCollectionInstance : IManyToManyCollectionInspector, ICollectionInstance
    {
        new IManyToManyInstance Relationship { get; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        new IManyToManyCollectionInstance Not { get; }
        new IManyToManyCollectionInstance OtherSide { get; }
    }
}