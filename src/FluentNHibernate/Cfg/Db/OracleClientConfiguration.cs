using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg.Db
{
    public class OracleClientConfiguration : PersistenceConfiguration<OracleClientConfiguration, OracleConnectionStringBuilder>
    {
        protected OracleClientConfiguration()
        {
            Driver<OracleClientDriver>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OracleClientConfiguration"/> class using the
        /// MS Oracle Client (System.Data.OracleClient) library.
        /// </summary>
        public static OracleClientConfiguration Oracle9
        {
            get { return new OracleClientConfiguration().Dialect<Oracle9iDialect>(); }
        }
    }
}
