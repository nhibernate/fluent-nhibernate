using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IKeyInstance : IKeyInspector
    {
        void ColumnName(string columnName);
    }
}