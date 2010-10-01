using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Automapping.TestFixtures.ComponentTypes;
using FluentNHibernate.Automapping.TestFixtures.CustomTypes;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Testing.Automapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Apm
{
    [TestFixture]
    public partial class AutoPersistenceModelTests : BaseAutoPersistenceTests
    {
        #region conventions

        [Test]
        public void TestAutoMapIdUsesConvention()
        {
            var autoMapper = AutoMap.AssemblyOf<PrivateIdSetterClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Conventions.Add(new TestIdConvention());

            new AutoMappingTester<PrivateIdSetterClass>(autoMapper)
                .Element("class/id/column").HasAttribute("name", "test");
        }

        [Test]
        public void AppliesConventionsToManyToOne()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Conventions.Add(new TestM2OConvention());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//many-to-one/column").HasAttribute("name", "test");
        }

        [Test]
        public void AppliesConventionsToOneToMany()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Conventions.Add(new TestO2MConvention());

            new AutoMappingTester<ExampleParentClass>(autoMapper)
                .Element("//bag").HasAttribute("table", "test");
        }

        [Test]
        public void TestAutoMapPropertySetFindPrimaryKeyConvention()
        {
            var autoMapper = AutoMap.Source(new StubTypeSource(typeof(ExampleClass)), new TestConfiguration_IdPrefixedByType());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/id").HasAttribute("name", "ExampleClassId")
                .Element("class/id/column").HasAttribute("name", "ExampleClassId");
        }

        [Test]
        public void TestAutoMapPropertySetPrimaryKeyConvention()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Conventions.Add(PrimaryKey.Name.Is(id => id.Property.Name + "Id"));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/id").HasAttribute("name", "Id")
                .Element("class/id/column").HasAttribute("name", "IdId");
        }

        [Test]
        public void TestAutoMapClassAppliesConventions()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Conventions.Add(new TestClassConvention());

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class").HasAttribute("table", "test");
        }

        [Test]
        public void TypeConventionShouldForcePropertyToBeMapped()
        {
            var autoMapper = AutoMap.AssemblyOf<ClassWithUserType>()
                .Conventions.Add<CustomTypeConvention>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<ClassWithUserType>(autoMapper)
                .Element("class/property").HasAttribute("name", "Custom");
        }

        [Test]
        public void TypeConventionShouldForceCompositePropertyToBeMappedWithCorrectNumberOfColumns()
        {
            var autoMapper = AutoMap.AssemblyOf<ClassWithCompositeUserType>()
                .Conventions.Add<CustomCompositeTypeConvention>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures" && t != typeof(DoubleString));

            var mappedColumns = new
                AutoMappingTester<ClassWithCompositeUserType>(autoMapper)
                .Element("class/property");

            mappedColumns.HasThisManyChildNodes(2);
        }

        [Test]
        public void TypeConventionShouldUsePropertyNameAsDefaultPrefixForCompositeUserType()
        {
            var autoMapper = AutoMap.AssemblyOf<ClassWithCompositeUserType>()
                .Conventions.Add<CustomCompositeTypeConvention>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures" && t != typeof(DoubleString));

            new AutoMappingTester<ClassWithCompositeUserType>(autoMapper)
                .Element("class/property/column[1]").HasAttribute("name", "SomeStringTuple_s1")
                .Element("class/property/column[2]").HasAttribute("name", "SomeStringTuple_s2");
        }

        [Test]
        public void TypeConventionShouldAllowCompositeUserTypePrefixToBeChanged()
        {
            var autoMapper = AutoMap.AssemblyOf<ClassWithCompositeUserType>()
                .Conventions.Add<CompositeTypeConventionWithCustomPrefix>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures" && t != typeof(DoubleString));

            new AutoMappingTester<ClassWithCompositeUserType>(autoMapper)
                .Element("class/property/column[1]").HasAttribute("name", "DoubleStringWithCustomPrefix_s1")
                .Element("class/property/column[2]").HasAttribute("name", "DoubleStringWithCustomPrefix_s2");
        }

        [Test]
        public void ShouldBeAbleToOverrideKeyColumnNameOfJoinedSubclassInConvention()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleInheritedClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Conventions.Add<JoinedSubclassConvention>();

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass/key/column")
                .HasAttribute("name", "test");
        }

        [Test]
        public void ShouldBeAbleToOverrideTableNameOfJoinedSubclassInConvention()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleInheritedClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Conventions.Add<JoinedSubclassConvention>();

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass")
                .HasAttribute("table", "test-table");
        }

        [Test]
        public void ShouldOverrideGeneratorWithConventions()
        {
            var autoMapper = AutoMap.Source(new StubTypeSource(typeof(ClassWithLongId)))
                .Conventions.Add<TestIdGeneratorConvention>(); ;

            new AutoMappingTester<ClassWithLongId>(autoMapper)
                .Element("class/id/generator").HasAttribute("class", "assigned");
        }

        class TestConfiguration_IdPrefixedByType : DefaultAutomappingConfiguration
        {
            public override bool IsId(Member member)
            {
                return member.Name == member.DeclaringType.Name + "Id";
            }
        }

        #endregion
    }
}