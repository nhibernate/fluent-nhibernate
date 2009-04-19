using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg.Db
{
    public class OracleDataClientConfiguration : PersistenceConfiguration<OracleDataClientConfiguration, OracleConnectionStringBuilder>
    {
        protected OracleDataClientConfiguration()
        {
            Driver<OracleDataClientDriver>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OracleDataClientConfiguration"/> class using the
        /// Oracle Data Provider (Oracle.DataAccess) library.  The Oracle.DataAccess library must
        /// be available to the calling application/library.
        /// </summary>
        public static OracleDataClientConfiguration Oracle9
        {
            get { return new OracleDataClientConfiguration().Dialect<Oracle9Dialect>(); }
        }
    }
}
