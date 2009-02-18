using System;
using System.Collections.Generic;
using System.Data;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;

namespace FluentNHibernate
{
    public interface ISessionSource
    {
        ISession CreateSession();
        void BuildSchema();
    }

    public class SessionSource : ISessionSource
    {
        private ISessionFactory _sessionFactory;
        private Configuration _configuration;
        private Dialect _dialect;

        public SessionSource(PersistenceModel model) 
        {
            Initialize(new Configuration().Configure(), model);
        }

        public SessionSource(IDictionary<string, string> properties, PersistenceModel model)
        {
            Initialize(new Configuration().AddProperties(properties), model);
        }

        public SessionSource(FluentConfiguration config)
        {
            _configuration = config.Configuration;

            _sessionFactory = config.BuildSessionFactory();
            _dialect = Dialect.GetDialect(_configuration.Properties);
        }

        protected void Initialize(Configuration nhibernateConfig, PersistenceModel model)
        {
            if( model == null ) throw new ArgumentNullException("model", "Model cannot be null");

            _configuration = nhibernateConfig;

            model.Configure(_configuration);

            _sessionFactory = _configuration.BuildSessionFactory();
            _dialect = Dialect.GetDialect(_configuration.Properties);
        }

        public virtual ISession CreateSession()
        {
            return _sessionFactory.OpenSession();
        }

        public virtual void BuildSchema()
        {
            using( var session = CreateSession())
            {
                BuildSchema(session);
            }
        }

        public virtual void BuildSchema(bool script)
        {
            using (var session = CreateSession())
            {
                BuildSchema(session, script);
            }
        }

        public void BuildSchema(ISession session)
        {
            BuildSchema(session, false);
        }

        public void BuildSchema(ISession session, bool script)
        {
            new SchemaExport(_configuration)
                .Execute(script, true, false, true, session.Connection, null);
        }
    }
}