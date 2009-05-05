using System.Collections.Generic;
using FluentNHibernate.Cfg;
using NHibernate;

namespace FluentNHibernate.Testing
{
    public class SingleConnectionSessionSourceForSQLiteInMemoryTesting : SessionSource
    {
        private ISession session;

        public SingleConnectionSessionSourceForSQLiteInMemoryTesting(IDictionary<string, string> properties, PersistenceModel model) : base(properties, model)
        {
        }

        public SingleConnectionSessionSourceForSQLiteInMemoryTesting(FluentConfiguration config) : base(config)
        {
        }

        protected void EnsureCurrentSession()
        {
            if (session == null)
                session = base.CreateSession();
        }

        public override ISession CreateSession()
        {
            EnsureCurrentSession();
            session.Clear();
            return session;
        }

        public override void BuildSchema()
        {
            BuildSchema(CreateSession());
        }

        public override void BuildSchema(bool script)
        {
            BuildSchema(CreateSession(), script);
        }
    }
}