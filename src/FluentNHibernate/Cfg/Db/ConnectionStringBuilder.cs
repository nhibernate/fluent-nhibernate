using System.Configuration;

namespace FluentNHibernate.Cfg.Db
{
    public class ConnectionStringBuilder
    {
        private string connectionString;

        public ConnectionStringBuilder FromAppSetting(string appSettingKey)
        {
            connectionString = ConfigurationManager.AppSettings[appSettingKey];
            IsDirty = true;
            return this;
        }

        public ConnectionStringBuilder FromConnectionStringWithKey(string connectionStringKey)
        {
            connectionString = ConfigurationManager.ConnectionStrings[connectionStringKey].ConnectionString;
            IsDirty = true;
            return this;
        }

        public ConnectionStringBuilder Is(string rawConnectionString)
        {
            connectionString = rawConnectionString;
            IsDirty = true;
            return this;
        }

        protected internal bool IsDirty { get; set; }

        protected internal virtual string Create()
        {
            return connectionString;
        }
    }
}