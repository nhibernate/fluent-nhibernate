using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg.Db
{
    public class MsSqlConfiguration : PersistenceConfiguration<MsSqlConfiguration, MsSqlConnectionStringBuilder>
    {
        protected MsSqlConfiguration()
        {
            Driver<SqlClientDriver>();
        }

        public static MsSqlConfiguration MsSql7
        {
            get { return new MsSqlConfiguration().Dialect<MsSql7Dialect>(); }
        }

        public static MsSqlConfiguration MsSql2000
        {
            get { return new MsSqlConfiguration().Dialect<MsSql2000Dialect>(); }
        }

        public static MsSqlConfiguration MsSql2005
        {
            get { return new MsSqlConfiguration().Dialect<MsSql2005Dialect>(); }
        }

        public static MsSqlConfiguration MsSql2008
        {
            get { return new MsSqlConfiguration().Dialect<MsSql2008Dialect>(); }
        }
    }
}