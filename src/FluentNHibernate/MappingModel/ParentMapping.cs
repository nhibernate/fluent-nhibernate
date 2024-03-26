using System;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel;

[Serializable]
public class ParentMapping : MappingBase, IEquatable<ParentMapping>
{
    readonly AttributeStore attributes;

    public ParentMapping()
        : this(new AttributeStore())
    {}

    protected ParentMapping(AttributeStore attributes)
    {
        this.attributes = attributes;
    }

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessParent(this);
    }

    public Type ContainingEntityType { get; set; }

    public string Name => attributes.GetOrDefault<string>("Name");

    public string Access => attributes.GetOrDefault<string>("Access");

    public override bool Equals(object obj)
    {
        if (obj.GetType() != typeof(ParentMapping)) return false;
            
        return Equals((ParentMapping)obj);
    }

    public bool Equals(ParentMapping other)
    {
        return Equals(other.attributes, attributes) && other.ContainingEntityType == ContainingEntityType;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((attributes is not null ? attributes.GetHashCode() : 0) * 397) ^
                   (ContainingEntityType is not null ? ContainingEntityType.GetHashCode() : 0);
        }
    }

    public void Set<T>(Expression<Func<ParentMapping, T>> expression, int layer, T value)
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
