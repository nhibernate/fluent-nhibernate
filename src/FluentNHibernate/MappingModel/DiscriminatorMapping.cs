using System;
using System.Linq.Expressions;

namespace FluentNHibernate.MappingModel
{
    public class DiscriminatorMapping : MappingBase
    {
        private readonly AttributeStore<DiscriminatorMapping> attributes;

        public DiscriminatorMapping()
            : this(new AttributeStore())
        {}

        public DiscriminatorMapping(AttributeStore underlyingStore)
        {
            attributes = new AttributeStore<DiscriminatorMapping>(underlyingStore);
            attributes.SetDefault(x => x.NotNull, true);
            attributes.SetDefault(x => x.Insert, true);
            attributes.SetDefault(x => x.Type, new TypeReference(typeof(string)));
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessDiscriminator(this);
        }

        public string Column
        {
            get { return attributes.Get(x => x.Column); }
            set { attributes.Set(x => x.Column, value); }
        }

        public bool NotNull
        {
            get { return attributes.Get(x => x.NotNull); }
            set { attributes.Set(x => x.NotNull, value); }
        }

        public int Length
        {
            get { return attributes.Get(x => x.Length); }
            set { attributes.Set(x => x.Length, value); }
        }

        public bool Force
        {
            get { return attributes.Get(x => x.Force); }
            set { attributes.Set(x => x.Force, value); }
        }

        public bool Insert
        {
            get { return attributes.Get(x => x.Insert); }
            set { attributes.Set(x => x.Insert, value); }
        }

        public string Formula
        {
            get { return attributes.Get(x => x.Formula); }
            set { attributes.Set(x => x.Formula, value); }
        }

        public TypeReference Type
        {
            get { return attributes.Get(x => x.Type); }
            set { attributes.Set(x => x.Type, value); }
        }

        public Type ContainingEntityType { get; set; }

        public bool IsSpecified<TResult>(Expression<Func<DiscriminatorMapping, TResult>> property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<DiscriminatorMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<DiscriminatorMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }
    }
}
