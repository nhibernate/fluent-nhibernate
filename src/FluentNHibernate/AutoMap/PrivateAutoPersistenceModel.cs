namespace FluentNHibernate.AutoMap
{
    public class PrivateAutoPersistenceModel : AutoPersistenceModel
    {
        public PrivateAutoPersistenceModel()
        {
            autoMapper = new PrivateAutoMapper(Conventions);
        }
    }
}