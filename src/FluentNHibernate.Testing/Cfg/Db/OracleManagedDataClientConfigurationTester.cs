using FluentNHibernate.Cfg.Db;
using NHibernate.Dialect;
using NHibernate.Driver;
using NUnit.Framework;


namespace FluentNHibernate.Testing.Cfg.Db;

[TestFixture]
public class OracleManagedDataClientConfigurationTester
{
    [Test]
    public void Oracle9_should_default_to_the_Oracle9_dialect()
    {
        OracleManagedDataClientConfiguration.Oracle9
            .ToProperties()
            .ShouldContain("connection.driver_class", typeof(OracleManagedDataClientDriver).AssemblyQualifiedName)
            .ShouldContain("dialect", typeof(Oracle9iDialect).AssemblyQualifiedName);
    }


    [Test]
    public void Oracle10_should_default_to_the_Oracle10_dialect()
    {
        OracleManagedDataClientConfiguration.Oracle10
            .ToProperties()
            .ShouldContain("connection.driver_class", typeof(OracleManagedDataClientDriver).AssemblyQualifiedName)
            .ShouldContain("dialect", typeof(Oracle10gDialect).AssemblyQualifiedName);
    }

    [Test]
    public void ConnectionString_is_added_to_the_configuration()
    {
        OracleManagedDataClientConfiguration.Oracle9
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
    public void ConnectionString_leaving_out_the_StatementCacheSize_removes_from_string()
    {
        OracleManagedDataClientConfiguration.Oracle9
            .ConnectionString(c => c
                .Server("db-srv")
                .Instance("mydatabase")
                .Username("test")
                .Password("secret"))
            .ToProperties().ShouldContain("connection.connection_string",
                "User Id=test;Password=secret;Pooling=False;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=db-srv)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=mydatabase)))");
    }

    [Test]
    public void ConnectionString_pooling_defaults_to_false_when_not_set()
    {
        OracleManagedDataClientConfiguration.Oracle9
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
    public void ConnectionString_pooling_is_enabled_when_set()
    {
        OracleManagedDataClientConfiguration.Oracle9
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
        OracleManagedDataClientConfiguration.Oracle9
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
        OracleManagedDataClientConfiguration.Oracle9
            .ConnectionString(c => c
                .Is("value"))
            .ToProperties().ShouldContain("connection.connection_string", "value");
    }

#if NETFRAMEWORK
      [Test]
      public void ConnectionString_set_fromAppSetting()
      {
         OracleManagedDataClientConfiguration.Oracle9
            .ConnectionString(c => c
               .FromAppSetting("connectionString"))
            .ToProperties().ShouldContain("connection.connection_string", "a-connection-string");
      }
#endif

#if NETFRAMEWORK
      [Test]
      public void ConnectionString_set_fromConnectionStrings()
      {
         OracleManagedDataClientConfiguration.Oracle9
            .ConnectionString(c => c
               .FromConnectionStringWithKey("main"))
            .ToProperties().ShouldContain("connection.connection_string", "connection string");
      }
#endif

    [Test]
    public void ShouldBeAbleToSpecifyConnectionStringDirectly()
    {
        OracleManagedDataClientConfiguration.Oracle9
            .ConnectionString("conn")
            .ToProperties().ShouldContain("connection.connection_string", "conn");
    }
}
