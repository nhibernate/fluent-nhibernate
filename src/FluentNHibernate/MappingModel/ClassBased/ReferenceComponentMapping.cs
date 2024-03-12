using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.ClassBased;

/// <summary>
/// A reference to a component which is declared externally. Contains properties
/// that can't be declared externally (property name, for example)
/// </summary>
[Serializable]
public class ReferenceComponentMapping(ComponentType componentType, Member property, Type componentEntityType, Type containingEntityType, string columnPrefix)
    : IComponentMapping
{
    public ComponentType ComponentType { get; set; } = componentType;
    private readonly Member property = property;
    private readonly Type componentType = componentEntityType;
    private ExternalComponentMapping mergedComponent;
    private Type containingEntityType = containingEntityType;

    public void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessComponent(this);

        mergedComponent?.AcceptVisitor(visitor);
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

    public IEnumerable<ManyToOneMapping> References => mergedComponent.References;

    public IEnumerable<CollectionMapping> Collections => mergedComponent.Collections;

    public IEnumerable<PropertyMapping> Properties => mergedComponent.Properties;

    public IEnumerable<IComponentMapping> Components => mergedComponent.Components;

    public IEnumerable<OneToOneMapping> OneToOnes => mergedComponent.OneToOnes;

    public IEnumerable<AnyMapping> Anys => mergedComponent.Anys;

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
        get => containingEntityType;
        set => containingEntityType = value;
    }

    public Member Member => (mergedComponent is null) ? property : mergedComponent.Member;

    public ParentMapping Parent => mergedComponent.Parent;

    public bool Unique => mergedComponent.Unique;

    public bool HasColumnPrefix => !string.IsNullOrEmpty(ColumnPrefix);

    public string ColumnPrefix { get; set; } = columnPrefix;

    public bool Insert => mergedComponent.Insert;

    public bool Update => mergedComponent.Update;

    public string Access => mergedComponent.Access;

    public bool OptimisticLock => mergedComponent.OptimisticLock;

    public string Name => (mergedComponent is null) ? property.Name : mergedComponent.Name;

    public Type Type => (mergedComponent is null) ? componentType : mergedComponent.Type;

    public TypeReference Class => mergedComponent.Class;

    public bool Lazy => mergedComponent.Lazy;

    public bool IsAssociated => mergedComponent is not null;

    public ComponentMapping MergedModel => mergedComponent;

    public bool Equals(ReferenceComponentMapping other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(other.property, property) &&
               other.componentType == componentType &&
               Equals(other.mergedComponent, mergedComponent) &&
               other.containingEntityType == containingEntityType;
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
            int result = (property is not null ? property.GetHashCode() : 0);
            result = (result * 397) ^ (componentType is not null ? componentType.GetHashCode() : 0);
            result = (result * 397) ^ (mergedComponent is not null ? mergedComponent.GetHashCode() : 0);
            result = (result * 397) ^ (containingEntityType is not null ? containingEntityType.GetHashCode() : 0);
            return result;
        }
    }
}
