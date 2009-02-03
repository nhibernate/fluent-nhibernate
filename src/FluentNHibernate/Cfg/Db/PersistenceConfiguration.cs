using System;
using System.Collections.Generic;
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

        protected const string ConnectionProviderKey = "connection.provider";
        protected const string DefaultConnectionProviderClassName = "NHibernate.Connection.DriverConnectionProvider";
        protected const string DriverClassKey = "connection.driver_class";
        protected const string ConnectionStringKey = "connection.connection_string";

        private readonly Dictionary<string, string> _rawValues;

        private readonly Cache<string, string> _values;

        private bool _nextBoolSettingValue = true;
        private readonly TConnectionString connectionString;
        private readonly CacheSettingsBuilder cache = new CacheSettingsBuilder();

        protected PersistenceConfiguration()
        {
            _rawValues = new Dictionary<string, string>();
            _values = new Cache<string, string>(_rawValues, s=>"");
            _values.Store(ConnectionProviderKey, DefaultConnectionProviderClassName);
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

            return _rawValues;
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

        protected void toggleBooleanSetting(string settingKey)
        {
            var value = _nextBoolSettingValue.ToString().ToLowerInvariant();

            _values.Store(settingKey, value);

            _nextBoolSettingValue = true;
        }

        public TThisConfiguration DoNot
        {
            get
            {
                _nextBoolSettingValue = false;
                return (TThisConfiguration)this;
            }
        }

        public TThisConfiguration Dialect(string dialect)
        {
            _values.Store(DialectKey, dialect);
            _values.Store(AltDialectKey, dialect);
            return (TThisConfiguration) this;
        }

        public TThisConfiguration Dialect<T>()
            where T : Dialect
        {
            return Dialect(typeof (T).AssemblyQualifiedName);
        }

        public TThisConfiguration DefaultSchema(string schema)
        {
            _values.Store(DefaultSchemaKey, schema);
            return (TThisConfiguration) this;
        }

        public TThisConfiguration UseOuterJoin()
        {
            toggleBooleanSetting(UseOuterJoinKey);
            return (TThisConfiguration) this;
        }

        public TThisConfiguration MaxFetchDepth(int maxFetchDepth)
        {
            _values.Store(MaxFetchDepthKey, maxFetchDepth.ToString());
            return (TThisConfiguration)this;
        }

        public TThisConfiguration UseReflectionOptimizer()
        {
            toggleBooleanSetting(UseReflectionOptimizerKey);
            return (TThisConfiguration) this;
        }

        public TThisConfiguration QuerySubstitutions(string query)
        {
            _values.Store(QuerySubstitutionsKey, query);
            return (TThisConfiguration)this;
        }

        public TThisConfiguration ShowSql()
        {
            toggleBooleanSetting(ShowSqlKey);
            return (TThisConfiguration)this;
        }

        public TThisConfiguration Provider(string provider)
        {
            _values.Store(ConnectionProviderKey, provider);
            return (TThisConfiguration)this;
        }

        public TThisConfiguration Provider<T>()
            where T : IConnectionProvider
        {
            return Provider(typeof(T).AssemblyQualifiedName);
        }

        public TThisConfiguration Driver(string driverClass)
        {
            _values.Store(DriverClassKey, driverClass);
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

        public TThisConfiguration Cache(Action<CacheSettingsBuilder> cacheExpression)
        {
            cacheExpression(cache);
            return (TThisConfiguration)this;
        }

        public TThisConfiguration Raw(string key, string value)
        {
            _values.Store(key, value);
            return (TThisConfiguration) this;
        }
    }
}