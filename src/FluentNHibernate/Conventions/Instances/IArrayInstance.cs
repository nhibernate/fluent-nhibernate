using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IArrayInstance : IArrayInspector
    {
        new IIndexInstanceBase Index { get; }
        new IAccessInstance Access { get; }
    }
}
