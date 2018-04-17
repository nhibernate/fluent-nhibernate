using FluentNHibernate.Cfg.Db;
using NHibernate;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Cfg.Db
{
    [TestFixture]
    public class MsSqlConfigurationTester
    {
        [Test]
        public void MsSql7_should_default_to_the_Sql7_dialect()
        {
            MsSqlConfiguration.MsSql7
                .ToProperties()
                .ShouldContain("dialect", "NHibernate.Dialect.MsSql7Dialect, " + typeof(ISession).Assembly.FullName);
        }

        [Test]
        public void MsSql2000_should_default_to_the_Sql2000_dialect()
        {
            MsSqlConfiguration.MsSql2000
                .ToProperties()
                .ShouldContain("dialect", "NHibernate.Dialect.MsSql2000Dialect, " + typeof(ISession).Assembly.FullName);
        }

        [Test]
        public void MsSql2005_should_default_to_the_Sql2005_dialect()
        {
            MsSqlConfiguration.MsSql2005
                .ToProperties()
                .ShouldContain("dialect", "NHibernate.Dialect.MsSql2005Dialect, " + typeof(ISession).Assembly.FullName);
        }

        [Test]
        public void MsSql2008_should_default_to_the_Sql2008_dialect()
        {
            MsSqlConfiguration.MsSql2008
                .ToProperties()
                .ShouldContain("dialect", "NHibernate.Dialect.MsSql2008Dialect, " + typeof(ISession).Assembly.FullName);
        }

        [Test]
        public void MsSql2012_should_default_to_thee_Sql2012_dialect()
        {
            MsSqlConfiguration.MsSql2012
                .ToProperties()
                .ShouldContain("dialect", "NHibernate.Dialect.MsSql2012Dialect, " + typeof(ISession).Assembly.FullName);
        }

        [Test]
        public void MsSql_driver_should_default_to_the_SqlClientDriver()
        {
            MsSqlConfiguration.MsSql2000
                .ToProperties()
                .ShouldContain("connection.driver_class", "NHibernate.Driver.SqlClientDriver, " + typeof(ISession).Assembly.FullName);
        }

        [Test]
        public void ConnectionString_is_added_to_the_configuration()
        {
            MsSqlConfiguration.MsSql2005
                .ConnectionString(c => c
                    .Server("db-srv")
                    .Database("tables")
                    .Username("toni tester")
                    .Password("secret"))
                .ToProperties().ShouldContain("connection.connection_string", "Data Source=db-srv;Initial Catalog=tables;Integrated Security=False;User ID=\"toni tester\";Password=secret");
        }

        [Test]
        public void ConnectionString_for_trustedConnection_is_added_to_the_configuration()
        {
            MsSqlConfiguration.MsSql2005
                .ConnectionString(c => c
                    .Server("db-srv")
                    .Database("tables")
                    .TrustedConnection())
                .ToProperties().ShouldContain("connection.connection_string" ,"Data Source=db-srv;Initial Catalog=tables;Integrated Security=True");
        }

        [Test]
        public void ConnectionStringSetExplicitly()
        {
            MsSqlConfiguration.MsSql2005
                .ConnectionString(c => c
                    .Is("value"))
                .ToProperties().ShouldContain("connection.connection_string", "value");
        }

#if NETFX
        [Test]
        public void ConnectionStringSetFromAppSetting()
        {
            MsSqlConfiguration.MsSql2005
                .ConnectionString(c => c
                    .FromAppSetting("connectionString"))
                .ToProperties().ShouldContain("connection.connection_string", "a-connection-string");
        }
#endif

#if NETFX
        [Test]
        public void ConnectionStringSetFromConnectionStrings()
        {
            MsSqlConfiguration.MsSql2005
                .ConnectionString(c => c
                    .FromConnectionStringWithKey("main"))
                .ToProperties().ShouldContain("connection.connection_string", "connection string");
        }
#endif

        [Test]
        public void ShouldBeAbleToSpecifyConnectionStringDirectly()
        {
            MsSqlConfiguration.MsSql2005
                .ConnectionString("conn")
                .ToProperties().ShouldContain("connection.connection_string", "conn");
        }
    }
}