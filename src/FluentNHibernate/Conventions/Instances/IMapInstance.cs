using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IMapInstance : IMapInspector
    {
        new IIndexInstanceBase Index { get; }
        new void OrderBy(string orderBy);
        new void Sort(string sort);
        new IAccessInstance Access { get; }
    }
}
