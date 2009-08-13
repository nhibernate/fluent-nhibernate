using System;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IClassInspector : ILazyLoadInspector, IReadOnlyInspector
    {
        string TableName { get; }
        OptimisticLock OptimisticLock { get; }
        string Schema { get; }
        bool DynamicUpdate { get; }
        bool DynamicInsert { get; }
        int BatchSize { get; }
        bool Abstract { get; }
        string Check { get; }
        object DiscriminatorValue { get; }
        string Name { get; }
        string Persister { get; }
        Polymorphism Polymorphism { get; }
        string Proxy { get; }
        string Where { get; }
        string Subselect { get; }
        bool SelectBeforeUpdate { get; }
        IIdentityInspectorBase Id { get; }
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
        IDefaultableEnumerable<ISubclassInspectorBase> Subclasses { get; }
    }
}