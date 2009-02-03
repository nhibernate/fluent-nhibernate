using System.IO;
using FluentNHibernate.AutoMap;
using FluentNHibernate.AutoMap.TestFixtures;
using FluentNHibernate.AutoMap.TestFixtures.ComponentTypes;
using FluentNHibernate.AutoMap.TestFixtures.CustomTypes;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Testing.Fixtures.Basic;
using NHibernate.Cfg;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Cfg
{
    [TestFixture]
    public class ValidFluentConfigurationTests
    {
        [Test]
        public void ExposeConfigurationPassesCfgInstanceIntoAction()
        {
            Fluently.Configure()
                .ExposeConfiguration(cfg =>
                {
                    SpecificationExtensions.ShouldNotBeNull(cfg);;
                    SpecificationExtensions.ShouldBeOfType(cfg, typeof(Configuration));
                });
        }

        [Test]
        public void DatabaseSetsPropertiesOnCfg()
        {
            Fluently.Configure()
                .Database(SQLiteConfiguration.Standard)
                .ExposeConfiguration(cfg =>
                {
                    cfg.Properties.ContainsKey("connection.provider").ShouldBeTrue();
                });
        }

        [Test]
        public void ExposeConfigurationGetsConfigAfterMappingsHaveBeenApplied()
        {
            Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory)
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
        public void ShouldGetASessionFactoryIfEverythingIsOK()
        {
            var sessionFactory = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory)
                .Mappings(m =>
                    m.FluentMappings.AddFromAssemblyOf<Record>())
                .BuildSessionFactory();

            sessionFactory.ShouldNotBeNull();
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
                    .Database(SQLiteConfiguration.Standard)
                    .BuildSessionFactory());

            ex.PotentialReasons.Contains(ExceptionDatabaseMessage)
                .ShouldBeFalse();
        }

        [Test]
        public void ExceptionShouldContainMappingsIfMappingsNotSetup()
        {
            var ex = Assert.Throws<FluentConfigurationException>(() =>
                Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard)
                    .BuildSessionFactory());

            ex.PotentialReasons.ShouldContain(ExceptionMappingMessage);
        }

        [Test]
        public void ExceptionShouldntContainMappingsIfMappingsSetup()
        {
            var ex = Assert.Throws<FluentConfigurationException>(() =>
                Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard)
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
                    .Database(SQLiteConfiguration.Standard)
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
                .Database(SQLiteConfiguration.Standard.InMemory)
                .Mappings(m =>
                    m.FluentMappings
                        .AddFromAssemblyOf<Record>()
                        .ExportTo(ExportPath))
                .BuildSessionFactory();

            Directory.GetFiles(ExportPath).ShouldContain(HbmFor<Record>);
        }

        [Test]
        public void WritesAutoMappingsOut()
        {
            Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory)
                .Mappings(m =>
                    m.AutoMappings.Add(AutoPersistenceModel.MapEntitiesFromAssemblyOf<Person>()
                            .Where(type => type.Namespace == "FluentNHibernate.Testing.Fixtures.Basic"))
                        .ExportTo(ExportPath))
                .BuildSessionFactory();

            Directory.GetFiles(ExportPath).ShouldContain(HbmFor<Person>);
        }

        [Test]
        public void WritesBothOut()
        {
            Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory)
                .Mappings(m =>
                {
                    m.FluentMappings
                        .AddFromAssemblyOf<Record>()
                        .ExportTo(ExportPath);

                    m.AutoMappings.Add(AutoPersistenceModel.MapEntitiesFromAssemblyOf<Person>()
                        .Where(type => type.Namespace == "FluentNHibernate.Testing.Fixtures.Basic"))
                        .ExportTo(ExportPath);
                })
                .BuildSessionFactory();

            var files = Directory.GetFiles(ExportPath);
            
            files.ShouldContain(HbmFor<Record>);
            files.ShouldContain(HbmFor<Person>);
        }

        private static bool HbmFor<T>(string path)
        {
            return Path.GetFileName(path) == typeof(T).FullName + ".hbm.xml";
        }
    }
}