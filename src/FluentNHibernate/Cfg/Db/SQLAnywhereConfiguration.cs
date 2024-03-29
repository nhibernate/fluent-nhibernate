using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg.Db;

public class SQLAnywhereConfiguration : PersistenceConfiguration<SQLAnywhereConfiguration, SybaseSQLAnywhereConnectionStringBuilder>
{
    protected SQLAnywhereConfiguration()
    {
#if NH21
            Driver<ASA10ClientDriver>();
#else
        Driver<SybaseSQLAnywhereDriver>();
#endif
    }

    public static SQLAnywhereConfiguration SQLAnywhere9 => new SQLAnywhereConfiguration().Dialect<SybaseASA9Dialect>();

#if !NH21
    public static SQLAnywhereConfiguration SQLAnywhere10 => new SQLAnywhereConfiguration().Dialect<SybaseSQLAnywhere10Dialect>();

    public static SQLAnywhereConfiguration SQLAnywhere11 => new SQLAnywhereConfiguration().Dialect<SybaseSQLAnywhere11Dialect>();
#endif

    public static SQLAnywhereConfiguration SQLAnywhere12 => new SQLAnywhereConfiguration().Dialect<SybaseSQLAnywhere12Dialect>().Driver<SybaseSQLAnywhereDotNet4Driver>();

    public static SQLAnywhereConfiguration SQLAnywhere17 => new SQLAnywhereConfiguration().Dialect<SapSQLAnywhere17Dialect>().Driver<SapSQLAnywhere17Driver>();
}
