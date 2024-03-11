using System.Collections.Generic;
using System.Data;
using System.IO;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Testing.Fixtures.Basic;
using FluentNHibernate.Testing.Fixtures.MixedMappingsInSameLocation;
using FluentNHibernate.Testing.Fixtures.MixedMappingsInSameLocation.Mappings;
using NHibernate.Cfg;
using NUnit.Framework;
using static FluentNHibernate.Testing.Cfg.SQLiteFrameworkConfigurationFactory;

namespace FluentNHibernate.Testing.Cfg;

[TestFixture]
public class ValidFluentConfigurationTests
{

    [Test]
    public void ExposeConfigurationPassesCfgInstanceIntoAction()
    {
        Fluently.Configure()
            .ExposeConfiguration(cfg =>
            {
                cfg.ShouldNotBeNull();
                cfg.ShouldBeOfType(typeof(Configuration));
            });
    }

    [Test]
    public void ExposeConfigurationHasSameCfgInstanceAsPassedInThroughConstructor()
    {
        var config = new Configuration();

        Fluently.Configure(config)
            .ExposeConfiguration(cfg => cfg.ShouldEqual(config));
    }

    [Test]
    public void ExposeConfigurationCanBeCalledMultipleTimesWithDifferentBodies()
    {
        var calls = new List<string>();

        Fluently.Configure()
            .Database(
                CreateStandardInMemoryConfiguration())
            .ExposeConfiguration(cfg => calls.Add("One"))
            .ExposeConfiguration(cfg => calls.Add("Two"))
            .BuildSessionFactory();

        calls.ShouldContain("One");
        calls.ShouldContain("Two");
    }

    [Test]
    public void DatabaseSetsPropertiesOnCfg()
    {
        Fluently.Configure()
            .Database(
                CreateStandardConfiguration())
            .ExposeConfiguration(cfg =>
            {
                cfg.Properties.ContainsKey("connection.provider").ShouldBeTrue();
            });
    }

    [Test]
    public void ExposeConfigurationGetsConfigAfterMappingsHaveBeenApplied()
    {
        Fluently.Configure()
            .Database(
                CreateStandardInMemoryConfiguration())
            .Mappings(m =>
                m.FluentMappings.AddFromAssemblyOf<Record>())
            .ExposeConfiguration(cfg =>
            {
                cfg.ClassMappings.Count.ShouldBeGreaterThan(0);
            });
    }

    [Test]
    public void MappingsShouldPassInstanceOfMappingConfigurationToAction()
    {
        Fluently.Configure()
            .Mappings(m =>
            {
                m.ShouldNotBeNull();
                m.ShouldBeOfType(typeof(MappingConfiguration));
            });
    }

    [Test]
    public void MappingsCanBeMixedAndDontConflict()
    {
        var sessionFactory = Fluently.Configure()
            .Database(
                CreateStandardInMemoryConfiguration())
            .Mappings(m =>
            {
                m.FluentMappings.Add<FooMap>();
                m.AutoMappings.Add(AutoMap.AssemblyOf<Bar>()
                    .Where(t => t.Namespace == typeof(Bar).Namespace));
            })
            .BuildSessionFactory();

        sessionFactory.ShouldNotBeNull();
    }

    [Test]
    public void ShouldGetASessionFactoryIfEverythingIsOK()
    {
        var sessionFactory = Fluently.Configure()
            .Database(
                CreateStandardInMemoryConfiguration())
            .Mappings(m =>
                m.FluentMappings.Add<BinaryRecordMap>())
            .BuildSessionFactory();

        sessionFactory.ShouldNotBeNull();
    }

    [Test]
    public void ShouldGetAConfigurationIfEverythingIsOK()
    {
        var configuration = Fluently.Configure()
            .Database(
                CreateStandardInMemoryConfiguration())
            .Mappings(m =>
                m.FluentMappings.Add<BinaryRecordMap>())
            .BuildConfiguration();

        configuration.ShouldNotBeNull();
    }

    [Test]
    public void ShouldSetCurrentSessionContext()
    {
        var configuration = Fluently.Configure()
            .CurrentSessionContext("thread_static")
            .BuildConfiguration();

        configuration.Properties["current_session_context_class"].ShouldEqual("thread_static");
    }

    [Test]
    public void ShouldSetCurrentSessionContextUsingGeneric()
    {
        var configuration = Fluently.Configure()
            .CurrentSessionContext<NHibernate.Context.ThreadStaticSessionContext>()
            .BuildConfiguration();

        configuration.Properties["current_session_context_class"].ShouldEqual(typeof(NHibernate.Context.ThreadStaticSessionContext).AssemblyQualifiedName);
    }



    [Test]
    public void ShouldSetConnectionIsolationLevel()
    {
        var configuration = Fluently.Configure()
            .Database(
                CreateStandardConfiguration(IsolationLevel.ReadUncommitted))
            .BuildConfiguration();

        configuration.Properties["connection.isolation"].ShouldEqual("ReadUncommitted");
    }

    [Test]
    public void Use_Minimal_Puts_should_set_value_to_const_true()
    {
        var configuration = Fluently.Configure()
            .Cache(x => x.UseMinimalPuts())
            .BuildConfiguration();

        configuration.Properties.ShouldContain("cache.use_minimal_puts", "true");
    }

    [Test]
    public void Use_Query_Cache_should_set_value_to_const_true()
    {
        var configuration = Fluently.Configure()
            .Cache(x => x.UseQueryCache())
            .BuildConfiguration();

        configuration.Properties.ShouldContain("cache.use_query_cache", "true");
    }

    [Test]
    public void Query_Cache_Factory_should_set_property_value()
    {
        var configuration = Fluently.Configure()
            .Cache(x => x.QueryCacheFactory("foo"))
            .BuildConfiguration();

        configuration.Properties.ShouldContain("cache.query_cache_factory", "foo");
    }

    [Test]
    public void Region_Prefix_should_set_property_value()
    {
        var configuration = Fluently.Configure()
            .Cache(x => x.RegionPrefix("foo"))
            .BuildConfiguration();

        configuration.Properties.ShouldContain("cache.region_prefix", "foo");
    }

    [Test]
    public void Provider_Class_should_set_property_value()
    {
        var configuration = Fluently.Configure()
            .Cache(x => x.ProviderClass("foo"))
            .BuildConfiguration();
    }
}

[TestFixture]
public class InvalidFluentConfigurationTests
{
    private const string ExceptionMessage = "An invalid or incomplete configuration was used while creating a SessionFactory. Check PotentialReasons collection, and InnerException for more detail.";
    private const string ExceptionDatabaseMessage = "Database was not configured through Database method.";
    private const string ExceptionMappingMessage = "No mappings were configured through the Mappings method.";

    [Test]
    public void BuildSessionFactoryShouldThrowIfCalledBeforeAnythingSetup()
    {
        var ex = Assert.Throws<FluentConfigurationException>(() =>
            Fluently.Configure()
                .BuildSessionFactory());

        ex.Message.ShouldStartWith(ExceptionMessage);
    }

    [Test]
    public void ExceptionShouldContainDbIfDbNotSetup()
    {
        var ex = Assert.Throws<FluentConfigurationException>(() =>
            Fluently.Configure()
                .BuildSessionFactory());

        ex.PotentialReasons.ShouldContain(ExceptionDatabaseMessage);
    }

    [Test]
    public void ExceptionShouldntContainDbIfDbSetup()
    {
        var ex = Assert.Throws<FluentConfigurationException>(() =>
            Fluently.Configure()
                .Database(
                    CreateStandardConfiguration())
                .BuildSessionFactory());

        ex.PotentialReasons.Contains(ExceptionDatabaseMessage)
            .ShouldBeFalse();
    }

    [Test]
    public void ExceptionShouldContainMappingsIfMappingsNotSetup()
    {
        var ex = Assert.Throws<FluentConfigurationException>(() =>
            Fluently.Configure()
                .Database(
                    CreateStandardConfiguration())
                .BuildSessionFactory());

        ex.PotentialReasons.ShouldContain(ExceptionMappingMessage);
    }

    [Test]
    public void ExceptionShouldntContainMappingsIfMappingsSetup()
    {
        var ex = Assert.Throws<FluentConfigurationException>(() =>
            Fluently.Configure()
                .Database(
                    CreateStandardConfiguration())
                .Mappings(m =>
                    m.FluentMappings.AddFromAssemblyOf<Record>())
                .BuildSessionFactory());

        ex.PotentialReasons.Contains(ExceptionMappingMessage)
            .ShouldBeFalse();
    }

    [Test]
    public void InnerExceptionSet()
    {
        var ex = Assert.Throws<FluentConfigurationException>(() =>
            Fluently.Configure()
                .Database(
                    CreateStandardConfiguration())
                .Mappings(m =>
                    m.FluentMappings.AddFromAssemblyOf<Record>())
                .BuildSessionFactory());

        ex.InnerException.ShouldNotBeNull();
    }
}

[TestFixture]
public class FluentConfigurationWriteMappingsTests
{
    private string ExportPath;

    [SetUp]
    public void CreateTempDir()
    {
        ExportPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(ExportPath);
    }

    [TearDown]
    public void DestroyTempDir()
    {
        Directory.Delete(ExportPath, true);
    }

    [Test]
    public void WritesFluentMappingsOut()
    {
        Fluently.Configure()
            .Database(
                CreateStandardInMemoryConfiguration())
            .Mappings(m =>
                m.FluentMappings
                    .Add<BinaryRecordMap>()
                    .ExportTo(ExportPath))
            .BuildConfiguration();

        Directory.GetFiles(ExportPath)
            .ShouldContain(HbmFor<BinaryRecord>);
    }

    [Test]
    public void WritesFluentMappingsOutToTextWriter()
    {
        var stringWriter = new StringWriter();

        Fluently.Configure()
            .Database(
                CreateStandardInMemoryConfiguration())
            .Mappings(m =>
                m.FluentMappings
                    .Add<BinaryRecordMap>()
                    .ExportTo(stringWriter))
            .BuildConfiguration();

        string export = stringWriter.ToString();
        export.ShouldNotBeEmpty();
    }

    [Test]
    public void WritesFluentMappingsOutMergedWhenFlagSet()
    {
        Fluently.Configure()
            .Database(
                CreateStandardInMemoryConfiguration())
            .Mappings(m =>
                m.MergeMappings()
                    .FluentMappings
                    .Add<BinaryRecordMap>()
                    .ExportTo(ExportPath))
            .BuildConfiguration();

        Directory.GetFiles(ExportPath)
            .ShouldContain(x => Path.GetFileName(x) == "FluentMappings.hbm.xml");
    }

    [Test]
    public void WritesAutoMappingsOut()
    {
        Fluently.Configure()
            .Database(
                CreateStandardInMemoryConfiguration())
            .Mappings(m =>
                m.AutoMappings.Add(AutoMap.AssemblyOf<Person>()
                        .Where(type => type.Namespace == "FluentNHibernate.Testing.Fixtures.Basic"))
                    .ExportTo(ExportPath))
            .BuildSessionFactory();

        Directory.GetFiles(ExportPath)
            .ShouldContain(HbmFor<Person>);
    }

    [Test]
    public void WritesAutoMappingsOutMergedWhenFlagSet()
    {
        Fluently.Configure()
            .Database(
                CreateStandardInMemoryConfiguration())
            .Mappings(m =>
                m.MergeMappings()
                    .AutoMappings.Add(AutoMap.AssemblyOf<Person>()
                        .Where(type => type.Namespace == "FluentNHibernate.Testing.Fixtures.Basic"))
                    .ExportTo(ExportPath))
            .BuildSessionFactory();

        Directory.GetFiles(ExportPath)
            .ShouldContain(x => Path.GetFileName(x) == "AutoMappings.hbm.xml");
    }

    [Test]
    public void WritesBothOut()
    {
        Fluently.Configure()
            .Database(
                CreateStandardInMemoryConfiguration())
            .Mappings(m =>
            {
                m.FluentMappings
                    .Add<BinaryRecordMap>()
                    .ExportTo(ExportPath);

                m.AutoMappings.Add(AutoMap.Source(new StubTypeSource(typeof(Person)))
                        .Where(type => type.Namespace == "FluentNHibernate.Testing.Fixtures.Basic"))
                    .ExportTo(ExportPath);
            })
            .BuildSessionFactory();

        var files = Directory.GetFiles(ExportPath);

        files.ShouldContain(HbmFor<BinaryRecord>);
        files.ShouldContain(HbmFor<Person>);
    }

    [Test]
    public void DoesNotThrowWhenExportToIsBeforeBuildConfigurationOnCachePartMapping()
    {
        //Regression test for isue 131
        Fluently.Configure()
            .Database(
                CreateStandardInMemoryConfiguration())
            .Mappings(m =>
                m.FluentMappings
                    .Add<CachedRecordMap>()
                    .ExportTo(ExportPath))
            .BuildConfiguration();
    }

    private static bool HbmFor<T>(string path)
    {
        return Path.GetFileName(path) == typeof(T).FullName + ".hbm.xml";
    }
}
