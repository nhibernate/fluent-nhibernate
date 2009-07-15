using System.Collections.Generic;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IManyToOneInspector : IAccessInspector, IExposedThroughPropertyInspector
    {
        string Name { get; }
        TypeReference Class { get; }
        IEnumerable<IColumnInspector> Columns { get; }
        string Cascade { get; }
    }
}