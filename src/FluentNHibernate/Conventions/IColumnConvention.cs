using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions
{
    public interface IColumnConvention : IConvention<IColumnInspector, IColumnInstance>
    {}
}