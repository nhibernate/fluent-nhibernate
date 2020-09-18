using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.TestFixtures.SuperTypes;
using FluentNHibernate.Automapping.TestFixtures.UnionChain;
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
            new AutoMappingTester<BaseUnionType>(
                AutoMap.AssemblyOf<BaseUnionType>()
                    .Where(x => x.Namespace == typeof(BaseUnionType).Namespace)
                    .Override<BaseUnionType>(m => m.UseUnionSubclassForInheritanceMapping()))
                .Element("class[@name = '" + typeof(BaseUnionType).AssemblyQualifiedName + "']")
                .Exists()
                .Element("class/union-subclass[@name='" + typeof(ChildUnionType).AssemblyQualifiedName + "']")
                .Exists()
                .Element("class/union-subclass/joined-subclass")
                .DoesntExist()
                .Element("class/union-subclass[@name='" + typeof(ChildUnionType).AssemblyQualifiedName + "']/" +
                          "union-subclass[@name='" + typeof(GrandChildUnionType).AssemblyQualifiedName + "']")
                .Exists()
                .Element("class/union-subclass[@name='" + typeof(ChildUnionType).AssemblyQualifiedName + "']/" +
                          "union-subclass[@name='" + typeof(GrandChildUnionType).AssemblyQualifiedName + "']/" +
                          "union-subclass[@name='" + typeof(GreatGrandChildUnionType).AssemblyQualifiedName + "']")
                .Exists()
                ;
        }

    }
}