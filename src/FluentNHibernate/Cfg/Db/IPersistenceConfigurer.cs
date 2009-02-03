using NHibernate.Cfg;

namespace FluentNHibernate.Cfg.Db
{
    public interface IPersistenceConfigurer
    {
        Configuration ConfigureProperties(Configuration nhibernateConfig);
    }
}