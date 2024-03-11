using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg.Db;

public class DB2400Configuration : PersistenceConfiguration<DB2400Configuration, DB2400ConnectionStringBuilder>
{
    protected DB2400Configuration()
    {
        Driver<DB2400Driver>();
    }

    public static DB2400Configuration Standard => new DB2400Configuration().Dialect<DB2400Dialect>();
}
