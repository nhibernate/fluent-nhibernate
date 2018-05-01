using FluentNHibernate.Driver;
using NHibernate.Dialect;

namespace FluentNHibernate.Cfg.Db
{
    public class MsSqliteConfiguration : PersistenceConfiguration<MsSqliteConfiguration>
    {
        public static MsSqliteConfiguration Standard
        {
            get { return new MsSqliteConfiguration(); }
        }

        public MsSqliteConfiguration()
        {
            Driver<MsSQLiteDriver>();
            Dialect<SQLiteDialect>();
            Raw("query.substitutions", "true=1;false=0");
        }

        public MsSqliteConfiguration InMemory()
        {
            Raw("connection.release_mode", "on_close");

            return ConnectionString(c => c
                .Is("Data Source=:memory:"));
        }

        public MsSqliteConfiguration UsingFile(string fileName)
        {
            return ConnectionString(c => c
                .Is(string.Format("Data Source={0}", fileName)));
        }
    }

}