using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Alterations.Instances
{
    public interface IColumnInstance : IColumnInspector, IColumnAlteration
    {}
}