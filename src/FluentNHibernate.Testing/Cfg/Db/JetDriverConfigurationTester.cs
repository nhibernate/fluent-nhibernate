using FluentNHibernate.Cfg.Db;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Cfg.Db
{
    [TestFixture]
    public class JetDriverConfigurationTester
    {
        [Test]
        public void Jet_driver_set_by_default()
        {
            JetDriverConfiguration.Standard.ToProperties()["connection.driver_class"].ShouldEqual(
                "NHibernate.JetDriver.JetDriver, NHibernate.JetDriver");
        }

        [Test]
        public void Jet_dialect_set_by_default()
        {
            JetDriverConfiguration.Standard.ToProperties()["dialect"].ShouldEqual(
                "NHibernate.JetDriver.JetDialect, NHibernate.JetDriver");
        }

        [Test]
        public void ConnectionString_is_added_to_the_configuration()
        {
            JetDriverConfiguration.Standard
                .ConnectionString(c => c
                    .DatabaseFile("database.mdb")
                    .Username("joe")
                    .Password("12345"))
                .ToProperties()["connection.connection_string"]
                    .ShouldEqual("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=database.mdb;User Id=joe;Password=12345;");
        }

        [Test]
        public void ConnectionString_with_explicit_provider_is_added_to_the_configuration()
        {
            JetDriverConfiguration.Standard
                .ConnectionString(c => c
                    .Provider("Microsoft.ACE.OLEDB.12.0")
                    .DatabaseFile("database.accdb")
                    .Username("")
                    .Password(""))
                .ToProperties()["connection.connection_string"]
                    .ShouldEqual("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=database.accdb;User Id=;Password=;");
        }

        [Test]
        public void ConnectionStringSetExplicitly()
        {
            JetDriverConfiguration.Standard
                .ConnectionString(c => c
                    .Is("value"))
                .ToProperties().ShouldContain("connection.connection_string", "value");
        }

        [Test]
        public void ConnectionStringSetFromAppSetting()
        {
            JetDriverConfiguration.Standard
                .ConnectionString(c => c
                    .FromAppSetting("connectionString"))
                .ToProperties().ShouldContain("connection.connection_string", "a-connection-string");
        }

        [Test]
        public void ConnectionStringSetFromConnectionStrings()
        {
            JetDriverConfiguration.Standard
                .ConnectionString(c => c
                    .FromConnectionStringWithKey("main"))
                .ToProperties().ShouldContain("connection.connection_string", "connection string");
        }

        [Test]
        public void ShouldBeAbleToSpecifyConnectionStringDirectly()
        {
            JetDriverConfiguration.Standard
                .ConnectionString("conn")
                .ToProperties().ShouldContain("connection.connection_string", "conn");
        }
    }
}
