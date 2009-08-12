using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.MappingModel.Collections
{
    public class CompositeElementMapping : MappingBase
    {
        private readonly MappedMembers mappedMembers;
        protected readonly AttributeStore<CompositeElementMapping> attributes;

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

        public TypeReference Class
        {
            get { return attributes.Get(x => x.Class); }
            set { attributes.Set(x => x.Class, value); }
        }

        public ParentMapping Parent
        {
            get { return attributes.Get(x => x.Parent); }
            set { attributes.Set(x => x.Parent, value); }
        }

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

        public Type ContainingEntityType { get; set; }

        public void AddReference(ManyToOneMapping manyToOne)
        {
            mappedMembers.AddReference(manyToOne);
        }

        public bool IsSpecified<TResult>(Expression<Func<CompositeElementMapping, TResult>> property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<CompositeElementMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<CompositeElementMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }
    }
}