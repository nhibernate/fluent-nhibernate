using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.TestFixtures.SuperTypes;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Automapping
{
    [TestFixture]
    public class UnionSubclassConventionTests
    {
        [Test]
        public void DefaultConventionsAreAppliedToUnionSubClasses()
        {
            new AutoMappingTester<SuperType>(
                AutoMap.AssemblyOf<SuperType>()
                    .Where(x => x.Namespace == typeof(SuperType).Namespace)
                    .Override<SuperType>(m => m.UseUnionSubclassForInheritanceMapping()))
                .Element("class/union-subclass[@name='" + typeof(ExampleClass).AssemblyQualifiedName + "']/many-to-one/column")
                .HasAttribute("name", "Parent_id");
            //.Element("class/union-subclass").Exists();
        }

        [Test]
        public void UnionSubtypePropagatesThroughHierarchy()
        {
            var model = AutoMap.AssemblyOf<Derived1>()
                .Where(x => x.Namespace == typeof(Derived1).Namespace)
                .Override<Derived1>(m => m.UseUnionSubclassForInheritanceMapping());

            new AutoMappingTester<Derived1>(model)
                .Element("class[@name = '" + typeof(Derived1).AssemblyQualifiedName + "']")
                .Exists()
                .Element("class/union-subclass[@name='" + typeof(SecondLevel).AssemblyQualifiedName + "']")
                .Exists()
                .Element("class/union-subclass/joined-subclass")
                .DoesntExist()
                .Element("class/union-subclass[@name='" + typeof(SecondLevel).AssemblyQualifiedName + "']/" +
                          "union-subclass[@name='" + typeof(ThirdLevel).AssemblyQualifiedName + "']")
                .Exists()
                .Element("class/union-subclass[@name='" + typeof(SecondLevel).AssemblyQualifiedName + "']/" +
                          "union-subclass[@name='" + typeof(ThirdLevel).AssemblyQualifiedName + "']/" +
                          "union-subclass[@name='" + typeof(FourthLevel).AssemblyQualifiedName + "']")
                .Exists();
        }
    }
}