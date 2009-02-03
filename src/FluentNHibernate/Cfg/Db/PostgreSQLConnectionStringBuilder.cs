using System.Text;

namespace FluentNHibernate.Cfg.Db
{
    public class PostgreSQLConnectionStringBuilder : ConnectionStringBuilder
    {
        private string _host;
        private int _port;
        private string _database;
        private string _username;
        private string _password;

        public PostgreSQLConnectionStringBuilder Host(string host)
        {
            _host = host;
            IsDirty = true;
            return this;
        }

        public PostgreSQLConnectionStringBuilder Port(int port)
        {
            _port = port;
            IsDirty = true;
            return this;
        }

        public PostgreSQLConnectionStringBuilder Database(string database)
        {
            _database = database;
            IsDirty = true;
            return this;
        }

        public PostgreSQLConnectionStringBuilder Username(string username)
        {
            _username = username;
            IsDirty = true;
            return this;
        }

        public PostgreSQLConnectionStringBuilder Password(string password)
        {
            _password = password;
            IsDirty = true;
            return this;
        }

        protected internal override string Create()
        {
            var connectionString = base.Create();

            if (!string.IsNullOrEmpty(connectionString))
                return connectionString;

            var sb = new StringBuilder();

            sb.AppendFormat("User Id={0};Password={1};Host={2};Port={3};Database={4};", _username, _password,
                _host, _port, _database);

            return sb.ToString();
        }
    }
}