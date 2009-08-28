using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IVersionInspector : IInspector
    {
        string Name { get; }
        Access Access { get; }
        IDefaultableEnumerable<IColumnInspector> Columns { get; }
        Generated Generated { get; }
        string UnsavedValue { get; }
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