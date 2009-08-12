using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface ISetInstance : ISetInspector
    {
        void OrderBy(string orderBy);
        void Sort(string sort);
        new IAccessInstance Access { get; }
    }
}
