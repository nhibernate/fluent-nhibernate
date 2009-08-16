using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions
{
    public interface IHasOneConventionAcceptance : IConventionAcceptance<IOneToOneInspector>
    {}
}