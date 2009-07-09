using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IIdentityInstance : IIdentityInspector
    {
        void ColumnName(string column);
    }
}