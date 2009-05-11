using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.MappingModel
{
    public class JoinMapping : IMappingBase
    {
        private readonly AttributeStore<JoinMapping> _attributes;

        private readonly IList<PropertyMapping> _properties;
        private readonly IList<ManyToOneMapping> _references;
        private readonly IList<ComponentMapping> _components;

        public KeyMapping Key { get; set; }

        public JoinMapping()
        {
            _attributes = new AttributeStore<JoinMapping>();
            _properties = new List<PropertyMapping>();
            _references = new List<ManyToOneMapping>();
            _components = new List<ComponentMapping>();
        }

        public IEnumerable<PropertyMapping> Properties
        {
            get { return _properties; }
        }

        public IEnumerable<ManyToOneMapping> References
        {
            get { return _references; }
        }

        public IEnumerable<ComponentMapping> Components
        {
            get { return _components; }
        }

        public void AddProperty(PropertyMapping property)
        {
            _properties.Add(property);
        }

        public void AddReference(ManyToOneMapping manyToOne)
        {
            _references.Add(manyToOne);
        }

        public void AddComponent(ComponentMapping componentMapping)
        {
            _components.Add(componentMapping);
        }

        public string TableName
        {
            get { return _attributes.Get(x => x.TableName); }
            set { _attributes.Set(x => x.TableName, value); }
        }

        public virtual void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessJoin(this);

            if (Key != null)
                visitor.Visit(Key);

            foreach (var property in Properties)
                visitor.Visit(property);

            foreach (var reference in References)
                visitor.Visit(reference);

            foreach (var component in Components)
                visitor.Visit(component);
        }
    }
}
