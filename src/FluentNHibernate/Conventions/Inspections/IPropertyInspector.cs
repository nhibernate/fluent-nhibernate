using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IPropertyInspector : IReadOnlyInspector, IExposedThroughPropertyInspector, IAccessInspector
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
        string Name { get; }
        bool OptimisticLock { get; }
        string Generated { get; }
        IEnumerable<IColumnInspector> Columns { get; }
    }
}