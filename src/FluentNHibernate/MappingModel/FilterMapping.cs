using System;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public class FilterMapping : IMapping
    {
        readonly AttributeStore attributes;

        public FilterMapping()
            : this(new AttributeStore())
        { }

        public FilterMapping(AttributeStore attributes)
        {
            this.attributes = attributes;
        }

        public string Name
        {
            get { return attributes.GetOrDefault<string>("Name"); }
        }

        public string Condition
        {
            get { return attributes.GetOrDefault<string>("Condition"); }
        }

        public void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessFilter(this);
        }

        public bool IsSpecified(string property)
        {
            return attributes.IsSpecified(property);
        }

        public bool Equals(FilterMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.attributes, attributes);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(FilterMapping)) return false;
            return Equals((FilterMapping)obj);
        }

        public override int GetHashCode()
        {
            return (attributes != null ? attributes.GetHashCode() : 0);
        }

        public void Set<T>(Expression<Func<FilterMapping, T>> expression, int layer, T value)
        {
            Set(expression.ToMember().Name, layer, value);
        }

        public void Set(string attribute, int layer, object value)
        {
            attributes.Set(attribute, layer, value);
        }
    }
}
