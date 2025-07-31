using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Identity;

[Serializable]
public class KeyManyToOneMapping : MappingBase, ICompositeIdKeyMapping, IEquatable<KeyManyToOneMapping>
{
    readonly AttributeStore attributes = new AttributeStore();
    readonly List<ColumnMapping> columns = [];

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessKeyManyToOne(this);

        foreach (var column in columns)
            visitor.Visit(column);
    }

    public string Access => attributes.GetOrDefault<string>();

    public string Name => attributes.GetOrDefault<string>();

    public TypeReference Class => attributes.GetOrDefault<TypeReference>();

    public string ForeignKey => attributes.GetOrDefault<string>();

    public bool Lazy => attributes.GetOrDefault<bool>();

    public string NotFound => attributes.GetOrDefault<string>();

    public string EntityName => attributes.GetOrDefault<string>();

    public IEnumerable<ColumnMapping> Columns => columns;

    public Type ContainingEntityType { get; set; }

    public void AddColumn(ColumnMapping mapping)
    {
        columns.Add(mapping);
    }

    public bool Equals(KeyManyToOneMapping other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(other.attributes, attributes) &&
               other.columns.ContentEquals(columns) &&
               other.ContainingEntityType == ContainingEntityType;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != typeof(KeyManyToOneMapping)) return false;
        return Equals((KeyManyToOneMapping)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int result = (attributes is not null ? attributes.GetHashCode() : 0);
            result = (result * 397) ^ (columns is not null ? columns.GetHashCode() : 0);
            result = (result * 397) ^ (ContainingEntityType is not null ? ContainingEntityType.GetHashCode() : 0);
            return result;
        }
    }

    public void Set<T>(Expression<Func<KeyManyToOneMapping, T>> expression, int layer, T value)
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
