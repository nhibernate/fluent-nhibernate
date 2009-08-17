using System.Linq;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class MultipleHelperConventions
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
            model.Conventions.Add(DefaultCascade.All());
            
            var mapping = model.BuildMappings().First();

            mapping.DefaultLazy.ShouldBeTrue();
            mapping.DefaultCascade.ShouldEqual("all");
        }

        private class Target
        { }
    }
}