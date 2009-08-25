using System;
using System.Collections.Generic;
using NHibernate.Bytecode;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibConfiguration = NHibernate.Cfg.Configuration;

namespace FluentNHibernate.Cfg.Db
{
    public abstract class PersistenceConfiguration<TThisConfiguration> : PersistenceConfiguration<TThisConfiguration, ConnectionStringBuilder>
        where TThisConfiguration : PersistenceConfiguration<TThisConfiguration, ConnectionStringBuilder>
    {}

    public abstract class PersistenceConfiguration<TThisConfiguration, TConnectionString> : IPersistenceConfigurer
        where TThisConfiguration : PersistenceConfiguration<TThisConfiguration, TConnectionString>
        where TConnectionString : ConnectionStringBuilder, new()
    {
        protected const string DialectKey = "dialect"; // Newer one, but not supported by everything
        protected const string AltDialectKey = "hibernate.dialect"; // Some older NHib tools require this
        protected const string DefaultSchemaKey = "default_schema"; 
        protected const string UseOuterJoinKey = "use_outer_join";
        protected const string MaxFetchDepthKey = "max_fetch_depth";
        protected const string UseReflectionOptimizerKey = "use_reflection_optimizer";
        protected const string QuerySubstitutionsKey = "query.substitutions";
        protected const string ShowSqlKey = "show_sql";
        protected const string FormatSqlKey = "format_sql";

        protected const string ConnectionProviderKey = "connection.provider";
        protected const string DefaultConnectionProviderClassName = "NHibernate.Connection.DriverConnectionProvider";
        protected const string DriverClassKey = "connection.driver_class";
        protected const string ConnectionStringKey = "connection.connection_string";
        protected const string ProxyFactoryFactoryClassKey = "proxyfactory.factory_class";
        protected const string DefaultProxyFactoryFactoryClassName = "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle";
        protected const string AdoNetBatchSizeKey = "adonet.batch_size";
		protected const string CurrentSessionContextClassKey = "current_session_context_class";

        private readonly Dictionary<string, string> values = new Dictionary<string, string>();

        private bool nextBoolSettingValue = true;
        private readonly TConnectionString connectionString;
        private readonly CacheSettingsBuilder cache = new CacheSettingsBuilder();

        protected PersistenceConfiguration()
        {
            values[ConnectionProviderKey] = DefaultConnectionProviderClassName;
            values[ProxyFactoryFactoryClassKey] =  DefaultProxyFactoryFactoryClassName;
            connectionString = new TConnectionString();
        }

        protected virtual IDictionary<string, string> CreateProperties()
        {
            if (connectionString.IsDirty)
                Raw(ConnectionStringKey, connectionString.Create());

            if (cache.IsDirty)
            {
                foreach (var pair in cache.Create())
                {
                    Raw(pair.Key, pair.Value);
                }
            }

            return values;
        }

        public NHibConfiguration ConfigureProperties(NHibConfiguration nhibernateConfig)
        {
            var settings = CreateProperties();

            nhibernateConfig.SetProperties(settings);

            return nhibernateConfig;
        }

        public IDictionary<string, string> ToProperties()
        {
            return CreateProperties();
        }

        protected void ToggleBooleanSetting(string settingKey)
        {
            var value = nextBoolSettingValue.ToString().ToLowerInvariant();

            values[settingKey] = value;

            nextBoolSettingValue = true;
        }

        public TThisConfiguration DoNot
        {
            get
            {
                nextBoolSettingValue = false;
                return (TThisConfiguration)this;
            }
        }

        public TThisConfiguration Dialect(string dialect)
        {
            values[DialectKey] = dialect;
            values[AltDialectKey] = dialect;
            return (TThisConfiguration) this;
        }

        public TThisConfiguration Dialect<T>()
            where T : Dialect
        {
            return Dialect(typeof (T).AssemblyQualifiedName);
        }

        public TThisConfiguration DefaultSchema(string schema)
        {
            values[DefaultSchemaKey] = schema;
            return (TThisConfiguration) this;
        }

        public TThisConfiguration UseOuterJoin()
        {
            ToggleBooleanSetting(UseOuterJoinKey);
            return (TThisConfiguration) this;
        }

        public TThisConfiguration MaxFetchDepth(int maxFetchDepth)
        {
            values[MaxFetchDepthKey] = maxFetchDepth.ToString();
            return (TThisConfiguration)this;
        }

        public TThisConfiguration UseReflectionOptimizer()
        {
            ToggleBooleanSetting(UseReflectionOptimizerKey);
            return (TThisConfiguration) this;
        }

        public TThisConfiguration QuerySubstitutions(string query)
        {
            values[QuerySubstitutionsKey] = query;
            return (TThisConfiguration)this;
        }

        public TThisConfiguration ShowSql()
        {
            ToggleBooleanSetting(ShowSqlKey);
            return (TThisConfiguration)this;
        }

		public TThisConfiguration FormatSql()
		{
			ToggleBooleanSetting(FormatSqlKey);
			return (TThisConfiguration)this;
		}

        public TThisConfiguration Provider(string provider)
        {
            values[ConnectionProviderKey] = provider;
            return (TThisConfiguration)this;
        }

        public TThisConfiguration Provider<T>()
            where T : IConnectionProvider
        {
            return Provider(typeof(T).AssemblyQualifiedName);
        }

        public TThisConfiguration Driver(string driverClass)
        {
            values[DriverClassKey] = driverClass;
            return (TThisConfiguration)this;
        }

        public TThisConfiguration Driver<T>()
            where T : IDriver
        {
            return Driver(typeof(T).AssemblyQualifiedName);
        }

        public TThisConfiguration ConnectionString(Action<TConnectionString> connectionStringExpression)
        {
            connectionStringExpression(connectionString);
            return (TThisConfiguration)this;
        }

        public TThisConfiguration ConnectionString(string value)
        {
            connectionString.Is(value);
            return (TThisConfiguration)this;
        }

        public TThisConfiguration Cache(Action<CacheSettingsBuilder> cacheExpression)
        {
            cacheExpression(cache);
            return (TThisConfiguration)this;
        }

        public TThisConfiguration Raw(string key, string value)
        {
            values[key] = value;
            return (TThisConfiguration) this;
        }

        /// <summary>
        /// Sets the proxyfactory.factory_class property.
        /// NOTE: NHibernate 2.1 only
        /// </summary>
        /// <param name="proxyFactoryFactoryClass">factory class</param>
        /// <returns>Configuration</returns>
        public TThisConfiguration ProxyFactoryFactory(string proxyFactoryFactoryClass)
        {
            values[ProxyFactoryFactoryClassKey] = proxyFactoryFactoryClass;
            return (TThisConfiguration)this;
        }

        public TThisConfiguration ProxyFactoryFactory(Type proxyFactoryFactory)
        {
            values[ProxyFactoryFactoryClassKey] = proxyFactoryFactory.AssemblyQualifiedName;
            return (TThisConfiguration)this;
        }

        public TThisConfiguration ProxyFactoryFactory<TProxyFactoryFactory>() where TProxyFactoryFactory : IProxyFactoryFactory
        {
            return ProxyFactoryFactory(typeof(TProxyFactoryFactory));
        }

        /// <summary>
        /// Sets the adonet.batch_size property.
        /// </summary>
        /// <param name="size">Batch size</param>
        /// <returns>Configuration</returns>
        public TThisConfiguration AdoNetBatchSize(int size)
        {
            values[AdoNetBatchSizeKey] = size.ToString();
            return (TThisConfiguration)this;
        }

		/// <summary>
		/// Sets the current_session_context_class property.
		/// </summary>
		/// <param name="currentSessionContextClass">current session context class</param>
		/// <returns>Configuration</returns>
		public TThisConfiguration CurrentSessionContext(string currentSessionContextClass)
		{
			values[CurrentSessionContextClassKey] = currentSessionContextClass;
			return (TThisConfiguration)this;
		}

		/// <summary>
		/// Sets the current_session_context_class property.
		/// </summary>
		/// <typeparam name="TSessionContext">Implementation of ICurrentSessionContext to use</typeparam>
		/// <returns>Configuration</returns>
		public TThisConfiguration CurrentSessionContext<TSessionContext>() where TSessionContext : NHibernate.Context.ICurrentSessionContext
		{
			return CurrentSessionContext(typeof(TSessionContext).AssemblyQualifiedName);
		}
    }
}