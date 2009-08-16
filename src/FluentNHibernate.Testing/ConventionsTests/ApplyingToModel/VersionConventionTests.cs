using System;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Automapping.TestFixtures.CustomTypes;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.ApplyingToModel
{
    [TestFixture]
    public class VersionConventionTests
    {
        private PersistenceModel model;

        [SetUp]
        public void CreatePersistenceModel()
        {
            model = new PersistenceModel();
        }

        [Test]
        public void ShouldSetAccessProperty()
        {
            Convention(x => x.Access.Property());

            VerifyModel(x => x.Access.ShouldEqual("property"));
        }

        [Test]
        public void ShouldSetColumnProperty()
        {
            Convention(x => x.Column("xxx"));

            VerifyModel(x => x.Column.ShouldEqual("xxx"));
        }

        [Test]
        public void ShouldSetGeneratedProperty()
        {
            Convention(x => x.Generated.Never());

            VerifyModel(x => x.Generated.ShouldEqual("never"));
        }

        [Test]
        public void ShouldSetUnsavedValueProperty()
        {
            Convention(x => x.UnsavedValue("one"));

            VerifyModel(x => x.UnsavedValue.ShouldEqual("one"));
        }

        #region Helpers

        private void Convention(Action<IVersionInstance> convention)
        {
            model.Conventions.Add(new VersionConventionBuilder().Always(convention));
        }

        private void VerifyModel(Action<VersionMapping> modelVerification)
        {
            var classMap = new ClassMap<ValidVersionClass>();
            var map = classMap.Version(x => x.Version);

            model.Add(classMap);

            var generatedModels = model.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == typeof(ValidVersionClass)) != null)
                .Classes.First()
                .Version;

            modelVerification(modelInstance);
        }

        #endregion
    }
}