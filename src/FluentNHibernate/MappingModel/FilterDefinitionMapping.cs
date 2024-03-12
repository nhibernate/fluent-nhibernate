using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using NHibernate.Type;

namespace FluentNHibernate.MappingModel;

[Serializable]
public class FilterDefinitionMapping(AttributeStore attributes) : MappingBase, IEquatable<FilterDefinitionMapping>
{
    readonly AttributeStore attributes = attributes;

    public FilterDefinitionMapping()
        : this(new AttributeStore())
    { }

    public IDictionary<string, IType> Parameters { get; } = new Dictionary<string, IType>();

    public string Name => attributes.GetOrDefault<string>("Name");

    public string Condition => attributes.GetOrDefault<string>("Condition");

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessFilterDefinition(this);
    }

    public bool Equals(FilterDefinitionMapping other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(other.attributes, attributes) &&
               other.Parameters.ContentEquals(Parameters);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != typeof(FilterDefinitionMapping)) return false;
        return Equals((FilterDefinitionMapping)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((attributes is not null ? attributes.GetHashCode() : 0) * 397) ^ (Parameters is not null ? Parameters.GetHashCode() : 0);
        }
    }

    public void Set<T>(Expression<Func<FilterDefinitionMapping, T>> expression, int layer, T value)
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
