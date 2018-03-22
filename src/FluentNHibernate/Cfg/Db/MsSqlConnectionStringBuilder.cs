using System.Text;

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

            var sb = new StringBuilder();

            sb.AppendFormat("Data Source={0};Initial Catalog={1};Integrated Security={2}", server, database, trustedConnection);

            if (!trustedConnection)
                sb.AppendFormat(";User Id={0};Password={1}", username, password);

            return sb.ToString();
        }
    }
}