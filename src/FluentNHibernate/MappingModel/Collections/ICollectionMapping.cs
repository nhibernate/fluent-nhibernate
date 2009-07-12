using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentNHibernate.MappingModel.Collections
{
    public interface ICollectionMapping : IMappingBase
    {
        CacheMapping Cache { get; set; }
        bool Inverse { get; set; }
        Laziness Lazy { get; set; }
        string Access { get; set; }
        string TableName { get; set; }
        string Schema { get; set; }
        string OuterJoin { get; set; }
        string Fetch { get; set; }
        string Cascade { get; set; }
        string Where { get; set; }
        string Persister { get; }
        string Name { get; set; }
        int BatchSize { get; }
        string Check { get; }
        TypeReference CollectionType { get; }
        string OptimisticLock { get; }
        bool Generic { get; }
        KeyMapping Key { get; set; }
        ICollectionRelationshipMapping Relationship { get; set; }
        MemberInfo MemberInfo { get; set;  }
        ElementMapping Element { get; set; }
        CompositeElementMapping CompositeElement { get; set; }
        Type ContainingEntityType { get; set; }
        Type ChildType { get; set; }

        bool IsSpecified<TResult>(Expression<Func<ICollectionMapping, TResult>> property);
        bool HasValue<TResult>(Expression<Func<ICollectionMapping, TResult>> property);
        void SetDefaultValue<TResult>(Expression<Func<ICollectionMapping, TResult>> property, TResult value);
    }
}