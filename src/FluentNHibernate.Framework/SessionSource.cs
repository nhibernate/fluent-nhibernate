using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;

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
		}

    	public PersistenceModel Model
        {
            get { return _model; }
        }

        public ISession CreateSession()
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

            string[] drops = _configuration.GenerateDropSchemaScript(_sessionFactory.Dialect);
            executeScripts(drops, connection);

            string[] scripts = _configuration.GenerateSchemaCreationScript(_sessionFactory.Dialect);
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