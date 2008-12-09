using FluentNHibernate.MappingModel.Collections;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Collections
{
    [TestFixture]
    public class OneToManyMappingTester
    {
        [Test]
        public void CanConstructValidInstance()
        {
            var oneToMany = new OneToManyMapping("class1");
            oneToMany.ShouldBeValidAgainstSchema();
        }
    }
}