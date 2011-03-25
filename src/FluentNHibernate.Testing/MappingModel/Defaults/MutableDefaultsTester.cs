using FluentNHibernate.MappingModel.Collections;
using NUnit.Framework;
using FluentNHibernate.MappingModel.ClassBased;
namespace FluentNHibernate.Testing.MappingModel.Defaults
{
    [TestFixture]
    public class MutableDefaultsTester
    {
        [Test]
        public void MutableShouldBeTrueByDefaultOnClassMapping()
        {
            var mapping = new ClassMapping();
            mapping.Mutable.ShouldBeTrue();
        }

        [Test]
        public void MutableShouldBeTrueByDefaultOnCollectionMapping()
        {
            var mapping = CollectionMapping.Bag();
            mapping.Mutable.ShouldBeTrue();
        }
    }
}