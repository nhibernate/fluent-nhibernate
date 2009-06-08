using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IPropertyInspector : IReadOnlyInspector, IExposedThroughPropertyInspector
    {
        bool Insert { get; }
        bool Update { get; }
        int Length { get; }
        bool Nullable { get; }
        string Formula { get; }
        TypeReference Type { get; }
        string SqlType { get; }
        bool Unique { get; }
        string UniqueKey { get; }
        Access Access { get; }
        string Name { get; }
        bool OptimisticLock { get; }
        string Generated { get; }
        IEnumerable<IColumnInspector> Columns { get; }
    }
}