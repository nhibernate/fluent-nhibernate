using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.MappingModel.Collections
{
    public class CompositeElementMapping : MappingBase, INameable
    {
        private readonly MappedMembers mappedMembers;
        protected readonly AttributeStore<CompositeElementMapping> attributes;
        protected readonly List<IMappingPart> unmigratedParts = new List<IMappingPart>();
        protected readonly IDictionary<string, string> unmigratedAttributes = new Dictionary<string, string>();

        public CompositeElementMapping()
            : this(new AttributeStore())
        { }

        public CompositeElementMapping(AttributeStore store)
        {
            attributes = new AttributeStore<CompositeElementMapping>(store);
            mappedMembers = new MappedMembers();
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessCompositeElement(this);

            mappedMembers.AcceptVisitor(visitor);

            if (Parent != null)
                visitor.Visit(Parent);
        }

        public string Class
        {
            get { return attributes.Get(x => x.Class); }
            set { attributes.Set(x => x.Class, value); }
        }
        
        public string Name
        {
            get { return attributes.Get(x => x.Name); }
            set { attributes.Set(x => x.Name, value); }
        }

        public bool IsNameSpecified
        {
            get { return attributes.IsSpecified(x => x.Name); }
        }

        public PropertyInfo PropertyInfo { get; set; }
        public ParentMapping Parent { get; set; }

        public IEnumerable<PropertyMapping> Properties
        {
            get { return mappedMembers.Properties; }
        }

        public void AddProperty(PropertyMapping property)
        {
            mappedMembers.AddProperty(property);
        }

        public IEnumerable<ManyToOneMapping> References
        {
            get { return mappedMembers.References; }
        }

        public void AddReference(ManyToOneMapping manyToOne)
        {
            mappedMembers.AddReference(manyToOne);
        }

        public AttributeStore<CompositeElementMapping> Attributes
        {
            get { return attributes; }
        }
    }
}