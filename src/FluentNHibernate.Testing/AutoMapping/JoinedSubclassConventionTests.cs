using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.TestFixtures.SuperTypes;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Automapping
{
    [TestFixture]
    public class JoinedSubclassConventionTests
    {
        [Test]
        public void DefaultConventionsAreAppliedToJoinedSubClasses()
        {
            new AutoMappingTester<SuperType>(
                AutoMap.AssemblyOf<SuperType>()
                    .Where(x => x.Namespace == typeof(SuperType).Namespace))
                .Element("class/joined-subclass[@name='" + typeof(ExampleClass).AssemblyQualifiedName + "']/many-to-one/column")
                .HasAttribute("name", "Parent_id");
        }
    }
}