using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Collections
{
    //[Serializable]
    //public class ArrayMapping : CollectionMapping, IIndexedCollectionMapping
    //{
    //    private readonly AttributeStore<ArrayMapping> attributes;

    //    public IIndexMapping Index
    //    {
    //        get { return attributes.Get(x => x.Index); }
    //        set { attributes.Set(x => x.Index, value); }
    //    }

    //    public ArrayMapping()
    //        : this(new AttributeStore())
    //    {}

    //    public ArrayMapping(AttributeStore underlyingStore)
    //        : base(underlyingStore)
    //    {
    //        attributes = new AttributeStore<ArrayMapping>(underlyingStore);
    //    }

    //    public override void AcceptVisitor(IMappingModelVisitor visitor)
    //    {
    //        visitor.ProcessArray(this);

    //        if (Index != null)
    //            visitor.Visit(Index);

    //        base.AcceptVisitor(visitor);
    //    }

    //    public override string OrderBy
    //    {
    //        get { return null; }
    //        set { /* no-op */  }
    //    }

    //    public new bool IsSpecified(string property)
    //    {
    //        return attributes.IsSpecified(property);
    //    }

    //    public bool HasValue<TResult>(Expression<Func<ArrayMapping, TResult>> property)
    //    {
    //        return attributes.HasValue(property);
    //    }

    //    public void SetDefaultValue<TResult>(Expression<Func<ArrayMapping, TResult>> property, TResult value)
    //    {
    //        attributes.SetDefault(property, value);
    //    }

    //    public bool Equals(ArrayMapping other)
    //    {
    //        if (ReferenceEquals(null, other)) return false;
    //        if (ReferenceEquals(this, other)) return true;
    //        return base.Equals(other) && Equals(other.attributes, attributes);
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        if (ReferenceEquals(null, obj)) return false;
    //        if (ReferenceEquals(this, obj)) return true;
    //        return Equals(obj as ArrayMapping);
    //    }

    //    public override int GetHashCode()
    //    {
    //        unchecked
    //        {
    //            {
    //                return (base.GetHashCode() * 397) ^ (attributes != null ? attributes.GetHashCode() : 0);
    //            }
    //        }
    //    }
    //}
}