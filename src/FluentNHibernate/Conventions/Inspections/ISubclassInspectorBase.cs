using System;
using System.Collections.Generic;

namespace FluentNHibernate.Conventions.Inspections;

public interface ISubclassInspectorBase : IInspector
{
    bool Abstract { get; }
    IEnumerable<IAnyInspector> Anys { get; }
    IEnumerable<ICollectionInspector> Collections { get; }
    IEnumerable<IComponentBaseInspector> Components { get; }
    IEnumerable<IJoinInspector> Joins { get; }
    IEnumerable<IOneToOneInspector> OneToOnes { get; }
    IEnumerable<IPropertyInspector> Properties { get; }
    IEnumerable<IManyToOneInspector> References { get; }
    IEnumerable<ISubclassInspectorBase> Subclasses { get; }
    bool DynamicInsert { get; }
    bool DynamicUpdate { get; }
    Type Extends { get; }
    bool LazyLoad { get; }
    string Name { get; }
    string Proxy { get; }
    bool SelectBeforeUpdate { get; }
    Type Type { get; }
}
