using System;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Automapping.TestFixtures.CustomTypes;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.ApplyingToModel
{
    [TestFixture]
    public class CompositeIdConventionTests
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
        public void ShouldSetUnsavedValueProperty()
        {
            Convention(x => x.UnsavedValue("two"));

            VerifyModel(x => x.UnsavedValue.ShouldEqual("two"));
        }

        [Test]
        public void ShouldSetMappedValueProperty()
        {
            Convention(x => x.Mapped());

            VerifyModel(x => x.Mapped.ShouldEqual(true));
        }

        #region Helpers

        private void Convention(Action<ICompositeIdentityInstance> convention)
        {
            model.Conventions.Add(new CompositeIdConventionBuilder().Always(convention));
        }

        private void VerifyModel(Action<CompositeIdMapping> modelVerification)
        {
            var classMap = new ClassMap<ExampleClass>();
            var map = classMap.CompositeId(x => x.Id);

            model.Add(classMap);

            var generatedModels = model.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == typeof(ExampleClass)) != null)
                .Classes.First()
                .Id;

            modelVerification((CompositeIdMapping)modelInstance);
        }

        #endregion
    }
}