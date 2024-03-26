using System;
using FluentNHibernate.Automapping;
using NHibernate.Cfg;
using NUnit.Framework;
using static FluentNHibernate.Testing.Cfg.SQLiteFrameworkConfigurationFactory;

namespace FluentNHibernate.Testing.Automapping;

public abstract class BaseAutoMapFixture
{
    Configuration cfg;
    AutoPersistenceModel apm;

    [SetUp]
    public void CreateDatabaseCfg()
    {
        cfg = new Configuration();

        CreateStandardInMemoryConfiguration()
            .ConfigureProperties(cfg);
    }

    protected void Model<T>()
    {
        apm = AutoMap.Source(new StubTypeSource(typeof(T)));
    }

    protected void Model<T>(Action<AutoPersistenceModel> modelSetup)
    {
        apm = AutoMap.Source(new StubTypeSource(typeof(T)));
        modelSetup(apm);
    }

    protected void Test<T>(Action<AutoMappingTester<T>> mappingTester)
    {
        mappingTester(new AutoMappingTester<T>(apm));
    }
}
