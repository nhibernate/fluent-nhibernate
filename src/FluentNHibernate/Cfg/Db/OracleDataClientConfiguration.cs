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
        /// Oracle Data Provider (Oracle.DataAccess) library specifying the Oracle 9i dialect. 
        /// The Oracle.DataAccess library must be available to the calling application/library. 
        /// </summary>
        public static OracleDataClientConfiguration Oracle9
        {
            get { return new OracleDataClientConfiguration().Dialect<Oracle9iDialect>(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OracleDataClientConfiguration"/> class using the
        /// Oracle Data Provider (Oracle.DataAccess) library specifying the Oracle 10g dialect. 
        /// The Oracle.DataAccess library must be available to the calling application/library. 
        /// </summary>
        public static OracleDataClientConfiguration Oracle10
        {
            get { return new OracleDataClientConfiguration().Dialect<Oracle10gDialect>(); }
        }
    }
}
