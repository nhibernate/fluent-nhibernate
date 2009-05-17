namespace FluentNHibernate.Conventions.Inspections
{
    public interface IIdentityInspector : IExposedThroughPropertyInspector
    {
        string ColumnName { get; }
        Generator Generator { get; }
        object UnsavedValue { get; }
    }
}