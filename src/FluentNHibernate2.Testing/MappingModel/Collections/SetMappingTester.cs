using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Collections
{
    [TestFixture]
    public class SetMappingTester
    {
        [Test]
        public void CanConstructValidInstance()
        {
            var setMapping = new SetMapping("set1", new KeyMapping(), new OneToManyMapping("class1"));
            setMapping.ShouldBeValidAgainstSchema();
        }
    }
}