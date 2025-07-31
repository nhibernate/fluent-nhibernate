using System;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.ClassBased;

[Serializable]
public class SubclassMapping(SubclassType subclassType, AttributeStore attributes) : ClassMappingBase(attributes), IEquatable<SubclassMapping>
{
    public SubclassType SubclassType { get; } = subclassType;
    AttributeStore attributes = attributes;

    public SubclassMapping(SubclassType subclassType)
        : this(subclassType, new AttributeStore())
    {}

    /// <summary>
    /// Set which type this subclass extends.
    /// Note: This doesn't actually get output into the XML, it's
    /// instead used as a marker for the <see cref="SeparateSubclassVisitor"/>
    /// to pair things up.
    /// </summary>
    public Type Extends => attributes.GetOrDefault<Type>();

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessSubclass(this);

        if (Tuplizer is not null)
            visitor.Visit(Tuplizer);
        
        if (SubclassType == SubclassType.JoinedSubclass && Key is not null)
            visitor.Visit(Key);

        base.AcceptVisitor(visitor);
    }

    public override string Name => attributes.GetOrDefault<string>();

    public override Type Type => attributes.GetOrDefault<Type>();

    public object DiscriminatorValue => attributes.GetOrDefault<object>();

    public bool Lazy => attributes.GetOrDefault<bool>();

    public string Proxy => attributes.GetOrDefault<string>();

    public bool DynamicUpdate => attributes.GetOrDefault<bool>();

    public bool DynamicInsert => attributes.GetOrDefault<bool>();

    public bool SelectBeforeUpdate => attributes.GetOrDefault<bool>();

    public bool Abstract => attributes.GetOrDefault<bool>();

    public string EntityName => attributes.GetOrDefault<string>();

    public string TableName => attributes.GetOrDefault<string>();

    public KeyMapping Key => attributes.GetOrDefault<KeyMapping>();

    public string Check => attributes.GetOrDefault<string>();

    public string Schema => attributes.GetOrDefault<string>();

    public string Subselect => attributes.GetOrDefault<string>();

    public TypeReference Persister => attributes.GetOrDefault<TypeReference>();

    public int BatchSize => attributes.GetOrDefault<int>();
    
    public TuplizerMapping Tuplizer => attributes.GetOrDefault<TuplizerMapping>();

    public void OverrideAttributes(AttributeStore store)
    {
        attributes = store;
    }

    public bool Equals(SubclassMapping other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other) && Equals(other.attributes, attributes);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return Equals(obj as SubclassMapping);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            {
                return (base.GetHashCode() * 397) ^ (attributes is not null
                    ? attributes.GetHashCode()
                    : 0);
            }
        }
    }

    public override string ToString()
    {
        return "Subclass(" + Type.Name + ")";
    }

    public void Set<T>(Expression<Func<SubclassMapping, T>> expression, int layer, T value)
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
