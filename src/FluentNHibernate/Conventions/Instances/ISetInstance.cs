using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface ISetInstance : ISetInspector
    {
        void SetOrderBy(string orderBy);
        void SetSort(string sort);
    }
}
