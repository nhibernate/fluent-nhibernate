using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg.Db;

public class IfxDRDAConfiguration : PersistenceConfiguration<IfxDRDAConfiguration, IfxDRDAConnectionStringBuilder>
{
    protected IfxDRDAConfiguration()
    {
        Driver<IfxDriver>();
    }

    public static IfxDRDAConfiguration Informix => new IfxDRDAConfiguration().Dialect<InformixDialect>();

    public static IfxDRDAConfiguration Informix0940 => new IfxDRDAConfiguration().Dialect<InformixDialect0940>();

    public static IfxDRDAConfiguration Informix1000 => new IfxDRDAConfiguration().Dialect<InformixDialect1000>();
}
