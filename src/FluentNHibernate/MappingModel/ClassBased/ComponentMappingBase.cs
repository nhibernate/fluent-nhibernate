using System;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.ClassBased;

[Serializable]
public abstract class ComponentMappingBase : ClassMappingBase
{
    private readonly AttributeStore attributes;

    protected ComponentMappingBase()
        : this(new AttributeStore())
    {}

    protected ComponentMappingBase(AttributeStore attributes)
        : base(attributes)
    {
        this.attributes = attributes;
    }

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        if (Parent is not null)
            visitor.Visit(Parent);

        base.AcceptVisitor(visitor);
    }

    public Type ContainingEntityType { get; set; }
    public Member Member { get; set; }

    public ParentMapping Parent => attributes.GetOrDefault<ParentMapping>("Parent");

    public bool Unique => attributes.GetOrDefault<bool>("Unique");

    public bool Insert => attributes.GetOrDefault<bool>("Insert");

    public bool Update => attributes.GetOrDefault<bool>("Update");

    public string Access => attributes.GetOrDefault<string>("Access");

    public bool OptimisticLock => attributes.GetOrDefault<bool>("OptimisticLock");

    public bool Equals(ComponentMappingBase other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other) &&
               Equals(other.attributes, attributes) &&
               Equals(other.ContainingEntityType, ContainingEntityType) &&
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
