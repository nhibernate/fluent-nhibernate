using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentNHibernate.Cfg.Db
{
    public class JetDriverConfiguration : PersistenceConfiguration<JetDriverConfiguration,JetDriverConnectionStringBuilder>
    {
        protected JetDriverConfiguration()
        {
            Dialect("NHibernate.JetDriver.JetDialect, NHibernate.JetDriver");
            Driver("NHibernate.JetDriver.JetDriver, NHibernate.JetDriver");
        }

        public static JetDriverConfiguration Standard
        {
            get { return new JetDriverConfiguration(); }
        }
    }
}
