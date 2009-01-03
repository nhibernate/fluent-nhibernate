using FluentNHibernate.Testing.Cfg;
using NHibernate.Cfg;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMap
{
    public abstract class BaseAutoPersistenceTests
    {
        protected Configuration cfg;

        [SetUp]
        public void SetUp()
        {
            cfg = new Configuration();
            var configTester = new PersistenceConfigurationTester.ConfigTester();
            configTester.Dialect("NHibernate.Dialect.MsSql2005Dialect");
            configTester.ConfigureProperties(cfg);
        }
    }
}