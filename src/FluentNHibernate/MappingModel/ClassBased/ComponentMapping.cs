﻿using System;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.ClassBased;

[Serializable]
public class ComponentMapping : ComponentMappingBase, IComponentMapping, IEquatable<ComponentMapping>
{
    public ComponentType ComponentType { get; set; }
    readonly AttributeStore attributes;

    public ComponentMapping(ComponentType componentType): this(componentType, new AttributeStore(), null)
    {}

    public ComponentMapping(ComponentType componentType, AttributeStore attributes, Member member): base(attributes)
    {
        ComponentType = componentType;
        this.attributes = attributes;
        Member = member;
    }

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessComponent(this);

        base.AcceptVisitor(visitor);
    }

    public bool HasColumnPrefix => !string.IsNullOrEmpty(ColumnPrefix);

    public string ColumnPrefix { get; set; }

    public override string Name => attributes.GetOrDefault<string>("Name");

    public override Type Type => attributes.GetOrDefault<Type>("Type");

    public TypeReference Class => attributes.GetOrDefault<TypeReference>("Class");

    public bool Lazy => attributes.GetOrDefault<bool>("Lazy");

    public bool Equals(ComponentMapping other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other) &&
               Equals(other.attributes, attributes);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return Equals(obj as ComponentMapping);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            {
                return (base.GetHashCode() * 397) ^ (attributes is not null ? attributes.GetHashCode() : 0);
            }
        }
    }

    public void Set<T>(Expression<Func<ComponentMapping, T>> expression, int layer, T value)
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
