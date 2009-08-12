using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IMapInstance : IMapInspector
    {
        new IIndexInstanceBase Index { get; }
        void OrderBy(string orderBy);
        void Sort(string sort);
        new IAccessInstance Access { get; }
    }
}
