using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions
{
    public interface IIdConventionAcceptance : IConventionAcceptance<IIdentityInspector>
    {}
}