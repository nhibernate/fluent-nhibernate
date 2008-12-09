using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Collections
{
    [TestFixture]
    public class BagMappingTester
    {
        [Test]
        public void CanConstructValidInstance()
        {
            var bagMapping = new BagMapping("bag1", new KeyMapping(), new OneToManyMapping("class1"));
            bagMapping.ShouldBeValidAgainstSchema();
        }
    }
}