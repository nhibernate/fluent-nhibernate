using System;
using System.Linq;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel
{
    public class ClassMapping : MappingBase, INameable
    {
        private readonly AttributeStore<ClassMapping> _attributes;

        private readonly IList<PropertyMapping> _properties;
        private readonly IList<ICollectionMapping> _collections;
        private readonly IList<ManyToOneMapping> _references;
        private readonly IList<ISubclassMapping> _subclasses;
        
        public ClassMapping()
        {
            _attributes = new AttributeStore<ClassMapping>();
            _properties = new List<PropertyMapping>();
            _collections = new List<ICollectionMapping>();   
            _references = new List<ManyToOneMapping>();
            _subclasses = new List<ISubclassMapping>();
        }

        public IIdentityMapping Id { get; set; }
        public Type Type { get; set; }

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

        public IEnumerable<ISubclassMapping> Subclasses
        {
            get { return _subclasses; }
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


        public void AddSubclass(ISubclassMapping subclass)
        {
            _subclasses.Add(subclass);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessClass(this);

            if (Id != null)
                visitor.Visit(Id);

            foreach (var collection in Collections)
                visitor.Visit(collection);

            foreach (var property in Properties)
                visitor.Visit(property);

            foreach (var reference in References)
                visitor.Visit(reference);

            foreach( var subclass in Subclasses)
                visitor.Visit(subclass);
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

    	public string Tablename
    	{
			get { return _attributes.Get(x => x.Tablename); }
			set { _attributes.Set(x => x.Tablename,value); }
    	}

        public AttributeStore<ClassMapping> Attributes
        {
            get { return _attributes; }
        }

        
    }
}