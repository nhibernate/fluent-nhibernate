using System;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.AutoMap.TestFixtures;
using FluentNHibernate.AutoMap.TestFixtures.CustomTypes;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class FluentInterfaceOverridingConventionsIdTests
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
            Mapping<ExampleClass>(x => x.Id, x => x.Access.AsField());

            Convention(x => x.Access.Property());

            VerifyModel(x => x.Access.ShouldEqual("field"));
        }

        [Test]
        public void ColumnShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.Id, x => x.ColumnName("name"));

            Convention(x => x.ColumnName("xxx"));

            VerifyModel(x => x.Columns.First().Name.ShouldEqual("name"));
        }

        [Test]
        public void GeneratedByShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.Id, x => x.GeneratedBy.Assigned());

            Convention(x => x.GeneratedBy.Identity());

            VerifyModel(x => x.Generator.Class.ShouldEqual("assigned"));
        }

        [Test]
        public void UnsavedValueShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.Id, x => x.UnsavedValue("one"));

            Convention(x => x.UnsavedValue("two"));

            VerifyModel(x => x.UnsavedValue.ShouldEqual("one"));
        }

        #region Helpers

        private void Convention(Action<IIdentityInstance> convention)
        {
            model.ConventionFinder.Add(new IdConventionBuilder().Always(convention));
        }

        private void Mapping<T>(Expression<Func<T, object>> property, Action<IdentityPart> mappingDefinition)
        {
            var classMap = new ClassMap<T>();
            var map = classMap.Id(property);

            mappingDefinition(map);

            mapping = classMap;
            mappingType = typeof(T);
        }

        private void VerifyModel(Action<IdMapping> modelVerification)
        {
            model.Add(mapping);

            var generatedModels = model.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == mappingType) != null)
                .Classes.First()
                .Id;

            modelVerification((IdMapping)modelInstance);
        }

        #endregion
    }
}