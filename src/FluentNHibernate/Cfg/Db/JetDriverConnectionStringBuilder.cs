using System.Text;

namespace FluentNHibernate.Cfg.Db
{
    public class JetDriverConnectionStringBuilder : ConnectionStringBuilder
    {
        private string databaseFile;
        private string provider;
        private string username;
        private string password;

        public JetDriverConnectionStringBuilder()
        {
            // Default provider
            provider = "Microsoft.Jet.OLEDB.4.0";
        }

        // Use Provider("Microsoft.ACE.OLEDB.12.0") for Access 2007 database
        public JetDriverConnectionStringBuilder Provider(string provider)
        {
            this.provider = provider;
            IsDirty = true;
            return this;
        }

 
        public JetDriverConnectionStringBuilder DatabaseFile(string databaseFile)
        {
            this.databaseFile = databaseFile;
            IsDirty = true;
            return this;
        }

        public JetDriverConnectionStringBuilder Username(string username)
        {
            this.username = username;
            IsDirty = true;
            return this;
        }

        public JetDriverConnectionStringBuilder Password(string password)
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

                sb.AppendFormat("Provider={0};Data Source={1};User Id={2};Password={3};", 
                    provider, databaseFile,username,password);

            return sb.ToString();
        }
    }
}
