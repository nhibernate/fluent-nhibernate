using System;
using FluentNHibernate.Cfg.Db;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg.Db
{
    public class IfxSQLIConfiguration : PersistenceConfiguration<IfxSQLIConfiguration, IfxSQLIConnectionStringBuilder>
    {
        protected IfxSQLIConfiguration()
        {
            Driver<IfxDriver>();
        }

        public static IfxSQLIConfiguration Informix
        {
            get { return new IfxSQLIConfiguration().Dialect<InformixDialect>(); }
        }

        public static IfxSQLIConfiguration Informix0940
        {
            get { return new IfxSQLIConfiguration().Dialect<InformixDialect0940>(); }
        }

        public static IfxSQLIConfiguration Informix1000
        {
            get { return new IfxSQLIConfiguration().Dialect<InformixDialect1000>(); }
        }
    }
}
