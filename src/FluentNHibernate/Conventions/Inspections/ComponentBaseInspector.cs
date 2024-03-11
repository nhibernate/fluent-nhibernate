using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Conventions.Inspections;

public abstract class ComponentBaseInspector : IComponentBaseInspector
{
    private readonly IComponentMapping mapping;

    public ComponentBaseInspector(IComponentMapping mapping)
    {
        this.mapping = mapping;
    }

    public Access Access
    {
        get { return Access.FromString(mapping.Access); }
    }

    public Type EntityType
    {
        get { return mapping.ContainingEntityType; }
    }

    public string StringIdentifierForModel
    {
        get { return mapping.Name; }
    }

    public abstract bool IsSet(Member property);

    public Member Property
    {
        get { return mapping.Member; }
    }

    public IParentInspector Parent
    {
        get
        {
            if (mapping.Parent is null)
                return new ParentInspector(new ParentMapping());

            return new ParentInspector(mapping.Parent);
        }
    }

    public bool Insert
    {
        get { return mapping.Insert; }
    }

    public bool Update
    {
        get { return mapping.Update; }
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

    public string Name
    {
        get { return mapping.Name; }
    }

    public bool OptimisticLock
    {
        get { return mapping.OptimisticLock; }
    }

    public bool Unique
    {
        get { return mapping.Unique; }
    }

    public TypeReference Class
    {
        get { return mapping is ComponentMapping ? ((ComponentMapping)mapping).Class : null; }
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

    public IEnumerable<IManyToOneInspector> References
    {
        get
        {
            return mapping.References
                .Select(x => new ManyToOneInspector(x))
                .Cast<IManyToOneInspector>();
        }
    }

    public Type Type
    {
        get { return mapping.Type; }
    }
}
