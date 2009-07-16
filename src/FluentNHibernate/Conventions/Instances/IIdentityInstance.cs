using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IIdentityInstance : IIdentityInspector
    {
        void ColumnName(string column);
        void UnsavedValue(string unsavedValue);
        IAccessInstance Access { get; }
        IGeneratorInstance GeneratedBy { get; }
    }
}