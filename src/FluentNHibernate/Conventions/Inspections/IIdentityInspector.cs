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
        int Precision { get; }
        int Scale { get; }
        bool Nullable { get; }
        bool Unique { get; }
        string UniqueKey { get; }
        string SqlType { get; }
        string Index { get; }
        string Check { get; }
        string Default { get; }
    }
}