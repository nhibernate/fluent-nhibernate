using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions
{
    public interface IVersionConventionAcceptance : IConventionAcceptance<IVersionInspector>
    {}
}