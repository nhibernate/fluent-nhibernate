using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public class JoinMapping : IMapping
    {
        private readonly AttributeStore attributes;

        private readonly MappedMembers mappedMembers;

        public JoinMapping()
            : this(new AttributeStore())
        {}

        public JoinMapping(AttributeStore attributes)
        {
            this.attributes = attributes;
            mappedMembers = new MappedMembers();
        }

        public KeyMapping Key
        {
            get { return attributes.GetOrDefault<KeyMapping>("Key"); }
        }

        public IEnumerable<PropertyMapping> Properties
        {
            get { return mappedMembers.Properties; }
        }

        public IEnumerable<ManyToOneMapping> References
        {
            get { return mappedMembers.References; }
        }

        public IEnumerable<IComponentMapping> Components
        {
            get { return mappedMembers.Components; }
        }

        public IEnumerable<AnyMapping> Anys
        {
            get { return mappedMembers.Anys; }
        }

        public IEnumerable<CollectionMapping> Collections
        {
            get { return mappedMembers.Collections; }
        }

        public void AddProperty(PropertyMapping property)
        {
            mappedMembers.AddProperty(property);
        }

        public void AddReference(ManyToOneMapping manyToOne)
        {
            mappedMembers.AddReference(manyToOne);
        }

        public void AddComponent(IComponentMapping componentMapping)
        {
            mappedMembers.AddComponent(componentMapping);
        }

        public void AddAny(AnyMapping mapping)
        {
            mappedMembers.AddAny(mapping);
        }

        public void AddCollection(CollectionMapping collectionMapping)
        {
            mappedMembers.AddCollection(collectionMapping);
        }

        public string TableName
        {
            get { return attributes.GetOrDefault<string>("TableName"); }
        }

        public string Schema
        {
            get { return attributes.GetOrDefault<string>("Schema"); }
        }

        public string Catalog
        {
            get { return attributes.GetOrDefault<string>("Catalog"); }
        }

        public string Subselect
        {
            get { return attributes.GetOrDefault<string>("Subselect"); }
        }

        public string Fetch
        {
            get { return attributes.GetOrDefault<string>("Fetch"); }
        }

        public bool Inverse
        {
            get { return attributes.GetOrDefault<bool>("Inverse"); }
        }

        public bool Optional
        {
            get { return attributes.GetOrDefault<bool>("Optional"); }
        }

        public Type ContainingEntityType { get; set; }

        public void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessJoin(this);

            if (Key != null)
                visitor.Visit(Key);

            mappedMembers.AcceptVisitor(visitor);
        }

        public bool Equals(JoinMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.attributes, attributes) &&
                Equals(other.mappedMembers, mappedMembers) &&
                Equals(other.ContainingEntityType, ContainingEntityType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(JoinMapping)) return false;
            return Equals((JoinMapping)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (attributes != null ? attributes.GetHashCode() : 0);
                result = (result * 397) ^ (mappedMembers != null ? mappedMembers.GetHashCode() : 0);
                result = (result * 397) ^ (ContainingEntityType != null ? ContainingEntityType.GetHashCode() : 0);
                return result;
            }
        }

        public void Set<T>(Expression<Func<JoinMapping, T>> expression, int layer, T value)
        {
            Set(expression.ToMember().Name, layer, value);
        }

        public void Set(string attribute, int layer, object value)
        {
            attributes.Set(attribute, layer, value);
        }

        public bool IsSpecified(string attribute)
        {
            return attributes.IsSpecified(attribute);
        }
    }
}
