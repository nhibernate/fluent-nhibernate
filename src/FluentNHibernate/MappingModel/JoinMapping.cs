using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public class JoinMapping : IMappingBase
    {
        private readonly AttributeStore<JoinMapping> attributes;

        private readonly MappedMembers mappedMembers;

        public JoinMapping()
            : this(new AttributeStore())
        {}

        public JoinMapping(AttributeStore underlyingStore)
        {
            attributes = new AttributeStore<JoinMapping>(underlyingStore);
            mappedMembers = new MappedMembers();
        }

        public KeyMapping Key
        {
            get { return attributes.Get(x => x.Key); }
            set { attributes.Set(x => x.Key, value); }
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
            get { return attributes.Get(x => x.TableName); }
            set { attributes.Set(x => x.TableName, value); }
        }

        public string Schema
        {
            get { return attributes.Get(x => x.Schema); }
            set { attributes.Set(x => x.Schema, value); }
        }

        public string Catalog
        {
            get { return attributes.Get(x => x.Catalog); }
            set { attributes.Set(x => x.Catalog, value); }
        }

        public string Subselect
        {
            get { return attributes.Get(x => x.Subselect); }
            set { attributes.Set(x => x.Subselect, value); }
        }

        public string Fetch
        {
            get { return attributes.Get(x => x.Fetch); }
            set { attributes.Set(x => x.Fetch, value); }
        }

        public bool Inverse
        {
            get { return attributes.Get(x => x.Inverse); }
            set { attributes.Set(x => x.Inverse, value); }
        }

        public bool Optional
        {
            get { return attributes.Get(x => x.Optional); }
            set { attributes.Set(x => x.Optional, value); }
        }

        public Type ContainingEntityType { get; set; }

        public void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessJoin(this);

            if (Key != null)
                visitor.Visit(Key);

            mappedMembers.AcceptVisitor(visitor);
        }

        public bool IsSpecified(string property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<JoinMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<JoinMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
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
    }
}
