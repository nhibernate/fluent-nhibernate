using System;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Automapping.TestFixtures.SuperTypes;
using FluentNHibernate.Testing.Automapping;
using NUnit.Framework;
using ExampleClass = FluentNHibernate.Automapping.TestFixtures.ExampleClass;
using ExampleInheritedClass = FluentNHibernate.Automapping.TestFixtures.ExampleInheritedClass;

namespace FluentNHibernate.Testing.AutoMapping.Apm
{
    [TestFixture]
    public partial class AutoPersistenceModelTests : BaseAutoPersistenceTests
    {
#pragma warning disable 612,618

        #region inheritance

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Could not find mapping for class 'SuperType'")]
        public void TestInheritanceMappingSkipsSuperTypes()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures.SuperTypes")
                .IgnoreBase<SuperType>();

            new AutoMappingTester<SuperType>(autoMapper);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Could not find mapping for class 'SuperType'")]
        public void TestInheritanceSubclassMappingSkipsSuperTypes()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures.SuperTypes")
                .IgnoreBase<SuperType>()
                .Setup(c =>
                {
                    c.IsDiscriminated = type => true;
                });

            new AutoMappingTester<SuperType>(autoMapper);
        }

        [Test]
        public void TestInheritanceMapping()
        {
            AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");
        }

        [Test]
        public void TestInheritanceSubclassMapping()
        {
            AutoMap.AssemblyOf<ExampleClass>()
                .Setup(x => x.IsDiscriminated = type => true)
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");
        }

        [Test]
        public void TestInheritanceMappingProperties()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass/property[@name='ExampleProperty']").Exists();
        }

        [Test]
        public void TestInheritanceMappingPropertiesWithSameSignatureOnDifferentSubClasses()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleBaseClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            const string propertyPathFormat = "class/joined-subclass[@name='{0}']/property[@name='PropertyAlsoOnSiblingInheritedClass']";
            string firstSubClassPropertyPath = string.Format
                (propertyPathFormat, typeof(FirstInheritedClass).AssemblyQualifiedName);
            string secondSubClassPropertyPath = string.Format
                (propertyPathFormat, typeof(SecondInheritedClass).AssemblyQualifiedName);
            new AutoMappingTester<ExampleBaseClass>(autoMapper)
                .Element(firstSubClassPropertyPath).Exists().RootElement.Element(secondSubClassPropertyPath).Exists();
        }

        [Test]
        public void TestInheritanceSubclassMappingProperties()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Setup(x => x.IsDiscriminated = type => true)
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/subclass/property[@name='ExampleProperty']").Exists();
        }

        [Test]
        public void TestDoNotAddJoinedSubclassesForConcreteBaseTypes()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Setup(c =>
                    c.IsConcreteBaseType = b =>
                        b == typeof(ExampleClass));

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass").DoesntExist();
        }

        [Test]
        public void TestDoNotAddSubclassesForConcreteBaseTypes()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Setup(c =>
                {
                    c.IsConcreteBaseType = b => b == typeof(ExampleClass);
                    c.IsDiscriminated = type => true;
                });

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/subclass").DoesntExist();
        }

        [Test]
        public void TestClassIsMappedForConcreteSubClass()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Setup(c =>
                    c.IsConcreteBaseType = b =>
                        b == typeof(ExampleClass));

            new AutoMappingTester<ExampleInheritedClass>(autoMapper)
                .Element("class")
                .HasAttribute("name", typeof(ExampleInheritedClass).AssemblyQualifiedName)
                .Exists();
        }

        [Test]
        public void TestClassIsMappedForConcreteSubClassWithSubclass()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .Setup(c =>
                {
                    c.IsConcreteBaseType = b => b == typeof(ExampleClass);
                    c.IsDiscriminated = type => true;
                });

            new AutoMappingTester<ExampleInheritedClass>(autoMapper)
                .Element("class")
                .HasAttribute("name", typeof(ExampleInheritedClass).AssemblyQualifiedName)
                .Exists();
        }

        [Test]
        public void TestInheritanceMappingDoesntIncludeBaseTypeProperties()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass")
                .ChildrenDontContainAttribute("name", "LineOne");
        }

        [Test]
        public void TestInheritanceMappingDoesntIncludeBaseTypePropertiesWithSubclass()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Setup(x => x.IsDiscriminated = type => true)
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/subclass")
                .ChildrenDontContainAttribute("name", "LineOne");
        }

        [Test]
        public void TestInheritanceOverridingMappingProperties()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Override<ExampleClass>(t => t.JoinedSubClass<ExampleInheritedClass>("OverridenKey", p => p.Map(c => c.ExampleProperty, "columnName")))
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass")
                .ChildrenDontContainAttribute("name", "LineOne");
        }

        [Test]
        public void TestInheritanceSubclassOverridingMappingProperties()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Setup(x => x.IsDiscriminated = type => true)
                .Override<ExampleClass>(t => t.SubClass<ExampleInheritedClass>("discriminator", p => p.Map(c => c.ExampleProperty, "columnName")))
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/subclass")
                .ChildrenDontContainAttribute("name", "LineOne");
        }

        [Test]
        public void SubclassShouldntRemapPropertiesMappedInParent()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleInheritedClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/property[@name='LineOne']").Exists()
                .Element("class/joined-subclass/property[@name='LineOne']").DoesntExist();
        }

        #endregion

#pragma warning restore 612,618
    }
}