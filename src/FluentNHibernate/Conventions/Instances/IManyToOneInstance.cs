using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IManyToOneInstance : IManyToOneInspector
    {
        void ColumnName(string columnName);
    }
}