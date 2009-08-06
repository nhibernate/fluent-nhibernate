using System;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IManyToManyCollectionInspector : ICollectionInspector
    {
        new IManyToManyInspector Relationship { get; }
        Type ChildType { get; }

        bool HasExplicitTable { get; }
        IManyToManyCollectionInspector OtherSide { get; }
    }
}