using System;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public class FilterMapping : IMappingBase
    {
        private readonly AttributeStore<FilterMapping> attributes;

        public FilterMapping()
            : this(new AttributeStore())
        { }

        public FilterMapping(AttributeStore underlyingStore)
        {
            attributes = new AttributeStore<FilterMapping>(underlyingStore);
        }

        public string Name
        {
            get { return attributes.Get(x => x.Name); }
            set { attributes.Set(x => x.Name, value); }
        }

        public string Condition
        {
            get { return attributes.Get(x => x.Condition); }
            set { attributes.Set(x => x.Condition, value); }
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
    }
}
