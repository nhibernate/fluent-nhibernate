using System;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel;

[Serializable]
public class PropertyMapping : ColumnBasedMappingBase
{
    public PropertyMapping()
        : this(new AttributeStore())
    {}

    public PropertyMapping(AttributeStore underlyingStore)
        : base(underlyingStore)
    {}

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessProperty(this);

        foreach (var column in Columns)
            visitor.Visit(column);
    }

    public Type ContainingEntityType { get; set; }

    public string Name => attributes.GetOrDefault<string>("Name");

    public string Access => attributes.GetOrDefault<string>("Access");

    public bool Insert => attributes.GetOrDefault<bool>("Insert");

    public bool Update => attributes.GetOrDefault<bool>("Update");

    public string Formula => attributes.GetOrDefault<string>("Formula");

    public bool Lazy => attributes.GetOrDefault<bool>("Lazy");

    public bool OptimisticLock => attributes.GetOrDefault<bool>("OptimisticLock");

    public string Generated => attributes.GetOrDefault<string>("Generated");

    public TypeReference Type => attributes.GetOrDefault<TypeReference>("Type");

    public Member Member { get; set; }

    public bool Equals(PropertyMapping other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other) &&
               other.ContainingEntityType == ContainingEntityType &&
               Equals(other.Member, Member);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != typeof(PropertyMapping)) return false;
        return Equals((PropertyMapping)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((ContainingEntityType is not null ? ContainingEntityType.GetHashCode() : 0) * 397) ^ (Member is not null ? Member.GetHashCode() : 0);
        }
    }

    public void Set<T>(Expression<Func<PropertyMapping, T>> expression, int layer, T value)
    {
        Set(expression.ToMember().Name, layer, value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
        attributes.Set(attribute, layer, value);
    }

    public override bool IsSpecified(string attribute)
    {
        return attributes.IsSpecified(attribute);
    }
}
