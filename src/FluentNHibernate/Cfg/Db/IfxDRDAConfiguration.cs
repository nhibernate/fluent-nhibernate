using System;
using FluentNHibernate.Cfg.Db;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg.Db
{
    public class IfxDRDAConfiguration : PersistenceConfiguration<IfxDRDAConfiguration, IfxDRDAConnectionStringBuilder>
    {
        protected IfxDRDAConfiguration()
        {
            Driver<IfxDriver>();
        }

        public static IfxDRDAConfiguration Informix
        {
            get { return new IfxDRDAConfiguration().Dialect<InformixDialect>(); }
        }

        public static IfxDRDAConfiguration Informix0940
        {
            get { return new IfxDRDAConfiguration().Dialect<InformixDialect0940>(); }
        }

        public static IfxDRDAConfiguration Informix1000
        {
            get { return new IfxDRDAConfiguration().Dialect<InformixDialect1000>(); }
        }
    }
}
