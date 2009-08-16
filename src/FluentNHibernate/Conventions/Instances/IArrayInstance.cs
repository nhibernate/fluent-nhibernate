using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IArrayInstance : IArrayInspector, ICollectionInstance
    {
        new IIndexInstanceBase Index { get; }
    }
}
