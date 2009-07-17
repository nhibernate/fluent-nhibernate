using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IVersionInstance : IVersionInspector
    {
        IAccessInstance Access { get; }
        IGeneratedInstance Generated { get; }
        void ColumnName(string columnName);
        void UnsavedValue(string unsavedValue);
    }
}