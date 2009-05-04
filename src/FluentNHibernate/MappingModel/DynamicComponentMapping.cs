using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.MappingModel
{
    public class DynamicComponentMapping : ClassMappingBase
    {
        private readonly AttributeStore<DynamicComponentMapping> attributes;
        private readonly List<IMappingPart> unmigratedParts = new List<IMappingPart>();
        private readonly IDictionary<string, string> unmigratedAttributes = new Dictionary<string, string>();

        public DynamicComponentMapping()
            : this(new AttributeStore())
        {}

        private DynamicComponentMapping(AttributeStore store)
            : base(store)
        {
            attributes = new AttributeStore<DynamicComponentMapping>(store);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessDynamicComponent(this);

            foreach (var property in Properties)
                visitor.Visit(property);

            if (Parent != null)
                visitor.Visit(Parent);

            base.AcceptVisitor(visitor);
        }

        public ParentMapping Parent { get; set; }

        public AttributeStore<DynamicComponentMapping> Attributes
        {
            get { return attributes; }
        }

        public IEnumerable<IMappingPart> UnmigratedParts
        {
            get { return unmigratedParts; }
        }

        public IEnumerable<KeyValuePair<string, string>> UnmigratedAttributes
        {
            get { return unmigratedAttributes; }
        }

        public void AddUnmigratedPart(IMappingPart part)
        {
            unmigratedParts.Add(part);
        }

        public void AddUnmigratedAttribute(string attribute, string value)
        {
            unmigratedAttributes.Add(attribute, value);
        }
    }
}