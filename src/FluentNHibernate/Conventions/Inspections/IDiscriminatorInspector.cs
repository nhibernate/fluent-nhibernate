using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IDiscriminatorInspector : IInspector
    {
        bool Insert { get; }
        IDefaultableEnumerable<IColumnInspector> Columns { get; }
        bool Force { get; }
        string Formula { get; }
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