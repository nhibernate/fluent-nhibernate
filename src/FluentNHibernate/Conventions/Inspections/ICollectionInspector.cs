using System;
using System.Reflection;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface ICollectionInspector : IInspector
    {
        IKeyInspector Key { get; }
        string TableName { get; }
        bool IsMethodAccess { get; }
        MemberInfo Member { get; }
        IRelationshipInspector Relationship { get; }
    }

    public interface IOneToManyCollectionInspector : ICollectionInspector
    {
        new IOneToManyInspector Relationship { get; }
    }

    public interface IManyToManyCollectionInspector : ICollectionInspector
    {
        new IManyToManyInspector Relationship { get; }
        Type ChildType { get; }
    }
}