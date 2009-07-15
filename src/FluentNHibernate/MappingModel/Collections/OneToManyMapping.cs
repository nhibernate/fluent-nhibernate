using System;
using System.Linq.Expressions;

namespace FluentNHibernate.MappingModel.Collections
{
    public class OneToManyMapping : MappingBase, ICollectionRelationshipMapping
    {
        private readonly AttributeStore<OneToManyMapping> attributes;
        public Type ChildType { get; set; }

        public OneToManyMapping()
            : this(new AttributeStore())
        {}

        public OneToManyMapping(AttributeStore underlyingStore)
        {
            attributes = new AttributeStore<OneToManyMapping>(underlyingStore);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessOneToMany(this);
        }

        public TypeReference Class
        {
            get { return attributes.Get(x => x.Class); }
            set { attributes.Set(x => x.Class, value); }
        }

        public string NotFound
        {
            get { return attributes.Get(x => x.NotFound); }
            set { attributes.Set(x => x.NotFound, value); }
        }

        public bool IsSpecified<TResult>(Expression<Func<OneToManyMapping, TResult>> property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<OneToManyMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<OneToManyMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }
    }
}