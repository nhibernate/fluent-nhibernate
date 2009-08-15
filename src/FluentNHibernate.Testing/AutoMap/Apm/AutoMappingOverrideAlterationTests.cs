using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Testing.Fixtures.AutoMappingAlterations.Model;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Automapping.Apm
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
            var model = AutoMap.AssemblyOf<Baz>()
                .Where(t => t.Namespace == typeof(Baz).Namespace)
                .Alterations(x => x.Add(alteration));

            new AutoMappingTester<Baz>(model)
                .Element("class").HasAttribute("batch-size", "10");
        }

        [Test]
        public void RegularAutoMappingsStillWorkWhenOverridesApplied()
        {
            var model = AutoMap.AssemblyOf<Baz>()
                .Where(t => t.Namespace == typeof(Baz).Namespace);

            alteration.Alter(model);

            new AutoMappingTester<Baz>(model)
                .Element("class/property[@name='Name']").Exists();
        }
    }
}
