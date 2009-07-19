using System.Collections.Generic;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IManyToOneInspector : IAccessInspector, IExposedThroughPropertyInspector
    {
        string Name { get; }
        IEnumerable<IColumnInspector> Columns { get; }
        Cascade Cascade { get; }
        TypeReference Class { get; }
        string Fetch { get; }
        string ForeignKey { get; }
        bool Insert { get; }
        Laziness LazyLoad { get; }
        string NotFound { get; }
        string OuterJoin { get; }
        string PropertyRef { get; }
        bool Update { get; }
    }
}