using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg.Db;

public class MsSqlConfiguration : PersistenceConfiguration<MsSqlConfiguration, MsSqlConnectionStringBuilder>
{
    protected MsSqlConfiguration()
    {
        Driver<SqlClientDriver>();
    }

    public static MsSqlConfiguration MsSql7 => new MsSqlConfiguration().Dialect<MsSql7Dialect>();

    public static MsSqlConfiguration MsSql2000 => new MsSqlConfiguration().Dialect<MsSql2000Dialect>();

    public static MsSqlConfiguration MsSql2005 => new MsSqlConfiguration().Dialect<MsSql2005Dialect>();

    public static MsSqlConfiguration MsSql2008 => new MsSqlConfiguration().Dialect<MsSql2008Dialect>();

    public static MsSqlConfiguration MsSql2012 => new MsSqlConfiguration().Dialect<MsSql2012Dialect>();
}
