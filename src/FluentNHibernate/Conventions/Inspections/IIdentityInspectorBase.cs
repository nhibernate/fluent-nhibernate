namespace FluentNHibernate.Conventions.Inspections
{
    public interface IIdentityInspectorBase : IInspector
    {
        Access Access { get; }
        string UnsavedValue { get; }
        string Name { get; }
    }
}