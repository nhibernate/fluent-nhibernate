using System;
using System.Linq.Expressions;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Collections
{
    //[Serializable]
    //public class BagMapping : CollectionMapping
    //{
    //    private readonly AttributeStore<BagMapping> attributes;

    //    public BagMapping()
    //        : this(new AttributeStore())
    //    {}

    //    public BagMapping(AttributeStore underlyingStore)
    //        : base(underlyingStore)
    //    {
    //        attributes = new AttributeStore<BagMapping>(underlyingStore);
    //    }

    //    public override void AcceptVisitor(IMappingModelVisitor visitor)
    //    {
    //        visitor.ProcessBag(this);
    //        base.AcceptVisitor(visitor);
    //    }

    //    public override string OrderBy
    //    {
    //        get { return attributes.Get(x => x.OrderBy); }
    //        set { attributes.Set(x => x.OrderBy, value); }
    //    }

    //    public new bool IsSpecified(string property)
    //    {
    //        return attributes.IsSpecified(property);
    //    }

    //    public bool HasValue<TResult>(Expression<Func<BagMapping, TResult>> property)
    //    {
    //        return attributes.HasValue(property);
    //    }

    //    public void SetDefaultValue<TResult>(Expression<Func<BagMapping, TResult>> property, TResult value)
    //    {
    //        attributes.SetDefault(property, value);
    //    }

    //    public bool Equals(BagMapping other)
    //    {
    //        if (ReferenceEquals(null, other)) return false;
    //        if (ReferenceEquals(this, other)) return true;
    //        return base.Equals(other) && Equals(other.attributes, attributes);
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        if (ReferenceEquals(null, obj)) return false;
    //        if (ReferenceEquals(this, obj)) return true;
    //        return Equals(obj as BagMapping);
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