using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.ClassBased
{
    /// <summary>
    /// A reference to a component which is declared externally. Contains properties
    /// that can't be declared externally (property name, for example)
    /// </summary>
    public class ReferenceComponentMapping : IComponentMapping
    {
        public ComponentType ComponentType { get; set; }
        private readonly Member property;
        private readonly Type componentType;
        private ExternalComponentMapping mergedComponent;
        private Type containingEntityType;
        private readonly string columnPrefix;

        public ReferenceComponentMapping(ComponentType componentType, Member property, Type componentEntityType, Type containingEntityType, string columnPrefix)
        {
            ComponentType = componentType;
            this.property = property;
            this.componentType = componentEntityType;
            this.containingEntityType = containingEntityType;
            this.columnPrefix = columnPrefix;
        }

        public void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessComponent(this);

            if (mergedComponent == null)
                throw new UnresolvedComponentReferenceVisitedException(componentType, containingEntityType, property);

            mergedComponent.AcceptVisitor(visitor);
        }

        public virtual void AssociateExternalMapping(ExternalComponentMapping mapping)
        {
            mergedComponent = mapping;
            mergedComponent.Member = property;
            mergedComponent.Name = property.Name;
            mergedComponent.Class = new TypeReference(componentType);
            mergedComponent.Type = componentType;
        }

        public IEnumerable<ManyToOneMapping> References
        {
            get { return mergedComponent.References; }
        }

        public IEnumerable<ICollectionMapping> Collections
        {
            get { return mergedComponent.Collections; }
        }
        
        public IEnumerable<PropertyMapping> Properties
        {
            get { return mergedComponent.Properties; }
        }

        public IEnumerable<IComponentMapping> Components
        {
            get { return mergedComponent.Components; }
        }

        public IEnumerable<OneToOneMapping> OneToOnes
        {
            get { return mergedComponent.OneToOnes; }
        }

        public IEnumerable<AnyMapping> Anys
        {
            get { return mergedComponent.Anys; }
        }

        public void AddProperty(PropertyMapping property)
        {
            mergedComponent.AddProperty(property);
        }

        public void AddCollection(ICollectionMapping collection)
        {
            mergedComponent.AddCollection(collection);
        }

        public void AddReference(ManyToOneMapping manyToOne)
        {
            mergedComponent.AddReference(manyToOne);
        }

        public void AddComponent(IComponentMapping componentMapping)
        {
            mergedComponent.AddComponent(componentMapping);
        }

        public void AddOneToOne(OneToOneMapping mapping)
        {
            mergedComponent.AddOneToOne(mapping);
        }

        public void AddAny(AnyMapping mapping)
        {
            mergedComponent.AddAny(mapping);
        }

        public Type ContainingEntityType
        {
            get { return containingEntityType; }
            set { containingEntityType = value; }
        }

        public Member Member
        {
            get { return (mergedComponent == null) ? property : mergedComponent.Member; }
        }

        public ParentMapping Parent
        {
            get { return mergedComponent.Parent; }
            set { mergedComponent.Parent = value; }
        }

        public bool Unique
        {
            get { return mergedComponent.Unique; }
            set { mergedComponent.Unique = value; }
        }

        public bool HasColumnPrefix
        {
            get { return !string.IsNullOrEmpty(ColumnPrefix); }
        }

        public string ColumnPrefix
        {
            get { return columnPrefix; }
        }

        public bool Insert
        {
            get { return mergedComponent.Insert; }
            set { mergedComponent.Insert = value; }
        }

        public bool Update
        {
            get { return mergedComponent.Update; }
            set { mergedComponent.Update = value; }
        }

        public string Access
        {
            get { return mergedComponent.Access; }
            set { mergedComponent.Access = value; }
        }

        public bool OptimisticLock
        {
            get { return mergedComponent.OptimisticLock; }
            set { mergedComponent.OptimisticLock = value; }
        }

        public string Name
        {
            get { return (mergedComponent == null) ? property.Name : mergedComponent.Name; }
            set { mergedComponent.Name = value; }
        }

        public Type Type
        {
            get { return (mergedComponent == null) ? componentType : mergedComponent.Type; }
        }

        public TypeReference Class
        {
            get { return mergedComponent.Class; }
            set { mergedComponent.Class = value; }
        }

        public bool Lazy
        {
            get { return mergedComponent.Lazy; }
            set { mergedComponent.Lazy = value; }
        }

        public bool IsAssociated
        {
            get { return mergedComponent != null; }
        }

        public ComponentMapping MergedModel
        {
            get { return mergedComponent; }
        }

        public bool HasValue(string property)
        {
            if (!IsAssociated)
                return false;

            return mergedComponent.HasValue(property);
        }

        public bool Equals(ReferenceComponentMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.property, property) &&
                Equals(other.componentType, componentType) &&
                Equals(other.mergedComponent, mergedComponent) &&
                Equals(other.containingEntityType, containingEntityType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(ReferenceComponentMapping)) return false;
            return Equals((ReferenceComponentMapping)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (property != null ? property.GetHashCode() : 0);
                result = (result * 397) ^ (componentType != null ? componentType.GetHashCode() : 0);
                result = (result * 397) ^ (mergedComponent != null ? mergedComponent.GetHashCode() : 0);
                result = (result * 397) ^ (containingEntityType != null ? containingEntityType.GetHashCode() : 0);
                return result;
            }
        }
    }
}