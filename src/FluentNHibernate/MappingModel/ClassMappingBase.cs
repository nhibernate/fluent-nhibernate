using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel
{
    public abstract class ClassMappingBase : MappingBase, INameable, IHasMappedMembers
    {
        private readonly AttributeStore<ClassMappingBase> _attributes;
        private readonly MappedMembers _mappedMembers;
        public Type Type { get; set; }

        protected ClassMappingBase(AttributeStore underlyingStore)
        {
            _attributes = new AttributeStore<ClassMappingBase>(underlyingStore);
            _mappedMembers = new MappedMembers();
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

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            _mappedMembers.AcceptVisitor(visitor);
        }

        #region IHasMappedMembers

        public IEnumerable<ManyToOneMapping> References
        {
            get { return _mappedMembers.References; }
        }

        public IEnumerable<ICollectionMapping> Collections
        {
            get { return _mappedMembers.Collections; }
        }

        public IEnumerable<PropertyMapping> Properties
        {
            get { return _mappedMembers.Properties; }
        }

        public IEnumerable<ComponentMapping> Components
        {
            get { return _mappedMembers.Components; }
        }

        public void AddProperty(PropertyMapping property)
        {
            _mappedMembers.AddProperty(property);
        }

        public void AddCollection(ICollectionMapping collection)
        {
            _mappedMembers.AddCollection(collection);
        }

        public void AddReference(ManyToOneMapping manyToOne)
        {
            _mappedMembers.AddReference(manyToOne);
        }

        public void AddComponent(ComponentMapping componentMapping)
        {
            _mappedMembers.AddComponent(componentMapping);
        }

        #endregion

		public override string ToString()
		{
			return string.Format("ClassMapping({0})", this.Type.Name);
		}

    }
}
