namespace FluentNHibernate.Cfg.Db
{
    public class IfxDRDAConnectionStringBuilder : ConnectionStringBuilder
    {
        private string authentication = "";
        private string database = "";
        private string hostVarParameter = "";
        private string isolationLevel = "";
        private string maxPoolSize = "";
        private string minPoolSize = "";
        private string password = "";
        private string pooling = "";
        private string server = "";
        private string username = "";
        private string otherOptions = "";

        /// <summary>
        /// The type of authentication to be used. Acceptable values:
        /// <list type="bullet">
        /// <item>
        ///     SERVER
        /// </item>
        /// <item>
        ///     SERVER_ENCRYPT
        /// </item>
        /// <item>
        ///     DATA_ENCRYPT
        /// </item>
        /// <item>
        ///     KERBEROS
        /// </item>
        /// <item>
        ///     GSSPLUGIN
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="authentication"></param>
        /// <returns>IfxDRDAConnectionStringBuilder object</returns>
        public IfxDRDAConnectionStringBuilder Authentication(string authentication)
        {
            this.authentication = authentication;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// The name of the database within the server instance.
        /// </summary>
        /// <param name="database"></param>
        /// <returns>IfxSQLIConnectionStringBuilder object</returns>
        public IfxDRDAConnectionStringBuilder Database(string database)
        {
            this.database = database;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// <list type="bullet">
        /// <item>
        ///     <term>true</term>
        ///     <description> - host variable (:param) support enabled.</description>
        /// </item>
        /// <item>
        ///     <term>false (default)</term>
        ///     <description> - host variable support disabled.</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="hostVarParameter"></param>
        /// <returns>IfxSQLIConnectionStringBuilder object</returns>
        public IfxDRDAConnectionStringBuilder HostVarParameter(string hostVarParameter)
        {
            this.hostVarParameter = hostVarParameter;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// Isolation level for the connection. Possible values:
        /// <list type="bullet">
        /// <item>
        ///     ReadCommitted
        /// </item>
        /// <item>
        ///     ReadUncommitted
        /// </item>
        /// <item>
        ///     RepeatableRead
        /// </item>
        /// <item>
        ///     Serializable
        /// </item>
        /// <item>
        ///     Transaction
        /// </item>
        /// </list>
        /// This keyword is only supported for applications participating in a
        /// distributed transaction.
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <returns>IfxDRDAConnectionStringBuilder object</returns>
        public IfxDRDAConnectionStringBuilder IsolationLevel(string isolationLevel)
        {
            this.isolationLevel = isolationLevel;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// The maximum number of connections allowed in the pool.
        /// </summary>
        /// <param name="maxPoolSize"></param>
        /// <returns>IfxDRDAConnectionStringBuilder object</returns>
        public IfxDRDAConnectionStringBuilder MaxPoolSize(string maxPoolSize)
        {
            this.maxPoolSize = maxPoolSize;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// The minimum number of connections allowed in the pool. Default value 0.
        /// </summary>
        /// <param name="minPoolSize"></param>
        /// <returns>IfxDRDAConnectionStringBuilder object</returns>
        public IfxDRDAConnectionStringBuilder MinPoolSize(string minPoolSize)
        {
            this.minPoolSize = minPoolSize;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// The password associated with the User ID.
        /// </summary>
        /// <param name="password"></param>
        /// <returns>IfxDRDAConnectionStringBuilder object</returns>
        public IfxDRDAConnectionStringBuilder Password(string password)
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
        /// <returns>IfxDRDAConnectionStringBuilder object</returns>
        public IfxDRDAConnectionStringBuilder Pooling(string pooling)
        {
            this.pooling = pooling;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// Server name with optional port number for direct connection using either
        /// IPv4 notation (<![CDATA[<server name/ip address>[:<port>]]]>) or IPv6 notation.
        /// </summary>
        /// <param name="server"></param>
        /// <returns>IfxDRDAConnectionStringBuilder object</returns>
        public IfxDRDAConnectionStringBuilder Server(string server)
        {
            this.server = server;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// The login account.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>IfxDRDAConnectionStringBuilder object</returns>
        public IfxDRDAConnectionStringBuilder Username(string username)
        {
            this.username = username;
            IsDirty = true;
            return this;
        }

        /// <summary>
        /// Other options: Connection Lifetime, Connection Reset, Connection Timeout, CurrentSchema, Enlist,
        /// Interrupt, Persist Security Info, ResultArrayAsReturnValue, Security, TrustedContextSystemUserID,
        /// TrustedContextSystemPassword
        /// </summary>
        /// <param name="otherOptions"></param>
        /// <returns>IfxDRDAConnectionStringBuilder object</returns>
        public IfxDRDAConnectionStringBuilder OtherOptions(string otherOptions)
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

            if (this.authentication.Length > 0)
            {
                connectionString += string.Format("Authentication={0};", this.authentication);
            }
            if (this.database.Length > 0)
            {
                connectionString += string.Format("Database={0};", this.database);
            }
            if (this.hostVarParameter.Length > 0)
            {
                connectionString += string.Format("HostVarParameter={0};", this.hostVarParameter);
            }
            if (this.isolationLevel.Length > 0)
            {
                connectionString += string.Format("IsolationLevel={0};", this.isolationLevel);
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