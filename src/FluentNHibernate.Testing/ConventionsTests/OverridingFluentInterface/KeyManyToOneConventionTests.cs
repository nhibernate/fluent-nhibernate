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

namespace FluentNHibernate.Testing.ConventionsTests.OverridingFluentInterface
{
    [TestFixture]
    public class KeyManyToOneConventionTests
    {
        private PersistenceModel model;
        private IMappingProvider mapping;

        [SetUp]
        public void CreatePersistenceModel()
        {
            model = new PersistenceModel();
        }

        [Test]
        public void AccessShouldntBeOveridden()
        {
            Mapping(x => x.Access.Property());

            Convention(x => x.Access.BackField());

            VerifyModel(x => x.Access.ShouldEqual("property"));
        }

        [Test]
        public void ForeignKeyShouldntBeOveridden() 
        {
            Mapping(x => x.ForeignKey("foo"));

            Convention(x => x.ForeignKey("bar"));

            VerifyModel(x => x.ForeignKey.ShouldEqual("foo"));
        }

        [Test]
        public void LazyShouldntBeOveridden() 
        {
            Mapping(x => x.Lazy());

            Convention(x => x.Not.Lazy());

            VerifyModel(x => x.Lazy.ShouldEqual(true));
        }

        [Test]
        public void NotFoundShouldntBeOveridden() 
        {
            Mapping(x => x.NotFound.Ignore());

            Convention(x => x.NotFound.Exception());

            VerifyModel(x => x.NotFound.ShouldEqual("ignore"));
        }

        #region Helpers

        private void Convention(Action<IKeyManyToOneInstance> convention)
        {
            model.Conventions.Add(new KeyManyToOneConventionBuilder().Always(convention));
        }

        private void Mapping(Action<KeyManyToOnePart> mappingDefinition)
        {
            var classMap = new ClassMap<ExampleClass>();
            var map = classMap.CompositeId()
                .KeyProperty(x => x.Id)
                .KeyReference(x => x.Parent, mappingDefinition);

            mapping = classMap;
        }

        private void VerifyModel(Action<KeyManyToOneMapping> modelVerification)
        {
            model.Add(mapping);

            var generatedModels = model.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == typeof(ExampleClass)) != null)
                .Classes.First()
                .Id;

            modelVerification((KeyManyToOneMapping)((CompositeIdMapping)modelInstance).Keys.First(x => x is KeyManyToOneMapping));
        }

        #endregion
    }
}