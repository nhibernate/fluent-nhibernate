using System.Text;

namespace FluentNHibernate.Cfg.Db
{
    public class MySQLConnectionStringBuilder : ConnectionStringBuilder
    {
        private string server;
        private string database;
        private string username;
        private string password;

        public MySQLConnectionStringBuilder Server(string server)
        {
            this.server = server;
            IsDirty = true;
            return this;
        }

        public MySQLConnectionStringBuilder Database(string database)
        {
            this.database = database;
            IsDirty = true;
            return this;
        }

        public MySQLConnectionStringBuilder Username(string username)
        {
            this.username = username;
            IsDirty = true;
            return this;
        }

        public MySQLConnectionStringBuilder Password(string password)
        {
            this.password = password;
            IsDirty = true;
            return this;
        }

        protected internal override string Create()
        {
            var connectionString = base.Create();

            if (!string.IsNullOrEmpty(connectionString))
                return connectionString;

            var sb = new StringBuilder();

            sb.AppendFormat("Server={0};Database={1};User ID={2};Password={3}", server, database, username, password);

            return sb.ToString();
        }
    }
}