using System;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel;

[Serializable]
public class MetaValueMapping : MappingBase, IEquatable<MetaValueMapping>
{
    readonly AttributeStore attributes;

    public MetaValueMapping()
        : this(new AttributeStore())
    {}

    protected MetaValueMapping(AttributeStore attributes)
    {
        this.attributes = attributes;
    }

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessMetaValue(this);
    }

    public string Value => attributes.GetOrDefault<string>();

    public TypeReference Class => attributes.GetOrDefault<TypeReference>();

    public Type ContainingEntityType { get; set; }

    public bool Equals(MetaValueMapping other)
    {
        return Equals(other.attributes, attributes) && other.ContainingEntityType == ContainingEntityType;
    }

    public override bool Equals(object obj)
    {
        if (obj.GetType() != typeof(MetaValueMapping)) return false;
        return Equals((MetaValueMapping)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((attributes is not null ? attributes.GetHashCode() : 0) * 397) ^
                   (ContainingEntityType is not null ? ContainingEntityType.GetHashCode() : 0);
        }
    }

    public void Set<T>(Expression<Func<MetaValueMapping, T>> expression, int layer, T value)
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
