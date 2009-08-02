using System.Collections.Generic;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IAnyInspector : IInspector
    {
        Access Access { get; }
        Cascade Cascade { get; }
        IDefaultableEnumerable<IColumnInspector> IdentifierColumns { get; }
        string IdType { get; }
        bool Insert { get; }
        TypeReference MetaType { get; }
        IEnumerable<IMetaValueInspector> MetaValues { get; }
        string Name { get; }
        IDefaultableEnumerable<IColumnInspector> TypeColumns { get; }
        bool Update { get; }
        bool LazyLoad { get; }
        bool OptimisticLock { get; }
    }
}