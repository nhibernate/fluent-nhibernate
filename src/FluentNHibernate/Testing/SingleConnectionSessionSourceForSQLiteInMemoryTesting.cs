using System.Collections.Generic;
using FluentNHibernate.Cfg;
using NHibernate;

namespace FluentNHibernate.Testing
{
    public class SingleConnectionSessionSourceForSQLiteInMemoryTesting : SessionSource
    {
        private ISession _session;

        public SingleConnectionSessionSourceForSQLiteInMemoryTesting(IDictionary<string, string> properties, PersistenceModel model) : base(properties, model)
        {
        }

        public SingleConnectionSessionSourceForSQLiteInMemoryTesting(FluentConfiguration config) : base(config)
        {
        }

        protected void ensure_current_session()
        {
            if (_session == null)
                _session = base.CreateSession();
        }

        public override ISession CreateSession()
        {
            ensure_current_session();
            _session.Clear();
            return _session;
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