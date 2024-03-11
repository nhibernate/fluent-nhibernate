using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Conventions.Inspections;

public class JoinedSubclassInspector : IJoinedSubclassInspector
{
    private readonly InspectorModelMapper<IJoinedSubclassInspector, SubclassMapping> mappedProperties = new InspectorModelMapper<IJoinedSubclassInspector, SubclassMapping>();
    private readonly SubclassMapping mapping;

    public JoinedSubclassInspector(SubclassMapping mapping)
    {
        this.mapping = mapping;
        mappedProperties.Map(x => x.LazyLoad, x => x.Lazy);
    }

    public Type EntityType
    {
        get { return mapping.Type; }
    }

    public string StringIdentifierForModel
    {
        get { return mapping.Name; }
    }

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(mappedProperties.Get(property));
    }

    public bool Abstract
    {
        get { return mapping.Abstract; }
    }

    public IEnumerable<IAnyInspector> Anys
    {
        get
        {
            return mapping.Anys
                .Select(x => new AnyInspector(x))
                .Cast<IAnyInspector>();
        }
    }

    public IKeyInspector Key
    {
        get
        {
            if (mapping.Key is null)
                return new KeyInspector(new KeyMapping());

            return new KeyInspector(mapping.Key);
        }
    }

    public string Check
    {
        get { return mapping.Check; }
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

    public bool DynamicInsert
    {
        get { return mapping.DynamicInsert; }
    }

    public bool DynamicUpdate
    {
        get { return mapping.DynamicUpdate; }
    }

    public Type Extends
    {
        get { return mapping.Extends; }
    }

    public IEnumerable<IJoinInspector> Joins
    {
        get
        {
            return mapping.Joins
                .Select(x => new JoinInspector(x))
                .Cast<IJoinInspector>();
        }
    }

    public bool LazyLoad
    {
        get { return mapping.Lazy; }
    }

    public string Schema
    {
        get { return mapping.Schema; }
    }

    public string Name
    {
        get { return mapping.Name; }
    }

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

    public string Proxy
    {
        get { return mapping.Proxy; }
    }

    public IEnumerable<IManyToOneInspector> References
    {
        get
        {
            return mapping.References
                .Select(x => new ManyToOneInspector(x))
                .Cast<IManyToOneInspector>();
        }
    }
    public bool SelectBeforeUpdate
    {
        get { return mapping.SelectBeforeUpdate; }
    }

    public IEnumerable<IJoinedSubclassInspector> Subclasses
    {
        get
        {
            return mapping.Subclasses
                .Select(x => new JoinedSubclassInspector(x))
                .Cast<IJoinedSubclassInspector>();
        }
    }

    IEnumerable<ISubclassInspectorBase> ISubclassInspectorBase.Subclasses
    {
        get { return Subclasses.Cast<ISubclassInspectorBase>(); }
    }

    public string TableName
    {
        get { return mapping.TableName; }
    }

    public Type Type
    {
        get { return mapping.Type; }
    }
}
