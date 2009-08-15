using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.TestFixtures;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Automapping
{
    [TestFixture]
    public class VersionTests : BaseAutoPersistenceTests
    {
        private AutoMappingTester<TEntity> VerifyAutoMap<TEntity>()
        {
            var autoMapper = AutoMap.AssemblyOf<TEntity>(t => t == typeof(TEntity));

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