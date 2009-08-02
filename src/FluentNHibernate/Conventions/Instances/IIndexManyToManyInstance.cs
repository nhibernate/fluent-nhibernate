using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IIndexManyToManyInstance : IIndexInstanceBase, IIndexManyToManyInspector
    {
        void SetForeignKey(string foreignKey);
    }
}
