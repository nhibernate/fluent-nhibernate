using System.Collections.Generic;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IKeyManyToOneInspector : IInspector
    {
        Access Access { get; }
        TypeReference Class { get; }
        string ForeignKey { get; }
        bool LazyLoad { get; }
        string Name { get; }
        NotFound NotFound { get; }
        IEnumerable<IColumnInspector> Columns { get; }
    }
}