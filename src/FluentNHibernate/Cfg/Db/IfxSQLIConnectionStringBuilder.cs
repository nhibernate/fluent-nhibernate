namespace FluentNHibernate.Cfg.Db
{
    public class IfxSQLIConnectionStringBuilder : ConnectionStringBuilder
    {
        private string clientLocale = "";
        private string database = "";
        private string databaseLocale = "";
        private bool delimident = true;
        private string host = "";
        private string maxPoolSize = "";
        private string minPoolSize = "";
        private string password = "";
        private string pooling = "";
        private string server = "";
        private string service = "";
        private string username = "";
        private string otherOptions = "";

        /// <summary>
        /// Client locale, default value is en_us.CP1252 (Windows)
        /// </summary>
        /// <param name="clientLocale"></param>
        /// <returns>IfxSQLIConnectionStringBuilder object</returns>
        public IfxSQLIConnectionStringBuilder ClientLocale(string clientLocale)
        {
            this.clientLocale = clientLocale;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// The name of the database within the server instance.
        /// </summary>
        /// <param name="database"></param>
        /// <returns>IfxSQLIConnectionStringBuilder object</returns>
        public IfxSQLIConnectionStringBuilder Database(string database)
        {
            this.database = database;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// The language locale of the database. Default value is en_US.8859-1
        /// </summary>
        /// <param name="databaseLocale"></param>
        /// <returns>IfxSQLIConnectionStringBuilder object</returns>
        public IfxSQLIConnectionStringBuilder DatabaseLocale(string databaseLocale)
        {
            this.databaseLocale = databaseLocale;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// When set to true or y for yes, any string within double
        /// quotes (") is treated as an identifier, and any string within
        /// single quotes (') is treated as a string literal. Default value 'y'.
        /// </summary>
        /// <param name="delimident"></param>
        /// <returns>IfxSQLIConnectionStringBuilder object</returns>
        public IfxSQLIConnectionStringBuilder Delimident(bool delimident)
        {
            this.delimident = delimident;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// The name or IP address of the machine on which the
        /// Informix server is running. Required.
        /// </summary>
        /// <param name="host"></param>
        /// <returns>IfxSQLIConnectionStringBuilder object</returns>
        public IfxSQLIConnectionStringBuilder Host(string host)
        {
            this.host = host;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// The maximum number of connections allowed in the pool. Default value 100.
        /// </summary>
        /// <param name="maxPoolSize"></param>
        /// <returns>IfxSQLIConnectionStringBuilder object</returns>
        public IfxSQLIConnectionStringBuilder MaxPoolSize(string maxPoolSize)
        {
            this.maxPoolSize = maxPoolSize;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// The minimum number of connections allowed in the pool. Default value 0.
        /// </summary>
        /// <param name="minPoolSize"></param>
        /// <returns>IfxSQLIConnectionStringBuilder object</returns>
        public IfxSQLIConnectionStringBuilder MinPoolSize(string minPoolSize)
        {
            this.minPoolSize = minPoolSize;
            IsDirty = true;
            return this;
        }
        /// <summary>
        /// The password associated with the User ID. Required if the
        /// client machine or user account is not trusted by the host.
        /// Prohibited if a User ID is not given.
        /// </summary>
        /// <param name="password"></param>
        /// <returns>IfxSQLIConnectionStringBuilder object</returns>
        public IfxSQLIConnectionStringBuilder Password(string password)
        {
            this.password = password;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// When set to true, the IfxConnection object is drawn from
        /// the appropriate pool, or if necessary, it is created and added
        /// to the appropriate pool. Default value 'true'.
        /// </summary>
        /// <param name="pooling"></param>
        /// <returns>IfxSQLIConnectionStringBuilder object</returns>
        public IfxSQLIConnectionStringBuilder Pooling(string pooling)
        {
            this.pooling = pooling;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// The name or alias of the instance of the Informix server to
        /// which to connect. Required.
        /// </summary>
        /// <param name="server"></param>
        /// <returns>IfxSQLIConnectionStringBuilder object</returns>
        public IfxSQLIConnectionStringBuilder Server(string server)
        {
            this.server = server;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// The service name or port number through which the server
        /// is listening for connection requests.
        /// </summary>
        /// <param name="service"></param>
        /// <returns>IfxSQLIConnectionStringBuilder object</returns>
        public IfxSQLIConnectionStringBuilder Service(string service)
        {
            this.service = service;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// The login account. Required, unless the client machine is
        /// trusted by the host machine.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>IfxSQLIConnectionStringBuilder object</returns>
        public IfxSQLIConnectionStringBuilder Username(string username)
        {
            this.username = username;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// Other options like: Connection Lifetime, Enlist, Exclusive, Optimize OpenFetchClose,
        /// Fetch Buffer Size, Persist Security Info, Protocol, Single Threaded, Skip Parsing
        /// </summary>
        /// <param name="otherOptions"></param>
        /// <returns>IfxSQLIConnectionStringBuilder object</returns>
        public IfxSQLIConnectionStringBuilder OtherOptions(string otherOptions)
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

            if (this.clientLocale.Length > 0)
            {
                connectionString += string.Format("Client_Locale={0};", this.clientLocale);
            }
            if (this.database.Length > 0)
            {
                connectionString += string.Format("DB={0};", this.database);
            }
            if (this.databaseLocale.Length > 0)
            {
                connectionString += string.Format("DB_LOCALE={0};", this.databaseLocale);
            }
            if (this.delimident != true)
            {
                connectionString += "DELIMIDENT='n'";
            }
            if (this.host.Length > 0)
            {
                connectionString += string.Format("Host={0};", this.host);
            }
            if (this.maxPoolSize.Length > 0)
            {
                connectionString += string.Format("Max Pool Size={0};", this.maxPoolSize);
            }
            if (this.minPoolSize.Length > 0)
            {
                connectionString += string.Format("Min Pool Size={0};", this.minPoolSize);
            }
            if (this.password.Length > 0)
            {
                connectionString += string.Format("PWD={0};", this.password);
            }
            if (this.pooling.Length > 0)
            {
                connectionString += string.Format("Pooling={0};", this.pooling);
            }
            if (this.server.Length > 0)
            {
                connectionString += string.Format("Server={0};", this.server);
            }
            if (this.service.Length > 0)
            {
                connectionString += string.Format("Service={0};", this.service);
            }
            if (this.username.Length > 0)
            {
                connectionString += string.Format("UID={0};", this.username);
            }
            if (this.otherOptions.Length > 0)
            {
                connectionString += this.otherOptions;
            }

            return connectionString;
        }
    }
}
