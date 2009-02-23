using FluentNHibernate.AutoMap;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMap
{
    [TestFixture]
    public class AutoMapOneToManyTester : BaseAutoMapTester<AutoMapOneToMany>
    {
        [Test]
        public void ShouldMapSets()
        {
            ShouldMap(x => x.Set);
        }

        [Test]
        public void ShouldMapLists()
        {
            ShouldMap(x => x.List);
        }

        [Test]
        public void ShouldntMapValueTypes()
        {
            ShouldntMap(x => x.Int);
            ShouldntMap(x => x.String);
            ShouldntMap(x => x.DateTime);
        }

        [Test]
        public void ShouldntMapEntities()
        {
            ShouldntMap(x => x.Entity);
        }
    }
}