using System.Collections.Generic;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IManyToManyInspector : IInspector
    {
        IEnumerable<IColumnInspector> Columns { get; }
    }
}