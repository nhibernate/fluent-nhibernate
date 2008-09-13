using System.Collections.Generic;
using System.Configuration;
using NHibernate.Cache;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibConfiguration = NHibernate.Cfg.Configuration;

namespace FluentNHibernate.Cfg
{
	public interface IPersistenceConfigurer
	{
		NHibConfiguration ConfigureProperties(NHibConfiguration nhibernateConfig);
	}

	public abstract class PersistenceConfiguration<THIS> : IPersistenceConfigurer
		where THIS : PersistenceConfiguration<THIS>
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

	    protected PersistenceConfiguration()
		{
			_rawValues = new Dictionary<string, string>();
			_values = new Cache<string, string>(_rawValues, s=>"");
			_values.Store(ConnectionProviderKey, DefaultConnectionProviderClassName);
		}

	    public NHibConfiguration ConfigureProperties(NHibConfiguration nhibernateConfig)
		{
			nhibernateConfig.SetProperties(_rawValues);
			return nhibernateConfig;
		}

	    public IDictionary<string, string> ToProperties()
		{
			return new Dictionary<string, string>(_rawValues);
		}

        protected void toggleBooleanSetting(string settingKey)
        {
            var value = _nextBoolSettingValue.ToString().ToLowerInvariant();

            _values.Store(settingKey, value);

            _nextBoolSettingValue = true;
        }

        public THIS DoNot
        {
            get
            {
                _nextBoolSettingValue = false;
                return (THIS)this;
            }
        }

	    public THIS Dialect(string dialect)
		{
			_values.Store(DialectKey, dialect);
			_values.Store(AltDialectKey, dialect);
			return (THIS) this;
		}

	    public THIS Dialect<T>()
			where T : Dialect
		{
			return Dialect(typeof (T).AssemblyQualifiedName);
		}

	    public THIS DefaultSchema(string schema)
	    {
            _values.Store(DefaultSchemaKey, schema);
            return (THIS) this;
	    }

	    public THIS UseOuterJoin()
		{
            toggleBooleanSetting(UseOuterJoinKey);
			return (THIS) this;
		}

        public THIS MaxFetchDepth(int maxFetchDepth)
        {
            _values.Store(MaxFetchDepthKey, maxFetchDepth.ToString());
            return (THIS)this;
        }

	    public THIS UseReflectionOptimizer()
		{
            toggleBooleanSetting(UseReflectionOptimizerKey);
			return (THIS) this;
		}

  
	    public THIS QuerySubstitutions(string query)
	    {
            _values.Store(QuerySubstitutionsKey, query);
            return (THIS)this;
	    }

	    public THIS ShowSql()
        {
            toggleBooleanSetting(ShowSqlKey);
            return (THIS)this;
        }

	    public THIS Provider(string provider)
        {
            _values.Store(ConnectionProviderKey, provider);
            return (THIS)this;
        }

	    public THIS Provider<T>()
            where T : IConnectionProvider
        {
            return Provider(typeof(T).AssemblyQualifiedName);
        }

	    public THIS Driver(string driverClass)
        {
            _values.Store(DriverClassKey, driverClass);
            return (THIS)this;
        }

	    public THIS Driver<T>()
            where T : IDriver
        {
            return Driver(typeof(T).AssemblyQualifiedName);
        }


	    public virtual ConnectionStringExpression<THIS> ConnectionString
		{
			get
			{
				return new ConnectionStringExpression<THIS>((THIS) this);
			}
		}

		public CacheSettingsExpression<THIS> Cache
		{
			get
			{
				return new CacheSettingsExpression<THIS>((THIS) this);
			}
		}

	    public THIS Raw(string key, string value)
		{
			_values.Store(key, value);
			return (THIS) this;
		}


	    public class ConnectionStringExpression<CONFIG>
			where CONFIG : PersistenceConfiguration<CONFIG>
		{
	        protected readonly CONFIG _config;

	        public ConnectionStringExpression(CONFIG config)
			{
				_config = config;
			}

			public CONFIG FromAppSetting(string appSettingKey)
			{
				var appSettingValue = ConfigurationManager.AppSettings[appSettingKey];

				return _config.Raw(ConnectionStringKey, appSettingValue);
			}

			public CONFIG FromConnectionStringWithKey(string connectionStringKey)
			{
				var connectionString = ConfigurationManager.ConnectionStrings[connectionStringKey].ConnectionString;

				return _config.Raw(ConnectionStringKey, connectionString);
			}

			public CONFIG Is(string rawConnectionString)
			{
				return _config.Raw(ConnectionStringKey, rawConnectionString);
			}
        }

		public class CacheSettingsExpression<CONFIG>
			where CONFIG : PersistenceConfiguration<CONFIG>
		{
			protected const string ProviderClassKey = "cache.provider_class";
			protected const string CacheUseMininmalPutsKey = "cache.use_minimal_puts";
			protected const string CacheUseQueryCacheKey = "cache.use_query_cache";
			protected const string CacheQueryCacheFactoryKey = "cache.query_cache_factory";
			protected const string CacheRegionPrefixKey = "cache.region_prefix";

			private readonly CONFIG _config;

			public CacheSettingsExpression(CONFIG config)
			{
				_config = config;
			}

			public CONFIG ProviderClass(string providerclass)
			{
				_config.Raw(ProviderClassKey, providerclass);
				return _config;
			}

			public CONFIG ProviderClass<T>()
				where T : ICacheProvider
			{
				return ProviderClass(typeof(T).AssemblyQualifiedName);
			}

			public CONFIG UseMininmalPuts()
			{
				_config.toggleBooleanSetting(CacheUseMininmalPutsKey);
				return _config;
			}

			public CONFIG UseQueryCache()
			{
				_config.toggleBooleanSetting(CacheUseQueryCacheKey);
				return _config;
			}

			public CONFIG QueryCacheFactory(string factory)
			{
				_config.Raw(CacheQueryCacheFactoryKey, factory);
				return _config;
			}

			public CONFIG QueryCacheFactory<T>()
				where T : IQueryCacheFactory
			{
				return QueryCacheFactory(typeof(T).AssemblyQualifiedName);
			}

			public CONFIG RegionPrefix(string prefix)
			{
				_config.Raw(CacheRegionPrefixKey, prefix);
				return _config;
			}
		}
	}
}
