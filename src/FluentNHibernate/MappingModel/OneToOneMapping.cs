using System;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel;

[Serializable]
public class OneToOneMapping : MappingBase
{
    private readonly AttributeStore attributes;

    public OneToOneMapping()
        : this(new AttributeStore())
    {}

    public OneToOneMapping(AttributeStore attributes)
    {
        this.attributes = attributes;
    }

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessOneToOne(this);
    }

    public string Name => attributes.GetOrDefault<string>("Name");

    public string Access => attributes.GetOrDefault<string>("Access");

    public TypeReference Class => attributes.GetOrDefault<TypeReference>("Class");

    public string Cascade => attributes.GetOrDefault<string>("Cascade");

    public bool Constrained => attributes.GetOrDefault<bool>("Constrained");

    public string Fetch => attributes.GetOrDefault<string>("Fetch");

    public string ForeignKey => attributes.GetOrDefault<string>("ForeignKey");

    public string PropertyRef => attributes.GetOrDefault<string>("PropertyRef");

    public string Lazy => attributes.GetOrDefault<string>("Lazy");

    public string EntityName => attributes.GetOrDefault<string>("EntityName");

    public Type ContainingEntityType { get; set; }

    public bool Equals(OneToOneMapping other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(other.attributes, attributes) && Equals(other.ContainingEntityType, ContainingEntityType);
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
