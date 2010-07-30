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

        Configuration Configuration { get; }
        ISessionFactory SessionFactory { get; }
    }

    public class SessionSource : ISessionSource
    {
        private Dialect dialect;
        public Configuration Configuration { get; private set; }
        public ISessionFactory SessionFactory { get; private set; }

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
            Configuration = config.Configuration;

            SessionFactory = config.BuildSessionFactory();
            dialect = Dialect.GetDialect(Configuration.Properties);
        }

        protected void Initialize(Configuration nhibernateConfig, PersistenceModel model)
        {
            if( model == null ) throw new ArgumentNullException("model", "Model cannot be null");

            Configuration = nhibernateConfig;

            model.Configure(Configuration);

            SessionFactory = Configuration.BuildSessionFactory();
            dialect = Dialect.GetDialect(Configuration.Properties);
        }

        public virtual ISession CreateSession()
        {
            return SessionFactory.OpenSession();
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
            new SchemaExport(Configuration)
                .Execute(script, true, false, session.Connection, null);
        }
    }
}