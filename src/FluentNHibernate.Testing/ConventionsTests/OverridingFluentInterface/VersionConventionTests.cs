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
    public class VersionConventionTests
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
            Mapping<ValidVersionClass>(x => x.Version, x => x.Access.Field());

            Convention(x => x.Access.Property());

            VerifyModel(x => x.Access.ShouldEqual("field"));
        }

        [Test]
        public void ColumnShouldntBeOverwritten()
        {
            Mapping<ValidVersionClass>(x => x.Version, x => x.Column("name"));

            Convention(x => x.Column("xxx"));

            VerifyModel(x => x.Column.ShouldEqual("name"));
        }

        [Test]
        public void GeneratedShouldntBeOverwritten()
        {
            Mapping<ValidVersionClass>(x => x.Version, x => x.Generated.Always());

            Convention(x => x.Generated.Never());

            VerifyModel(x => x.Generated.ShouldEqual("always"));
        }

        [Test]
        public void UnsavedValueShouldntBeOverwritten()
        {
            Mapping<ValidVersionClass>(x => x.Version, x => x.UnsavedValue("value"));

            Convention(x => x.UnsavedValue("one"));

            VerifyModel(x => x.UnsavedValue.ShouldEqual("value"));
        }

        #region Helpers

        private void Convention(Action<IVersionInstance> convention)
        {
            model.Conventions.Add(new VersionConventionBuilder().Always(convention));
        }

        private void Mapping<T>(Expression<Func<T, object>> property, Action<VersionPart> mappingDefinition)
        {
            var classMap = new ClassMap<T>();
            var map = classMap.Version(property);

            mappingDefinition(map);

            mapping = classMap;
            mappingType = typeof(T);
        }

        private void VerifyModel(Action<VersionMapping> modelVerification)
        {
            model.Add(mapping);

            var generatedModels = model.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == mappingType) != null)
                .Classes.First()
                .Version;

            modelVerification(modelInstance);
        }

        #endregion
    }
}