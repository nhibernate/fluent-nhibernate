using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel
{
    public class JoinedSubclassMapping : MappingBase, ISubclassMapping
    {
        private IList<JoinedSubclassMapping> _subclasses;
        private readonly IList<PropertyMapping> _properties;
        private readonly IList<ICollectionMapping> _collections;
        private readonly IList<ManyToOneMapping> _references;
        private AttributeStore<JoinedSubclassMapping> _attributes;

        public JoinedSubclassMapping()
        {
            _subclasses = new List<JoinedSubclassMapping>();
            _properties = new List<PropertyMapping>();
            _collections = new List<ICollectionMapping>();
            _references = new List<ManyToOneMapping>();
            _attributes = new AttributeStore<JoinedSubclassMapping>();
        }

        public AttributeStore<JoinedSubclassMapping> Attributes
        {
            get { return _attributes; }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessJoinedSubclass(this);

            if(Key != null)
                visitor.Visit(Key);

            foreach (var collection in Collections)
                visitor.Visit(collection);

            foreach (var property in Properties)
                visitor.Visit(property);

            foreach (var reference in References)
                visitor.Visit(reference);

            foreach (var subclass in _subclasses)
                visitor.Visit(subclass);
        }

        public KeyMapping Key { get; set; }

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

        public IEnumerable<JoinedSubclassMapping> Subclasses
        {
            get { return _subclasses; }
        }

        public void AddSubclass(JoinedSubclassMapping joinedSubclassMapping)
        {
            _subclasses.Add(joinedSubclassMapping);
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

        public string Name
        {
            get { return _attributes.Get(x => x.Name); }
            set { _attributes.Set(x => x.Name, value); }
        }

        
    }
}