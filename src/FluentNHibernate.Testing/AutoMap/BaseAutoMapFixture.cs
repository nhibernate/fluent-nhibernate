using System;
using FluentNHibernate.AutoMap;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMap
{
    public abstract class BaseAutoMapFixture
    {
        private Configuration cfg;
        private AutoPersistenceModel apm;

        [SetUp]
        public void CreateDatabaseCfg()
        {
            cfg = new Configuration();

            SQLiteConfiguration.Standard
                .InMemory()
                .ConfigureProperties(cfg);
        }

        protected void Model<T>(Action<AutoPersistenceModel> modelSetup)
        {
            apm = AutoPersistenceModel.MapEntitiesFromAssemblyOf<T>();
            modelSetup(apm);
        }

        protected void Test<T>(Action<AutoMappingTester<T>> mappingTester)
        {
            apm.Configure(cfg);
            mappingTester(new AutoMappingTester<T>(apm));
        }
    }
}