using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel.ClassBased
{
    public abstract class ClassMappingBase : MappingBase, INameable, IHasMappedMembers
    {
        private readonly AttributeStore<ClassMappingBase> attributes;
        private readonly MappedMembers mappedMembers;
        public Type Type { get; set; }

        protected ClassMappingBase(AttributeStore underlyingStore)
        {
            attributes = new AttributeStore<ClassMappingBase>(underlyingStore);
            mappedMembers = new MappedMembers();
        }

        public string Name
        {
            get { return attributes.Get(x => x.Name); }
            set { attributes.Set(x => x.Name, value); }
        }

        public bool IsNameSpecified
        {
            get { return attributes.IsSpecified(x => x.Name); }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            mappedMembers.AcceptVisitor(visitor);
        }

        #region IHasMappedMembers

        public IEnumerable<ManyToOneMapping> References
        {
            get { return mappedMembers.References; }
        }

        public IEnumerable<ICollectionMapping> Collections
        {
            get { return mappedMembers.Collections; }
        }

        public IEnumerable<PropertyMapping> Properties
        {
            get { return mappedMembers.Properties; }
        }

        public IEnumerable<ComponentMapping> Components
        {
            get { return mappedMembers.Components; }
        }

        public void AddProperty(PropertyMapping property)
        {
            mappedMembers.AddProperty(property);
        }

        public void AddCollection(ICollectionMapping collection)
        {
            mappedMembers.AddCollection(collection);
        }

        public void AddReference(ManyToOneMapping manyToOne)
        {
            mappedMembers.AddReference(manyToOne);
        }

        public void AddComponent(ComponentMapping componentMapping)
        {
            mappedMembers.AddComponent(componentMapping);
        }

        public void AddDynamicComponent(DynamicComponentMapping componentMapping)
        {
            mappedMembers.AddDynamicComponent(componentMapping);
        }

        #endregion

        public override string ToString()
        {
            return string.Format("ClassMapping({0})", this.Type.Name);
        }

    }
}