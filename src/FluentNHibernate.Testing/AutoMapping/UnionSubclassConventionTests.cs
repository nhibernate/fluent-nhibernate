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
    }
}