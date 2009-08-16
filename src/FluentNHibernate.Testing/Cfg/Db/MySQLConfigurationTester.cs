using FluentNHibernate.Cfg.Db;
using NHibernate;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Cfg
{
    [TestFixture]
    public class MySQLConfigurationTester
    {
        [Test]
        public void MySQL_should_default_to_the_MySQL_dialect()
        {
            MySQLConfiguration.Standard.ToProperties()["dialect"].ShouldEqual("NHibernate.Dialect.MySQLDialect, " + typeof(ISession).Assembly.FullName);
        }

        [Test]
        public void MySQL_driver_should_default_to_the_MySQL_ClientDriver()
        {
            MySQLConfiguration.Standard.ToProperties()["connection.driver_class"].ShouldEqual("NHibernate.Driver.MySqlDataDriver, " + typeof(ISession).Assembly.FullName);
        }

        [Test]
        public void ConnectionString_is_added_to_the_configuration()
        {
            MySQLConfiguration.Standard
                .ConnectionString(c => c
                    .Server("db-srv")
                    .Database("tables")
                    .Username("toni tester")
                    .Password("secret"))
                .ToProperties()["connection.connection_string"].ShouldEqual("Server=db-srv;Database=tables;User ID=toni tester;Password=secret");
        }

        [Test]
        public void ConnectionStringSetExplicitly()
        {
            MySQLConfiguration.Standard
                .ConnectionString(c => c
                    .Is("value"))
                .ToProperties().ShouldContain("connection.connection_string", "value");
        }

        [Test]
        public void ConnectionStringSetFromAppSetting()
        {
            MySQLConfiguration.Standard
                .ConnectionString(c => c
                    .FromAppSetting("connectionString"))
                .ToProperties().ShouldContain("connection.connection_string", "a-connection-string");
        }

        [Test]
        public void ConnectionStringSetFromConnectionStrings()
        {
            MySQLConfiguration.Standard
                .ConnectionString(c => c
                    .FromConnectionStringWithKey("main"))
                .ToProperties().ShouldContain("connection.connection_string", "connection string");
        }

        [Test]
        public void ShouldBeAbleToSpecifyConnectionStringDirectly()
        {
            MySQLConfiguration.Standard
                .ConnectionString("conn")
                .ToProperties().ShouldContain("connection.connection_string", "conn");
        }
    }
}