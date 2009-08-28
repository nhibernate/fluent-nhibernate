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

            VerifyModel(x => x.Columns.First().Name.ShouldEqual("name"));
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


        [Test]
        public void LengthValueShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.Id, x => x.Length(100));

            Convention(x => x.Length(200));

            VerifyModel(x => x.Columns.First().Length.ShouldEqual(100));
        }

        [Test]
        public void PrecisionValueShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.Id, x => x.Precision(100));

            Convention(x => x.Precision(200));

            VerifyModel(x => x.Columns.First().Precision.ShouldEqual(100));
        }

        [Test]
        public void ScaleValueShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.Id, x => x.Scale(100));

            Convention(x => x.Scale(200));

            VerifyModel(x => x.Columns.First().Scale.ShouldEqual(100));
        }

        [Test]
        public void NullableValueShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.Id, x => x.Not.Nullable());

            Convention(x => x.Nullable());

            VerifyModel(x => x.Columns.First().NotNull.ShouldEqual(true));
        }

        [Test]
        public void UniqueValueShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.Id, x => x.Unique());

            Convention(x => x.Not.Unique());

            VerifyModel(x => x.Columns.First().Unique.ShouldEqual(true));
        }

        [Test]
        public void UniqueKeyValueShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.Id, x => x.UniqueKey("key"));

            Convention(x => x.UniqueKey("xxx"));

            VerifyModel(x => x.Columns.First().UniqueKey.ShouldEqual("key"));
        }

        [Test]
        public void SqlTypeValueShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.Id, x => x.CustomSqlType("sql"));

            Convention(x => x.CustomSqlType("xxx"));

            VerifyModel(x => x.Columns.First().SqlType.ShouldEqual("sql"));
        }

        [Test]
        public void IndexValueShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.Id, x => x.Index("idx"));

            Convention(x => x.Index("xxx"));

            VerifyModel(x => x.Columns.First().Index.ShouldEqual("idx"));
        }

        [Test]
        public void CheckValueShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.Id, x => x.Check("constraint"));

            Convention(x => x.Check("xxx"));

            VerifyModel(x => x.Columns.First().Check.ShouldEqual("constraint"));
        }

        [Test]
        public void DefaultValueShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.Id, x => x.Default(200));

            Convention(x => x.Default(100));

            VerifyModel(x => x.Columns.First().Default.ShouldEqual("200"));
        }

        [Test]
        public void TypeValueShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.Id, x => x.CustomType<int>());

            Convention(x => x.CustomType<string>());

            VerifyModel(x => x.Type.GetUnderlyingSystemType().ShouldEqual(typeof(int)));
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