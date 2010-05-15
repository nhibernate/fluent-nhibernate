using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Collections
{
    [Serializable]
    public class CompositeElementMapping : MappingBase
    {
        readonly MappedMembers mappedMembers;
        readonly List<NestedCompositeElementMapping> compositeElements = new List<NestedCompositeElementMapping>();
        readonly AttributeStore<CompositeElementMapping> attributes;

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

            if (Parent != null)
                visitor.Visit(Parent);

            foreach (var compositeElement in CompositeElements)
                visitor.Visit(compositeElement);

            mappedMembers.AcceptVisitor(visitor);
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

        public IEnumerable<NestedCompositeElementMapping> CompositeElements
        {
            get { return compositeElements; }
        }

        public Type ContainingEntityType { get; set; }

        public void AddReference(ManyToOneMapping manyToOne)
        {
            mappedMembers.AddReference(manyToOne);
        }

        public void AddCompositeElement(NestedCompositeElementMapping compositeElement)
        {
            compositeElements.Add(compositeElement);
        }

        public override bool IsSpecified(string property)
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

        public bool Equals(CompositeElementMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.mappedMembers, mappedMembers) && Equals(other.attributes, attributes) && Equals(other.ContainingEntityType, ContainingEntityType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(CompositeElementMapping)) return false;
            return Equals((CompositeElementMapping)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (mappedMembers != null ? mappedMembers.GetHashCode() : 0);
                result = (result * 397) ^ (attributes != null ? attributes.GetHashCode() : 0);
                result = (result * 397) ^ (ContainingEntityType != null ? ContainingEntityType.GetHashCode() : 0);
                return result;
            }
        }
    }
}