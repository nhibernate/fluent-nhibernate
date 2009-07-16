namespace FluentNHibernate.Conventions.Inspections
{
    public interface IColumnInspector : IInspector
    {
        string Name { get; }
        string Check { get; }
        string Index { get; set; }
        int Length { get; }
        bool NotNull { get; set; }
        string SqlType { get; }
        bool Unique { get; set; }
        string UniqueKey { get; set; }
    }
}