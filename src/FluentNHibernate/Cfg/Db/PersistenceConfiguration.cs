using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibConfiguration = NHibernate.Cfg.Configuration;
using NHibEnvironment = NHibernate.Cfg.Environment;

namespace FluentNHibernate.Cfg.Db
{
    public abstract class PersistenceConfiguration<TThisConfiguration> : PersistenceConfiguration<TThisConfiguration, ConnectionStringBuilder>
        where TThisConfiguration : PersistenceConfiguration<TThisConfiguration, ConnectionStringBuilder>
    {}

    public abstract class PersistenceConfiguration<TThisConfiguration, TConnectionString> : IPersistenceConfigurer
        where TThisConfiguration : PersistenceConfiguration<TThisConfiguration, TConnectionString>
        where TConnectionString : ConnectionStringBuilder, new()
    {
        protected const string DialectKey = NHibEnvironment.Dialect; // Newer one, but not supported by everything
        protected const string AltDialectKey = "hibernate.dialect"; // Some older NHib tools require this
        protected const string DefaultSchemaKey = "default_schema"; 
        protected const string UseOuterJoinKey = "use_outer_join";
        protected const string MaxFetchDepthKey = NHibEnvironment.MaxFetchDepth;
        protected const string UseReflectionOptimizerKey = NHibEnvironment.PropertyUseReflectionOptimizer;
        protected const string QuerySubstitutionsKey = NHibEnvironment.QuerySubstitutions;
        protected const string ShowSqlKey = NHibEnvironment.ShowSql;
        protected const string FormatSqlKey = NHibEnvironment.FormatSql;

		protected const string CollectionTypeFactoryClassKey = NHibernate.Cfg.Environment.CollectionTypeFactoryClass;
        protected const string ConnectionProviderKey = NHibEnvironment.ConnectionProvider;
        protected const string DefaultConnectionProviderClassName = "NHibernate.Connection.DriverConnectionProvider";
        protected const string DriverClassKey = NHibEnvironment.ConnectionDriver;
        protected const string ConnectionStringKey = NHibEnvironment.ConnectionString;
        protected const string IsolationLevelKey = NHibEnvironment.Isolation;
        protected const string AdoNetBatchSizeKey = NHibEnvironment.BatchSize;
        protected const string CurrentSessionContextClassKey = "current_session_context_class";

        private readonly Dictionary<string, string> values = new Dictionary<string, string>();

        private bool nextBoolSettingValue = true;
        private readonly TConnectionString connectionString;
        private readonly CacheSettingsBuilder cache = new CacheSettingsBuilder();

        protected PersistenceConfiguration()
        {
            values[ConnectionProviderKey] = DefaultConnectionProviderClassName;
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
        
        static IEnumerable<string> OverridenDefaults(IDictionary<string,string> settings)
        {
            if (settings[ConnectionProviderKey] != DefaultConnectionProviderClassName)
                yield return ConnectionProviderKey;
        }

        private static IEnumerable<string> KeysToPreserve(NHibConfiguration nhibernateConfig, IDictionary<string, string> settings)
        {
            var @default = new NHibConfiguration();
            return nhibernateConfig.Properties
                .Except(@default.Properties)
                    .Select(k => k.Key)
                    .Except(OverridenDefaults(settings));
        }

        public NHibConfiguration ConfigureProperties(NHibConfiguration nhibernateConfig)
        {
            var settings = CreateProperties();
            var keepers = KeysToPreserve(nhibernateConfig, settings);

            foreach (var kv in settings.Where(s => !keepers.Contains(s.Key)))
            {
                nhibernateConfig.SetProperty(kv.Key, kv.Value);
            }

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

        /// <summary>
        /// Negates the next boolean option.
        /// </summary>
        public TThisConfiguration DoNot
        {
            get
            {
                nextBoolSettingValue = false;
                return (TThisConfiguration)this;
            }
        }

        /// <summary>
        /// Sets the database dialect. This shouldn't be necessary
        /// if you've used one of the provided database configurations.
        /// </summary>
        /// <returns>Configuration builder</returns>
        public TThisConfiguration Dialect(string dialect)
        {
            values[DialectKey] = dialect;
            values[AltDialectKey] = dialect;
            return (TThisConfiguration) this;
        }

        /// <summary>
        /// Sets the database dialect. This shouldn't be necessary
        /// if you've used one of the provided database configurations.
        /// </summary>
        /// <returns>Configuration builder</returns>
        public TThisConfiguration Dialect<T>()
            where T : Dialect
        {
            return Dialect(typeof (T).AssemblyQualifiedName);
        }

        /// <summary>
        /// Sets the default database schema
        /// </summary>
        /// <param name="schema">Default schema name</param>
        /// <returns>Configuration builder</returns>
        public TThisConfiguration DefaultSchema(string schema)
        {
            values[DefaultSchemaKey] = schema;
            return (TThisConfiguration) this;
        }

        /// <summary>
        /// Enables the outer-join option.
        /// </summary>
        /// <returns>Configuration builder</returns>
        public TThisConfiguration UseOuterJoin()
        {
            ToggleBooleanSetting(UseOuterJoinKey);
            return (TThisConfiguration) this;
        }

        /// <summary>
        /// Sets the max fetch depth.
        /// </summary>
        /// <param name="maxFetchDepth">Max fetch depth</param>
        /// <returns>Configuration builder</returns>
        public TThisConfiguration MaxFetchDepth(int maxFetchDepth)
        {
            values[MaxFetchDepthKey] = maxFetchDepth.ToString();
            return (TThisConfiguration)this;
        }

        /// <summary>
        /// Enables the reflection optimizer.
        /// </summary>
        /// <returns>Configuration builder</returns>
        public TThisConfiguration UseReflectionOptimizer()
        {
            ToggleBooleanSetting(UseReflectionOptimizerKey);
            return (TThisConfiguration) this;
        }

        /// <summary>
        /// Sets any query stubstitutions that NHibernate should
        /// perform.
        /// </summary>
        /// <param name="substitutions">Substitutions</param>
        /// <returns>Configuration builder</returns>
        public TThisConfiguration QuerySubstitutions(string substitutions)
        {
            values[QuerySubstitutionsKey] = substitutions;
            return (TThisConfiguration)this;
        }

        /// <summary>
        /// Enables the show SQL option.
        /// </summary>
        /// <returns>Configuration builder</returns>
        public TThisConfiguration ShowSql()
        {
            ToggleBooleanSetting(ShowSqlKey);
            return (TThisConfiguration)this;
        }

        /// <summary>
        /// Enables the format SQL option.
        /// </summary>
        /// <returns>Configuration builder</returns>
        public TThisConfiguration FormatSql()
        {
            ToggleBooleanSetting(FormatSqlKey);
            return (TThisConfiguration)this;
        }

        /// <summary>
        /// Sets the database provider. This shouldn't be necessary
        /// if you're using one of the provided database configurations.
        /// </summary>
        /// <param name="provider">Provider type</param>
        /// <returns>Configuration builder</returns>
        public TThisConfiguration Provider(string provider)
        {
            values[ConnectionProviderKey] = provider;
            return (TThisConfiguration)this;
        }

        /// <summary>
        /// Sets the database provider. This shouldn't be necessary
        /// if you're using one of the provided database configurations.
        /// </summary>
        /// <typeparam name="T">Provider type</typeparam>
        /// <returns>Configuration builder</returns>
        public TThisConfiguration Provider<T>()
            where T : IConnectionProvider
        {
            return Provider(typeof(T).AssemblyQualifiedName);
        }

        /// <summary>
        /// Specify the database driver. This isn't necessary
        /// if you're using one of the provided database configurations.
        /// </summary>
        /// <param name="driverClass">Driver type</param>
        /// <returns>Configuration builder</returns>
        public TThisConfiguration Driver(string driverClass)
        {
            values[DriverClassKey] = driverClass;
            return (TThisConfiguration)this;
        }

        /// <summary>
        /// Specify the database driver. This isn't necessary
        /// if you're using one of the provided database configurations.
        /// </summary>
        /// <typeparam name="T">Driver type</typeparam>
        /// <returns>Configuration builder</returns>
        public TThisConfiguration Driver<T>()
            where T : IDriver
        {
            return Driver(typeof(T).AssemblyQualifiedName);
        }

        /// <summary>
        /// Configure the connection string
        /// </summary>
        /// <example>
        ///     ConnectionString(x =>
        ///     {
        ///       x.Server("db_server");
        ///       x.Database("Products");
        ///     });
        /// </example>
        /// <param name="connectionStringExpression">Closure for building the connection string</param>
        /// <returns>Configuration builder</returns>
        public TThisConfiguration ConnectionString(Action<TConnectionString> connectionStringExpression)
        {
            connectionStringExpression(connectionString);
            return (TThisConfiguration)this;
        }

        /// <summary>
        /// Set the connection string.
        /// </summary>
        /// <param name="value">Connection string to use</param>
        /// <returns>Configuration builder</returns>
        public TThisConfiguration ConnectionString(string value)
        {
            connectionString.Is(value);
            return (TThisConfiguration)this;
        }

        /// <summary>
        /// Sets a raw property on the NHibernate configuration. Use this method
        /// if there isn't a specific option available in the API.
        /// </summary>
        /// <param name="key">Setting key</param>
        /// <param name="value">Setting value</param>
        /// <returns>Configuration builder</returns>
        public TThisConfiguration Raw(string key, string value)
        {
            values[key] = value;
            return (TThisConfiguration) this;
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
        /// Sets the connection isolation level. NHibernate setting: connection.isolation
        /// </summary>
        /// <param name="connectionIsolation">Isolation level</param>
        /// <returns>Configuration builder</returns>
        public TThisConfiguration IsolationLevel(IsolationLevel connectionIsolation)
        {
            return IsolationLevel(connectionIsolation.ToString());
        }

        /// <summary>
        /// Sets the connection isolation level. NHibernate setting: connection.isolation
        /// </summary>
        /// <param name="connectionIsolation">Isolation level</param>
        /// <returns>Configuration builder</returns>
        public TThisConfiguration IsolationLevel(string connectionIsolation)
        {
            values[IsolationLevelKey] = connectionIsolation;
            return (TThisConfiguration)this;
        }
    }
}