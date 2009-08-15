using System;
using System.Reflection;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface ICollectionInspector : IInspector
    {
        IKeyInspector Key { get; }
        string TableName { get; }
        bool IsMethodAccess { get; }
        MemberInfo Member { get; }
        IRelationshipInspector Relationship { get; }
        Cascade Cascade { get; }
        Fetch Fetch { get; }
        OptimisticLock OptimisticLock { get; }
        bool Generic { get; }
        bool Inverse { get; }
        Access Access { get; }
        int BatchSize { get; }
        ICacheInspector Cache { get; }
        string Check { get; }
        Type ChildType { get; }
        TypeReference CollectionType { get; }
        ICompositeElementInspector CompositeElement { get; }
        IElementInspector Element { get; }
        bool LazyLoad { get; }
        string Name { get; }
        TypeReference Persister { get; }
        string Schema { get; }
        string Where { get; }
        string OrderBy { get; }
    }
}