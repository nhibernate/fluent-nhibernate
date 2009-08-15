using System;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Automapping
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
            apm = AutoMap.AssemblyOf<T>();
            modelSetup(apm);
        }

        protected void Test<T>(Action<AutoMappingTester<T>> mappingTester)
        {
            mappingTester(new AutoMappingTester<T>(apm));
        }
    }
}