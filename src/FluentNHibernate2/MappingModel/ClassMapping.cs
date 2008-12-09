using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel
{
    public class ClassMapping : MappingBase<HbmClass>
    {
        private IIdentityMapping _id;
        private readonly IList<PropertyMapping> _properties;
        private readonly IList<ICollectionMapping> _collections;
        private readonly IList<ManyToOneMapping> _references;

        public ClassMapping()
        {
            _properties = new List<PropertyMapping>();
            _collections = new List<ICollectionMapping>();   
            _references = new List<ManyToOneMapping>();
        }

        public ClassMapping(string name, IIdentityMapping identityMapping) : this()
        {            
            Name = name;
            Id = identityMapping;
        }

        public string Name
        {
            get { return _hbm.name; }
            set { _hbm.name = value; }
        }

        public IIdentityMapping Id
        {
            get { return _id; }
            set
            {
                _id = value;
                _hbm.Item1 = _id.Hbm;
            }
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

        public void AddProperty(PropertyMapping property)
        {
            _properties.Add(property);
            property.Hbm.AddTo(ref _hbm.Items);
        }

        public void AddCollection(ICollectionMapping collection)
        {
            _collections.Add(collection);
            collection.Hbm.AddTo(ref _hbm.Items);
        }

        public void AddReference(ManyToOneMapping manyToOne)
        {
            _references.Add(manyToOne);
            manyToOne.Hbm.AddTo(ref _hbm.Items);
        }
    }
}