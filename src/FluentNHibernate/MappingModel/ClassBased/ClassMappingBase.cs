using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel.ClassBased
{
    public abstract class ClassMappingBase : MappingBase, IHasMappedMembers
    {
        private readonly AttributeStore<ClassMappingBase> attributes;
        private readonly MappedMembers mappedMembers;
        private readonly IList<ISubclassMapping> subclasses;
        public Type Type { get; set; }

        protected ClassMappingBase(AttributeStore underlyingStore)
        {
            attributes = new AttributeStore<ClassMappingBase>(underlyingStore);
            mappedMembers = new MappedMembers();
            subclasses = new List<ISubclassMapping>();
        }

        public string Name
        {
            get { return attributes.Get(x => x.Name); }
            set { attributes.Set(x => x.Name, value); }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            mappedMembers.AcceptVisitor(visitor);

            foreach (var subclass in Subclasses)
                visitor.Visit(subclass);
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

        public IEnumerable<JoinMapping> Joins
        {
            get { return mappedMembers.Joins; }
        }

        public IEnumerable<ISubclassMapping> Subclasses
        {
            get { return subclasses; }
        }

        public void AddProperty(PropertyMapping property)
        {
            mappedMembers.AddProperty(property);
        }

        public void AddOrReplaceProperty(PropertyMapping mapping)
        {
            mappedMembers.AddOrReplaceProperty(mapping);
        }

        public void AddCollection(ICollectionMapping collection)
        {
            mappedMembers.AddCollection(collection);
        }

        public void AddOrReplaceCollection(ICollectionMapping mapping)
        {
            mappedMembers.AddOrReplaceCollection(mapping);
        }

        public void AddReference(ManyToOneMapping manyToOne)
        {
            mappedMembers.AddReference(manyToOne);
        }

        public void AddOrReplaceReference(ManyToOneMapping manyToOne)
        {
            mappedMembers.AddOrReplaceReference(manyToOne);
        }

        public void AddComponent(ComponentMappingBase componentMapping)
        {
            mappedMembers.AddComponent(componentMapping);
        }

        public void AddOrReplaceComponent(ComponentMappingBase mapping)
        {
            mappedMembers.AddOrReplaceComponent(mapping);
        }

        public void AddOneToOne(OneToOneMapping mapping)
        {
            mappedMembers.AddOneToOne(mapping);
        }

        public void AddOrReplaceOneToOne(OneToOneMapping mapping)
        {
            mappedMembers.AddOrReplaceOneToOne(mapping);
        }

        public void AddAny(AnyMapping mapping)
        {
            mappedMembers.AddAny(mapping);
        }

        public void AddOrReplaceAny(AnyMapping mapping)
        {
            mappedMembers.AddOrReplaceAny(mapping);
        }

        public void AddJoin(JoinMapping mapping)
        {
            mappedMembers.AddJoin(mapping);
        }

        public void AddSubclass(ISubclassMapping subclass)
        {
            subclasses.Add(subclass);
        }

        #endregion

        public override string ToString()
        {
            return string.Format("ClassMapping({0})", Type.Name);
        }
    }
}