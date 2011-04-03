using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Collections
{
    [Serializable]
    public class CollectionMapping : MappingBase, IRelationship
    {
        readonly AttributeStore<CollectionMapping> attributes;
        readonly IList<FilterMapping> filters = new List<FilterMapping>();

        public Type ContainingEntityType { get; set; }
        public Member Member { get; set; }

        CollectionMapping(AttributeStore underlyingStore)
        {
            Collection = Collection.Bag;
            attributes = new AttributeStore<CollectionMapping>(underlyingStore);
        }

        public IEnumerable<FilterMapping> Filters
        {
            get { return filters; }
        }

        public void AddFilter(FilterMapping mapping)
        {
            filters.Add(mapping);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessCollection(this);

            if (Key != null)
                visitor.Visit(Key);

            if (Index != null && (Collection == Collection.Array || Collection == Collection.List || Collection == Collection.Map))
                visitor.Visit(Index);

            if (Element != null)
                visitor.Visit(Element);

            if (CompositeElement != null)
                visitor.Visit(CompositeElement);

            if (Relationship != null)
                visitor.Visit(Relationship);

            foreach (var filter in Filters)
                visitor.Visit(filter);

            if (Cache != null)
                visitor.Visit(Cache);
        }

        public Type ChildType
        {
            get { return attributes.Get(x => x.ChildType); }
            set { attributes.Set(x => x.ChildType, value); }
        }

        public IRelationship OtherSide { get; set; }

        public KeyMapping Key
        {
            get { return attributes.Get(x => x.Key); }
            set { attributes.Set(x => x.Key, value); }
        }

        public ElementMapping Element
        {
            get { return attributes.Get(x => x.Element); }
            set { attributes.Set(x => x.Element, value); }
        }

        public CompositeElementMapping CompositeElement
        {
            get { return attributes.Get(x => x.CompositeElement); }
            set { attributes.Set(x => x.CompositeElement, value); }
        }

        public CacheMapping Cache
        {
            get { return attributes.Get(x => x.Cache); }
            set { attributes.Set(x => x.Cache, value); }
        }

        public ICollectionRelationshipMapping Relationship
        {
            get { return attributes.Get(x => x.Relationship); }
            set { attributes.Set(x => x.Relationship, value); }
        }

        public bool Generic
        {
            get { return attributes.Get(x => x.Generic); }
            set { attributes.Set(x => x.Generic, value); }
        }

        public Lazy Lazy
        {
            get { return attributes.Get(x => x.Lazy); }
            set { attributes.Set(x => x.Lazy, value); }
        }

        public bool Inverse
        {
            get { return attributes.Get(x => x.Inverse); }
            set { attributes.Set(x => x.Inverse, value); }
        }

        public string Name
        {
            get { return attributes.Get(x => x.Name); }
            set { attributes.Set(x => x.Name, value); }
        }

        public string Access
        {
            get { return attributes.Get(x => x.Access); }
            set { attributes.Set(x => x.Access, value); }
        }

        public string TableName
        {
            get { return attributes.Get(x => x.TableName); }
            set { attributes.Set(x => x.TableName, value); }
        }

        public string Schema
        {
            get { return attributes.Get(x => x.Schema); }
            set { attributes.Set(x => x.Schema, value); }
        }

        public string Fetch
        {
            get { return attributes.Get(x => x.Fetch); }
            set { attributes.Set(x => x.Fetch, value); }
        }

        public string Cascade
        {
            get { return attributes.Get(x => x.Cascade); }
            set { attributes.Set(x => x.Cascade, value); }
        }

        public string Where
        {
            get { return attributes.Get(x => x.Where); }
            set { attributes.Set(x => x.Where, value); }
        }

        public bool Mutable
        {
            get { return attributes.Get(x => x.Mutable); }
            set { attributes.Set(x => x.Mutable, value); }
        }

        public string Subselect
        {
            get { return attributes.Get(x => x.Subselect); }
            set { attributes.Set(x => x.Subselect, value); }
        }

    	public TypeReference Persister
        {
            get { return attributes.Get(x => x.Persister); }
            set { attributes.Set(x => x.Persister, value); }
        }

        public int BatchSize
        {
            get { return attributes.Get(x => x.BatchSize); }
            set { attributes.Set(x => x.BatchSize, value); }
        }

        public string Check
        {
            get { return attributes.Get(x => x.Check); }
            set { attributes.Set(x => x.Check, value); }
        }

        public TypeReference CollectionType
        {
            get { return attributes.Get(x => x.CollectionType); }
            set { attributes.Set(x => x.CollectionType, value); }
        }

        public string OptimisticLock
        {
            get { return attributes.Get(x => x.OptimisticLock); }
            set { attributes.Set(x => x.OptimisticLock, value); }
        }

        public override bool IsSpecified(string property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<CollectionMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<CollectionMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }

        public string OrderBy
        {
            get { return attributes.Get(x => x.OrderBy); }
            set { attributes.Set(x => x.OrderBy, value); }
        }

        public Collection Collection { get; set; }
        
        public string Sort
        {
            get { return attributes.Get(x => x.Sort); }
            set { attributes.Set(x => x.Sort, value); }
        }

        public IIndexMapping Index
        {
            get { return attributes.Get(x => x.Index); }
            set { attributes.Set(x => x.Index, value); }
        }

        public bool Equals(CollectionMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.attributes, attributes) &&
                other.filters.ContentEquals(filters) &&
                Equals(other.ContainingEntityType, ContainingEntityType)
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
                int result = (attributes != null ? attributes.GetHashCode() : 0);
                result = (result * 397) ^ (filters != null ? filters.GetHashCode() : 0);
                result = (result * 397) ^ (ContainingEntityType != null ? ContainingEntityType.GetHashCode() : 0);
                result = (result * 397) ^ (Member != null ? Member.GetHashCode() : 0);
                return result;
            }
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
}