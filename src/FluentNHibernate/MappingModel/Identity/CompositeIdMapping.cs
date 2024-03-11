using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Identity;

[Serializable]
public class CompositeIdMapping : MappingBase, IIdentityMapping
{
    private readonly AttributeStore attributes;
    private readonly IList<ICompositeIdKeyMapping> keys = new List<ICompositeIdKeyMapping>();

    public CompositeIdMapping()
        : this(new AttributeStore())
    {}

    public CompositeIdMapping(AttributeStore attributes)
    {
        this.attributes = attributes;
    }

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessCompositeId(this);

        foreach (var key in keys)
        {
            if (key is KeyPropertyMapping)
                visitor.Visit((KeyPropertyMapping)key);
            if (key is KeyManyToOneMapping)
                visitor.Visit((KeyManyToOneMapping)key);
        }
    }

    public string Name => attributes.GetOrDefault<string>("Name");

    public string Access => attributes.GetOrDefault<string>("Access");

    public bool Mapped => attributes.GetOrDefault<bool>("Mapped") || !string.IsNullOrEmpty(Name);

    public TypeReference Class => attributes.GetOrDefault<TypeReference>("Class");

    public string UnsavedValue => attributes.GetOrDefault<string>("UnsavedValue");

    public IEnumerable<ICompositeIdKeyMapping> Keys => keys;

    public Type ContainingEntityType { get; set; }

    public void AddKey(ICompositeIdKeyMapping mapping)
    {
        keys.Add(mapping);
    }

    public bool Equals(CompositeIdMapping other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(other.attributes, attributes) &&
               other.keys.ContentEquals(keys) &&
               other.ContainingEntityType == ContainingEntityType;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != typeof(CompositeIdMapping)) return false;
        return Equals((CompositeIdMapping)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int result = (attributes is not null ? attributes.GetHashCode() : 0);
            result = (result * 397) ^ (keys is not null ? keys.GetHashCode() : 0);
            result = (result * 397) ^ (ContainingEntityType is not null ? ContainingEntityType.GetHashCode() : 0);
            return result;
        }
    }

    public void Set<T>(Expression<Func<CompositeIdMapping, T>> expression, int layer, T value)
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
