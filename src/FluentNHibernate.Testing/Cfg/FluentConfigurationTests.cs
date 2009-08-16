using System;
using System.Collections.Generic;
using System.IO;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Testing.Fixtures.Basic;
using FluentNHibernate.Testing.Fixtures.MixedMappingsInSameLocation;
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
                .Database(SQLiteConfiguration.Standard.InMemory)
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
        public void MappingsCanBeMixedAndDontConflict()
        {
            var sessionFactory = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory)
                .Mappings(m =>
                {
                    m.FluentMappings.AddFromAssemblyOf<Foo>();
                    m.AutoMappings.Add(AutoMap.AssemblyOf<Bar>(t => t.Namespace == typeof(Bar).Namespace));
                })
                .BuildSessionFactory();

            sessionFactory.ShouldNotBeNull();
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

		[Test]
		public void ShouldGetAConfigurationIfEverythingIsOK()
		{
			var configuration = Fluently.Configure()
				.Database(SQLiteConfiguration.Standard.InMemory)
				.Mappings(m =>
					m.FluentMappings.AddFromAssemblyOf<Record>())
				.BuildConfiguration();

			configuration.ShouldNotBeNull();
		}

    	[Test]
    	public void ShouldSetCurrentSessionContext()
    	{
			var configuration = Fluently.Configure()
				.Database(SQLiteConfiguration.Standard.CurrentSessionContext("thread_static").InMemory)
				.BuildConfiguration();

			configuration.Properties["current_session_context_class"].ShouldEqual("thread_static");
    	}

    	[Test]
    	public void ShouldSetCurrentSessionContextUsingGeneric()
    	{
			var configuration = Fluently.Configure()
				.Database(SQLiteConfiguration.Standard.CurrentSessionContext<NHibernate.Context.ThreadStaticSessionContext>())
				.BuildConfiguration();

			configuration.Properties["current_session_context_class"].ShouldEqual(typeof(NHibernate.Context.ThreadStaticSessionContext).AssemblyQualifiedName);
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
                .BuildConfiguration();

            Directory.GetFiles(ExportPath)
                .ShouldContain(HbmFor<Record>);
        }

        [Test]
        public void WritesFluentMappingsOutMergedWhenFlagSet()
        {
            Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory)
                .Mappings(m =>
                    m.MergeMappings()
                     .FluentMappings
                         .AddFromAssemblyOf<Record>()
                         .ExportTo(ExportPath))
                .BuildConfiguration();

            Directory.GetFiles(ExportPath)
                .ShouldContain(x => Path.GetFileName(x) == "FluentMappings.hbm.xml");
        }

        [Test]
        public void WritesAutoMappingsOut()
        {
            Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory)
                .Mappings(m =>
                    m.AutoMappings.Add(AutoMap.AssemblyOf<Person>(type => type.Namespace == "FluentNHibernate.Testing.Fixtures.Basic"))
                        .ExportTo(ExportPath))
                .BuildSessionFactory();

            Directory.GetFiles(ExportPath)
                .ShouldContain(HbmFor<Person>);
        }

        [Test]
        public void WritesAutoMappingsOutMergedWhenFlagSet()
        {
            Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory)
                .Mappings(m =>
                    m.MergeMappings()
                     .AutoMappings.Add(AutoMap.AssemblyOf<Person>(type => type.Namespace == "FluentNHibernate.Testing.Fixtures.Basic"))
                     .ExportTo(ExportPath))
                .BuildSessionFactory();

            Directory.GetFiles(ExportPath)
                .ShouldContain(x => Path.GetFileName(x) == "AutoMappings.hbm.xml");
        }

        [Test]
        public void WritesBothOut()
        {
            Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory())
                .Mappings(m =>
                {
                    m.FluentMappings
                        .AddFromAssemblyOf<Record>()
                        .ExportTo(ExportPath);

                    m.AutoMappings.Add(AutoMap.AssemblyOf<Person>(type => type.Namespace == "FluentNHibernate.Testing.Fixtures.Basic"))
                        .ExportTo(ExportPath);
                })
                .BuildSessionFactory();

            var files = Directory.GetFiles(ExportPath);
            
            files.ShouldContain(HbmFor<Record>);
            files.ShouldContain(HbmFor<Person>);
        }

        [Test]
        public void DoesNotThrowWhenExportToIsBeforeBuildConfigurationOnCachePartMapping()
        {
            //Regression test for isue 131
            Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory)
                .Mappings(m =>
                    m.FluentMappings
                        .AddFromAssemblyOf<CachedRecord>()
                        .ExportTo(ExportPath))
                .BuildConfiguration();
        }

        private static bool HbmFor<T>(string path)
        {
            return Path.GetFileName(path) == typeof(T).FullName + ".hbm.xml";
        }
    }
}