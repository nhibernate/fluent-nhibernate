using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IVersionInstance : IVersionInspector
    {
        new IAccessInstance Access { get; }
        new IGeneratedInstance Generated { get; }
        void Column(string columnName);
        void UnsavedValue(string unsavedValue);
    }
}