using System.Collections.Generic;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IKeyInspector : IInspector
    {
        IEnumerable<IColumnInspector> Columns { get; }
    }
}