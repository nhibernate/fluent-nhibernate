using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.TestFixtures.SuperTypes;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Automapping
{
    [TestFixture]
    public class UnionSubclassTests
    {
        [Test]
        public void WhenOverloadedWithUseUnionSubclassForInheritanceMappingUnionSubclassElementShouldExists()
        {
            new AutoMappingTester<SuperType>(
                AutoMap.AssemblyOf<SuperType>()
                    .Where(x => x.Namespace == typeof(SuperType).Namespace)
                    .Override<SuperType>(m => m.UseUnionSubclassForInheritanceMapping()))
                .Element("class/union-subclass").Exists();
        }
    }
}