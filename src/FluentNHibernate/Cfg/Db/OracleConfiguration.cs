using System;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg.Db
{
    [Obsolete("Replaced by OracleDataClientConfiguration. Also, for System.Data.OracleClient, use OracleClientConfiguration.")]
    public class OracleConfiguration : PersistenceConfiguration<OracleConfiguration, OracleConnectionStringBuilder>
    {
        protected OracleConfiguration()
        {
            Driver<OracleDataClientDriver>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OracleConfiguration"/> class using the
        /// Oracle Data Provider (Oracle.DataAccess) library specifying the Oracle 9i dialect.
        /// The Oracle.DataAccess library must be available to the calling application/library.
        /// </summary>
        public static OracleConfiguration Oracle9
        {
            get { return new OracleConfiguration().Dialect<Oracle9iDialect>(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OracleConfiguration"/> class using the
        /// Oracle Data Provider (Oracle.DataAccess) library specifying the Oracle 10g dialect.
        /// The Oracle.DataAccess library must be available to the calling application/library.
        /// This allows for ANSI join syntax.
        /// </summary>
        public static OracleConfiguration Oracle10
        {
            get { return new OracleConfiguration().Dialect<Oracle10gDialect>(); }
        }
    }
}
