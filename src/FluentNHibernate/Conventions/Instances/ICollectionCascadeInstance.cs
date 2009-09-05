namespace FluentNHibernate.Conventions.Instances
{
    public interface ICollectionCascadeInstance : ICascadeInstance
    {
        void AllDeleteOrphan();
        void DeleteOrphan();
    }
}