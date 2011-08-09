using FluentNHibernate.Cfg.Db;
using NHibernate;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Cfg.Db
{
    [TestFixture]
    public class IngresConfigurationTester
    {
        [Test]
        public void Ingres_should_default_to_the_Ingres_dialect()
        {
            IngresConfiguration.Standard.ToProperties()["dialect"].ShouldEqual("NHibernate.Dialect.IngresDialect, " + typeof(ISession).Assembly.FullName);
        }

        [Test]
        public void Ingres_driver_should_default_to_the_Ingres_ClientDriver()
        {
            IngresConfiguration.Standard.ToProperties()["connection.driver_class"].ShouldEqual("NHibernate.Driver.IngresDriver, " + typeof(ISession).Assembly.FullName);
        }

        [Test]
        public void ConnectionString_is_added_to_the_configuration()
        {
            IngresConfiguration.Standard
                .ConnectionString(c => c
                    .Server("host")
                    .Database("vnode::db")
                    .Port("ii7")
                    .Username("tester")
                    .Password("secret"))
                .ToProperties()["connection.connection_string"].ShouldEqual("Host=host;Database=vnode::db;Port=ii7;User ID=tester;PWD=secret");
        }

        [Test]
        public void ConnectionStringSetExplicitly()
        {
            IngresConfiguration.Standard
                .ConnectionString(c => c
                    .Is("value"))
                .ToProperties().ShouldContain("connection.connection_string", "value");
        }
        
        [Test]
        public void ConnectionStringSetFromAppSetting()
        {
            IngresConfiguration.Standard
                .ConnectionString(c => c
                    .FromAppSetting("connectionString"))
                .ToProperties().ShouldContain("connection.connection_string", "a-connection-string");
        }

        [Test]
        public void ConnectionStringSetFromConnectionStrings()
        {
            IngresConfiguration.Standard
                .ConnectionString(c => c
                    .FromConnectionStringWithKey("main"))
                .ToProperties().ShouldContain("connection.connection_string", "connection string");
        }

        [Test]
        public void ShouldBeAbleToSpecifyConnectionStringDirectly()
        {
            IngresConfiguration.Standard
                .ConnectionString("conn")
                .ToProperties().ShouldContain("connection.connection_string", "conn");
        }
    }
}