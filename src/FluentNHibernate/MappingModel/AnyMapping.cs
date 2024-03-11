using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel;

[Serializable]
public class AnyMapping : MappingBase
{
    readonly AttributeStore attributes;
    readonly LayeredColumns typeColumns = new LayeredColumns();
    readonly LayeredColumns identifierColumns = new LayeredColumns();
    readonly IList<MetaValueMapping> metaValues = new List<MetaValueMapping>();

    public AnyMapping()
        : this(new AttributeStore())
    {}

    public AnyMapping(AttributeStore attributes)
    {
        this.attributes = attributes;
    }

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessAny(this);

        foreach (var metaValue in metaValues)
            visitor.Visit(metaValue);

        foreach (var column in TypeColumns)
            visitor.Visit(column);

        foreach (var column in IdentifierColumns)
            visitor.Visit(column);
    }

    public string Name => attributes.GetOrDefault<string>("Name");

    public string IdType => attributes.GetOrDefault<string>("IdType");

    public TypeReference MetaType => attributes.GetOrDefault<TypeReference>("MetaType");

    public string Access => attributes.GetOrDefault<string>("Access");

    public bool Insert => attributes.GetOrDefault<bool>("Insert");

    public bool Update => attributes.GetOrDefault<bool>("Update");

    public string Cascade => attributes.GetOrDefault<string>("Cascade");

    public bool Lazy => attributes.GetOrDefault<bool>("Lazy");

    public bool OptimisticLock => attributes.GetOrDefault<bool>("OptimisticLock");

    public IEnumerable<ColumnMapping> TypeColumns => typeColumns.Columns;

    public IEnumerable<ColumnMapping> IdentifierColumns => identifierColumns.Columns;

    public IEnumerable<MetaValueMapping> MetaValues => metaValues;

    public Type ContainingEntityType { get; set; }

    public void AddTypeColumn(int layer, ColumnMapping column)
    {
        typeColumns.AddColumn(layer, column);
    }

    public void AddIdentifierColumn(int layer, ColumnMapping column)
    {
        identifierColumns.AddColumn(layer, column);
    }

    public void AddMetaValue(MetaValueMapping metaValue)
    {
        metaValues.Add(metaValue);
    }

    public bool Equals(AnyMapping other)
    {
        return Equals(other.attributes, attributes) &&
               other.typeColumns.ContentEquals(typeColumns) &&
               other.identifierColumns.ContentEquals(identifierColumns) &&
               other.metaValues.ContentEquals(metaValues) &&
               Equals(other.ContainingEntityType, ContainingEntityType);
    }

    public override bool Equals(object obj)
    {
        if (obj.GetType() != typeof(AnyMapping)) return false;
        return Equals((AnyMapping)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int result = (attributes is not null ? attributes.GetHashCode() : 0);
            result = (result * 397) ^ (typeColumns is not null ? typeColumns.GetHashCode() : 0);
            result = (result * 397) ^ (identifierColumns is not null ? identifierColumns.GetHashCode() : 0);
            result = (result * 397) ^ (metaValues is not null ? metaValues.GetHashCode() : 0);
            result = (result * 397) ^ (ContainingEntityType is not null ? ContainingEntityType.GetHashCode() : 0);
            return result;
        }
    }

    public void Set<T>(Expression<Func<AnyMapping, T>> expression, int layer, T value)
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
