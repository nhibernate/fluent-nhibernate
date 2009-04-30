namespace FluentNHibernate.Mapping
{
    public interface ICollectionCascadeExpression : ICascadeExpression
    {
        void AllDeleteOrphan();
    }
}