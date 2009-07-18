using System;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg.Db
{
    public class FirebirdConfiguration : PersistenceConfiguration<FirebirdConfiguration>
    {
        public FirebirdConfiguration()
        {
            Driver<FirebirdClientDriver>();
            Dialect<FirebirdDialect>();
            QuerySubstitutions("true 1, false 0, yes 1, no 0");
            Raw("connection.isolation", "ReadCommitted");
            Raw("command_timeout", "444");
            UseOuterJoin();
        }
    }
}