namespace FluentNHibernate.AutoMap.Alterations
{
    public interface IAutoMappingAlteration
    {
        void Alter(AutoPersistenceModel model);
    }
}
