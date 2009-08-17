using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.OverridingFluentInterface
{
    [TestFixture]
    public class HibernateMappingConventionTests
    {
        private PersistenceModel model;
        private IMappingProvider mapping;
        private Type mappingType;

        [SetUp]
        public void CreatePersistanceModel()
        {
            model = new PersistenceModel();
        }

        [Test]
        public void CatalogShouldntBeOverridden()
        {
            Mapping(x => x.Catalog("xxx"));

            Convention(x => x.Catalog("cat"));

            VerifyModel(x => x.Catalog.ShouldEqual("xxx"));
        }

        [Test]
        public void DefaultAccessShouldntBeOverridden()
        {
            Mapping(x => x.DefaultAccess.Property());

            Convention(x => x.DefaultAccess.Field());

            VerifyModel(x => x.DefaultAccess.ShouldEqual("property"));
        }

        [Test]
        public void DefaultCascadeShouldntBeOverridden()
        {
            Mapping(x => x.DefaultCascade.None());

            Convention(x => x.DefaultCascade.All());

            VerifyModel(x => x.DefaultCascade.ShouldEqual("none"));
        }

        [Test]
        public void SchemaShouldntBeOverridden()
        {
            Mapping(x => x.Schema("xxx"));

            Convention(x => x.Schema("dbo"));

            VerifyModel(x => x.Schema.ShouldEqual("xxx"));
        }

        [Test]
        public void DefaultLazyShouldntBeOverridden()
        {
            Mapping(x => x.Not.DefaultLazy());

            Convention(x => x.DefaultLazy());

            VerifyModel(x => x.DefaultLazy.ShouldEqual(false));
        }

        #region Helpers

        private void Convention(Action<IHibernateMappingInstance> convention)
        {
            model.Conventions.Add(new HibernateMappingConventionBuilder().Always(convention));
        }

        private void Mapping(Action<HibernateMappingPart> mappingDefinition)
        {
            var classMap = new ClassMap<ExampleClass>();

            mappingDefinition(classMap.HibernateMapping);

            mapping = classMap;
            mappingType = typeof(ExampleClass);
        }

        private void VerifyModel(Action<HibernateMapping> modelVerification)
        {
            model.Add(mapping);

            var generatedModels = model.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == typeof(ExampleClass)) != null);

            modelVerification(modelInstance);
        }

        #endregion
    }
}