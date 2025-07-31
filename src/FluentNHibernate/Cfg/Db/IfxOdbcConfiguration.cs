using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg.Db;

public class IfxOdbcConfiguration : PersistenceConfiguration<IfxOdbcConfiguration, OdbcConnectionStringBuilder>
{
    protected IfxOdbcConfiguration()
    {
        Driver<OdbcDriver>();
    }

    public static IfxOdbcConfiguration Informix => new IfxOdbcConfiguration().Dialect<InformixDialect>();

    public static IfxOdbcConfiguration Informix0940 => new IfxOdbcConfiguration().Dialect<InformixDialect0940>();

    public static IfxOdbcConfiguration Informix1000 => new IfxOdbcConfiguration().Dialect<InformixDialect1000>();
}
