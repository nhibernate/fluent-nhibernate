using System.Collections.Generic;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections;

public interface IKeyPropertyInspector : IInspector
{
    Access Access { get; }
    string Name { get; }
    TypeReference Type { get; }
    IEnumerable<IColumnInspector> Columns { get; }
    int Length { get; }
}
