using System.Collections.Generic;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IElementInspector : IInspector
    {
        TypeReference Type { get; }
        IEnumerable<IColumnInspector> Columns { get; }
        string Formula { get; }
    }
}