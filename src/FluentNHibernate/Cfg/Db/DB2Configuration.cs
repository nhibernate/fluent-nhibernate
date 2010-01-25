using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg.Db
{
    public class DB2Configuration : PersistenceConfiguration<DB2Configuration, DB2ConnectionStringBuilder>
    {
        protected DB2Configuration()
        {
            Driver<DB2Driver>();
        }

        public static DB2Configuration Standard
        {
            get { return new DB2Configuration().Dialect<DB2Dialect>(); }
        }
    }
}