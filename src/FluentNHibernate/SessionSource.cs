using System;
using System.Collections.Generic;
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
        private ISessionFactory sessionFactory;
        private Configuration configuration;
        private Dialect dialect;

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
            configuration = config.Configuration;

            sessionFactory = config.BuildSessionFactory();
            dialect = Dialect.GetDialect(configuration.Properties);
        }

        protected void Initialize(Configuration nhibernateConfig, PersistenceModel model)
        {
            if( model == null ) throw new ArgumentNullException("model", "Model cannot be null");

            configuration = nhibernateConfig;

            model.Configure(configuration);

            sessionFactory = configuration.BuildSessionFactory();
            dialect = Dialect.GetDialect(configuration.Properties);
        }

        public virtual ISession CreateSession()
        {
            return sessionFactory.OpenSession();
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
            new SchemaExport(configuration)
                .Execute(script, true, false, session.Connection, null);
        }
    }
}