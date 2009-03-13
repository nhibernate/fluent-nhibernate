using FluentNHibernate.Cfg.Db;
using NHibernate;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Cfg.Db
{
    [TestFixture]
    public class OracleConfigurationTester
    {
        [Test]
        public void Oracle9_should_default_to_the_Oracle9_dialect()
        {
            OracleConfiguration.Oracle9
                .ToProperties()
                .ShouldContain("dialect", "NHibernate.Dialect.Oracle9Dialect, " + typeof(ISession).Assembly.FullName);
        }

        [Test]
        public void ConnectionString_is_added_to_the_configuration()
        {
            OracleConfiguration.Oracle9
                .ConnectionString(c => c
                                           .Server("db-srv")
                                           .Instance("mydatabase")
                                           .Username("test")
                                           .Password("secret")
                                           .StatementCacheSize(50))
                .ToProperties().ShouldContain("connection.connection_string",
                                              "User Id=test;Password=secret;Pooling=False;Statement Cache Size=50;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=db-srv)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=mydatabase)))");
        }

        [Test]
        public void ConnectionString_pooling_is_enabled_when_set()
        {
            OracleConfiguration.Oracle9
                .ConnectionString(c => c
                    .Server("db-srv")
                    .Instance("mydatabase")
                    .Username("test")
                    .Password("secret")
                    .Pooling(true)
                    .StatementCacheSize(50))
                .ToProperties().ShouldContain("connection.connection_string",
                                              "User Id=test;Password=secret;Pooling=True;Statement Cache Size=50;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=db-srv)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=mydatabase)))");
        }
        [Test]
        public void ConnectionString_other_options_are_enabled_and_parsed_when_set()
        {
            OracleConfiguration.Oracle9
                .ConnectionString(c => c
                    .Server("db-srv")
                    .Instance("mydatabase")
                    .Username("test")
                    .Password("secret")
                    .OtherOptions("Min Pool Size=10;Incr Pool Size=5;Decr Pool Size=2")
                    .StatementCacheSize(50))
                .ToProperties().ShouldContain("connection.connection_string",
                                              "User Id=test;Password=secret;Pooling=False;Statement Cache Size=50;Min Pool Size=10;Incr Pool Size=5;Decr Pool Size=2;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=db-srv)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=mydatabase)))");
        }

        [Test]
        public void ConnectionString_set_explicitly()
        {
            OracleConfiguration.Oracle9
                .ConnectionString(c => c
                    .Is("value"))
                .ToProperties().ShouldContain("connection.connection_string", "value");
        }

        [Test]
        public void ConnectionString_set_fromAppSetting()
        {
            OracleConfiguration.Oracle9
                .ConnectionString(c => c
                    .FromAppSetting("connectionString"))
                .ToProperties().ShouldContain("connection.connection_string", "a-connection-string");
        }

        [Test]
        public void ConnectionString_set_fromConnectionStrings()
        {
            OracleConfiguration.Oracle9
                .ConnectionString(c => c
                    .FromConnectionStringWithKey("main"))
                .ToProperties().ShouldContain("connection.connection_string", "connection string");
        }
    }
}
