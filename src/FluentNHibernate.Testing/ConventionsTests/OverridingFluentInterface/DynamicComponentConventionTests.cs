using System;
using System.Collections;
using System.Linq;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.OverridingFluentInterface
{
    [TestFixture]
    public class DynamicComponentConventionTests
    {
        private PersistenceModel model;
        private IMappingProvider mapping;
        private Type mappingType;

        [SetUp]
        public void CreatePersistenceModel()
        {
            model = new PersistenceModel();
        }

        [Test]
        public void AccessShouldntBeOverwritten()
        {
            Mapping(x => x.Access.Field());

            Convention(x => x.Access.Property());

            VerifyModel(x => x.Access.ShouldEqual("field"));
        }

        [Test]
        public void InsertShouldntBeOverwritten()
        {
            Mapping(x => x.Insert());

            Convention(x => x.Not.Insert());

            VerifyModel(x => x.Insert.ShouldBeTrue());
        }

        [Test]
        public void UpdateShouldntBeOverwritten()
        {
            Mapping(x => x.Update());

            Convention(x => x.Not.Update());

            VerifyModel(x => x.Update.ShouldBeTrue());
        }

        [Test]
        public void UniqueShouldntBeOverwritten()
        {
            Mapping(x => x.Unique());

            Convention(x => x.Not.Unique());

            VerifyModel(x => x.Unique.ShouldBeTrue());
        }

        [Test]
        public void OptimisticLockShouldntBeOverwritten()
        {
            Mapping(x => x.OptimisticLock());

            Convention(x => x.Not.OptimisticLock());

            VerifyModel(x => x.OptimisticLock.ShouldBeTrue());
        }

        #region Helpers

        private void Convention(Action<IDynamicComponentInstance> convention)
        {
            model.Conventions.Add(new DynamicComponentConventionBuilder().Always(convention));
        }

        private void Mapping(Action<DynamicComponentPart<IDictionary>> mappingDefinition)
        {
            var classMap = new ClassMap<PropertyTarget>();
            var map = classMap.DynamicComponent(x => x.ExtensionData, m =>
            {
                m.Map(x => (string)x["Name"]);
                m.Map(x => (int)x["Age"]);
                m.Map(x => (string)x["Profession"]);
            });

            mappingDefinition(map);

            mapping = classMap;
            mappingType = typeof(PropertyTarget);
        }

        private void VerifyModel(Action<DynamicComponentMapping> modelVerification)
        {
            model.Add(mapping);

            var generatedModels = model.BuildMappings();
            var modelInstance = (DynamicComponentMapping)generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == mappingType) != null)
                .Classes.First()
                .Components.Where(x => x is DynamicComponentMapping).First();

            modelVerification(modelInstance);
        }

        #endregion
    }
}