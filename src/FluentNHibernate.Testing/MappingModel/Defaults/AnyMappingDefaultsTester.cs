using FluentNHibernate.MappingModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Defaults
{
    [TestFixture]
    public class AnyMappingDefaultsTester
    {
        [Test]
        public void InsertShouldDefaultToTrue()
        {
            var mapping = new AnyMapping();
            mapping.Insert.ShouldBeTrue();
        }

        [Test]
        public void UpdateShouldDefaultToTrue()
        {
            var mapping = new AnyMapping();
            mapping.Update.ShouldBeTrue();
        }

        [Test]
        public void OptimisticLockShouldDefaultToTrue()
        {
            var mapping = new AnyMapping();
            mapping.OptimisticLock.ShouldBeTrue();
        }

        [Test]
        public void LazyShouldDefaultToFalse()
        {
            var mapping = new AnyMapping();
            mapping.Lazy.ShouldBeFalse();
        }
    }
}
