using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;

namespace FluentNHibernate.Framework
{
    public interface ISessionSource
    {
        ISession CreateSession();
        void BuildSchema();
        PersistenceModel Model { get; }
    }

    public class SessionSource : ISessionSource
    {
        private ISessionFactory _sessionFactory;
        private Configuration _configuration;
        private PersistenceModel _model;
        private Dialect _dialect;

        public SessionSource(PersistenceModel model) 
    	{
    		Initialize(new Configuration().Configure(), model);
    	}

    	public SessionSource(IDictionary<string, string> properties, PersistenceModel model)
    	{
    		Initialize(new Configuration().AddProperties(properties), model);
    	}

    	protected void Initialize(Configuration nhibernateConfig, PersistenceModel model)
		{
			if( model == null ) throw new ArgumentNullException("model", "Model cannot be null");

			_configuration = nhibernateConfig;
			_model = model;
			
			model.Configure(_configuration);

			_sessionFactory = _configuration.BuildSessionFactory();
    	    _dialect = Dialect.GetDialect(_configuration.Properties);
		}

    	public PersistenceModel Model
        {
            get { return _model; }
        }

        public virtual ISession CreateSession()
        {
            return _sessionFactory.OpenSession();
        }

        public void BuildSchema()
        {
        	BuildSchema(CreateSession());
        }

		public void BuildSchema(ISession session)
		{
    		IDbConnection connection = session.Connection;

            string[] drops = _configuration.GenerateDropSchemaScript(_dialect);
            executeScripts(drops, connection);

            string[] scripts = _configuration.GenerateSchemaCreationScript(_dialect);
            executeScripts(scripts, connection);
        }

        private static void executeScripts(IEnumerable<string> scripts, IDbConnection connection)
        {
            foreach (var script in scripts)
            {
                IDbCommand command = connection.CreateCommand();
                command.CommandText = script;
                command.ExecuteNonQuery();
            }
        }


    }
}