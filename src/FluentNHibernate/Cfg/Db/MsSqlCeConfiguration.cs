using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg.Db;

public class MsSqlCeConfiguration : PersistenceConfiguration<MsSqlCeConfiguration>
{
    protected MsSqlCeConfiguration()
    {
        Driver<SqlServerCeDriver>();
    }

    public static MsSqlCeConfiguration Standard => new MsSqlCeConfiguration().Dialect<MsSqlCeDialect>();

    public static MsSqlCeConfiguration MsSqlCe40 => new MsSqlCeConfiguration().Dialect<MsSqlCe40Dialect>();
}
