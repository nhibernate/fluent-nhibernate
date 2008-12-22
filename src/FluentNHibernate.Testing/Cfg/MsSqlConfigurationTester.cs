using FluentNHibernate.Cfg;
using NHibernate;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Cfg
{
	[TestFixture]
	public class MsSqlConfigurationTester
	{
		[Test]
		public void MsSql7_should_default_to_the_Sql7_dialect()
		{
			MsSqlConfiguration.MsSql7.ToProperties()["dialect"].ShouldEqual("NHibernate.Dialect.MsSql7Dialect, " + typeof(ISession).Assembly.FullName);
		}

		[Test]
		public void MsSql2000_should_default_to_the_Sql2000_dialect()
		{
			MsSqlConfiguration.MsSql2000.ToProperties()["dialect"].ShouldEqual("NHibernate.Dialect.MsSql2000Dialect, " + typeof(ISession).Assembly.FullName);
		}

		[Test]
		public void MsSql2005_should_default_to_the_Sql2005_dialect()
		{
			MsSqlConfiguration.MsSql2005.ToProperties()["dialect"].ShouldEqual("NHibernate.Dialect.MsSql2005Dialect, " + typeof(ISession).Assembly.FullName);
		}

		[Test]
		public void MsSql_driver_should_default_to_the_SqlClientDriver()
		{
			MsSqlConfiguration.MsSql2000.ToProperties()["connection.driver_class"].ShouldEqual("NHibernate.Driver.SqlClientDriver, " + typeof(ISession).Assembly.FullName);
		}

	    [Test]
	    public void ConnectionString_is_added_to_the_configuration()
        {
	        MsSqlConfiguration.MsSql2005
                .ConnectionString
                    .Server("db-srv")
                    .Database("tables")
                    .Username("toni tester")
                    .Password("secret")
                    .Create
                .ToProperties()["connection.connection_string"].ShouldEqual("Data Source=db-srv;Initial Catalog=tables;Integrated Security=False;User ID=\"toni tester\";Password=secret");
	    }

        [Test]
        public void ConnectionString_for_trustedConnection_is_added_to_the_configuration() {
            MsSqlConfiguration.MsSql2005
                .ConnectionString
                    .Server("db-srv")
                    .Database("tables")
                    .TrustedConnection
                    .Create
                .ToProperties()["connection.connection_string"].ShouldEqual("Data Source=db-srv;Initial Catalog=tables;Integrated Security=True");
        }
    }
}
