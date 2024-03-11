using System;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel;

[Serializable]
public class VersionMapping : ColumnBasedMappingBase
{
    public VersionMapping()
        : this(new AttributeStore())
    {}

    public VersionMapping(AttributeStore underlyingStore)
        : base(underlyingStore)
    {}

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessVersion(this);

        Columns.Each(visitor.Visit);
    }

    public string Name => attributes.GetOrDefault<string>("Name");

    public string Access => attributes.GetOrDefault<string>("Access");

    public TypeReference Type => attributes.GetOrDefault<TypeReference>("Type");

    public string UnsavedValue => attributes.GetOrDefault<string>("UnsavedValue");

    public string Generated => attributes.GetOrDefault<string>("Generated");

    public Type ContainingEntityType { get; set; }

    public bool Equals(VersionMapping other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other) && other.ContainingEntityType == ContainingEntityType;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return Equals(obj as VersionMapping);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            {
                return (base.GetHashCode() * 397) ^ (ContainingEntityType is not null ? ContainingEntityType.GetHashCode() : 0);
            }
        }
    }

    public void Set<T>(Expression<Func<VersionMapping, T>> expression, int layer, T value)
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
