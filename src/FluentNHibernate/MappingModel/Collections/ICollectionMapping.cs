using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentNHibernate.MappingModel.Collections
{
    public interface ICollectionMapping : IMappingBase
    {
        CacheMapping Cache { get; set; }
        bool Inverse { get; set; }
        bool Lazy { get; set; }
        string Access { get; set; }
        string TableName { get; set; }
        string Schema { get; set; }
        string Fetch { get; set; }
        string Cascade { get; set; }
        string Where { get; set; }
		string OrderBy { get; set; }
        bool Mutable { get; set;}
        string Subselect { get; set; }
        TypeReference Persister { get; set; }
        string Name { get; set; }
        int BatchSize { get; set; }
        string Check { get; set; }
        TypeReference CollectionType { get; set; }
        string OptimisticLock { get; set; }
        bool Generic { get; set; }
        KeyMapping Key { get; set; }
        ICollectionRelationshipMapping Relationship { get; set; }
        MemberInfo MemberInfo { get; set;  }
        ElementMapping Element { get; set; }
        CompositeElementMapping CompositeElement { get; set; }
        Type ContainingEntityType { get; set; }
        Type ChildType { get; set; }
        ICollectionMapping OtherSide { get; set; }

        bool IsSpecified<TResult>(Expression<Func<ICollectionMapping, TResult>> property);
        bool HasValue<TResult>(Expression<Func<ICollectionMapping, TResult>> property);
        void SetDefaultValue<TResult>(Expression<Func<ICollectionMapping, TResult>> property, TResult value);
    }
}