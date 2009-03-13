namespace FluentNHibernate.Cfg.Db
{
    public class OracleConnectionStringBuilder : ConnectionStringBuilder
    {
        private string _instance;
        private string _otherOptions;
        private string _password;
        private int _port;
        private string _server;
        private string _username;
        private bool _pooling;
        private int _statementCacheSize;

        public OracleConnectionStringBuilder()
        {
            // Port is pre-slugged as 1521 is the default Oracle port.
            _port = 1521;
        }

        /// <summary>
        /// Specifies the server to connect. This can be either the DNS name of the
        /// server or the IP (as a string).
        /// </summary>
        /// <param name="server">The server.</param>
        /// <returns></returns>
        public OracleConnectionStringBuilder Server(string server)
        {
            _server = server;
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
            _instance = instance;
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
            _username = username;
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
            _password = password;
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
            _port = port;
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
            _pooling = pooling;
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
            _statementCacheSize = cacheSize;
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
            _otherOptions = string.Format("{0};", otherOptions);
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
                     "User Id={0};Password={1};Pooling={2};Statement Cache Size={3};{4}Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={5})(PORT={6})))(CONNECT_DATA=(SERVICE_NAME={7})))",
                     _username, _password, _pooling, _statementCacheSize, _otherOptions, _server, _port, _instance);
            
            return connectionString;
        }
    }
}
