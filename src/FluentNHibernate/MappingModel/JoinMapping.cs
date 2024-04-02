using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel;

[Serializable]
public class JoinMapping(AttributeStore attributes) : IMapping, IEquatable<JoinMapping>
{
    readonly AttributeStore attributes = attributes;

    readonly MappedMembers mappedMembers = new();

    public JoinMapping()
        : this(new AttributeStore())
    {}

    public KeyMapping Key => attributes.GetOrDefault<KeyMapping>();

    public IEnumerable<PropertyMapping> Properties => mappedMembers.Properties;

    public IEnumerable<ManyToOneMapping> References => mappedMembers.References;

    public IEnumerable<IComponentMapping> Components => mappedMembers.Components;

    public IEnumerable<AnyMapping> Anys => mappedMembers.Anys;

    public IEnumerable<CollectionMapping> Collections => mappedMembers.Collections;

    public void AddProperty(PropertyMapping property)
    {
        mappedMembers.AddProperty(property);
    }

    public void AddReference(ManyToOneMapping manyToOne)
    {
        mappedMembers.AddReference(manyToOne);
    }

    public void AddComponent(IComponentMapping componentMapping)
    {
        mappedMembers.AddComponent(componentMapping);
    }

    public void AddAny(AnyMapping mapping)
    {
        mappedMembers.AddAny(mapping);
    }

    public void AddCollection(CollectionMapping collectionMapping)
    {
        mappedMembers.AddCollection(collectionMapping);
    }

    public void AddStoredProcedure(StoredProcedureMapping storedProcedureMapping)
    {
        mappedMembers.AddStoredProcedure(storedProcedureMapping);
    }

    public string TableName => attributes.GetOrDefault<string>();

    public string Schema => attributes.GetOrDefault<string>();

    public string Catalog => attributes.GetOrDefault<string>();

    public string Subselect => attributes.GetOrDefault<string>();

    public string Fetch => attributes.GetOrDefault<string>();

    public bool Inverse => attributes.GetOrDefault<bool>();

    public bool Optional => attributes.GetOrDefault<bool>();

    public Type ContainingEntityType { get; set; }

    public void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessJoin(this);

        if (Key is not null)
            visitor.Visit(Key);

        mappedMembers.AcceptVisitor(visitor);
    }

    public bool Equals(JoinMapping other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(other.attributes, attributes) &&
               Equals(other.mappedMembers, mappedMembers) &&
               other.ContainingEntityType == ContainingEntityType;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != typeof(JoinMapping)) return false;
        return Equals((JoinMapping)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int result = (attributes is not null ? attributes.GetHashCode() : 0);
            result = (result * 397) ^ (mappedMembers is not null ? mappedMembers.GetHashCode() : 0);
            result = (result * 397) ^ (ContainingEntityType is not null ? ContainingEntityType.GetHashCode() : 0);
            return result;
        }
    }

    public void Set<T>(Expression<Func<JoinMapping, T>> expression, int layer, T value)
    {
        Set(expression.ToMember().Name, layer, value);
    }

    public void Set(string attribute, int layer, object value)
    {
        attributes.Set(attribute, layer, value);
    }

    public bool IsSpecified(string attribute)
    {
        return attributes.IsSpecified(attribute);
    }
}
