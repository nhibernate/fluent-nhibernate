using System.Data.SqlClient;

namespace FluentNHibernate.Cfg.Db
{
    public class MsSqlConnectionStringBuilder : ConnectionStringBuilder
    {
        private string server;
        private string database;
        private string username;
        private string password;
        private bool trustedConnection;

        public MsSqlConnectionStringBuilder Server(string server)
        {
            this.server = server;
            IsDirty = true;
            return this;
        }

        public MsSqlConnectionStringBuilder Database(string database)
        {
            this.database = database;
            IsDirty = true;
            return this;
        }

        public MsSqlConnectionStringBuilder Username(string username)
        {
            this.username = username;
            IsDirty = true;
            return this;
        }

        public MsSqlConnectionStringBuilder Password(string password)
        {
            this.password = password;
            IsDirty = true;
            return this;
        }

        public MsSqlConnectionStringBuilder TrustedConnection()
        {
            trustedConnection = true;
            IsDirty = true;
            return this;
        }

        protected internal override string Create()
        {
            var connectionString = base.Create();

            if (!string.IsNullOrEmpty(connectionString))
                return connectionString;

            var builder = new SqlConnectionStringBuilder(connectionString)
            {
                DataSource = server,
                InitialCatalog = database,
                IntegratedSecurity = trustedConnection
            };

            if (!trustedConnection)
            {
                builder.UserID = username;
                builder.Password = password;
            }

            return builder.ToString();
        }
    }
}