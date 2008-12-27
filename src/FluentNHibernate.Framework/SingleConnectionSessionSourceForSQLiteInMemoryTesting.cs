using System.Collections.Generic;
using NHibernate;

namespace FluentNHibernate.Framework
{
    public class SingleConnectionSessionSourceForSQLiteInMemoryTesting : SessionSource
    {
        private ISession _session;

        public SingleConnectionSessionSourceForSQLiteInMemoryTesting(IDictionary<string, string> properties, PersistenceModel model) : base(properties, model)
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
    }
}