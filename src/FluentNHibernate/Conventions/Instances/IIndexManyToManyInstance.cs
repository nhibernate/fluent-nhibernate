using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IIndexManyToManyInstance : IIndexInstanceBase, IIndexManyToManyInspector
    {
        void ForeignKey(string foreignKey);
    }
}
