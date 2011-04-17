using System;
using System.Collections.Generic;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.ClassBased
{
    [Serializable]
    public abstract class ClassMappingBase : MappingBase, IHasMappedMembers
    {
        readonly AttributeStore attributes;
        readonly MappedMembers mappedMembers;
        readonly IList<SubclassMapping> subclasses;

        protected ClassMappingBase(AttributeStore attributes)
        {
            this.attributes = attributes;
            mappedMembers = new MappedMembers();
            subclasses = new List<SubclassMapping>();
        }

        public abstract string Name { get; }
        public abstract Type Type { get; }

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

        public IEnumerable<Collections.CollectionMapping> Collections
        {
            get { return mappedMembers.Collections; }
        }

        public IEnumerable<PropertyMapping> Properties
        {
            get { return mappedMembers.Properties; }
        }

        public IEnumerable<IComponentMapping> Components
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

        public IEnumerable<FilterMapping> Filters
        {
            get { return mappedMembers.Filters; }
        }

        public IEnumerable<SubclassMapping> Subclasses
        {
            get { return subclasses; }
        }

        public IEnumerable<StoredProcedureMapping> StoredProcedures
        {
            get { return mappedMembers.StoredProcedures; }
        }

        public void AddProperty(PropertyMapping property)
        {
            mappedMembers.AddProperty(property);
        }

        public void AddOrReplaceProperty(PropertyMapping mapping)
        {
            mappedMembers.AddOrReplaceProperty(mapping);
        }

        public void AddCollection(Collections.CollectionMapping collection)
        {
            mappedMembers.AddCollection(collection);
        }

        public void AddOrReplaceCollection(Collections.CollectionMapping mapping)
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

        public void AddComponent(IComponentMapping componentMapping)
        {
            mappedMembers.AddComponent(componentMapping);
        }

        public void AddOrReplaceComponent(IComponentMapping mapping)
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

        public void AddFilter(FilterMapping mapping)
        {
            mappedMembers.AddFilter(mapping);
        }

        public void AddOrReplaceFilter(FilterMapping mapping)
        {
            mappedMembers.AddOrReplaceFilter(mapping);
        }

        public void AddSubclass(SubclassMapping subclass)
        {
            subclasses.Add(subclass);
        }

        public void AddStoredProcedure(StoredProcedureMapping mapping)
        {
            mappedMembers.AddStoredProcedure(mapping);
        }
        #endregion

        public override string ToString()
        {
            return string.Format("ClassMapping({0})", Type.Name);
        }

        public bool Equals(ClassMappingBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.mappedMembers, mappedMembers) &&
                other.subclasses.ContentEquals(subclasses);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(ClassMappingBase)) return false;
            return Equals((ClassMappingBase)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((mappedMembers != null ? mappedMembers.GetHashCode() : 0) * 397) ^ (subclasses != null ? subclasses.GetHashCode() : 0);
            }
        }

        public void MergeAttributes(AttributeStore clone)
        {
            clone.CopyTo(attributes);
        }
    }
}