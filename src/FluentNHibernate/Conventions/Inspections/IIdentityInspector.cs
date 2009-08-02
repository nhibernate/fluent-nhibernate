using System.Collections.Generic;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IIdentityInspector : IExposedThroughPropertyInspector, IIdentityInspectorBase
    {
        IEnumerable<IColumnInspector> Columns { get; }
        IGeneratorInspector Generator { get; }
        TypeReference Type { get; }
        int Length { get; }
    }
}