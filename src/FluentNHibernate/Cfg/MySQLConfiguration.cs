using System.Text;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace FluentNHibernate.Cfg
{
    public class MySQLConfiguration : PersistenceConfiguration<MySQLConfiguration>
    {
        protected MySQLConfiguration()
        {
            Driver<MySqlDataDriver>();
        }

        public static MySQLConfiguration Standard
        {
            get
            {
                return new MySQLConfiguration().Dialect<MySQLDialect>();
            }
        }

        public new MySQLConnectionStringExpression ConnectionString
        {
            get
            {
                return new MySQLConnectionStringExpression(this);
            }
        }

        public class MySQLConnectionStringExpression : ConnectionStringExpression<MySQLConfiguration>
        {
            private string _server;
            private string _database;
            private string _username;
            private string _password;

            public MySQLConnectionStringExpression(MySQLConfiguration config)
                : base(config)
            {
            }

            public MySQLConnectionStringExpression Server(string server)
            {
                _server = server;
                return this;
            }

            public MySQLConnectionStringExpression Database(string database)
            {
                _database = database;
                return this;
            }

            public MySQLConnectionStringExpression Username(string username)
            {
                _username = username;
                return this;
            }

            public MySQLConnectionStringExpression Password(string password)
            {
                _password = password;
                return this;
            }

            public MySQLConfiguration Create
            {
                get
                {
                    var sb = new StringBuilder();

                    sb.AppendFormat("Server={0};Database={1};User ID={2};Password={3}", _server, _database, _username, _password);

                    return _config.Raw(ConnectionStringKey, sb.ToString());
                }
            }
        }
    }
}

