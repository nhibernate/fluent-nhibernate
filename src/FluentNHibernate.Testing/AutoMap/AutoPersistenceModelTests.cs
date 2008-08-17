using FluentNHibernate.AutoMap;
using FluentNHibernate.AutoMap.TestFixtures;
using FluentNHibernate.Testing.Cfg;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NHibernate.Cfg;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMap
{
    [TestFixture]
    public class AutoPersistenceModelTests
    {
        private Configuration cfg;

        [SetUp]
        public void SetUp()
        {
            cfg = new Configuration();
            var configTester = new PersistenceConfigurationTester.ConfigTester();
            configTester.Dialect("NHibernate.Dialect.MsSql2005Dialect");
            configTester.ConfigureProperties(cfg);
        }

        [Test]
        public void TestAutoMapperIds()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleCustomColumn>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            new MappingTester<ExampleCustomColumn>()
                .ForMapping(autoMapper.FindMapping<ExampleCustomColumn>())
                .Element("class/id").Exists();
        }

        [Test]
        public void TestAutoMapperProperties()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            new MappingTester<ExampleClass>()
                .ForMapping(autoMapper.FindMapping<ExampleClass>())
                .Element("//property").HasAttribute("name", "LineOne");
        }

        
    }
}