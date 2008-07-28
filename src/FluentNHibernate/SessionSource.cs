using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;

namespace FluentNHibernate
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

        public SessionSource(IDictionary<string, string> properties, PersistenceModel model)
        {
            _configuration = new Configuration();
            _configuration.AddProperties(properties);

            model.Configure(_configuration);
            _model = model;

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
            ISession session = CreateSession();
            IDbConnection connection = session.Connection;

            string[] drops = _configuration.GenerateDropSchemaScript(_sessionFactory.Dialect);
            executeScripts(drops, connection);

            string[] scripts = _configuration.GenerateSchemaCreationScript(_sessionFactory.Dialect);
            executeScripts(scripts, connection);
        }

        private static void executeScripts(string[] scripts, IDbConnection connection)
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
