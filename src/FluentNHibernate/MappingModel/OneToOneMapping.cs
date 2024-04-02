using System;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel;

[Serializable]
public class OneToOneMapping(AttributeStore attributes) : MappingBase, IEquatable<OneToOneMapping>
{
    readonly AttributeStore attributes = attributes;

    public OneToOneMapping()
        : this(new AttributeStore())
    {}

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessOneToOne(this);
    }

    public string Name => attributes.GetOrDefault<string>();

    public string Access => attributes.GetOrDefault<string>();

    public TypeReference Class => attributes.GetOrDefault<TypeReference>();

    public string Cascade => attributes.GetOrDefault<string>();

    public bool Constrained => attributes.GetOrDefault<bool>();

    public string Fetch => attributes.GetOrDefault<string>();

    public string ForeignKey => attributes.GetOrDefault<string>();

    public string PropertyRef => attributes.GetOrDefault<string>();

    public string Lazy => attributes.GetOrDefault<string>();

    public string EntityName => attributes.GetOrDefault<string>();

    public Type ContainingEntityType { get; set; }

    public bool Equals(OneToOneMapping other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(other.attributes, attributes) && other.ContainingEntityType == ContainingEntityType;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != typeof(OneToOneMapping)) return false;
        return Equals((OneToOneMapping)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((attributes is not null ? attributes.GetHashCode() : 0) * 397) ^ (ContainingEntityType is not null ? ContainingEntityType.GetHashCode() : 0);
        }
    }

    public void Set<T>(Expression<Func<OneToOneMapping, T>> expression, int layer, T value)
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
