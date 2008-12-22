using System.Data.SqlClient;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg
{
	public class MsSqlConfiguration : PersistenceConfiguration<MsSqlConfiguration>
	{
        protected MsSqlConfiguration()
		{
			Driver<SqlClientDriver>();
		}

		public static MsSqlConfiguration MsSql7
		{
			get
			{
				return new MsSqlConfiguration().Dialect<MsSql7Dialect>();
			}
		}

		public static MsSqlConfiguration MsSql2000
		{
			get
			{
				return new MsSqlConfiguration().Dialect<MsSql2000Dialect>();
			}
		}

		public static MsSqlConfiguration MsSql2005
		{
			get
			{
				return new MsSqlConfiguration().Dialect<MsSql2005Dialect>();
			}
		}

	    public new MsSqlConnectionStringExpression ConnectionString
        {
	        get
            {
                return new MsSqlConnectionStringExpression(this);
            }
	    }

	    public class MsSqlConnectionStringExpression : ConnectionStringExpression<MsSqlConfiguration>
        {
            private string _server;
            private string _database;
            private string _username;
            private string _password;
	        private bool _trustedConnection;

	        public MsSqlConnectionStringExpression(MsSqlConfiguration config)
                : base(config)
            {
            }

            public MsSqlConnectionStringExpression Server(string server)
            {
                _server = server;
                return this;
            }

            public MsSqlConnectionStringExpression Database(string database)
            {
                _database = database;
                return this;
            }

            public MsSqlConnectionStringExpression Username(string username)
            {
                _username = username;
                return this;
            }

            public MsSqlConnectionStringExpression Password(string password)
            {
                _password = password;
                return this;
            }

            public MsSqlConnectionStringExpression TrustedConnection
            {
                get {
                    _trustedConnection = true;
                    return this;
                }
            }

            public MsSqlConfiguration Create
            {
                get {
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                    builder.DataSource = _server;
                    builder.InitialCatalog = _database;
                    builder.IntegratedSecurity = _trustedConnection;
                    if (!_trustedConnection) {
                        builder.UserID = _username;
                        builder.Password = _password;
                    }

                    return _config.Raw(ConnectionStringKey, builder.ToString());
                }
            }
        }
	}
}
