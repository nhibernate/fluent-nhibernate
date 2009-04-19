using FluentNHibernate.AutoMap;
using FluentNHibernate.AutoMap.TestFixtures;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMap
{
    [TestFixture]
    public class VersionTests : BaseAutoPersistenceTests
    {
        private AutoMappingTester<TEntity> VerifyAutoMap<TEntity>()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<TEntity>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            return new AutoMappingTester<TEntity>(autoMapper);
        }

        [Test]
        public void PropertyNamedTimestampMappedAsVersion()
        {
            VerifyAutoMap<ValidTimestampClass>()
                .Element("//version")
                .HasAttribute("name", "Timestamp")
                .HasAttribute("column", "Timestamp");
        }

        [Test]
        public void PropertyNamedVersionMappedAsVersion()
        {
            VerifyAutoMap<ValidVersionClass>()
                .Element("//version")
                .HasAttribute("name", "Version")
                .HasAttribute("column", "Version");
        }

        [Test]
        public void PropertyNamedTimestampWithInvalidTypeNotMappedAsVersion()
        {
            VerifyAutoMap<InvalidTimestampClass>()
                .Element("//version")
                .DoesntExist();
        }

        [Test]
        public void PropertyNamedVersionWithInvalidTypeNotMappedAsVersion()
        {
            VerifyAutoMap<InvalidVersionClass>()
                .Element("//version")
                .DoesntExist();
        }
    }
}