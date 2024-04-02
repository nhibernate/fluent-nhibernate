using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Collections;

[Serializable]
public class IndexManyToManyMapping(AttributeStore attributes) : MappingBase, IIndexMapping, IHasColumnMappings, IEquatable<IndexManyToManyMapping>
{
    readonly AttributeStore attributes = attributes;
    readonly LayeredColumns columns = new LayeredColumns();

    public IndexManyToManyMapping()
        : this(new AttributeStore())
    {}

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessIndex(this);

        foreach (var column in Columns)
            visitor.Visit(column);
    }

    public Type ContainingEntityType { get; set; }

    public TypeReference Class => attributes.GetOrDefault<TypeReference>();

    public IEnumerable<ColumnMapping> Columns => columns.Columns;

    public void AddColumn(int layer, ColumnMapping mapping)
    {
        columns.AddColumn(layer, mapping);
    }

    public void MakeColumnsEmpty(int layer)
    {
        columns.MakeColumnsEmpty(layer);
    }

    public string ForeignKey => attributes.GetOrDefault<string>();

    public string EntityName => attributes.GetOrDefault<string>();

    public bool Equals(IndexManyToManyMapping other)
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
        if (obj.GetType() != typeof(IndexManyToManyMapping)) return false;
        return Equals((IndexManyToManyMapping)obj);
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

    public void Set<T>(Expression<Func<IndexManyToManyMapping, T>> expression, int layer, T value)
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
