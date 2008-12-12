using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg
{
    using System.Text;

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

        public new PostgreSQLConnectionStringExpression ConnectionString
        {
            get
            {
                return new PostgreSQLConnectionStringExpression(this);
            }
        }

        public class PostgreSQLConnectionStringExpression : ConnectionStringExpression<PostgreSQLConfiguration>
        {
            private string _host;
            private int _port;
            private string _database;
            private string _username;
            private string _password;

            public PostgreSQLConnectionStringExpression(PostgreSQLConfiguration config)
                : base(config)
            {
            }

            public PostgreSQLConnectionStringExpression Host(string host)
            {
                _host = host;
                return this;
            }

            public PostgreSQLConnectionStringExpression Port(int port)
            {
                _port = port;
                return this;
            }

            public PostgreSQLConnectionStringExpression Database(string database)
            {
                _database = database;
                return this;
            }

            public PostgreSQLConnectionStringExpression Username(string username)
            {
                _username = username;
                return this;
            }

            public PostgreSQLConnectionStringExpression Password(string password)
            {
                _password = password;
                return this;
            }

            public PostgreSQLConfiguration Create
            {
                get
                {
                    //User ID=postgres;Password=roxy;Host=localhost;Port=5432;Database=hub;
                    var sb = new StringBuilder();

                    sb.AppendFormat("User Id={0};Password={1};Host={2};Port={3};Database={4};", _username, _password,
                                    _host, _port, _database);
                    

                    return _config.Raw(ConnectionStringKey, sb.ToString());
                }
            }
        }
	}
}
