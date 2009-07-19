using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface ISubclassInspector : IInspector
    {
        bool Abstract { get; }
        IEnumerable<IAnyInspector> Anys { get; }
        IEnumerable<ICollectionInspector> Collections { get; }
        object DiscriminatorValue { get; }
        bool DynamicInsert { get; }
        bool DynamicUpdate { get; }
        string Extends { get; }
        IEnumerable<IJoinInspector> Joins { get; }
        Laziness LazyLoad { get; }
        string Name { get; }
        IEnumerable<IOneToOneInspector> OneToOnes { get; }
        IEnumerable<IPropertyInspector> Properties { get; }
        string Proxy { get; }
        IEnumerable<IManyToOneInspector> References { get; }
        bool SelectBeforeUpdate { get; }
        IEnumerable<ISubclassInspector> Subclasses { get; }
        Type Type { get; }
    }
}