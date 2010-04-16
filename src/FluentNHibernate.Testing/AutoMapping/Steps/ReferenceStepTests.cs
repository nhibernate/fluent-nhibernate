using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.Testing.Automapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Steps
{
    [TestFixture]
    public class ReferenceStepTests : BaseAutoMapTester<ReferenceStep>
    {
        [Test]
        public void ShouldntMapSets()
        {
            ShouldntMap(x => x.Set);
        }

        [Test]
        public void ShouldntMapLists()
        {
            ShouldntMap(x => x.List);
        }

        [Test]
        public void ShouldntMapValueTypes()
        {
            ShouldntMap(x => x.Int);
            ShouldntMap(x => x.String);
            ShouldntMap(x => x.DateTime);
        }

        [Test]
        public void ShouldMapEntities()
        {
            ShouldMap(x => x.Entity);
        }
    }
}