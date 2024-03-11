using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Conventions.Inspections;

public class SubclassInspector : ISubclassInspector
{
    private readonly InspectorModelMapper<ISubclassInspector, SubclassMapping> mappedProperties = new InspectorModelMapper<ISubclassInspector, SubclassMapping>();
    private readonly SubclassMapping mapping;

    public SubclassInspector(SubclassMapping mapping)
    {
        this.mapping = mapping;
        mappedProperties.Map(x => x.LazyLoad, x => x.Lazy);
    }

    public Type EntityType => mapping.Type;

    public string StringIdentifierForModel => mapping.Name;

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(mappedProperties.Get(property));
    }

    public bool Abstract => mapping.Abstract;

    public IEnumerable<IAnyInspector> Anys
    {
        get
        {
            return mapping.Anys
                .Select(x => new AnyInspector(x))
                .Cast<IAnyInspector>();
        }
    }

    public IEnumerable<ICollectionInspector> Collections
    {
        get
        {
            return mapping.Collections
                .Select(x => new CollectionInspector(x))
                .Cast<ICollectionInspector>();
        }
    }

    public IEnumerable<IComponentBaseInspector> Components
    {
        get
        {
            return mapping.Components
                .Select(x =>
                {
                    if (x.ComponentType == ComponentType.Component)
                        return (IComponentBaseInspector)new ComponentInspector(x);

                    return (IComponentBaseInspector)new DynamicComponentInspector(x);
                });
        }
    }

    public object DiscriminatorValue => mapping.DiscriminatorValue;

    public bool DynamicInsert => mapping.DynamicInsert;

    public bool DynamicUpdate => mapping.DynamicUpdate;

    public Type Extends => mapping.Extends;

    public IEnumerable<IJoinInspector> Joins
    {
        get
        {
            return mapping.Joins
                .Select(x => new JoinInspector(x))
                .Cast<IJoinInspector>();
        }
    }

    public bool LazyLoad => mapping.Lazy;

    public string Name => mapping.Name;

    public IEnumerable<IOneToOneInspector> OneToOnes
    {
        get
        {
            return mapping.OneToOnes
                .Select(x => new OneToOneInspector(x))
                .Cast<IOneToOneInspector>();
        }
    }

    public IEnumerable<IPropertyInspector> Properties
    {
        get
        {
            return mapping.Properties
                .Select(x => new PropertyInspector(x))
                .Cast<IPropertyInspector>();
        }
    }

    public string Proxy => mapping.Proxy;

    public IEnumerable<IManyToOneInspector> References
    {
        get
        {
            return mapping.References
                .Select(x => new ManyToOneInspector(x))
                .Cast<IManyToOneInspector>();
        }
    }
    public bool SelectBeforeUpdate => mapping.SelectBeforeUpdate;

    public IEnumerable<ISubclassInspector> Subclasses
    {
        get
        {
            return mapping.Subclasses
                .Select(x => new SubclassInspector((SubclassMapping)x))
                .Cast<ISubclassInspector>();
        }
    }

    IEnumerable<ISubclassInspectorBase> ISubclassInspectorBase.Subclasses => Subclasses.Cast<ISubclassInspectorBase>();

    public Type Type => mapping.Type;
}
