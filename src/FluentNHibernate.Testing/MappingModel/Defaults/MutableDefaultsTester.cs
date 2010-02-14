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
        public void MutableShouldBeTrueByDefaultOnBagMapping()
        {
            var mapping = new BagMapping();
            mapping.Mutable.ShouldBeTrue();
        }

        [Test]
        public void MutableShouldBeTrueByDefaultOnListMapping()
        {
            var mapping = new ListMapping();
            mapping.Mutable.ShouldBeTrue();
        }

        [Test]
        public void MutableShouldBeTrueByDefaultOnSetMapping()
        {
            var mapping = new SetMapping();
            mapping.Mutable.ShouldBeTrue();
        }

        [Test]
        public void MutableShouldBeTrueByDefaultOnMapMapping()
        {
            var mapping = new MapMapping();
            mapping.Mutable.ShouldBeTrue();
        }

        [Test]
        public void MutableShouldBeTrueByDefaultOnArrayMapping()
        {
            var mapping = new ArrayMapping();
            mapping.Mutable.ShouldBeTrue();
        }
        
    }
}