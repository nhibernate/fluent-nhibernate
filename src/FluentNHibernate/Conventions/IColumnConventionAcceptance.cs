using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions
{
    public interface IColumnConventionAcceptance : IConventionAcceptance<IColumnInspector>
    {}
}