using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IIndexManyToManyInstance : IIndexManyToManyInspector
    {
        void Column(string columnName);
        void SetForeignKey(string foreignKey);
    }
}
