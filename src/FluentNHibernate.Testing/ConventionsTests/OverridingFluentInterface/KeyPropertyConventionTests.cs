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
    public class KeyPropertyConventionTests
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

        #region Helpers

        private void Convention(Action<IKeyPropertyInstance> convention)
        {
            model.Conventions.Add(new KeyPropertyConventionBuilder().Always(convention));
        }

        private void Mapping(Action<KeyPropertyPart> mappingDefinition)
        {
            var classMap = new ClassMap<ExampleClass>();
            var map = classMap.CompositeId()
                .KeyProperty(x => x.Id, mappingDefinition)
                .KeyReference(x => x.Parent);

            mapping = classMap;
        }

        private void VerifyModel(Action<KeyPropertyMapping> modelVerification)
        {
            model.Add(mapping);

            var generatedModels = model.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == typeof(ExampleClass)) != null)
                .Classes.First()
                .Id;

            modelVerification((KeyPropertyMapping)((CompositeIdMapping)modelInstance).Keys.First(x => x is KeyPropertyMapping));
        }

        #endregion
    }
}