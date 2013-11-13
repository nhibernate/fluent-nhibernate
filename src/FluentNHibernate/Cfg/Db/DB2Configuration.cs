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
        
        /// <summary>
        /// DB2 Data Server/Client supports Informix 11.50+ with Informix syntax
        /// </summary>
        public static DB2Configuration Informix1150
        {
             get { return new DB2Configuration().Dialect<InformixDialect1000>(); }
        }
    }
}
