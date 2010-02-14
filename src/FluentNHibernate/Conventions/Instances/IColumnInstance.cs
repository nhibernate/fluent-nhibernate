using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IColumnInstance : IColumnInspector
    {
        new void Length(int length);
    }
}