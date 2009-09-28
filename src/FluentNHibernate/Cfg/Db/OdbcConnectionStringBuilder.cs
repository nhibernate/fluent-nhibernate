namespace FluentNHibernate.Cfg.Db
{
    public class OdbcConnectionStringBuilder : ConnectionStringBuilder
    {
        private string dsn = "";
        private string username = "";
        private string password = "";
        private string otherOptions = "";

        public OdbcConnectionStringBuilder Dsn(string dsn)
        {
            this.dsn = dsn;
            IsDirty = true;
            return this;
        }

        public OdbcConnectionStringBuilder Username(string username)
        {
            this.username = username;
            IsDirty = true;
            return this;
        }

        public OdbcConnectionStringBuilder Password(string password)
        {
            this.password = password;
            IsDirty = true;
            return this;
        }

        public OdbcConnectionStringBuilder OtherOptions(string otherOptions)
        {
            this.otherOptions = otherOptions;
            IsDirty = true;
            return this;
        }

        protected internal override string Create()
        {
            var connectionString = base.Create();

            if (!string.IsNullOrEmpty(connectionString))
                return connectionString;

            if (this.username.Length > 0)
            {
                connectionString = string.Format(
                         "Uid={0};Pwd={1};DSN={2};{3}",
                         this.username, this.password, this.dsn, this.otherOptions);
            }
            else
            {
                connectionString = string.Format(
                         "DSN={0};{1}", this.dsn, this.otherOptions);
            }
            return connectionString;
        }
    }
}
