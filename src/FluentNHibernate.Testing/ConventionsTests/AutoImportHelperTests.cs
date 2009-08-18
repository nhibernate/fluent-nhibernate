using System.Linq;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class AutoImportHelperTests
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
            model.Conventions.Add(AutoImport.Never());
            model.BuildMappings()
                .First()
                .AutoImport.ShouldEqual(false);
        }

        private class Target
        { }
    }
}