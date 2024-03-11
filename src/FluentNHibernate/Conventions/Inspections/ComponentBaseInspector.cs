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

    public Access Access => Access.FromString(mapping.Access);

    public Type EntityType => mapping.ContainingEntityType;

    public string StringIdentifierForModel => mapping.Name;

    public abstract bool IsSet(Member property);

    public Member Property => mapping.Member;

    public IParentInspector Parent
    {
        get
        {
            if (mapping.Parent is null)
                return new ParentInspector(new ParentMapping());

            return new ParentInspector(mapping.Parent);
        }
    }

    public bool Insert => mapping.Insert;

    public bool Update => mapping.Update;

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

    public string Name => mapping.Name;

    public bool OptimisticLock => mapping.OptimisticLock;

    public bool Unique => mapping.Unique;

    public TypeReference Class => mapping is ComponentMapping ? ((ComponentMapping)mapping).Class : null;

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

    public IEnumerable<IManyToOneInspector> References
    {
        get
        {
            return mapping.References
                .Select(x => new ManyToOneInspector(x));
        }
    }

    public Type Type => mapping.Type;
}
