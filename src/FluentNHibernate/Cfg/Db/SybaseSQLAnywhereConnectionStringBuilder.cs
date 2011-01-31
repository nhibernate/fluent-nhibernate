using System.Data.SqlClient;
using System.Text;

namespace FluentNHibernate.Cfg.Db
{
    public class SybaseSQLAnywhereConnectionStringBuilder : ConnectionStringBuilder
    {
        private string server;
        private string links;
        private string username;
        private string password; 

        public SybaseSQLAnywhereConnectionStringBuilder Server(string server)
        {
            this.server = server;
            IsDirty = true;
            return this;
        }

        public SybaseSQLAnywhereConnectionStringBuilder Links(string links)
        {
            this.links = links;
            IsDirty = true;
            return this;
        }

        public SybaseSQLAnywhereConnectionStringBuilder Username(string username)
        {
            this.username = username;
            IsDirty = true;
            return this;
        }

        public SybaseSQLAnywhereConnectionStringBuilder Password(string password)
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
                             
            sb.AppendFormat("uid={0};pwd={1};ServerName={2};links={3};", username, password,
                server, links );

            return sb.ToString();
        }
    }
}