using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IColumnInstance : IColumnInspector
    {
        void Length(int length);
    }
}