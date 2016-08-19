using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Cfg.Db
{
    [TestFixture]
    public class SqlAnywhereConfigurationTester
    {
        [Test]
        public void SqlAnywhere9_should_specify_SybaseAsa9_dialect()
        {
            SQLAnywhereConfiguration.SQLAnywhere9.ToProperties()["dialect"]
                .ShouldEqual("NHibernate.Dialect.SybaseASA9Dialect, " + typeof(ISession).Assembly.FullName);
        }

        [Test]
        public void SqlAnywhere10_should_specify_Sybase10_dialect()
        {
            SQLAnywhereConfiguration.SQLAnywhere10.ToProperties()["dialect"]
                .ShouldEqual("NHibernate.Dialect.SybaseSQLAnywhere10Dialect, " + typeof(ISession).Assembly.FullName);
        }

        [Test]
        public void SqlAnywhere11_should_specify_Sybase11_dialect()
        {
            SQLAnywhereConfiguration.SQLAnywhere11.ToProperties()["dialect"]
                .ShouldEqual("NHibernate.Dialect.SybaseSQLAnywhere11Dialect, " + typeof(ISession).Assembly.FullName);
        }

        [Test]
        public void SqlAnywhere12_should_specify_Sybase12_dialect_and_net40driver()
        {
            var properties = SQLAnywhereConfiguration.SQLAnywhere12.ToProperties();
            properties["dialect"].ShouldEqual("NHibernate.Dialect.SybaseSQLAnywhere12Dialect, " + typeof(ISession).Assembly.FullName);
            properties["connection.driver_class"].ShouldEqual("NHibernate.Driver.SybaseSQLAnywhereDotNet4Driver, " + typeof(ISession).Assembly.FullName);
        }
    }
}
