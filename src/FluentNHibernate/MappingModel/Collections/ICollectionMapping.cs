using System;
using System.Reflection;

namespace FluentNHibernate.MappingModel.Collections
{
    public interface ICollectionMapping : IMappingBase
    {
        CacheMapping Cache { get; set; }
        bool Inverse { get; }
        Laziness Lazy { get; }
        string Access { get; }
        string TableName { get; set; }
        string Schema { get; }
        string OuterJoin { get; }
        string Fetch { get; }
        string Cascade { get; }
        string Where { get; }
        string Persister { get; }
        string Name { get; set; }
        int BatchSize { get; }
        string Check { get; }
        TypeReference CollectionType { get; }
        string OptimisticLock { get; }
        bool Generic { get; }
        KeyMapping Key { get; set; }
        ICollectionRelationshipMapping Relationship { get; set; }
        AttributeStore<ICollectionMapping> Attributes { get; }
        MemberInfo MemberInfo { get; set;  }
        ElementMapping Element { get; set; }
        CompositeElementMapping CompositeElement { get; set; }
        Type ContainedEntityType { get; set; }
        Type ChildType { get; set; }
    }
}