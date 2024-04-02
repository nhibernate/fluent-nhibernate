using System;
using System.Linq.Expressions;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.ClassBased;

[Serializable]
public class ClassMapping(AttributeStore attributes) : ClassMappingBase(attributes), IEquatable<ClassMapping>
{
    readonly AttributeStore attributes = attributes;

    public ClassMapping()
        : this(new AttributeStore())
    {}

    public IIdentityMapping Id => attributes.GetOrDefault<IIdentityMapping>();

    public NaturalIdMapping NaturalId => attributes.GetOrDefault<NaturalIdMapping>();

    public override string Name => attributes.GetOrDefault<string>();

    public override Type Type => attributes.GetOrDefault<Type>();

    public CacheMapping Cache => attributes.GetOrDefault<CacheMapping>();

    public VersionMapping Version => attributes.GetOrDefault<VersionMapping>();

    public DiscriminatorMapping Discriminator => attributes.GetOrDefault<DiscriminatorMapping>();

    public bool IsUnionSubclass => attributes.GetOrDefault<bool>();

    public TuplizerMapping Tuplizer => attributes.GetOrDefault<TuplizerMapping>();

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

    public string TableName => attributes.GetOrDefault<string>();

    public int BatchSize => attributes.GetOrDefault<int>();

    public object DiscriminatorValue => attributes.GetOrDefault<object>();

    public string Schema => attributes.GetOrDefault<string>();

    public bool Lazy => attributes.GetOrDefault<bool>();

    public bool Mutable => attributes.GetOrDefault<bool>();

    public bool DynamicUpdate => attributes.GetOrDefault<bool>();

    public bool DynamicInsert => attributes.GetOrDefault<bool>();

    public string OptimisticLock => attributes.GetOrDefault<string>();

    public string Polymorphism => attributes.GetOrDefault<string>();

    public string Persister => attributes.GetOrDefault<string>();

    public string Where => attributes.GetOrDefault<string>();

    public string Check => attributes.GetOrDefault<string>();

    public string Proxy => attributes.GetOrDefault<string>();

    public bool SelectBeforeUpdate => attributes.GetOrDefault<bool>();

    public bool Abstract => attributes.GetOrDefault<bool>();

    public string Subselect => attributes.GetOrDefault<string>();

    public string SchemaAction => attributes.GetOrDefault<string>();

    public string EntityName => attributes.GetOrDefault<string>();

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
