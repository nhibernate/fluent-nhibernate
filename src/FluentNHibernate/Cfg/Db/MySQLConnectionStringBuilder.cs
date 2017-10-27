using System.Text;

namespace FluentNHibernate.Cfg.Db
{
    public class MySQLConnectionStringBuilder : ConnectionStringBuilder
    {
        private string server;
        private ushort port;
        private string database;
        private string username;
        private string password;

        public MySQLConnectionStringBuilder Server(string server)
        {
            this.server = server;
            IsDirty = true;
            return this;
        }

        public MySQLConnectionStringBuilder Server(string[] servers)
        {
            this.server = string.Join(", ", servers);
            IsDirty = true;
            return this;
        }

        public MySQLConnectionStringBuilder Port(ushort port)
        {
            this.port = port;
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

            sb.AppendFormat("Server={0};Port={1};Database={2};User ID={3};Password={4}", server, port, database, username, password);

            return sb.ToString();
        }
    }
}