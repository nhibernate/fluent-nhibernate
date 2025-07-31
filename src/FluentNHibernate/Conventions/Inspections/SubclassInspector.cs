using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Conventions.Inspections;

public class SubclassInspector : ISubclassInspector
{
    readonly InspectorModelMapper<ISubclassInspector, SubclassMapping> mappedProperties = new InspectorModelMapper<ISubclassInspector, SubclassMapping>();
    readonly SubclassMapping mapping;

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
                .Select(x => new AnyInspector(x));
        }
    }

    public IEnumerable<ICollectionInspector> Collections
    {
        get
        {
            return mapping.Collections
                .Select(x => new CollectionInspector(x));
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
                .Select(x => new JoinInspector(x));
        }
    }

    public bool LazyLoad => mapping.Lazy;

    public string Name => mapping.Name;

    public IEnumerable<IOneToOneInspector> OneToOnes
    {
        get
        {
            return mapping.OneToOnes
                .Select(x => new OneToOneInspector(x));
        }
    }

    public IEnumerable<IPropertyInspector> Properties
    {
        get
        {
            return mapping.Properties
                .Select(x => new PropertyInspector(x));
        }
    }

    public string Proxy => mapping.Proxy;

    public IEnumerable<IManyToOneInspector> References
    {
        get
        {
            return mapping.References
                .Select(x => new ManyToOneInspector(x));
        }
    }
    public bool SelectBeforeUpdate => mapping.SelectBeforeUpdate;

    public IEnumerable<ISubclassInspector> Subclasses
    {
        get
        {
            return mapping.Subclasses
                .Select(x => new SubclassInspector((SubclassMapping)x));
        }
    }

    IEnumerable<ISubclassInspectorBase> ISubclassInspectorBase.Subclasses => Subclasses;

    public Type Type => mapping.Type;
}
