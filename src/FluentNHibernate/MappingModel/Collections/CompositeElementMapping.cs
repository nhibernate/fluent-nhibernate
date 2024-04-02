using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Collections;

[Serializable]
public class CompositeElementMapping(AttributeStore attributes) : MappingBase, IEquatable<CompositeElementMapping>
{
    readonly MappedMembers mappedMembers = new();
    readonly List<NestedCompositeElementMapping> compositeElements = new List<NestedCompositeElementMapping>();
    readonly AttributeStore attributes = attributes;

    public CompositeElementMapping()
        : this(new AttributeStore())
    { }

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessCompositeElement(this);

        if (Parent is not null)
            visitor.Visit(Parent);

        foreach (var compositeElement in CompositeElements)
            visitor.Visit(compositeElement);

        mappedMembers.AcceptVisitor(visitor);
    }

    public TypeReference Class => attributes.GetOrDefault<TypeReference>();

    public ParentMapping Parent => attributes.GetOrDefault<ParentMapping>();

    public IEnumerable<PropertyMapping> Properties => mappedMembers.Properties;

    public void AddProperty(PropertyMapping property)
    {
        mappedMembers.AddProperty(property);
    }

    public IEnumerable<ManyToOneMapping> References => mappedMembers.References;

    public IEnumerable<NestedCompositeElementMapping> CompositeElements => compositeElements;

    public Type ContainingEntityType { get; set; }

    public void AddReference(ManyToOneMapping manyToOne)
    {
        mappedMembers.AddReference(manyToOne);
    }

    public void AddCompositeElement(NestedCompositeElementMapping compositeElement)
    {
        compositeElements.Add(compositeElement);
    }

    public bool Equals(CompositeElementMapping other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(other.mappedMembers, mappedMembers) && Equals(other.attributes, attributes) && other.ContainingEntityType == ContainingEntityType;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != typeof(CompositeElementMapping)) return false;
        return Equals((CompositeElementMapping)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int result = (mappedMembers is not null ? mappedMembers.GetHashCode() : 0);
            result = (result * 397) ^ (attributes is not null ? attributes.GetHashCode() : 0);
            result = (result * 397) ^ (ContainingEntityType is not null ? ContainingEntityType.GetHashCode() : 0);
            return result;
        }
    }

    public void Set<T>(Expression<Func<CompositeElementMapping, T>> expression, int layer, T value)
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
