using System;
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
            var classMap = new ClassMap<Target>();
            classMap.Id(x => x.Id);
            model.Add(classMap);
            model.Conventions.Add(DefaultLazy.Always());
            model.BuildMappings()
                .First()
                .DefaultLazy.ShouldBeTrue();
        }

        [Test]
        public void NeverShouldSetDefaultLazyToFalse()
        {
            var classMap = new ClassMap<Target>();
            classMap.Id(x => x.Id);
            model.Add(classMap);
            model.Conventions.Add(DefaultLazy.Never());
            model.BuildMappings()
                .First()
                .DefaultLazy.ShouldBeFalse();
        }

        private class Target
        {
            public int Id { get; set; }
        }
    }
}