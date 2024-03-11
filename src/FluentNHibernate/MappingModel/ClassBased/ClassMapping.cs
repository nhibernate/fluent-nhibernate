using System;
using System.Linq.Expressions;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.ClassBased;

[Serializable]
public class ClassMapping : ClassMappingBase
{
    readonly AttributeStore attributes;

    public ClassMapping()
        : this(new AttributeStore())
    {}

    public ClassMapping(AttributeStore attributes)
        : base(attributes)
    {
        this.attributes = attributes;
    }

    public IIdentityMapping Id => attributes.GetOrDefault<IIdentityMapping>("Id");

    public NaturalIdMapping NaturalId => attributes.GetOrDefault<NaturalIdMapping>("NaturalId");

    public override string Name => attributes.GetOrDefault<string>("Name");

    public override Type Type => attributes.GetOrDefault<Type>("Type");

    public CacheMapping Cache => attributes.GetOrDefault<CacheMapping>("Cache");

    public VersionMapping Version => attributes.GetOrDefault<VersionMapping>("Version");

    public DiscriminatorMapping Discriminator => attributes.GetOrDefault<DiscriminatorMapping>("Discriminator");

    public bool IsUnionSubclass => attributes.GetOrDefault<bool>("IsUnionSubclass");

    public TuplizerMapping Tuplizer => attributes.GetOrDefault<TuplizerMapping>("Tuplizer");

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessClass(this);            

        if (Id is not null)
            visitor.Visit(Id);

        if (NaturalId is not null)
            visitor.Visit(NaturalId);

        if (Discriminator is not null)
            visitor.Visit(Discriminator);

        if (Cache is not null)
            visitor.Visit(Cache);

        if (Version is not null)
            visitor.Visit(Version);

        if (Tuplizer is not null)
            visitor.Visit(Tuplizer);

        base.AcceptVisitor(visitor);
    }

    public string TableName => attributes.GetOrDefault<string>("TableName");

    public int BatchSize => attributes.GetOrDefault<int>("BatchSize");

    public object DiscriminatorValue => attributes.GetOrDefault<object>("DiscriminatorValue");

    public string Schema => attributes.GetOrDefault<string>("Schema");

    public bool Lazy => attributes.GetOrDefault<bool>("Lazy");

    public bool Mutable => attributes.GetOrDefault<bool>("Mutable");

    public bool DynamicUpdate => attributes.GetOrDefault<bool>("DynamicUpdate");

    public bool DynamicInsert => attributes.GetOrDefault<bool>("DynamicInsert");

    public string OptimisticLock => attributes.GetOrDefault<string>("OptimisticLock");

    public string Polymorphism => attributes.GetOrDefault<string>("Polymorphism");

    public string Persister => attributes.GetOrDefault<string>("Persister");

    public string Where => attributes.GetOrDefault<string>("Where");

    public string Check => attributes.GetOrDefault<string>("Check");

    public string Proxy => attributes.GetOrDefault<string>("Proxy");

    public bool SelectBeforeUpdate => attributes.GetOrDefault<bool>("SelectBeforeUpdate");

    public bool Abstract => attributes.GetOrDefault<bool>("Abstract");

    public string Subselect => attributes.GetOrDefault<string>("Subselect");

    public string SchemaAction => attributes.GetOrDefault<string>("SchemaAction");

    public string EntityName => attributes.GetOrDefault<string>("EntityName");

    public bool Equals(ClassMapping other)
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
        if (obj.GetType() != typeof(ClassMapping)) return false;
        return Equals((ClassMapping)obj);
    }

    public override int GetHashCode()
    {
        return (attributes is not null ? attributes.GetHashCode() : 0);
    }

    public void Set<T>(Expression<Func<ClassMapping, T>> expression, int layer, T value)
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
