using FluentNHibernate.MappingModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Collections
{
    [TestFixture]
    public class KeyMappingTester
    {
        [Test]
        public void CanConstructValidInstance()
        {
            var keyMapping = new KeyMapping();
            keyMapping.ShouldBeValidAgainstSchema();
        }
    }
}