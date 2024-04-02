using System;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.ClassBased;

[Serializable]
public abstract class ComponentMappingBase(AttributeStore attributes) : ClassMappingBase(attributes)
{
    readonly AttributeStore attributes = attributes;

    protected ComponentMappingBase()
        : this(new AttributeStore())
    {}

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        if (Parent is not null)
            visitor.Visit(Parent);

        base.AcceptVisitor(visitor);
    }

    public Type ContainingEntityType { get; set; }
    public Member Member { get; set; }

    public ParentMapping Parent => attributes.GetOrDefault<ParentMapping>();

    public bool Unique => attributes.GetOrDefault<bool>();

    public bool Insert => attributes.GetOrDefault<bool>();

    public bool Update => attributes.GetOrDefault<bool>();

    public string Access => attributes.GetOrDefault<string>();

    public bool OptimisticLock => attributes.GetOrDefault<bool>();

    public bool Equals(ComponentMappingBase other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other) &&
               Equals(other.attributes, attributes) &&
               other.ContainingEntityType == ContainingEntityType &&
               Equals(other.Member, Member);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return Equals(obj as ComponentMappingBase);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int result = base.GetHashCode();
            result = (result * 397) ^ (attributes is not null ? attributes.GetHashCode() : 0);
            result = (result * 397) ^ (ContainingEntityType is not null ? ContainingEntityType.GetHashCode() : 0);
            result = (result * 397) ^ (Member is not null ? Member.GetHashCode() : 0);
            return result;
        }
    }
}
