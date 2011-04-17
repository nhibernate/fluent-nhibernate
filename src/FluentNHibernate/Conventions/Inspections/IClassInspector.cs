using System;
using System.Collections.Generic;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IClassInspector : ILazyLoadInspector, IReadOnlyInspector
    {
        string TableName { get; }
        OptimisticLock OptimisticLock { get; }
        SchemaAction SchemaAction { get; }
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
        IEnumerable<IAnyInspector> Anys { get; }
        IEnumerable<ICollectionInspector> Collections { get; }
        IEnumerable<IComponentBaseInspector> Components { get; }
        IEnumerable<IJoinInspector> Joins { get; }
        IEnumerable<IOneToOneInspector> OneToOnes { get; }
        IEnumerable<IPropertyInspector> Properties { get; }
        IEnumerable<IManyToOneInspector> References { get; }
        IEnumerable<ISubclassInspectorBase> Subclasses { get; }
    }
}