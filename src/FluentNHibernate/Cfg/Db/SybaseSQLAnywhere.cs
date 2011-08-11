using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg.Db
{
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

        public static SQLAnywhereConfiguration SQLAnywhere9
        {
            get { return new SQLAnywhereConfiguration().Dialect<SybaseASA9Dialect>(); }
        }

#if !NH21
        public static SQLAnywhereConfiguration SQLAnywhere10
        {
            get { return new SQLAnywhereConfiguration().Dialect<SybaseSQLAnywhere10Dialect>(); }
        }

        public static SQLAnywhereConfiguration SQLAnywhere11
        {
            get { return new SQLAnywhereConfiguration().Dialect<SybaseSQLAnywhere11Dialect>(); }
        }
#endif
    }
}