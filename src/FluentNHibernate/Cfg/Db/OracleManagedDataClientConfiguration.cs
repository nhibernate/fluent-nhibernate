using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg.Db
{
   public class OracleManagedDataClientConfiguration : PersistenceConfiguration<OracleManagedDataClientConfiguration, OracleConnectionStringBuilder>
   {
      protected OracleManagedDataClientConfiguration()
      {
         Driver<OracleManagedDataClientDriver>();
      }
      /// <summary>
      /// Initializes a new instance of the <see cref="FluentNHibernate.Cfg.Db.OracleDataClientConfiguration"/> class using the
      /// Oracle Data Provider (Oracle.DataAccess) library specifying the Oracle 9i dialect. 
      /// The Oracle.DataAccess library must be available to the calling application/library. 
      /// </summary>
      public static OracleManagedDataClientConfiguration Oracle9
      {
         get { return new OracleManagedDataClientConfiguration().Dialect<Oracle9iDialect>(); }
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="FluentNHibernate.Cfg.Db.OracleDataClientConfiguration"/> class using the
      /// Oracle Data Provider (Oracle.DataAccess) library specifying the Oracle 10g dialect. 
      /// The Oracle.DataAccess library must be available to the calling application/library. 
      /// </summary>
      public static OracleManagedDataClientConfiguration Oracle10
      {
         get { return new OracleManagedDataClientConfiguration().Dialect<Oracle10gDialect>(); }
      }
   }
}