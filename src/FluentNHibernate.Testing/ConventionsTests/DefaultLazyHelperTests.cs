using System.Linq;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class DefaultLazyHelperTests
    {
        private PersistenceModel model;

        [SetUp]
        public void CreatePersistenceModel()
        {
            model = new PersistenceModel();
        }

        [Test]
        public void AlwaysShouldSetDefaultLazyToTrue()
        {
            model.Add(new ClassMap<Target>());
            model.Conventions.Add(DefaultLazy.Always());
            model.BuildMappings()
                .First()
                .DefaultLazy.ShouldBeTrue();
        }

        [Test]
        public void NeverShouldSetDefaultLazyToFalse()
        {
            model.Add(new ClassMap<Target>());
            model.Conventions.Add(DefaultLazy.Never());
            model.BuildMappings()
                .First()
                .DefaultLazy.ShouldBeFalse();
        }

        private class Target
        { }
    }
}