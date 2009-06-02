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

        public VersionMapping Version
        {
            get { return mappedMembers.Version; }
            set { mappedMembers.Version = value; }
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

        public IEnumerable<ComponentMappingBase> Components
        {
            get { return mappedMembers.Components; }
        }

        public IEnumerable<OneToOneMapping> OneToOnes
        {
            get { return mappedMembers.OneToOnes; }
        }

        public IEnumerable<AnyMapping> Anys
        {
            get { return mappedMembers.Anys; }
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

        public void AddComponent(ComponentMappingBase componentMapping)
        {
            mappedMembers.AddComponent(componentMapping);
        }

        public void AddOneToOne(OneToOneMapping mapping)
        {
            mappedMembers.AddOneToOne(mapping);
        }

        public void AddAny(AnyMapping mapping)
        {
            mappedMembers.AddAny(mapping);
        }

        #endregion

        public override string ToString()
        {
            return string.Format("ClassMapping({0})", Type.Name);
        }

    }
}