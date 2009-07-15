namespace FluentNHibernate.Conventions.Instances
{
    public interface IOptimisticLockInstance
    {
        void None();
        void Version();
        void Dirty();
        void All();
    }
}