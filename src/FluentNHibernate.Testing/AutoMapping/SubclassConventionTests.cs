using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.TestFixtures.SuperTypes;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Automapping
{
    [TestFixture]
    public class SubclassConventionTests
    {
        [Test]
        public void DefaultConventionsAreAppliedToDiscriminatedSubClasses()
        {
            var model = AutoMap.AssemblyOf<SuperType>()
                .Where(x => x.Namespace == typeof(SuperType).Namespace)
                .Override<SuperType>(m => m.DiscriminateSubClassesOnColumn("Discriminator"));

            new AutoMappingTester<SuperType>(model)
                .Element("class/subclass[@name='" + typeof(ExampleClass).AssemblyQualifiedName + "']/many-to-one/column")
                .HasAttribute("name", "Parent_id");
        }

        [Test]
        public void DiscriminatedSubtypePropagatesThroughHierarchy()
        {
            var model = AutoMap.AssemblyOf<Derived1>()
                .Where(x => x.Namespace == typeof(Derived1).Namespace)
                .Override<Derived1>(m => m.DiscriminateSubClassesOnColumn("Discriminator"));

            new AutoMappingTester<Derived1>(model)
                .Element("class[@name = '" + typeof(Derived1).AssemblyQualifiedName + "']")
                .Exists()
                .Element("class/subclass[@name='" + typeof(SecondLevel).AssemblyQualifiedName + "']")
                .Exists()
                .Element("class/subclass/joined-subclass")
                .DoesntExist()
                .Element("class/subclass[@name='" + typeof(SecondLevel).AssemblyQualifiedName + "']/" +
                         "subclass[@name='" + typeof(ThirdLevel).AssemblyQualifiedName + "']")
                .Exists()
                .Element("class/subclass[@name='" + typeof(SecondLevel).AssemblyQualifiedName + "']/" +
                         "subclass[@name='" + typeof(ThirdLevel).AssemblyQualifiedName + "']/" +
                         "subclass[@name='" + typeof(FourthLevel).AssemblyQualifiedName + "']")
                .Exists();
        }
    }
}