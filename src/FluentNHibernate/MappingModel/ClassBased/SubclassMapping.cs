using System;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.ClassBased;

[Serializable]
public class SubclassMapping : ClassMappingBase
{
    public SubclassType SubclassType { get; }
    AttributeStore attributes;

    public SubclassMapping(SubclassType subclassType)
        : this(subclassType, new AttributeStore())
    {}

    public SubclassMapping(SubclassType subclassType, AttributeStore attributes)
        : base(attributes)
    {
        SubclassType = subclassType;
        this.attributes = attributes;
    }

    /// <summary>
    /// Set which type this subclass extends.
    /// Note: This doesn't actually get output into the XML, it's
    /// instead used as a marker for the <see cref="SeparateSubclassVisitor"/>
    /// to pair things up.
    /// </summary>
    public Type Extends => attributes.GetOrDefault<Type>("Extends");

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessSubclass(this);

        if (SubclassType == SubclassType.JoinedSubclass && Key is not null)
            visitor.Visit(Key);

        base.AcceptVisitor(visitor);
    }

    public override string Name => attributes.GetOrDefault<string>("Name");

    public override Type Type => attributes.GetOrDefault<Type>("Type");

    public object DiscriminatorValue => attributes.GetOrDefault<object>("DiscriminatorValue");

    public bool Lazy => attributes.GetOrDefault<bool>("Lazy");

    public string Proxy => attributes.GetOrDefault<string>("Proxy");

    public bool DynamicUpdate => attributes.GetOrDefault<bool>("DynamicUpdate");

    public bool DynamicInsert => attributes.GetOrDefault<bool>("DynamicInsert");

    public bool SelectBeforeUpdate => attributes.GetOrDefault<bool>("SelectBeforeUpdate");

    public bool Abstract => attributes.GetOrDefault<bool>("Abstract");

    public string EntityName => attributes.GetOrDefault<string>("EntityName");

    public string TableName => attributes.GetOrDefault<string>("TableName");

    public KeyMapping Key => attributes.GetOrDefault<KeyMapping>("Key");

    public string Check => attributes.GetOrDefault<string>("Check");

    public string Schema => attributes.GetOrDefault<string>("Schema");

    public string Subselect => attributes.GetOrDefault<string>("Subselect");

    public TypeReference Persister => attributes.GetOrDefault<TypeReference>("Persister");

    public int BatchSize => attributes.GetOrDefault<int>("BatchSize");

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
