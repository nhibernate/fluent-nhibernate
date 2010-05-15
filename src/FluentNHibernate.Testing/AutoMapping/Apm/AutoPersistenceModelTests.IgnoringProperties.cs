using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Testing.Automapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Apm
{
    [TestFixture]
    public partial class AutoPersistenceModelTests
    {
        #region ignoring properties

        [Test]
        public void TestAutoMapIgnoresProperties()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Override<ExampleCustomColumn>(c => c.IgnoreProperty(p => p.ExampleCustomColumnId));

            new AutoMappingTester<ExampleCustomColumn>(autoMapper)
                .Element("//property[@name='ExampleCustomColumnId']").DoesntExist();
        }

        [Test]
        public void ShouldBeAbleToIgnorePropertiesRegardlessOfType()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleInheritedClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .OverrideAll(t => t.IgnoreProperty("Dummy"));

            new AutoMappingTester<ClassWithDummyProperty>(autoMapper)
                .Element("class/property[@name='Dummy']").DoesntExist();

            new AutoMappingTester<AnotherClassWithDummyProperty>(autoMapper)
                .Element("class/property[@name='Dummy']").DoesntExist();
        }

        [Test]
        public void ShouldBeAbleToIgnoreMultiplePropertiesRegardlessOfType()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleInheritedClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .OverrideAll(t => t.IgnoreProperties("Dummy", "Dummy1", "Dummy2"));

            new AutoMappingTester<ClassWithDummyProperty>(autoMapper)
                .Element("class/property[@name='Dummy']").DoesntExist()
                .Element("class/property[@name='Dummy1']").DoesntExist()
                .Element("class/property[@name='Dummy2']").DoesntExist();

            new AutoMappingTester<AnotherClassWithDummyProperty>(autoMapper)
                .Element("class/property[@name='Dummy']").DoesntExist()
                .Element("class/property[@name='Dummy1']").DoesntExist()
                .Element("class/property[@name='Dummy2']").DoesntExist();
        }

        [Test]
        public void ShouldBeAbleToIgnoreMultiplePropertiesByDelegateRegardlessOfType()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleInheritedClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .OverrideAll(t => t.IgnoreProperties(x => x.Name.Contains("Dummy")));

            new AutoMappingTester<ClassWithDummyProperty>(autoMapper)
                .Element("class/property[@name='Dummy']").DoesntExist()
                .Element("class/property[@name='Dummy1']").DoesntExist()
                .Element("class/property[@name='Dummy2']").DoesntExist();

            new AutoMappingTester<AnotherClassWithDummyProperty>(autoMapper)
                .Element("class/property[@name='Dummy']").DoesntExist()
                .Element("class/property[@name='Dummy1']").DoesntExist()
                .Element("class/property[@name='Dummy2']").DoesntExist();
        }

        [Test]
        public void ShouldAllowIgnoresInOverrides()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .UseOverridesFromAssemblyOf<IgnorerOverride>();

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/property[@name='LineOne']").DoesntExist();
        }

        #endregion
    }
}