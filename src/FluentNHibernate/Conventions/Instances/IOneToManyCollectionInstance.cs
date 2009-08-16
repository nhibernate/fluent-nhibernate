using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IOneToManyCollectionInstance : IOneToManyCollectionInspector, ICollectionInstance
    {
        new IOneToManyInstance Relationship { get; }

        new IOneToManyCollectionInstance Not { get; }
    }
}