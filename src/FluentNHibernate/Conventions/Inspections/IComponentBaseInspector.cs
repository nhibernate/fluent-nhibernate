using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IComponentBaseInspector : IAccessInspector, IExposedThroughPropertyInspector
    {
        IParentInspector Parent { get; }
        bool Insert { get; }
        bool Update { get; }
        IEnumerable<IAnyInspector> Anys { get; }
        IEnumerable<ICollectionInspector> Collections { get; }
        IEnumerable<IComponentBaseInspector> Components { get; }
        string Name { get; }
        bool LazyLoad { get; }
        bool OptimisticLock { get; }
        bool Unique { get; }
        Type Type { get; }
        TypeReference Class { get; }
        IEnumerable<IOneToOneInspector> OneToOnes { get; }
        IEnumerable<IPropertyInspector> Properties { get; }
        IEnumerable<IManyToOneInspector> References { get; }
    }
}