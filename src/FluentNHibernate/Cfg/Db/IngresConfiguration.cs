using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg.Db
{
    public class IngresConfiguration : PersistenceConfiguration<IngresConfiguration, IngresConnectionStringBuilder>
    {
        protected IngresConfiguration()
        {
            Driver<IngresDriver>();
        }

        public static IngresConfiguration Standard
        {
            get { return new IngresConfiguration().Dialect<IngresDialect>(); }
        }
    }
}