using FluentNHibernate.Cfg.Db;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Cfg.Db;

[TestFixture]
public class ConnectionStringBuilderTester
{
    private ConnectionStringBuilderDouble builder;

    [SetUp]
    public void CreateBuilder()
    {
        builder = new ConnectionStringBuilderDouble();
    }

    [Test]
    public void CanExplicitlySetConnectionString()
    {
        builder.Is("a string");
        builder.ConnectionString.ShouldEqual("a string");
    }

#if NETFRAMEWORK
    [Test]
    public void ConnectionStringSetFromAppSetting()
    {
        builder.FromAppSetting("connectionString");
        builder.ConnectionString.ShouldContain("a-connection-string");
    }
#endif

#if NETFRAMEWORK
    [Test]
    public void ConnectionStringSetFromConnectionStrings()
    {
        builder.FromConnectionStringWithKey("main");
        builder.ConnectionString.ShouldContain("connection string");
    }
#endif

    private class ConnectionStringBuilderDouble : ConnectionStringBuilder
    {
        public string ConnectionString => Create();
    }
}
