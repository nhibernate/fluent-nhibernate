using FluentNHibernate.Cfg.Db;
using NHibernate;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Cfg.Db
{
    [TestFixture]
    public class DB2400ConfigurationTester
    {
        [Test]
        public void DB2400_should_default_to_the_DB2400_dialect()
        {
            DB2400Configuration.Standard.ToProperties()["dialect"].ShouldEqual("NHibernate.Dialect.DB2400Dialect, " + typeof(ISession).Assembly.FullName);
        }

        [Test]
        public void DB2400_driver_should_default_to_the_DB2400_ClientDriver()
        {
            DB2400Configuration.Standard.ToProperties()["connection.driver_class"].ShouldEqual("NHibernate.Driver.DB2400Driver, " + typeof(ISession).Assembly.FullName);
        }

        [Test]
        public void ConnectionString_is_added_to_the_configuration()
        {
            DB2400Configuration.Standard
                .ConnectionString(c => c
                    .DataSource("db2400-srv")
                    .UserID("toni tester")
                    .Password("secret"))
                .ToProperties()["connection.connection_string"].ShouldEqual("DataSource=db2400-srv;UserID=toni tester;Password=secret;DataCompression=True;");
        }

        [Test]
        public void ConnectionStringSetExplicitly()
        {
            DB2400Configuration.Standard
                .ConnectionString(c => c
                    .Is("value"))
                .ToProperties().ShouldContain("connection.connection_string", "value");
        }

        [Test]
        public void ConnectionStringSetFromAppSetting()
        {
            DB2400Configuration.Standard
                .ConnectionString(c => c
                    .FromAppSetting("connectionString"))
                .ToProperties().ShouldContain("connection.connection_string", "a-connection-string");
        }

        [Test]
        public void ConnectionStringSetFromConnectionStrings()
        {
            DB2400Configuration.Standard
                .ConnectionString(c => c
                    .FromConnectionStringWithKey("main"))
                .ToProperties().ShouldContain("connection.connection_string", "connection string");
        }

        [Test]
        public void ShouldBeAbleToSpecifyConnectionStringDirectly()
        {
            DB2400Configuration.Standard
                .ConnectionString("conn")
                .ToProperties().ShouldContain("connection.connection_string", "conn");
        }
    }
}