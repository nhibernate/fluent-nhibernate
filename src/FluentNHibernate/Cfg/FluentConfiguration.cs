using System;
using System.Collections.Generic;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Diagnostics;
using NHibernate;
using NHibernate.Bytecode;
using NHibernate.Cfg;
using NHibEnvironment = NHibernate.Cfg.Environment;

namespace FluentNHibernate.Cfg
{
    /// <summary>
    /// Fluent configuration API for NHibernate
    /// </summary>
    public class FluentConfiguration
    {
        const string ExceptionMessage = "An invalid or incomplete configuration was used while creating a SessionFactory. Check PotentialReasons collection, and InnerException for more detail.";
        const string ExceptionDatabaseMessage = "Database was not configured through Database method.";
        const string ExceptionMappingMessage = "No mappings were configured through the Mappings method.";

        const string CollectionTypeFactoryClassKey = NHibEnvironment.CollectionTypeFactoryClass;
        const string ProxyFactoryFactoryClassKey = NHibEnvironment.ProxyFactoryFactoryClass;
        const string DefaultProxyFactoryFactoryClassName = "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle";
        const string CurrentSessionContextClassKey = NHibEnvironment.CurrentSessionContextClass;

        readonly Configuration cfg;
        readonly IList<Action<Configuration>> configAlterations = new List<Action<Configuration>>();
        readonly IDiagnosticMessageDespatcher despatcher = new DefaultDiagnosticMessageDespatcher();
        readonly List<Action<MappingConfiguration>> mappingsBuilders = new List<Action<MappingConfiguration>>();

        bool dbSet;
        bool mappingsSet;
        
        IDiagnosticLogger logger = new NullDiagnosticsLogger();
        readonly CacheSettingsBuilder cache = new CacheSettingsBuilder();

        internal FluentConfiguration()
            : this(new Configuration())
        { }

        internal FluentConfiguration(Configuration cfg)
        {
            this.cfg = cfg;
            this.ProxyFactoryFactory(DefaultProxyFactoryFactoryClassName);
        }

        internal Configuration Configuration
        {
            get { return cfg; }
        }

        /// <summary>
        /// Configure diagnostic logging
        /// </summary>
        /// <param name="diagnosticsSetup">Diagnostic configuration</param>
        public FluentConfiguration Diagnostics(Action<DiagnosticsConfiguration> diagnosticsSetup)
        {
            var diagnosticsCfg = new DiagnosticsConfiguration(despatcher, new_logger => logger = new_logger);

            diagnosticsSetup(diagnosticsCfg);

            return this;
        }

        /// <summary>
        /// Apply database settings
        /// </summary>
        /// <param name="config">Lambda returning database configuration</param>
        /// <returns>Fluent configuration</returns>
        public FluentConfiguration Database(Func<IPersistenceConfigurer> config)
        {
            return Database(config());
        }

        /// <summary>
        /// Apply database settings
        /// </summary>
        /// <param name="config">Database configuration instance</param>
        /// <returns>Fluent configuration</returns>
        public FluentConfiguration Database(IPersistenceConfigurer config)
        {
            config.ConfigureProperties(Configuration);
            dbSet = true;
            return this;
        }

        /// <summary>
        /// Configure caching.
        /// </summary>
        /// <example>
        ///     Cache(x =>
        ///     {
        ///       x.UseQueryCache();
        ///       x.UseMinimalPuts();
        ///     });
        /// </example>
        /// <param name="cacheExpression">Closure for configuring caching</param>
        /// <returns>Configuration builder</returns>
        public FluentConfiguration Cache(Action<CacheSettingsBuilder> cacheExpression)
        {
            cacheExpression(cache);
            return this;
        }

        /// <summary>
        /// Sets the collectiontype.factory_class property.
        /// NOTE: NHibernate 2.1 only
        /// </summary>
        /// <param name="collectionTypeFactoryClass">factory class</param>
        /// <returns>Configuration</returns>
        public FluentConfiguration CollectionTypeFactory(string collectionTypeFactoryClass)
        {
            this.Configuration.SetProperty(CollectionTypeFactoryClassKey, collectionTypeFactoryClass);
            return this;
        }

        /// <summary>
        /// Sets the collectiontype.factory_class property.
        /// NOTE: NHibernate 2.1 only
        /// </summary>
        /// <param name="collectionTypeFactoryClass">factory class</param>
        /// <returns>Configuration</returns>
        public FluentConfiguration CollectionTypeFactory(Type collectionTypeFactoryClass)
        {
            return CollectionTypeFactory(collectionTypeFactoryClass.AssemblyQualifiedName);
        }

        /// <summary>
        /// Sets the collectiontype.factory_class property.
        /// NOTE: NHibernate 2.1 only
        /// </summary>
        /// <typeparam name="TCollectionTypeFactory">factory class</typeparam>
        /// <returns>Configuration</returns>
        public FluentConfiguration CollectionTypeFactory<TCollectionTypeFactory>() where TCollectionTypeFactory : ICollectionTypeFactory
        {
            return CollectionTypeFactory(typeof(TCollectionTypeFactory));
        }

        /// <summary>
        /// Sets the proxyfactory.factory_class property.
        /// NOTE: NHibernate 2.1 only
        /// </summary>
        /// <param name="proxyFactoryFactoryClass">factory class</param>
        /// <returns>Configuration</returns>
        public FluentConfiguration ProxyFactoryFactory(string proxyFactoryFactoryClass)
        {
            this.Configuration.SetProperty(ProxyFactoryFactoryClassKey, proxyFactoryFactoryClass);
            return this;
        }

        /// <summary>
        /// Sets the proxyfactory.factory_class property.
        /// NOTE: NHibernate 2.1 only
        /// </summary>
        /// <param name="proxyFactoryFactory">factory class</param>
        /// <returns>Configuration</returns>
        public FluentConfiguration ProxyFactoryFactory(Type proxyFactoryFactory)
        {
            return ProxyFactoryFactory(proxyFactoryFactory.AssemblyQualifiedName);
        }

        /// <summary>
        /// Sets the proxyfactory.factory_class property.
        /// NOTE: NHibernate 2.1 only
        /// </summary>
        /// <typeparam name="TProxyFactoryFactory">factory class</typeparam>
        /// <returns>Configuration</returns>
        public FluentConfiguration ProxyFactoryFactory<TProxyFactoryFactory>() where TProxyFactoryFactory : IProxyFactoryFactory
        {
            return ProxyFactoryFactory(typeof(TProxyFactoryFactory));
        }

        /// <summary>
        /// Sets the current_session_context_class property.
        /// </summary>
        /// <param name="currentSessionContextClass">current session context class</param>
        /// <returns>Configuration</returns>
        public FluentConfiguration CurrentSessionContext(string currentSessionContextClass)
        {
            this.Configuration.SetProperty(CurrentSessionContextClassKey, currentSessionContextClass);
            return this;
        }

        /// <summary>
        /// Sets the current_session_context_class property.
        /// </summary>
        /// <typeparam name="TSessionContext">Implementation of ICurrentSessionContext to use</typeparam>
        /// <returns>Configuration</returns>
        public FluentConfiguration CurrentSessionContext<TSessionContext>() where TSessionContext : NHibernate.Context.ICurrentSessionContext
        {
            return CurrentSessionContext(typeof(TSessionContext).AssemblyQualifiedName);
        }

        /// <summary>
        /// Apply mappings to NHibernate
        /// </summary>
        /// <param name="mappings">Lambda used to apply mappings</param>
        /// <returns>Fluent configuration</returns>
        public FluentConfiguration Mappings(Action<MappingConfiguration> mappings)
        {
            mappingsBuilders.Add(mappings);
            mappingsSet = true;
            return this;
        }

        /// <summary>
        /// Allows altering of the raw NHibernate Configuration object before creation
        /// </summary>
        /// <param name="config">Lambda used to alter Configuration</param>
        /// <returns>Fluent configuration</returns>
        public FluentConfiguration ExposeConfiguration(Action<Configuration> config)
        {
            if (config != null)
                configAlterations.Add(config);

            return this;
        }

        /// <summary>
        /// Verify's the configuration and instructs NHibernate to build a SessionFactory.
        /// </summary>
        /// <returns>ISessionFactory from supplied settings.</returns>
        public ISessionFactory BuildSessionFactory()
        {
            try
            {
                return BuildConfiguration()
                    .BuildSessionFactory();
            }
            catch (Exception ex)
            {
                throw CreateConfigurationException(ex);
            }
        }

        /// <summary>
        /// Verifies the configuration and populates the NHibernate Configuration instance.
        /// </summary>
        /// <returns>NHibernate Configuration instance</returns>
        public Configuration BuildConfiguration()
        {
            try
            {
                var mappingCfg = new MappingConfiguration(logger);

			    foreach (var builder in mappingsBuilders)
			        builder(mappingCfg);

                mappingCfg.Apply(Configuration);

                if (cache.IsDirty)
                    Configuration.AddProperties(cache.Create());

                foreach (var configAlteration in configAlterations)
                    configAlteration(Configuration);

                return Configuration;
            }
            catch (Exception ex)
            {
                throw CreateConfigurationException(ex);
            }
        }

        /// <summary>
        /// Creates an exception based on the current state of the configuration.
        /// </summary>
        /// <param name="innerException">Inner exception</param>
        /// <returns>FluentConfigurationException with state</returns>
        private FluentConfigurationException CreateConfigurationException(Exception innerException)
        {
            var ex = new FluentConfigurationException(ExceptionMessage, innerException);

            if (!dbSet)
                ex.PotentialReasons.Add(ExceptionDatabaseMessage);
            if (!mappingsSet)
                ex.PotentialReasons.Add(ExceptionMappingMessage);

            return ex;
        }
    }
}