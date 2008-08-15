using NHibernate;

namespace FluentNHibernate.QuickStart.Domain.DataAccess
{
    public class NHibernateDataProvider
    {
        private ISession _session;

        public NHibernateDataProvider(ISession session)
        {
            _session = session;
        }

        public ISession Session
        {
            set { _session = value; }
        }

        public string Save(Cat item)
        {
           return _session.Save(item) as string;
        }

        public Cat GetById(string id)
        {
            _session.Flush();
            return _session.Get<Cat>(id);
        }
    }
}