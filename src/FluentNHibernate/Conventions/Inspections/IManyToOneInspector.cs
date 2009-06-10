using System.Collections.Generic;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IManyToOneInspector : IAccessInspector, ICascadeInspector, IOuterJoinInspector, IExposedThroughPropertyInspector
    {
        string Name { get; }
        TypeReference Class { get; }
        IEnumerable<IColumnInspector> Columns { get; }
    }
}