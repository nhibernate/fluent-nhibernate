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
    public class CompositeIdConventionTests
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
            Mapping<ExampleClass>(x => x.Id, x => x.Access.Field());

            Convention(x => x.Access.Property());

            VerifyModel(x => x.Access.ShouldEqual("field"));
        }

        [Test]
        public void UnsavedValueShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.Id, x => x.UnsavedValue("one"));

            Convention(x => x.UnsavedValue("two"));

            VerifyModel(x => x.UnsavedValue.ShouldEqual("one"));
        }

        [Test]
        public void MappedValueShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.Id, x => x.Mapped());

            Convention(x => x.Not.Mapped());

            VerifyModel(x => x.Mapped.ShouldEqual(true));
        }

        #region Helpers

        private void Convention(Action<ICompositeIdentityInstance> convention)
        {
            model.Conventions.Add(new CompositeIdConventionBuilder().Always(convention));
        }

        private void Mapping<T>(Expression<Func<T, object>> property, Action<CompositeIdentityPart<object>> mappingDefinition)
        {
            var classMap = new ClassMap<T>();
            var map = classMap.CompositeId(property);

            mappingDefinition(map);

            mapping = classMap;
            mappingType = typeof(T);
        }

        private void VerifyModel(Action<CompositeIdMapping> modelVerification)
        {
            model.Add(mapping);

            var generatedModels = model.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == mappingType) != null)
                .Classes.First()
                .Id;

            modelVerification((CompositeIdMapping)modelInstance);
        }

        #endregion
    }
}