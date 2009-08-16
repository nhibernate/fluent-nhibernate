using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions
{
    public interface IHasManyToManyConvention : IConvention<IManyToManyCollectionInspector, IManyToManyCollectionInstance>
    { }
}