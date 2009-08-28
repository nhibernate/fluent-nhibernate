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
    public class IdConventionTests
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
        public void ColumnShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.Id, x => x.Column("name"));

            Convention(x => x.Column("xxx"));

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

        private void Convention(Action<IIdentityInstance> convention)
        {
            model.Conventions.Add(new IdConventionBuilder().Always(convention));
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