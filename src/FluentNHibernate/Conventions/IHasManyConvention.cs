using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions
{
    public interface IHasManyConvention : IConvention<IOneToManyCollectionInspector, IOneToManyCollectionInstance>
    {}
}