using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Alterations.Instances
{
    public interface IManyToManyCollectionInstance : IManyToManyCollectionInspector, IManyToManyCollectionAlteration
    {
        new IKeyInstance Key { get; }
        new IManyToManyInstance Relationship { get; }
    }
}