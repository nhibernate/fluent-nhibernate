using System;

namespace FluentNHibernate.MappingModel
{
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
    }
}
