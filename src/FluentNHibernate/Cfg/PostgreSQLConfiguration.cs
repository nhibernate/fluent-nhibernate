using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg
{
	public class PostgreSQLConfiguration : PersistenceConfiguration<PostgreSQLConfiguration>
	{
		protected PostgreSQLConfiguration()
		{
			Driver<NpgsqlDriver>();
		}

		public static PostgreSQLConfiguration Standard
		{
			get { return new PostgreSQLConfiguration().Dialect<PostgreSQLDialect>(); }
		}

		public static PostgreSQLConfiguration PostgreSQL81
		{
			get { return new PostgreSQLConfiguration().Dialect<PostgreSQL81Dialect>(); }
		}

		public static PostgreSQLConfiguration PostgreSQL82
		{
			get { return new PostgreSQLConfiguration().Dialect<PostgreSQL82Dialect>(); }
		}
	}
}