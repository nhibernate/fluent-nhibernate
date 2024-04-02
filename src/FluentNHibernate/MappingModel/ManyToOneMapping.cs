using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel;

[Serializable]
public class ManyToOneMapping(AttributeStore attributes) : MappingBase, IHasColumnMappings, IRelationship, IEquatable<ManyToOneMapping>
{
    readonly AttributeStore attributes = attributes;
    readonly LayeredColumns columns = new LayeredColumns();

    public ManyToOneMapping()
        : this(new AttributeStore())
    {}

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessManyToOne(this);

        foreach (var column in Columns)
            visitor.Visit(column);
    }

    public Type ContainingEntityType { get; set; }
    public Member Member { get; set; }

    public string Name => attributes.GetOrDefault<string>();

    public string Access => attributes.GetOrDefault<string>();

    public TypeReference Class => attributes.GetOrDefault<TypeReference>();

    public string Cascade => attributes.GetOrDefault<string>();

    public string Fetch => attributes.GetOrDefault<string>();

    public bool Update => attributes.GetOrDefault<bool>();

    public bool Insert => attributes.GetOrDefault<bool>();

    public string Formula => attributes.GetOrDefault<string>();

    public string ForeignKey => attributes.GetOrDefault<string>();

    public string PropertyRef => attributes.GetOrDefault<string>();

    public string NotFound => attributes.GetOrDefault<string>();

    public string Lazy => attributes.GetOrDefault<string>();

    public string EntityName => attributes.GetOrDefault<string>();

    public bool OptimisticLock => attributes.GetOrDefault<bool>();

    public IEnumerable<ColumnMapping> Columns => columns.Columns;

    public void AddColumn(int layer, ColumnMapping mapping)
    {
        columns.AddColumn(layer, mapping);
    }

    public void MakeColumnsEmpty(int layer)
    {
        columns.MakeColumnsEmpty(layer);
    }

    public void Set<T>(Expression<Func<ManyToOneMapping, T>> expression, int layer, T value)
    {
        Set(expression.ToMember().Name, layer, value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
        attributes.Set(attribute, layer, value);
    }

    public bool Equals(ManyToOneMapping other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(other.attributes, attributes) &&
               other.columns.ContentEquals(columns) &&
               other.ContainingEntityType == ContainingEntityType &&
               Equals(other.Member, Member);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != typeof(ManyToOneMapping)) return false;
        return Equals((ManyToOneMapping)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int result = (attributes is not null ? attributes.GetHashCode() : 0);
            result = (result * 397) ^ (columns is not null ? columns.GetHashCode() : 0);
            result = (result * 397) ^ (ContainingEntityType is not null ? ContainingEntityType.GetHashCode() : 0);
            result = (result * 397) ^ (Member is not null ? Member.GetHashCode() : 0);
            return result;
        }
    }

    public IRelationship OtherSide { get; set; }

    public override bool IsSpecified(string attribute)
    {
        return attributes.IsSpecified(attribute);
    }
}
