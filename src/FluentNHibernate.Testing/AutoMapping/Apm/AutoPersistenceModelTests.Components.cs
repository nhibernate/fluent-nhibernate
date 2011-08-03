using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Automapping.TestFixtures.ComponentTypes;
using FluentNHibernate.Automapping.TestFixtures.CustomTypes;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Testing.Automapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Apm
{
    [TestFixture]
    public class AutoPersistenceModelComponentTests : BaseAutoPersistenceTests
    {
        TestConfiguration_AddressComponent addressCfg;
        StubTypeSource source;

        [SetUp]
        public void CreateConfigurations()
        {
            source = new StubTypeSource(typeof(Customer), typeof(Address));
            addressCfg = new TestConfiguration_AddressComponent();
        }

        [Test]
        public void ComponentTypesAutoMapped()
        {
            var autoMapper = AutoMap.Source(source, addressCfg)
                .Conventions.Add<CustomTypeConvention>();

            new AutoMappingTester<Customer>(autoMapper)
                .Element("class/component[@name='HomeAddress']").Exists()
                .Element("class/component[@name='WorkAddress']").Exists();
        }

        [Test]
        public void ComponentPropertiesAutoMapped()
        {
            var autoMapper = AutoMap.Source(source, addressCfg)
                .Conventions.Add<CustomTypeConvention>();

            new AutoMappingTester<Customer>(autoMapper)
                .Element("class/component/property[@name='Number']").Exists()
                .Element("class/component/property[@name='Street']").Exists();
        }

        [Test]
        public void ShouldBeAbleToMapComponentHasMany()
        {
            var autoMapper = AutoMap.Source(source, addressCfg);

            new AutoMappingTester<Customer>(autoMapper)
                .Element("class/component/bag[@name='Residents']").Exists();
        }

        [Test]
        public void ComponentHasManyShouldHavePrefixedKeyColumn()
        {
            var autoMapper = AutoMap.Source(source, addressCfg);

            new AutoMappingTester<Customer>(autoMapper)
                .Element("class/component/bag[@name='Residents']/key/column").HasAttribute("name", "HomeAddress_Address_id");
        }

        [Test]
        public void ShouldntAutoMapComponentsThatArentExcludedByWhereClause()
        {
            var autoMapper = AutoMap.Source(source, addressCfg);
            var mappings = autoMapper.BuildMappings();

            var exampleClassMapping = mappings
                .SelectMany(x => x.Classes)
                .First(x => x.Type == typeof(Customer));

            exampleClassMapping.Components.ShouldContain(x => x.Name == "HomeAddress");

            var exampleParentClassMapping = mappings
                .SelectMany(x => x.Classes)
                .FirstOrDefault(x => x.Type == typeof(Address));

            exampleParentClassMapping.ShouldBeNull();
        }

        [Test]
        public void ShouldBeAbleToIgnoreComponentProperties()
        {
            var autoMapper = AutoMap.Source(source, addressCfg)
                .Override<Address>(t => t.IgnoreProperty(x => x.Number));

            new AutoMappingTester<Customer>(autoMapper)
                .Element("class/component/property[@name='Number']").DoesntExist();
        }

        [Test]
        public void OverrideShouldOverrideExistingComponent()
        {
            var autoMapper = AutoMap.Source(source, addressCfg)
                .Override<Customer>(m =>
                    m.Component(x => x.HomeAddress, c =>
                        c.Map(x => x.Number, "test")));

            new AutoMappingTester<Customer>(autoMapper)
                .Element("//component[@name='HomeAddress']/property[@name='Number']/column").HasAttribute("name", "test");
        }

        [Test]
        public void ComponentPropertiesWithUserTypeAutoMapped()
        {
            var autoMapper = AutoMap.Source(source, addressCfg)
                .Conventions.Add<CustomTypeConvention>();

            new AutoMappingTester<Customer>(autoMapper)
                .Element("class/component/property[@name='Custom']").HasAttribute("type", typeof(CustomUserType).AssemblyQualifiedName);
        }

        [Test]
        public void ComponentPropertiesAssumeComponentColumnPrefix()
        {
            var autoMapper = AutoMap.Source(source, addressCfg)
                .Conventions.Add<CustomTypeConvention>();

            new AutoMappingTester<Customer>(autoMapper)
                .Element("class/component[@name='WorkAddress']/property[@name='Number']/column").HasAttribute("name", "WorkAddress_Number");
        }

        [Test]
        public void ComponentPropertiesAssumeComponentColumnPrefixWithPropertyConvention()
        {
            var autoMapper = AutoMap.Source(source, addressCfg)
                .Conventions.Add<CustomColumnNameConvention>();

            new AutoMappingTester<Customer>(autoMapper)
                .Element("class/component[@name='WorkAddress']/property[@name='Number']/column").HasAttribute("name", "WorkAddress_Number1");
        }

        [Test]
        public void ComponentColumnConventionReceivesProperty()
        {
            var autoMapper = AutoMap.Source(source, addressCfg)
                .Conventions.Add<CustomTypeConvention>();

            new AutoMappingTester<Customer>(autoMapper)
                .Element("class/component[@name='WorkAddress']/property[@name='Number']/column")
                .HasAttribute("name", value => value.StartsWith("WorkAddress_"));
        }

        [Test]
        public void JoinedSubclassOverrideShouldOverrideExistingComponent()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
#pragma warning disable 612,618
                .Setup(x => x.IsComponentType = type => type == typeof(ExampleParentClass))
#pragma warning restore 612,618
                .Override<ExampleInheritedClass>(m => m.Component(x => x.Component, c => c.Map(x => x.ExampleParentClassId).Column("test")));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//joined-subclass/component[@name='Component']/property[@name='ExampleParentClassId']/column").HasAttribute("name", "test");
        }

        class TestConfiguration_AddressComponent : DefaultAutomappingConfiguration
        {
            public override bool IsComponent(System.Type type)
            {
                return type == typeof(Address);
            }

            public override string GetComponentColumnPrefix(Member member)
            {
                return member.Name + "_";
            }
        }

        class CustomColumnNameConvention : IPropertyConvention
        {
            public void Apply(IPropertyInstance instance)
            {
                instance.Column(instance.Name + "1");
            }
        }
    }
}