using System;
using System.Linq;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel
{
    public class ClassMapping : MappingBase
    {
        private readonly AttributeStore<ClassMapping> _attributes;

        private readonly IList<PropertyMapping> _properties;
        private readonly IList<ICollectionMapping> _collections;
        private readonly IList<ManyToOneMapping> _references;
        
        public ClassMapping()
        {
            _attributes = new AttributeStore<ClassMapping>();
            _properties = new List<PropertyMapping>();
            _collections = new List<ICollectionMapping>();   
            _references = new List<ManyToOneMapping>();
        }

        public IIdentityMapping Id { get; set; }

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
            visitor.ProcessClass(this);        
    
            if(Id != null)
                visitor.ProcessIdentity(Id);

            foreach (var collection in Collections)
                visitor.ProcessCollection(collection);

            foreach (var property in Properties)
                visitor.ProcessProperty(property);

            foreach (var reference in References)
                visitor.ProcessManyToOne(reference);
        }

        public string Name
        {
            get { return _attributes.Get(x => x.Name); }
            set { _attributes.Set(x => x.Name, value); }
        }

    	public string Tablename
    	{
			get { return _attributes.Get(x => x.Tablename); }
			set { _attributes.Set(x => x.Tablename,value); }
    	}
    }
}