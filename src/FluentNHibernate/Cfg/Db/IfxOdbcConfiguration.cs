using System;
using FluentNHibernate.Cfg.Db;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg.Db
{
    public class IfxOdbcConfiguration : PersistenceConfiguration<IfxOdbcConfiguration, OdbcConnectionStringBuilder>
    {
        protected IfxOdbcConfiguration()
        {
            Driver<OdbcDriver>();
        }

        public static IfxOdbcConfiguration Informix
        {
            get { return new IfxOdbcConfiguration().Dialect<InformixDialect>(); }
        }

        public static IfxOdbcConfiguration Informix0940
        {
            get { return new IfxOdbcConfiguration().Dialect<InformixDialect0940>(); }
        }

        public static IfxOdbcConfiguration Informix1000
        {
            get { return new IfxOdbcConfiguration().Dialect<InformixDialect1000>(); }
        }
    }
}
