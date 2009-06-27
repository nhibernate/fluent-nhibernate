using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Alterations.Instances
{
    public interface IOneToManyCollectionInstance : IOneToManyCollectionInspector, IOneToManyCollectionAlteration
    {
        new IKeyInstance Key { get; }
        new IOneToManyInstance OneToMany { get; }
    }
}