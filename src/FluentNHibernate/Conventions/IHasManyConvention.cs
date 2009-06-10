using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions
{
    /// <summary>
    /// HasMany convention, used for applying changes to one-to-many relationships.
    /// </summary>
    public interface IHasManyConvention : IConvention<IOneToManyInspector, IOneToManyAlteration>
    { }
}