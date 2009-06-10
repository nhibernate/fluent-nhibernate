using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions
{
    public interface IColumnConvention : IConvention<IColumnInspector, IColumnAlteration>
    {}
}