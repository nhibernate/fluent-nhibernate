using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface ISetInstance : ISetInspector
    {
        new void OrderBy(string orderBy);
        new void Sort(string sort);
        new IAccessInstance Access { get; }
    }
}
