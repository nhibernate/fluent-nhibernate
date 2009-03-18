using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentNHibernate.Cfg.Db
{
    public class JetDriverConnectionStringBuilder : ConnectionStringBuilder
    {
        private string _databaseFile;
        private string _provider;
        private string _username;
        private string _password;

        public JetDriverConnectionStringBuilder()
        {
            // Default provider
            _provider = "Microsoft.Jet.OLEDB.4.0";
        }

        // Use Provider("Microsoft.ACE.OLEDB.12.0") for Access 2007 database
        public JetDriverConnectionStringBuilder Provider(string provider)
        {
            _provider = provider;
            IsDirty = true;
            return this;
        }

 
        public JetDriverConnectionStringBuilder DatabaseFile(string databaseFile)
        {
            _databaseFile = databaseFile;
            IsDirty = true;
            return this;
        }

        public JetDriverConnectionStringBuilder Username(string username)
        {
            _username = username;
            IsDirty = true;
            return this;
        }

        public JetDriverConnectionStringBuilder Password(string password)
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

                sb.AppendFormat("Provider={0};Data Source={1};User Id={2};Password={3};", 
                    _provider, _databaseFile,_username,_password);

            return sb.ToString();
        }
    }
}
