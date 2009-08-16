using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IIndexInstanceBase : IIndexInspectorBase
    {
        void Column(string columnName);
    }
}
