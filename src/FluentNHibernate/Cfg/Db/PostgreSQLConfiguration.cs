using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg.Db;

public class PostgreSQLConfiguration : PersistenceConfiguration<PostgreSQLConfiguration, PostgreSQLConnectionStringBuilder>
{
    protected PostgreSQLConfiguration()
    {
        Driver<NpgsqlDriver>();
    }

    public static PostgreSQLConfiguration Standard => new PostgreSQLConfiguration().Dialect<PostgreSQLDialect>();

    public static PostgreSQLConfiguration PostgreSQL81 => new PostgreSQLConfiguration().Dialect<PostgreSQL81Dialect>();

    public static PostgreSQLConfiguration PostgreSQL82 => new PostgreSQLConfiguration().Dialect<PostgreSQL82Dialect>();

    public static PostgreSQLConfiguration PostgreSQL83 => new PostgreSQLConfiguration().Dialect<PostgreSQL83Dialect>();
}
