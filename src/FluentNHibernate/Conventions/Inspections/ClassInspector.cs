using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Conventions.Inspections
{
    public class ClassInspector : IClassInspector
    {
        private readonly ClassMapping mapping;
        private readonly InspectorModelMapper<IClassInspector, ClassMapping> propertyMappings = new InspectorModelMapper<IClassInspector, ClassMapping>();

        public ClassInspector(ClassMapping mapping)
        {
            this.mapping = mapping;

            propertyMappings.Map(x => x.LazyLoad, x => x.Lazy);
            propertyMappings.Map(x => x.ReadOnly, x => x.Mutable);
            propertyMappings.Map(x => x.EntityType, x => x.Type);
        }

        public Type EntityType
        {
            get { return mapping.Type; }
        }

        public string StringIdentifierForModel
        {
            get { return mapping.Name; }
        }

        public bool LazyLoad
        {
            get { return mapping.Lazy; }
        }

        public bool ReadOnly
        {
            get { return !mapping.Mutable; }
        }

        public string TableName
        {
            get { return mapping.TableName; }
        }

        ICacheInspector IClassInspector.Cache
        {
            get { return Cache; }
        }

        public ICacheInstance Cache
        {
            get
            {
                if (mapping.Cache == null)
                    // conventions are hitting it, user must want a cache
                    mapping.Set(x => x.Cache, Layer.Conventions, new CacheMapping());

                return new CacheInstance(mapping.Cache);
            }
        }

        public OptimisticLock OptimisticLock
        {
            get { return OptimisticLock.FromString(mapping.OptimisticLock); }
        }

        public SchemaAction SchemaAction
        {
            get { return SchemaAction.FromString(mapping.SchemaAction); }
        }

        public string Schema
        {
            get { return mapping.Schema; }
        }

        public bool DynamicUpdate
        {
            get { return mapping.DynamicUpdate; }
        }

        public bool DynamicInsert
        {
            get { return mapping.DynamicInsert; }
        }

        public int BatchSize
        {
            get { return mapping.BatchSize; }
        }

        public bool Abstract
        {
            get { return mapping.Abstract; }
        }

        public IVersionInspector Version
        {
            get
            {
                if (mapping.Version == null)
                    return new VersionInspector(new VersionMapping());

                return new VersionInspector(mapping.Version);
            }
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
                            return (IComponentBaseInspector)new ComponentInspector((ComponentMapping)x);

                        return (IComponentBaseInspector)new DynamicComponentInspector((ComponentMapping)x);
                    });
            }
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
                if (mapping.Discriminator == null)
                    // deliberately empty so nothing evaluates to true
                    return new DiscriminatorInspector(new DiscriminatorMapping());

                return new DiscriminatorInspector(mapping.Discriminator);
            }
        }

        public object DiscriminatorValue
        {
            get { return mapping.DiscriminatorValue; }
        }

        public string Name
        {
            get { return mapping.Name; }
        }

        public string Persister
        {
            get { return mapping.Persister; }
        }

        public Polymorphism Polymorphism
        {
            get { return Polymorphism.FromString(mapping.Polymorphism); }
        }

        public string Proxy
        {
            get { return mapping.Proxy; }
        }

        public string Where
        {
            get { return mapping.Where; }
        }

        public string Subselect
        {
            get { return mapping.Subselect; }
        }

        public bool SelectBeforeUpdate
        {
            get { return mapping.SelectBeforeUpdate; }
        }

        public IIdentityInspectorBase Id
        {
            get
            {
                if (mapping.Id == null)
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
}