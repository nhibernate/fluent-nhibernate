using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class ClassMapOptimisticLockTester
    {
        [Test]
        public void CanOverrideOptimisticLockNone()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.OptimisticLock.None())
                .Element("class").HasAttribute("optimistic-lock", "none");
        }

        [Test]
        public void CanOverrideOptimisticLockVersion()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.OptimisticLock.Version())
                .Element("class").HasAttribute("optimistic-lock", "version");
        }

        [Test]
        public void CanOverrideOptimisticLockDirty()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.OptimisticLock.Dirty())
                .Element("class").HasAttribute("optimistic-lock", "dirty");
        }

        [Test]
        public void CanOverrideOptimisticLockAll()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.OptimisticLock.All())
                .Element("class").HasAttribute("optimistic-lock", "all");
        }
    }
}