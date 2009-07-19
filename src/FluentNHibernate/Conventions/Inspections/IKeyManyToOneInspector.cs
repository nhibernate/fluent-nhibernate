using System.Collections.Generic;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IKeyManyToOneInspector : IInspector
    {
        Access Access { get; }
        TypeReference Class { get; }
        string ForeignKey { get; }
        Laziness LazyLoad { get; }
        string Name { get; }
        string NotFound { get; }
        IEnumerable<IColumnInspector> Columns { get; }
    }
}