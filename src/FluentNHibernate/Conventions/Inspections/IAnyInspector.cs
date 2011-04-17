using System.Collections.Generic;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IAnyInspector : IAccessInspector, IInspector
    {        
        Cascade Cascade { get; }
        IEnumerable<IColumnInspector> IdentifierColumns { get; }
        string IdType { get; }
        bool Insert { get; }
        TypeReference MetaType { get; }
        IEnumerable<IMetaValueInspector> MetaValues { get; }
        string Name { get; }
        IEnumerable<IColumnInspector> TypeColumns { get; }
        bool Update { get; }
        bool LazyLoad { get; }
        bool OptimisticLock { get; }
    }
}