using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Collections;

[Serializable]
public class CollectionMapping : MappingBase, IRelationship
{
    readonly AttributeStore attributes;
    readonly IList<FilterMapping> filters = new List<FilterMapping>();

    public Type ContainingEntityType { get; set; }
    public Member Member { get; set; }

    CollectionMapping(AttributeStore attributes)
    {
        Collection = Collection.Bag;
        this.attributes = attributes;
    }

    public IEnumerable<FilterMapping> Filters => filters;

    public void AddFilter(FilterMapping mapping)
    {
        filters.Add(mapping);
    }

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessCollection(this);

        if (Key is not null)
            visitor.Visit(Key);

        if (Index is not null && (Collection == Collection.Array || Collection == Collection.List || Collection == Collection.Map))
            visitor.Visit(Index);

        if (Element is not null)
            visitor.Visit(Element);

        if (CompositeElement is not null)
            visitor.Visit(CompositeElement);

        if (Relationship is not null)
            visitor.Visit(Relationship);

        foreach (var filter in Filters)
            visitor.Visit(filter);

        if (Cache is not null)
            visitor.Visit(Cache);
    }

    public Type ChildType => attributes.GetOrDefault<Type>("ChildType");

    public IRelationship OtherSide { get; set; }

    public KeyMapping Key => attributes.GetOrDefault<KeyMapping>("Key");

    public ElementMapping Element => attributes.GetOrDefault<ElementMapping>("Element");

    public CompositeElementMapping CompositeElement => attributes.GetOrDefault<CompositeElementMapping>("CompositeElement");

    public CacheMapping Cache => attributes.GetOrDefault<CacheMapping>("Cache");

    public ICollectionRelationshipMapping Relationship => attributes.GetOrDefault<ICollectionRelationshipMapping>("Relationship");

    public bool Generic => attributes.GetOrDefault<bool>("Generic");

    public Lazy Lazy => attributes.GetOrDefault<Lazy>("Lazy");

    public bool Inverse => attributes.GetOrDefault<bool>("Inverse");

    public string Name => attributes.GetOrDefault<string>("Name");

    public string Access => attributes.GetOrDefault<string>("Access");

    public string TableName => attributes.GetOrDefault<string>("TableName");

    public string Schema => attributes.GetOrDefault<string>("Schema");

    public string Fetch => attributes.GetOrDefault<string>("Fetch");

    public string Cascade => attributes.GetOrDefault<string>("Cascade");

    public string Where => attributes.GetOrDefault<string>("Where");

    public bool Mutable => attributes.GetOrDefault<bool>("Mutable");

    public string Subselect => attributes.GetOrDefault<string>("Subselect");

    public TypeReference Persister => attributes.GetOrDefault<TypeReference>("Persister");

    public int BatchSize => attributes.GetOrDefault<int>("BatchSize");

    public string Check => attributes.GetOrDefault<string>("Check");

    public TypeReference CollectionType => attributes.GetOrDefault<TypeReference>("CollectionType");

    public bool OptimisticLock => attributes.GetOrDefault<bool>("OptimisticLock");

    public string OrderBy => attributes.GetOrDefault<string>("OrderBy");

    public Collection Collection { get; set; }
        
    public string Sort => attributes.GetOrDefault<string>("Sort");

    public IIndexMapping Index => attributes.GetOrDefault<IIndexMapping>("Index");

    public bool Equals(CollectionMapping other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(other.attributes, attributes) &&
               other.filters.ContentEquals(filters) &&
               other.ContainingEntityType == ContainingEntityType
               && Equals(other.Member, Member);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != typeof(CollectionMapping)) return false;
        return Equals((CollectionMapping)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int result = (attributes is not null ? attributes.GetHashCode() : 0);
            result = (result * 397) ^ (filters is not null ? filters.GetHashCode() : 0);
            result = (result * 397) ^ (ContainingEntityType is not null ? ContainingEntityType.GetHashCode() : 0);
            result = (result * 397) ^ (Member is not null ? Member.GetHashCode() : 0);
            return result;
        }
    }

    public void Set<T>(Expression<Func<CollectionMapping, T>> expression, int layer, T value)
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

    public static CollectionMapping Array()
    {
        return Array(new AttributeStore());
    }

    public static CollectionMapping Array(AttributeStore underlyingStore)
    {
        return For(Collection.Array, underlyingStore);
    }

    public static CollectionMapping Bag()
    {
        return Bag(new AttributeStore());
    }

    public static CollectionMapping Bag(AttributeStore underlyingStore)
    {
        return For(Collection.Bag, underlyingStore);
    }

    public static CollectionMapping List()
    {
        return List(new AttributeStore());
    }

    public static CollectionMapping List(AttributeStore underlyingStore)
    {
        return For(Collection.List, underlyingStore);
    }

    public static CollectionMapping Map()
    {
        return Map(new AttributeStore());
    }

    public static CollectionMapping Map(AttributeStore underlyingStore)
    {
        return For(Collection.Map, underlyingStore);
    }

    public static CollectionMapping Set()
    {
        return Set(new AttributeStore());
    }

    public static CollectionMapping Set(AttributeStore underlyingStore)
    {
        return For(Collection.Set, underlyingStore);
    }

    public static CollectionMapping For(Collection collectionType)
    {
        return For(collectionType, new AttributeStore());
    }

    public static CollectionMapping For(Collection collectionType, AttributeStore underlyingStore)
    {
        return new CollectionMapping(underlyingStore)
        {
            Collection = collectionType
        };
    }
}
