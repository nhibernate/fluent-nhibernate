using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IIdentityInstance : IIdentityInspector
    {
        void Column(string column);
        void UnsavedValue(string unsavedValue);
        new IAccessInstance Access { get; }
        IGeneratorInstance GeneratedBy { get; }
    }
}