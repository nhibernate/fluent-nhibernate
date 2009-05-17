using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions
{
    /// <summary>
    /// Convention for identities, implement this interface to apply changes to
    /// identity mappings.
    /// </summary>
    public interface IIdConvention : IConvention<IIdentityInspector, IIdentityAlteration>
    {}
}