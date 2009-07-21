using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IIndexInstance : IIndexInspector
    {
        void Column(string columnName);
    }
}
