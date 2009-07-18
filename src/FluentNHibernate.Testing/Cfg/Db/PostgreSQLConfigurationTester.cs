using FluentNHibernate.Cfg.Db;
using NHibernate;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Cfg.Db
{
    [TestFixture]
    public class PostgreSQLConfigurationTester
    {
        [Test]
        public void PostgreSQL_driver_should_default_to_the_Npgsql_driver()
        {
            PostgreSQLConfiguration.PostgreSQL81.ToProperties()["connection.driver_class"].ShouldEqual(
                "NHibernate.Driver.NpgsqlDriver, " + typeof(ISession).Assembly.FullName);
        }

        [Test]
        public void PostgreSQL_standard_should_set_the_correct_dialect()
        {
            PostgreSQLConfiguration.Standard.ToProperties()["dialect"].ShouldEqual(
                "NHibernate.Dialect.PostgreSQLDialect, " + typeof(ISession).Assembly.FullName);
        }

        [Test]
        public void PostgreSQL_81_should_set_the_correct_dialect()
        {
            PostgreSQLConfiguration.PostgreSQL81.ToProperties()["dialect"].ShouldEqual(
                "NHibernate.Dialect.PostgreSQL81Dialect, " + typeof(ISession).Assembly.FullName);
        }

        [Test]
        public void PostgreSQL_82_should_set_the_correct_dialect()
        {
            PostgreSQLConfiguration.PostgreSQL82.ToProperties()["dialect"].ShouldEqual(
                "NHibernate.Dialect.PostgreSQL82Dialect, " + typeof(ISession).Assembly.FullName);
        }

        [Test]
        public void ConnectionString_is_added_to_the_configuration()
        {
            PostgreSQLConfiguration.PostgreSQL82
                .ConnectionString(c => c
                    .Host("db-srv")
                    .Database("tables")
                    .Username("toni tester")
                    .Password("secret")
                    .Port(22))
                .ToProperties()["connection.connection_string"].ShouldEqual("User Id=toni tester;Password=secret;Host=db-srv;Port=22;Database=tables;");
        }

        [Test]
        public void ConnectionStringSetExplicitly()
        {
            PostgreSQLConfiguration.PostgreSQL82
                .ConnectionString(c => c
                    .Is("value"))
                .ToProperties().ShouldContain("connection.connection_string", "value");
        }

        [Test]
        public void ConnectionStringSetFromAppSetting()
        {
            PostgreSQLConfiguration.PostgreSQL82
                .ConnectionString(c => c
                    .FromAppSetting("connectionString"))
                .ToProperties().ShouldContain("connection.connection_string", "a-connection-string");
        }

        [Test]
        public void ConnectionStringSetFromConnectionStrings()
        {
            PostgreSQLConfiguration.PostgreSQL82
                .ConnectionString(c => c
                    .FromConnectionStringWithKey("main"))
                .ToProperties().ShouldContain("connection.connection_string", "connection string");
        }

        [Test]
        public void ShouldBeAbleToSpecifyConnectionStringDirectly()
        {
            PostgreSQLConfiguration.PostgreSQL82
                .ConnectionString("conn")
                .ToProperties().ShouldContain("connection.connection_string", "conn");
        }
    }
}