using System;
using System.Linq.Expressions;

namespace FluentNHibernate.MappingModel
{
    public class ParentMapping : MappingBase
    {
        private readonly AttributeStore<ParentMapping> attributes = new AttributeStore<ParentMapping>();

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessParent(this);
        }

        public string Name
        {
            get { return attributes.Get(x => x.Name); }
            set { attributes.Set(x => x.Name, value); }
        }

        public Type ContainingEntityType { get; set; }

        public bool IsSpecified<TResult>(Expression<Func<ParentMapping, TResult>> property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<ParentMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<ParentMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }
    }
}