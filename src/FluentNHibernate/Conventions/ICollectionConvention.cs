using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions
{
    public interface ICollectionConvention : IConvention<ICollectionInspector, ICollectionInstance>
    {}
}