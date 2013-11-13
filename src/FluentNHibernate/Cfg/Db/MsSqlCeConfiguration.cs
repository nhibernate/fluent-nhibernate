using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg.Db
{
    public class MsSqlCeConfiguration : PersistenceConfiguration<MsSqlCeConfiguration>
    {
        protected MsSqlCeConfiguration()
        {
            Driver<SqlServerCeDriver>();
        }

        public static MsSqlCeConfiguration Standard
        {
            get { return new MsSqlCeConfiguration().Dialect<MsSqlCeDialect>(); }
        }

        public static MsSqlCeConfiguration MsSqlCe40
        {
            get { return new MsSqlCeConfiguration().Dialect<MsSqlCe40Dialect>(); }
        }
    }
}