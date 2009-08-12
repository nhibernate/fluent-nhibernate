using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IListInstance : IListInspector
    {
        new IIndexInstanceBase Index { get; }
        new IAccessInstance Access { get; }
    }
}
