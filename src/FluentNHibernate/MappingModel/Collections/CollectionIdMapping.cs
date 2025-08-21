using System;
using System.Linq.Expressions;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Collections;

[Serializable]
public class CollectionIdMapping(AttributeStore attributes) : MappingBase, IEquatable<CollectionIdMapping>
{
    readonly AttributeStore attributes = attributes;

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessCollectionId(this);
        if (Generator is not null)
            visitor.Visit(Generator);
    }
    
    public GeneratorMapping Generator => attributes.GetOrDefault<GeneratorMapping>();
    
    public string Column => attributes.GetOrDefault<string>();
    
    public int Length => attributes.GetOrDefault<int>();
    
    public TypeReference Type => attributes.GetOrDefault<TypeReference>();
    
    public Type ContainingEntityType { get; set; }
    
    public bool Equals(CollectionIdMapping other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(other.attributes, attributes) &&
               other.ContainingEntityType == ContainingEntityType;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != typeof(CollectionIdMapping)) return false;
        return Equals((CollectionIdMapping)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int result = (attributes is not null ? attributes.GetHashCode() : 0);
            result = (result * 397) ^ (ContainingEntityType is not null ? ContainingEntityType.GetHashCode() : 0);
            return result;
        }
    }
    
    public void Set<T>(Expression<Func<CollectionIdMapping, T>> expression, int layer, T value)
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
