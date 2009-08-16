using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentNHibernate.MappingModel.Collections
{
    public abstract class CollectionMappingBase : MappingBase, ICollectionMapping
    {
        private readonly AttributeStore<ICollectionMapping> attributes;
        public Type ContainingEntityType { get; set; }
        public MemberInfo MemberInfo { get; set; }

        protected CollectionMappingBase(AttributeStore underlyingStore)
        {
            attributes = new AttributeStore<ICollectionMapping>(underlyingStore);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            if (Key != null)
                visitor.Visit(Key);

            if (Element != null)
                visitor.Visit(Element);

            if (CompositeElement != null)
                visitor.Visit(CompositeElement);

            if (Relationship != null)
                visitor.Visit(Relationship);

            if (Cache != null)
                visitor.Visit(Cache);
        }

        public Type ChildType
        {
            get { return attributes.Get(x => x.ChildType); }
            set { attributes.Set(x => x.ChildType, value); }
        }

        public ICollectionMapping OtherSide { get; set; }

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

        public bool Lazy
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

        public bool IsSpecified<TResult>(Expression<Func<ICollectionMapping, TResult>> property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<ICollectionMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<ICollectionMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }

		public abstract string OrderBy { get; set; }
    }
}