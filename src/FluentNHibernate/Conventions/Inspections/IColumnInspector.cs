namespace FluentNHibernate.Conventions.Inspections
{
    public interface IColumnInspector : IInspector
    {
        string Name { get; }
        string Check { get; }
        string Index { get; }
        int Length { get; }
        bool NotNull { get;  }
        string SqlType { get; }
        bool Unique { get;  }
        string UniqueKey { get; }
        int Precision { get; }
        int Scale { get; }
        string Default { get; }
    }
}