using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg
{
    public class MySQLConfiguration : PersistenceConfiguration<MySQLConfiguration>
    {
        protected MySQLConfiguration()
        {
            Driver<MySqlDataDriver>();
        }

        public static MySQLConfiguration Standard
        {
            get
            {
                return new MySQLConfiguration().Dialect<MySQLDialect>();
            }
        }
    }
}

