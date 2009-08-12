namespace FluentNHibernate.Cfg.Db
{
    public class OracleConnectionStringBuilder : ConnectionStringBuilder
    {
        private string instance;
        private string otherOptions;
        private string password;
        private int port;
        private string server;
        private string username;
        private bool pooling;
        private string statementCacheSize;

        public OracleConnectionStringBuilder()
        {
            // Port is pre-slugged as 1521 is the default Oracle port.
            port = 1521;
        }

        /// <summary>
        /// Specifies the server to connect. This can be either the DNS name of the
        /// server or the IP (as a string).
        /// </summary>
        /// <param name="server">The server.</param>
        /// <returns></returns>
        public OracleConnectionStringBuilder Server(string server)
        {
            this.server = server;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// Specifies the instance (database name) to use.  This can be the short name or the
        /// fully qualified name (Oracle service name).
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public OracleConnectionStringBuilder Instance(string instance)
        {
            this.instance = instance;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// Specifies the name of the user account accessing the database.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public OracleConnectionStringBuilder Username(string username)
        {
            this.username = username;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// Specifies the password of the user account accessing the database.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public OracleConnectionStringBuilder Password(string password)
        {
            this.password = password;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// Optional. Ports the specified port the oracle database is running on.  This defaults to 1521.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        public OracleConnectionStringBuilder Port(int port)
        {
            this.port = port;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// Enable or disable pooling connections for this data configuration.
        /// </summary>
        /// <param name="pooling">if set to <c>true</c> enable pooling.</param>
        /// <returns></returns>
        public OracleConnectionStringBuilder Pooling(bool pooling)
        {
            this.pooling = pooling;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// Specifies the SQL statement cache size to use for this connection.
        /// </summary>
        /// <param name="cacheSize">Size of the cache.</param>
        /// <returns></returns>
        public OracleConnectionStringBuilder StatementCacheSize(int cacheSize)
        {
            this.statementCacheSize = string.Format("Statement Cache Size={0};", cacheSize);
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// Specifies, as a string, other Oracle options to pass to the connection.
        /// </summary>
        /// <param name="otherOptions">The other options.</param>
        /// <returns></returns>
        public OracleConnectionStringBuilder OtherOptions(string otherOptions)
        {
            this.otherOptions = string.Format("{0};", otherOptions);
            IsDirty = true;
            return this;
        }

        protected internal override string Create()
        {
            var connectionString = base.Create();
            if (!string.IsNullOrEmpty(connectionString))
            {
                return connectionString;
            }

            connectionString = string.Format(
                     "User Id={0};Password={1};Pooling={2};{3}{4}Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={5})(PORT={6})))(CONNECT_DATA=(SERVICE_NAME={7})))",
                     username, password, pooling, statementCacheSize, otherOptions, server, port, instance);
            
            return connectionString;
        }
    }
}
