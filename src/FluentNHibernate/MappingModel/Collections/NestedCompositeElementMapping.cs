using System;

namespace FluentNHibernate.MappingModel.Collections
{
    [Serializable]
    public class NestedCompositeElementMapping : CompositeElementMapping
    {
        readonly AttributeStore<NestedCompositeElementMapping> attributes;

        public NestedCompositeElementMapping()
            : this(new AttributeStore())
        { }

        public NestedCompositeElementMapping(AttributeStore store)
            : base(store)
        {
            attributes = new AttributeStore<NestedCompositeElementMapping>(store);
        }

        public string Name
        {
            get { return attributes.Get(x => x.Name); }
            set { attributes.Set(x => x.Name, value); }
        }
    }
}