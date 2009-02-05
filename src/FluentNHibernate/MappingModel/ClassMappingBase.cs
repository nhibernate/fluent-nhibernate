using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel
{
    public abstract class ClassMappingBase : MappingBase, INameable
    {
        private readonly AttributeStore<ClassMappingBase> _attributes;
        protected IList<PropertyMapping> _properties;
        protected IList<ICollectionMapping> _collections;
        protected IList<ManyToOneMapping> _references;
        public Type Type { get; set; }

        protected ClassMappingBase(AttributeStore underlyingStore)
        {
            _attributes = new AttributeStore<ClassMappingBase>(underlyingStore);
            _properties = new List<PropertyMapping>();
            _collections = new List<ICollectionMapping>();
            _references = new List<ManyToOneMapping>();
        }

        public IEnumerable<PropertyMapping> Properties
        {
            get { return _properties; }
        }

        public IEnumerable<ICollectionMapping> Collections
        {
            get { return _collections; }
        }

        public IEnumerable<ManyToOneMapping> References
        {
            get { return _references; }
        }

        public string Name
        {
            get { return _attributes.Get(x => x.Name); }
            set { _attributes.Set(x => x.Name, value); }
        }

        public bool IsNameSpecified
        {
            get { return _attributes.IsSpecified(x => x.Name); }
        }

        public void AddProperty(PropertyMapping property)
        {
            _properties.Add(property);
        }

        public void AddCollection(ICollectionMapping collection)
        {
            _collections.Add(collection);
        }

        public void AddReference(ManyToOneMapping manyToOne)
        {
            _references.Add(manyToOne);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            foreach (var collection in Collections)
                visitor.Visit(collection);

            foreach (var property in Properties)
                visitor.Visit(property);

            foreach (var reference in References)
                visitor.Visit(reference);
        }
    }
}