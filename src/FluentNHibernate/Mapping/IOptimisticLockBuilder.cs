namespace FluentNHibernate.Mapping
{
    public interface IOptimisticLockBuilder
    {
        void None();
        void Version();
        void Dirty();
        void All();
    }
}