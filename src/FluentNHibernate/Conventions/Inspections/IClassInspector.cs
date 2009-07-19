using System;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IClassInspector : ILazyLoadInspector, IReadOnlyInspector
    {
        string TableName { get; }
        string OptimisticLock { get; }
        string Schema { get; }
        bool DynamicUpdate { get; }
        bool DynamicInsert { get; }
        int BatchSize { get; }
        bool Abstract { get; }
        string Check { get; }
        object DiscriminatorValue { get; }
        string Name { get; }
        string Persister { get; }
        string Polymorphism { get; }
        string Proxy { get; }
        bool SelectBeforeUpdate { get; }
        IIdentityInspector Id { get; }
        ICacheInspector Cache { get; }
        IDiscriminatorInspector Discriminator { get; }
        IVersionInspector Version { get; }
        IDefaultableEnumerable<IAnyInspector> Anys { get; }
        IDefaultableEnumerable<ICollectionInspector> Collections { get; }
        IDefaultableEnumerable<IComponentBaseInspector> Components { get; }
        IDefaultableEnumerable<IJoinInspector> Joins { get; }
        IDefaultableEnumerable<IOneToOneInspector> OneToOnes { get; }
        IDefaultableEnumerable<IPropertyInspector> Properties { get; }
        IDefaultableEnumerable<IManyToOneInspector> References { get; }
        IDefaultableEnumerable<ISubclassInspector> Subclasses { get; }
    }
}