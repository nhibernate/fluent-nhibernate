using System;
using System.Linq;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.ApplyingToModel
{
    [TestFixture]
    public class ComponentConventionTests
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
        public void ShouldSetInsertProperty()
        {
            Convention(x => x.Insert());

            VerifyModel(x => x.Insert.ShouldBeTrue());
        }

        [Test]
        public void ShouldSetUpdateProperty()
        {
            Convention(x => x.Update());

            VerifyModel(x => x.Update.ShouldBeTrue());
        }

        [Test]
        public void ShouldSetUniqueProperty()
        {
            Convention(x => x.Unique());

            VerifyModel(x => x.Unique.ShouldBeTrue());
        }

        [Test]
        public void ShouldSetOptimisticLockProperty()
        {
            Convention(x => x.OptimisticLock());

            VerifyModel(x => x.OptimisticLock.ShouldBeTrue());
        }

        #region Helpers

        private void Convention(Action<IComponentInstance> convention)
        {
            model.Conventions.Add(new ComponentConventionBuilder().Always(convention));
        }

        private void VerifyModel(Action<ComponentMapping> modelVerification)
        {
            var classMap = new ClassMap<PropertyTarget>();
            var map = classMap.Component(x => x.Component, m => {});

            model.Add(classMap);

            var generatedModels = model.BuildMappings();
            var modelInstance = (ComponentMapping)generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == typeof(PropertyTarget)) != null)
                .Classes.First()
                .Components.Where(x => x is ComponentMapping).First();

            modelVerification(modelInstance);
        }

        #endregion
    }
}