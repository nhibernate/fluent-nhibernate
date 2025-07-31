using System;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel;

[Serializable]
public class VersionMapping(AttributeStore underlyingStore) : ColumnBasedMappingBase(underlyingStore)
{
    public VersionMapping()
        : this(new AttributeStore())
    {}

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessVersion(this);

        foreach (var column in Columns)
            visitor.Visit(column);
    }

    public string Name => attributes.GetOrDefault<string>();

    public string Access => attributes.GetOrDefault<string>();

    public TypeReference Type => attributes.GetOrDefault<TypeReference>();

    public string UnsavedValue => attributes.GetOrDefault<string>();

    public string Generated => attributes.GetOrDefault<string>();

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
