using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.MappingModel
{
    public class JoinMapping : IMappingBase
    {
        private readonly AttributeStore<JoinMapping> attributes;

        public KeyMapping Key { get; set; }
        private readonly MappedMembers mappedMembers;

        public JoinMapping()
        {
            attributes = new AttributeStore<JoinMapping>();
            mappedMembers = new MappedMembers();
        }

        public IEnumerable<PropertyMapping> Properties
        {
            get { return mappedMembers.Properties; }
        }

        public IEnumerable<ManyToOneMapping> References
        {
            get { return mappedMembers.References; }
        }

        public IEnumerable<ComponentMappingBase> Components
        {
            get { return mappedMembers.Components; }
        }

        public IEnumerable<AnyMapping> Anys
        {
            get { return mappedMembers.Anys; }
        }

        public void AddProperty(PropertyMapping property)
        {
            mappedMembers.AddProperty(property);
        }

        public void AddReference(ManyToOneMapping manyToOne)
        {
            mappedMembers.AddReference(manyToOne);
        }

        public void AddComponent(ComponentMappingBase componentMapping)
        {
            mappedMembers.AddComponent(componentMapping);
        }

        public void AddAny(AnyMapping mapping)
        {
            mappedMembers.AddAny(mapping);
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

        public void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessJoin(this);

            if (Key != null)
                visitor.Visit(Key);

            mappedMembers.AcceptVisitor(visitor);
        }

        public bool IsSpecified<TResult>(Expression<Func<JoinMapping, TResult>> property)
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
    }
}
