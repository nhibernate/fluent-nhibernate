using System;
using System.Linq;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.ApplyingToModel
{
    [TestFixture]
    public class SubclassConventionTests
    {
        private PersistenceModel model;

        [SetUp]
        public void CreatePersistenceModel()
        {
            model = new PersistenceModel();
        }

        [Test]
        public void ShouldSetAbstractProperty()
        {
            Convention(x => x.Abstract());

            VerifyModel(x => x.Abstract.ShouldBeTrue());
        }

        [Test]
        public void ShouldSetDynamicInsertProperty()
        {
            Convention(x => x.DynamicInsert());

            VerifyModel(x => x.DynamicInsert.ShouldBeTrue());
        }

        [Test]
        public void ShouldSetDynamicUpdateProperty()
        {
            Convention(x => x.DynamicUpdate());

            VerifyModel(x => x.DynamicUpdate.ShouldBeTrue());
        }

        [Test]
        public void ShouldSetLazyLoadProperty()
        {
            Convention(x => x.LazyLoad());

            VerifyModel(x => x.Lazy.ShouldEqual(true));
        }

        [Test]
        public void ShouldSetProxyProperty()
        {
            Convention(x => x.Proxy(typeof(string)));

            VerifyModel(x => x.Proxy.ShouldEqual(typeof(string).AssemblyQualifiedName));
        }

        [Test]
        public void ShouldSetSelectBeforeUpdateProperty()
        {
            Convention(x => x.SelectBeforeUpdate());

            VerifyModel(x => x.SelectBeforeUpdate.ShouldBeTrue());
        }

        #region Helpers

        private void Convention(Action<ISubclassInstance> convention)
        {
            model.Conventions.Add(new SubclassConventionBuilder().Always(convention));
        }

        private void VerifyModel(Action<SubclassMapping> modelVerification)
        {
            var classMap = new ClassMap<ExampleClass>();

            classMap.DiscriminateSubClassesOnColumn("col");

            var subclassMap = new SubclassMap<ExampleInheritedClass>();

            model.Add(classMap);
            model.Add(subclassMap);

            var generatedModels = model.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == typeof(ExampleClass)) != null)
                .Classes.First()
                .Subclasses.First();

            modelVerification((SubclassMapping)modelInstance);
        }

        #endregion
    }
}