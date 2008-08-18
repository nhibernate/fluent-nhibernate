using System.Collections.Generic;
using System.Configuration;
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
		protected const string ShowSqlKey = "show_sql";
		protected const string ConnectionProviderKey = "connection.provider";
		protected const string DefaultConnectionProviderClassName = "NHibernate.Connection.DriverConnectionProvider";
		protected const string DriverClassKey = "connection.driver_class";
		protected const string ConnectionStringKey = "connection.connection_string";
		protected const string UseOuterJoinKey = "use_outer_join";
        protected const string UseReflectionOptimizerKey = "use_reflection_optimizer";
        protected const string MaxFetchDepthKey = "max_fetch_depth";

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

		public THIS Provider(string provider)
		{
			_values.Store(ConnectionProviderKey, provider);
			return (THIS) this;
		}

		public THIS Provider<T>()
			where T : IConnectionProvider
		{
			return Provider(typeof (T).AssemblyQualifiedName);
		}

		public THIS Driver(string driverClass)
		{
			_values.Store(DriverClassKey, driverClass);
			return (THIS) this;
		}

		public THIS Driver<T>()
			where T : IDriver
		{
			return Driver(typeof (T).AssemblyQualifiedName);
		}

		public THIS ShowSql()
		{
            toggleBooleanSetting(ShowSqlKey);
			return (THIS) this;
		}

	    public THIS UseOuterJoin()
		{
            toggleBooleanSetting(UseOuterJoinKey);
			return (THIS)this;
		}

	    public THIS UseReflectionOptimizer()
		{
            toggleBooleanSetting(UseReflectionOptimizerKey);
			return (THIS)this;
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

	    public ConnectionStringExpression<THIS> ConnectionString
		{
			get
			{
				return new ConnectionStringExpression<THIS>((THIS) this);
			}
		}

	    public THIS Raw(string key, string value)
		{
			_values.Store(key, value);
			return (THIS) this;
		}

	    public THIS MaxFetchDepth(int maxFetchDepth)
	    {
	        _values.Store(MaxFetchDepthKey, maxFetchDepth.ToString());
            return (THIS)this;
	    }

	    public class ConnectionStringExpression<CONFIG>
			where CONFIG : PersistenceConfiguration<CONFIG>
		{
			private readonly CONFIG _config;

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
	}
}
