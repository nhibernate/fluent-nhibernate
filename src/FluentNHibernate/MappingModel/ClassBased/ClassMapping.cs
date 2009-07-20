using System;
using System.Linq.Expressions;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.MappingModel.ClassBased
{
    public class ClassMapping : ClassMappingBase
    {
        private readonly AttributeStore<ClassMapping> attributes;

        public ClassMapping()
            : this(new AttributeStore())
        {}

        public ClassMapping(Type type)
            : this(new AttributeStore())
        {
            Type = type;
        }

        protected ClassMapping(AttributeStore store)
            : base(store)
        {
            attributes = new AttributeStore<ClassMapping>(store);
        }

        public IIdentityMapping Id
        {
            get { return attributes.Get(x => x.Id); }
            set { attributes.Set(x => x.Id, value); }
        }

        public CacheMapping Cache
        {
            get { return attributes.Get(x => x.Cache); }
            set { attributes.Set(x => x.Cache, value); }
        }

        public VersionMapping Version
        {
            get { return attributes.Get(x => x.Version); }
            set { attributes.Set(x => x.Version, value); }
        }

        public DiscriminatorMapping Discriminator
        {
            get { return attributes.Get(x => x.Discriminator); }
            set { attributes.Set(x => x.Discriminator, value); }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessClass(this);            

            if (Id != null)
                visitor.Visit(Id);

            if (Discriminator != null)
                visitor.Visit(Discriminator);

            if (Cache != null)
                visitor.Visit(Cache);

            foreach (var join in Joins)
                visitor.Visit(join);

            if (Version != null)
                visitor.Visit(Version);

            base.AcceptVisitor(visitor);
        }

        public string TableName
        {
            get { return attributes.Get(x => x.TableName); }
            set { attributes.Set(x => x.TableName, value); }
        }

        public int BatchSize
        {
            get { return attributes.Get(x => x.BatchSize); }
            set { attributes.Set(x => x.BatchSize, value); }
        }

        public object DiscriminatorValue
        {
            get { return attributes.Get(x => x.DiscriminatorValue); }
            set { attributes.Set(x => x.DiscriminatorValue, value); }
        }

        public string Schema
        {
            get { return attributes.Get(x => x.Schema); }
            set { attributes.Set(x => x.Schema, value); }
        }

        public bool Lazy
        {
            get { return attributes.Get(x => x.Lazy); }
            set { attributes.Set(x => x.Lazy, value); }
        }

        public bool Mutable
        {
            get { return attributes.Get(x => x.Mutable); }
            set { attributes.Set(x => x.Mutable, value); }
        }

        public bool DynamicUpdate
        {
            get { return attributes.Get(x => x.DynamicUpdate); }
            set { attributes.Set(x => x.DynamicUpdate, value); }
        }

        public bool DynamicInsert
        {
            get { return attributes.Get(x => x.DynamicInsert); }
            set { attributes.Set(x => x.DynamicInsert, value); }
        }

        public string OptimisticLock
        {
            get { return attributes.Get(x => x.OptimisticLock); }
            set { attributes.Set(x => x.OptimisticLock, value); }
        }

        public string Polymorphism
        {
            get { return attributes.Get(x => x.Polymorphism); }
            set { attributes.Set(x => x.Polymorphism, value); }
        }

        public string Persister
        {
            get { return attributes.Get(x => x.Persister); }
            set { attributes.Set(x => x.Persister, value); }
        }

        public string Where
        {
            get { return attributes.Get(x => x.Where); }
            set { attributes.Set(x => x.Where, value); }
        }

        public string Check
        {
            get { return attributes.Get(x => x.Check); }
            set { attributes.Set(x => x.Check, value); }
        }

        public string Proxy
        {
            get { return attributes.Get(x => x.Proxy); }
            set { attributes.Set(x => x.Proxy, value); }
        }

        public bool SelectBeforeUpdate
        {
            get { return attributes.Get(x => x.SelectBeforeUpdate); }
            set { attributes.Set(x => x.SelectBeforeUpdate, value); }
        }

        public bool Abstract
        {
            get { return attributes.Get(x => x.Abstract); }
            set { attributes.Set(x => x.Abstract, value); }
        }

        public bool IsSpecified<TResult>(Expression<Func<ClassMapping, TResult>> property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<ClassMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<ClassMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }
    }
}