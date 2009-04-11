using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.MappingModel.Conventions;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace FluentNHibernate.Testing.FluentInterface
{
    public class IntegrationTestHelper
    {
        public PersistenceModel PersistenceModel { get; set; }

        public IntegrationTestHelper()
        {
            PersistenceModel = new PersistenceModel();
            PersistenceModel.AddConvention(new NamingConvention());
        }

        public void Execute(Action<ISession> action)
        {
            var cfg = new SQLiteConfiguration()
                .InMemory()
                .ShowSql()
                .ConfigureProperties(new Configuration());

            // UGLY HACK
            var nhVersion = typeof(Configuration).Assembly.GetName().Version;
            if (!nhVersion.ToString().StartsWith("2.0."))
            {
                cfg.SetProperty("proxyfactory.factory_class",
                                "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
            }

            PersistenceModel.Configure(cfg);

            var sessionFactory = cfg.BuildSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    SchemaExport export = new SchemaExport(cfg);
                    export.Execute(true, true, false, false, session.Connection, null);
                    tx.Commit();
                }

                using (var tx = session.BeginTransaction())
                {
                    action(session);
                    tx.Commit();
                }
            }
        }
    }
}