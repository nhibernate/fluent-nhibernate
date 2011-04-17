using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.ClassBased
{
    /// <summary>
    /// A reference to a component which is declared externally. Contains properties
    /// that can't be declared externally (property name, for example)
    /// </summary>
    [Serializable]
    public class ReferenceComponentMapping : IComponentMapping
    {
        public ComponentType ComponentType { get; set; }
        private readonly Member property;
        private readonly Type componentType;
        private ExternalComponentMapping mergedComponent;
        private Type containingEntityType;

        public ReferenceComponentMapping(ComponentType componentType, Member property, Type componentEntityType, Type containingEntityType, string columnPrefix)
        {
            ComponentType = componentType;
            this.property = property;
            this.componentType = componentEntityType;
            this.containingEntityType = containingEntityType;
            ColumnPrefix = columnPrefix;
        }

        public void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessComponent(this);

            if (mergedComponent != null)
                mergedComponent.AcceptVisitor(visitor);
        }

        public bool IsSpecified(string name)
        {
            if (!IsAssociated)
                return false;

            return mergedComponent.IsSpecified(name);
        }

        public void Set(string attribute, int layer, object value)
        {
            ((IMapping)mergedComponent).Set(attribute, layer, value);
        }

        public virtual void AssociateExternalMapping(ExternalComponentMapping mapping)
        {
            mergedComponent = mapping;
            mergedComponent.Member = property;
            mergedComponent.Set(x => x.Name, Layer.Defaults, property.Name);
            mergedComponent.Set(x => x.Class, Layer.Defaults, new TypeReference(componentType));
            mergedComponent.Set(x => x.Type, Layer.Defaults, componentType);
        }

        public IEnumerable<ManyToOneMapping> References
        {
            get { return mergedComponent.References; }
        }

        public IEnumerable<CollectionMapping> Collections
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

        public void AddCollection(CollectionMapping collection)
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
        }

        public bool Unique
        {
            get { return mergedComponent.Unique; }
        }

        public bool HasColumnPrefix
        {
            get { return !string.IsNullOrEmpty(ColumnPrefix); }
        }

        public string ColumnPrefix { get; set; }

        public bool Insert
        {
            get { return mergedComponent.Insert; }
        }

        public bool Update
        {
            get { return mergedComponent.Update; }
        }

        public string Access
        {
            get { return mergedComponent.Access; }
        }

        public bool OptimisticLock
        {
            get { return mergedComponent.OptimisticLock; }
        }

        public string Name
        {
            get { return (mergedComponent == null) ? property.Name : mergedComponent.Name; }
        }

        public Type Type
        {
            get { return (mergedComponent == null) ? componentType : mergedComponent.Type; }
        }

        public TypeReference Class
        {
            get { return mergedComponent.Class; }
        }

        public bool Lazy
        {
            get { return mergedComponent.Lazy; }
        }

        public bool IsAssociated
        {
            get { return mergedComponent != null; }
        }

        public ComponentMapping MergedModel
        {
            get { return mergedComponent; }
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