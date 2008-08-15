using System.Configuration;
using System.Reflection;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Configuration=NHibernate.Cfg.Configuration;

namespace FluentNHibernate.QuickStart.Domain.DataAccess
{
    public class NHibernateSessionManager
    {
        private readonly string _mappingAssembly;
        private readonly ISessionFactory _sessionFactory;

        public NHibernateSessionManager()
        {
            _mappingAssembly = ConfigurationManager.AppSettings["NHibernate.Mapping.Assembly"];
            _sessionFactory = GetSessionFactory();
        }

        public ISession GetSession()
        {
            return _sessionFactory.OpenSession();
        }

        private ISessionFactory GetSessionFactory()
        {
            Configuration cfg = new Configuration().Configure();
            var persistenceModel = new PersistenceModel();
            persistenceModel.addMappingsFromAssembly(Assembly.Load(_mappingAssembly));
            persistenceModel.Configure(cfg);
            return cfg.BuildSessionFactory();
        }

        internal static void ExportSchema()
        {
            Configuration cfg = new Configuration().Configure();
            var persistenceModel = new PersistenceModel();
            persistenceModel.addMappingsFromAssembly(
                Assembly.Load(ConfigurationManager.AppSettings["NHibernate.Mapping.Assembly"]));
            persistenceModel.Configure(cfg);
            new SchemaExport(cfg).Create(true, true);
        }
    }
}