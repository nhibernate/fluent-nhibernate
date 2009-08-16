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

namespace FluentNHibernate.Testing.ConventionsTests.OverridingFluentInterface
{
    [TestFixture]
    public class HasOneConventionTests
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
        public void CascadeShouldntBeOverwritten()
        {
            Mapping(x => x.Cascade.All());

            Convention(x => x.Cascade.None());

            VerifyModel(x => x.Cascade.ShouldEqual("all"));
        }

        [Test]
        public void ClassShouldntBeOverwritten()
        {
            Mapping(x => x.Class<int>());

            Convention(x => x.Class<string>());

            VerifyModel(x => x.Class.GetUnderlyingSystemType().ShouldEqual(typeof(int)));
        }

        [Test]
        public void ConstrainedShouldntBeOverwritten()
        {
            Mapping(x => x.Constrained());

            Convention(x => x.Not.Constrained());

            VerifyModel(x => x.Constrained.ShouldBeTrue());
        }

        [Test]
        public void FetchShouldntBeOverwritten()
        {
            Mapping(x => x.Fetch.Join());

            Convention(x => x.Fetch.Select());

            VerifyModel(x => x.Fetch.ShouldEqual("join"));
        }

        [Test]
        public void ForeignKeyShouldntBeOverwritten()
        {
            Mapping(x => x.ForeignKey("key"));

            Convention(x => x.ForeignKey("xxx"));

            VerifyModel(x => x.ForeignKey.ShouldEqual("key"));
        }

        [Test]
        public void LazyShouldntBeOverwritten()
        {
            Mapping(x => x.LazyLoad());

            Convention(x => x.Not.LazyLoad());

            VerifyModel(x => x.Lazy.ShouldEqual(true));
        }

        [Test]
        public void PropertyRefShouldntBeOverwritten()
        {
            Mapping(x => x.PropertyRef("ref"));

            Convention(x => x.PropertyRef("value"));

            VerifyModel(x => x.PropertyRef.ShouldEqual("ref"));
        }

        #region Helpers

        private void Convention(Action<IOneToOneInstance> convention)
        {
            model.Conventions.Add(new HasOneConventionBuilder().Always(convention));
        }

        private void Mapping(Action<OneToOnePart<ExampleParentClass>> mappingDefinition)
        {
            var classMap = new ClassMap<ExampleClass>();
            var map = classMap.HasOne(x => x.Parent);

            mappingDefinition(map);

            mapping = classMap;
            mappingType = typeof(ExampleClass);
        }

        private void VerifyModel(Action<OneToOneMapping> modelVerification)
        {
            model.Add(mapping);

            var generatedModels = model.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == mappingType) != null)
                .Classes.First()
                .OneToOnes.First();

            modelVerification(modelInstance);
        }

        #endregion
    }
}