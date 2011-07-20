using System.Collections.Generic;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IManyToOneInspector : IAccessInspector, IExposedThroughPropertyInspector
    {
        string Name { get; }
        IEnumerable<IColumnInspector> Columns { get; }
        Cascade Cascade { get; }
        TypeReference Class { get; }
        string Formula { get; }
        Fetch Fetch { get; }
        string ForeignKey { get; }
        bool Insert { get; }
        Laziness LazyLoad { get; }
        NotFound NotFound { get; }
        string PropertyRef { get; }
        bool Update { get; }
        bool Nullable { get; }
        bool OptimisticLock { get; }
    }
}