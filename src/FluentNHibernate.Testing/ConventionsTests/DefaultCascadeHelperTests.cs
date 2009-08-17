using System.Linq;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class DefaultCascadeHelperTests
    {
        private PersistenceModel model;

        [SetUp]
        public void CreatePersistenceModel()
        {
            model = new PersistenceModel();
        }

        [Test]
        public void ShouldSetDefaultAccessToValue()
        {
            model.Add(new ClassMap<Target>());
            model.Conventions.Add(DefaultCascade.All());
            model.BuildMappings()
                .First()
                .DefaultCascade.ShouldEqual("all");
        }

        private class Target
        { }
    }
}