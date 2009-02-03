using System.Text;

namespace FluentNHibernate.Cfg.Db
{
    public class MySQLConnectionStringBuilder : ConnectionStringBuilder
    {
        private string _server;
        private string _database;
        private string _username;
        private string _password;

        public MySQLConnectionStringBuilder Server(string server)
        {
            _server = server;
            IsDirty = true;
            return this;
        }

        public MySQLConnectionStringBuilder Database(string database)
        {
            _database = database;
            IsDirty = true;
            return this;
        }

        public MySQLConnectionStringBuilder Username(string username)
        {
            _username = username;
            IsDirty = true;
            return this;
        }

        public MySQLConnectionStringBuilder Password(string password)
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

            sb.AppendFormat("Server={0};Database={1};User ID={2};Password={3}", _server, _database, _username, _password);

            return sb.ToString();
        }
    }
}