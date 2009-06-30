using FluentNHibernate.AutoMap;
using FluentNHibernate.AutoMap.TestFixtures.SuperTypes;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMap
{
    [TestFixture]
    public class JoinedSubclassConventionTests
    {
        [Test]
        public void DefaultConventionsAreAppliedToJoinedSubClasses()
        {
            new AutoMappingTester<SuperType>(
                AutoPersistenceModel.MapEntitiesFromAssemblyOf<SuperType>()
                    .Where(x => x.Namespace == typeof(SuperType).Namespace))
                .Element("class/joined-subclass[@name='" + typeof(ExampleClass).AssemblyQualifiedName + "']/many-to-one/column")
                .HasAttribute("name", "Parent_id");
        }
    }
}