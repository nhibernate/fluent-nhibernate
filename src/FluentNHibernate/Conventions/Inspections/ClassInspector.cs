using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Conventions.Inspections;

public class ClassInspector : IClassInspector
{
    readonly ClassMapping mapping;
    readonly InspectorModelMapper<IClassInspector, ClassMapping> propertyMappings = new InspectorModelMapper<IClassInspector, ClassMapping>();

    public ClassInspector(ClassMapping mapping)
    {
        this.mapping = mapping;

        propertyMappings.Map(x => x.LazyLoad, x => x.Lazy);
        propertyMappings.Map(x => x.ReadOnly, x => x.Mutable);
        propertyMappings.Map(x => x.EntityType, x => x.Type);
    }

    public Type EntityType => mapping.Type;

    public string StringIdentifierForModel => mapping.Name;

    public bool LazyLoad => mapping.Lazy;

    public bool ReadOnly => !mapping.Mutable;

    public string TableName => mapping.TableName;

    ICacheInspector IClassInspector.Cache => Cache;

    public ICacheInstance Cache
    {
        get
        {
            if (mapping.Cache is null)
                // conventions are hitting it, user must want a cache
                mapping.Set(x => x.Cache, Layer.Conventions, new CacheMapping());

            return new CacheInstance(mapping.Cache);
        }
    }

    public OptimisticLock OptimisticLock => OptimisticLock.FromString(mapping.OptimisticLock);

    public SchemaAction SchemaAction => SchemaAction.FromString(mapping.SchemaAction);

    public string Schema => mapping.Schema;

    public bool DynamicUpdate => mapping.DynamicUpdate;

    public bool DynamicInsert => mapping.DynamicInsert;

    public int BatchSize => mapping.BatchSize;

    public bool Abstract => mapping.Abstract;

    public IVersionInspector Version
    {
        get
        {
            if (mapping.Version is null)
                return new VersionInspector(new VersionMapping());

            return new VersionInspector(mapping.Version);
        }
    }

    public IEnumerable<IAnyInspector> Anys
    {
        get
        {
            return mapping.Anys
                .Select(x => new AnyInspector(x));
        }
    }

    public string Check => mapping.Check;

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

    public IEnumerable<IJoinInspector> Joins
    {
        get
        {
            return mapping.Joins
                .Select(x => new JoinInspector(x));
        }
    }

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

    public IEnumerable<ISubclassInspectorBase> Subclasses
    {
        get
        {
            return mapping.Subclasses
                .Where(x => x.SubclassType == SubclassType.Subclass)
                .Select(x => (ISubclassInspectorBase)new SubclassInspector(x));
        }
    }

    public IDiscriminatorInspector Discriminator
    {
        get
        {
            if (mapping.Discriminator is null)
                // deliberately empty so nothing evaluates to true
                return new DiscriminatorInspector(new DiscriminatorMapping());

            return new DiscriminatorInspector(mapping.Discriminator);
        }
    }

    public object DiscriminatorValue => mapping.DiscriminatorValue;

    public string Name => mapping.Name;

    public string Persister => mapping.Persister;

    public Polymorphism Polymorphism => Polymorphism.FromString(mapping.Polymorphism);

    public string Proxy => mapping.Proxy;

    public string Where => mapping.Where;

    public string Subselect => mapping.Subselect;

    public bool SelectBeforeUpdate => mapping.SelectBeforeUpdate;

    public IIdentityInspectorBase Id
    {
        get
        {
            if (mapping.Id is null)
                return new IdentityInspector(new IdMapping());
            if (mapping.Id is CompositeIdMapping)
                return new CompositeIdentityInspector((CompositeIdMapping)mapping.Id);

            return new IdentityInspector((IdMapping)mapping.Id);
        }
    }

    public bool IsSet(Member property)
    {
        return mapping.IsSpecified(propertyMappings.Get(property));
    }
}
