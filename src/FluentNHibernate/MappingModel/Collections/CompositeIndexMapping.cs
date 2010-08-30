using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Collections
{
    [Serializable]
    public class CompositeIndexMapping : MappingBase, IIndexMapping
    {
        readonly List<KeyPropertyMapping> properties = new List<KeyPropertyMapping>();
        readonly List<KeyManyToOneMapping> manyToOnes = new List<KeyManyToOneMapping>();
        readonly AttributeStore<CompositeIndexMapping> attributes;

        public CompositeIndexMapping()
            : this(new AttributeStore())
        { }

        public CompositeIndexMapping(AttributeStore store)
        {
            attributes = new AttributeStore<CompositeIndexMapping>(store);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessCompositeIndex(this);

            foreach (var property in Properties)
                visitor.Visit(property);

            foreach (var reference in References)
                visitor.Visit(reference);
        }

        public TypeReference Type
        {
            get { return attributes.Get(x => x.Type); }
            set { attributes.Set(x => x.Type, value); }
        }

        public IEnumerable<KeyPropertyMapping> Properties
        {
            get { return properties; }
        }

        public void AddProperty(KeyPropertyMapping property)
        {
            properties.Add(property);
        }

        public IEnumerable<KeyManyToOneMapping> References
        {
            get { return manyToOnes; }
        }

        public void AddReference(KeyManyToOneMapping manyToOne)
        {
            manyToOnes.Add(manyToOne);
        }

        public Type ContainingEntityType { get; set; }

        public override bool IsSpecified(string property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<CompositeIndexMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<CompositeIndexMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }

        public bool Equals(CompositeIndexMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return properties.ContentEquals(other.properties) && manyToOnes.ContentEquals(other.manyToOnes) && Equals(other.attributes, attributes) && Equals(other.ContainingEntityType, ContainingEntityType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(CompositeElementMapping)) return false;
            return Equals((CompositeIndexMapping)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (properties != null ? properties.GetHashCode() : 0);
                result = (result * 397) ^ (manyToOnes != null ? manyToOnes.GetHashCode() : 0);
                result = (result * 397) ^ (attributes != null ? attributes.GetHashCode() : 0);
                result = (result * 397) ^ (ContainingEntityType != null ? ContainingEntityType.GetHashCode() : 0);
                return result;
            }
        }
    }
}