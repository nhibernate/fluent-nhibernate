using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IBagInstance :IBagInspector
    {
        void SetOrderBy(string orderBy);
    }
}
