using FluentNHibernate.AutoMap;
using FluentNHibernate.AutoMap.Alterations;
using FluentNHibernate.AutoMap.TestFixtures;
using FluentNHibernate.Testing.Fixtures.AutoMappingAlterations.Model;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMap.Apm
{
    [TestFixture]
    public class AutoMappingOverrideAlterationTests
    {
        private AutoMappingOverrideAlteration alteration;

        [SetUp]
        public void CreateOverride()
        {
            alteration = new AutoMappingOverrideAlteration(typeof(ExampleClass).Assembly);
        }

        [Test]
        public void OverridesApplied()
        {
            var model = AutoPersistenceModel.MapEntitiesFromAssemblyOf<Baz>()
                .Where(t => t.Namespace == typeof(Baz).Namespace)
                .Alterations(x => x.Add(alteration));

            new AutoMappingTester<Baz>(model)
                .Element("class").HasAttribute("batch-size", "10");
        }

        [Test]
        public void RegularAutoMappingsStillWorkWhenOverridesApplied()
        {
            var model = AutoPersistenceModel.MapEntitiesFromAssemblyOf<Baz>()
                .Where(t => t.Namespace == typeof(Baz).Namespace);

            alteration.Alter(model);

            new AutoMappingTester<Baz>(model)
                .Element("class/property[@name='Name']").Exists();
        }
    }
}
