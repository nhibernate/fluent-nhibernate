using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IIndexManyToManyInstance : IIndexInstanceBase, IIndexManyToManyInspector
    {
        new void ForeignKey(string foreignKey);
    }
}
