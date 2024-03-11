using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg.Db;

public class MySQLConfiguration : PersistenceConfiguration<MySQLConfiguration, MySQLConnectionStringBuilder>
{
    protected MySQLConfiguration()
    {
        Driver<MySqlDataDriver>();
    }

    public static MySQLConfiguration Standard => new MySQLConfiguration().Dialect<MySQLDialect>();
}
