using FluentNHibernate.Cfg.Db;
using NHibernate;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Cfg
{
    [TestFixture]
    public class DB2ConfigurationTester
    {
        [Test]
        public void DB2_should_default_to_the_DB2_dialect()
        {
            DB2Configuration.Standard.ToProperties()["dialect"].ShouldEqual("NHibernate.Dialect.DB2Dialect, " + typeof(ISession).Assembly.FullName);
        }

        [Test]
        public void DB2_driver_should_default_to_the_DB2_ClientDriver()
        {
            DB2Configuration.Standard.ToProperties()["connection.driver_class"].ShouldEqual("NHibernate.Driver.DB2Driver, " + typeof(ISession).Assembly.FullName);
        }

        [Test]
        public void ConnectionString_is_added_to_the_configuration()
        {
            DB2Configuration.Standard
                .ConnectionString(c => c
                    .Server("db-srv")
                    .Database("tables")
                    .Username("toni tester")
                    .Password("secret"))
                .ToProperties()["connection.connection_string"].ShouldEqual("Server=db-srv;Database=tables;UID=toni tester;PWD=secret");
        }

        [Test]
        public void ConnectionStringSetExplicitly()
        {
            DB2Configuration.Standard
                .ConnectionString(c => c
                    .Is("value"))
                .ToProperties().ShouldContain("connection.connection_string", "value");
        }

        [Test]
        public void ConnectionStringSetFromAppSetting()
        {
            DB2Configuration.Standard
                .ConnectionString(c => c
                    .FromAppSetting("connectionString"))
                .ToProperties().ShouldContain("connection.connection_string", "a-connection-string");
        }

        [Test]
        public void ConnectionStringSetFromConnectionStrings()
        {
            DB2Configuration.Standard
                .ConnectionString(c => c
                    .FromConnectionStringWithKey("main"))
                .ToProperties().ShouldContain("connection.connection_string", "connection string");
        }

        [Test]
        public void ShouldBeAbleToSpecifyConnectionStringDirectly()
        {
            DB2Configuration.Standard
                .ConnectionString("conn")
                .ToProperties().ShouldContain("connection.connection_string", "conn");
        }
    }
}