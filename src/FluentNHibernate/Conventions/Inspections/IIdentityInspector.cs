using System.Collections.Generic;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IIdentityInspector : IExposedThroughPropertyInspector
    {
        IEnumerable<IColumnInspector> Columns { get; }
        IGeneratorInspector Generator { get; }
        object UnsavedValue { get; }
        string Name { get; }
        Access Access { get; }
        TypeReference Type { get; }
    }
}