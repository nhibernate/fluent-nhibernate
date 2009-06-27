using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Alterations.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions
{
    public interface IHasManyToManyConvention : IConvention<IManyToManyCollectionInspector, IManyToManyCollectionAlteration, IManyToManyCollectionInstance>
    { }
}