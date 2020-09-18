using System.Configuration;

namespace FluentNHibernate.Cfg.Db
{
    public class ConnectionStringBuilder
    {
        private string connectionString;

#if NETFX
        public ConnectionStringBuilder FromAppSetting(string appSettingKey)
        {
            connectionString = ConfigurationManager.AppSettings[appSettingKey];
            IsDirty = true;
            return this;
        }
#endif

#if NETFX
        public ConnectionStringBuilder FromConnectionStringWithKey(string connectionStringKey)
        {
            connectionString = ConfigurationManager.ConnectionStrings[connectionStringKey].ConnectionString;
            IsDirty = true;
            return this;
        }
#endif

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