using System.Text;

namespace FluentNHibernate.Cfg.Db
{
    public class DB2ConnectionStringBuilder : ConnectionStringBuilder
    {
        private string server;
        private string database;
        private string username;
        private string password;

        public DB2ConnectionStringBuilder Server(string server)
        {
            this.server = server;
            IsDirty = true;
            return this;
        }

        public DB2ConnectionStringBuilder Database(string database)
        {
            this.database = database;
            IsDirty = true;
            return this;
        }

        public DB2ConnectionStringBuilder Username(string username)
        {
            this.username = username;
            IsDirty = true;
            return this;
        }

        public DB2ConnectionStringBuilder Password(string password)
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

            sb.AppendFormat("Server={0};Database={1};UID={2};PWD={3}", server, database, username, password);

            return sb.ToString();
        }
    }
}
