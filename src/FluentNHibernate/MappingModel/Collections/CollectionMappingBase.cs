using System;
using System.Reflection;

namespace FluentNHibernate.MappingModel.Collections
{
    public abstract class CollectionMappingBase : MappingBase, ICollectionMapping
    {
        private readonly AttributeStore<ICollectionMapping> attributes;
        public KeyMapping Key { get; set; }
        public ElementMapping Element { get; set; }
        public CompositeElementMapping CompositeElement { get; set; }
        public ICollectionRelationshipMapping Relationship { get; set; }
        public PropertyInfo PropertyInfo { get; set; }

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

        public CacheMapping Cache { get; set; }

        AttributeStore<ICollectionMapping> ICollectionMapping.Attributes
        {
            get { return attributes; }
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

        public string OuterJoin
        {
            get { return attributes.Get(x => x.OuterJoin); }
            set { attributes.Set(x => x.OuterJoin, value); }
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

        public string Persister
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

        public string CollectionType
        {
            get { return attributes.Get(x => x.CollectionType); }
            set { attributes.Set(x => x.CollectionType, value); }
        }

        public string OptimisticLock
        {
            get { return attributes.Get(x => x.OptimisticLock); }
            set { attributes.Set(x => x.OptimisticLock, value); }
        }

        public bool IsNameSpecified
        {
            get { return attributes.IsSpecified(x => x.Name); }
        }
    }
}