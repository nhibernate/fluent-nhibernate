using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IMapInstance : IMapInspector
    {
        new IIndexInstanceBase Index { get; }
        void SetOrderBy(string orderBy);
        void SetSort(string sort);
    }
}
