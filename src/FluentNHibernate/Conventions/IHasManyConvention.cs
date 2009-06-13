using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions
{
    public interface IHasManyConvention : IConvention<IOneToManyCollectionInspector, IOneToManyCollectionAlteration>
    {}
}