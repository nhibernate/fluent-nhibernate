using System.Data.SqlClient;

namespace FluentNHibernate.Cfg.Db
{
    public class MsSqlConnectionStringBuilder : ConnectionStringBuilder
    {
        private string _server;
        private string _database;
        private string _username;
        private string _password;
        private bool _trustedConnection;

        public MsSqlConnectionStringBuilder Server(string server)
        {
            _server = server;
            IsDirty = true;
            return this;
        }

        public MsSqlConnectionStringBuilder Database(string database)
        {
            _database = database;
            IsDirty = true;
            return this;
        }

        public MsSqlConnectionStringBuilder Username(string username)
        {
            _username = username;
            IsDirty = true;
            return this;
        }

        public MsSqlConnectionStringBuilder Password(string password)
        {
            _password = password;
            IsDirty = true;
            return this;
        }

        public MsSqlConnectionStringBuilder TrustedConnection()
        {
            _trustedConnection = true;
            IsDirty = true;
            return this;
        }

        protected internal override string Create()
        {
            var connectionString = base.Create();

            if (!string.IsNullOrEmpty(connectionString))
                return connectionString;

            var builder = new SqlConnectionStringBuilder(connectionString);

            builder.DataSource = _server;
            builder.InitialCatalog = _database;
            builder.IntegratedSecurity = _trustedConnection;

            if (!_trustedConnection)
            {
                builder.UserID = _username;
                builder.Password = _password;
            }

            return builder.ToString();
        }
    }
}