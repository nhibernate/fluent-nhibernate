namespace FluentNHibernate.Conventions.InspectionDsl
{
    public interface IPropertyInspector : IReadOnlyInspector, IExposedThroughPropertyInspector
    {
        bool Insert { get; }
        bool Update { get; }
        int Length { get; }
        bool Nullable { get; }
        string Formula { get; }
        string CustomType { get; }
        string SqlType { get; }
        bool Unique { get; }
        string UniqueKey { get; }
        Access Access { get; }
    }
}