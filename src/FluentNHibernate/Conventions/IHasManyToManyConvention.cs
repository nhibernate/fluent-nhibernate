using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions
{
    public interface IHasManyToManyConvention : IConvention<IManyToManyCollectionInspector, IManyToManyCollectionAlteration>
    { }
}