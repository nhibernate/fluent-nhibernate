using FluentNHibernate.Dialects;
using FluentNHibernate.Driver;

namespace FluentNHibernate.Cfg.Db;

public class MsSqliteConfiguration : PersistenceConfiguration<MsSqliteConfiguration>
{
    public static MsSqliteConfiguration Standard => new();

    public MsSqliteConfiguration()
    {
        Driver<MsSQLiteDriver>();
        Dialect<MsSQLiteDialect>();
        Raw("query.substitutions", "true=1;false=0");
    }

    public MsSqliteConfiguration InMemory() =>
        ConnectionString(c => c.Is("Data Source=:memory:"))
            .Raw("connection.release_mode", "on_close");

    public MsSqliteConfiguration UsingFile(string fileName) =>
        ConnectionString(c => c.Is($"Data Source={fileName}"));
}
