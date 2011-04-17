using System;
using System.Linq.Expressions;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Collections
{
    [Serializable]
    public class NestedCompositeElementMapping : CompositeElementMapping
    {
        readonly AttributeStore attributes;

        public NestedCompositeElementMapping()
            : this(new AttributeStore())
        { }

        public NestedCompositeElementMapping(AttributeStore attributes)
            : base(attributes)
        {
            this.attributes = attributes;
        }

        public string Name
        {
            get { return attributes.GetOrDefault<string>("Name"); }
        }

        public void Set<T>(Expression<Func<NestedCompositeElementMapping, T>> expression, int layer, T value)
        {
            Set(expression.ToMember().Name, layer, value);
        }
    }
}