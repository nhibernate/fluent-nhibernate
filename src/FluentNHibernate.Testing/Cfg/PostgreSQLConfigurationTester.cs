using FluentNHibernate.Cfg;
using NHibernate;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Cfg
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
                .ConnectionString
                    .Host("db-srv")
                    .Database("tables")
                    .Username("toni tester")
                    .Password("secret")
                    .Port(22)
                    .Create
                .ToProperties()["connection.connection_string"].ShouldEqual("User Id=toni tester;Password=secret;Host=db-srv;Port=22;Database=tables;");
        }
	}
}
