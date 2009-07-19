using System;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.AutoMap.TestFixtures;
using FluentNHibernate.AutoMap.TestFixtures.CustomTypes;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class FluentInterfaceOverridingConventionsPropertyTests
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
            Mapping<ExampleClass>(x => x.LineOne, x => x.Access.AsField());

            Convention(x => x.Access.Property());

            VerifyModel(x => x.Access.ShouldEqual("field"));
        }

        [Test]
        public void ColumnNameShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.LineOne, x => x.ColumnName("xxx"));

            Convention(x => x.ColumnName("yyy"));

            VerifyModel(x =>
            {
                x.Columns.Count().ShouldEqual(1);
                x.Columns.First().Name.ShouldEqual("xxx");
            });
        }

        [Test]
        public void SqlTypeShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.LineOne, x => x.CustomSqlType("sql-type"));

            Convention(x => x.CustomSqlTypeIs("type"));

            VerifyModel(x => x.Columns.First().SqlType.ShouldEqual("sql-type"));
        }

        [Test]
        public void TypeShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.LineOne, x => x.CustomType<CustomUserType>());

            Convention(x => x.CustomTypeIs<int>());

            VerifyModel(x => x.Type.Name.ShouldEqual(typeof(CustomUserType).AssemblyQualifiedName));
        }

        [Test]
        public void FormulaShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.LineOne, x => x.Formula("form"));

            Convention(x => x.Formula("xxx"));

            VerifyModel(x => x.Formula.ShouldEqual("form"));
        }

        [Test]
        public void GeneratedShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.LineOne, x => x.Generated.Always());

            Convention(x => x.Generated.Never());

            VerifyModel(x => x.Generated.ShouldEqual("always"));
        }

        [Test]
        public void InsertShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.LineOne, x => x.Insert());

            Convention(x => x.Not.Insert());

            VerifyModel(x => x.Insert.ShouldBeTrue());
        }

        [Test]
        public void NullableShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.LineOne, x => x.Nullable());

            Convention(x => x.Not.Nullable());

            VerifyModel(x => x.Columns.First().NotNull.ShouldBeFalse());
        }

        [Test]
        public void OptimisticLockShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.LineOne, x => x.OptimisticLock());

            Convention(x => x.Not.OptimisticLock());

            VerifyModel(x => x.OptimisticLock.ShouldBeTrue());
        }

        [Test]
        public void ReadOnlyShouldntOverwriteInsert()
        {
            Mapping<ExampleClass>(x => x.LineOne, x => x.Insert());

            Convention(x => x.ReadOnly());

            VerifyModel(x => x.Insert.ShouldBeTrue());
        }

        [Test]
        public void ReadOnlyShouldntOverwriteUpdate()
        {
            Mapping<ExampleClass>(x => x.LineOne, x => x.Update());

            Convention(x => x.ReadOnly());

            VerifyModel(x => x.Update.ShouldBeTrue());
        }

        [Test]
        public void ReadOnlyShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.LineOne, x => x.ReadOnly());

            Convention(x => x.Not.ReadOnly());

            VerifyModel(x =>
            {
                x.Insert.ShouldBeFalse();
                x.Update.ShouldBeFalse();
            });
        }

        [Test]
        public void UniqueShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.LineOne, x => x.Unique());

            Convention(x => x.Not.Unique());

            VerifyModel(x => x.Columns.First().Unique.ShouldBeTrue());
        }

        [Test]
        public void UniqueKeyShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.LineOne, x => x.UniqueKey("key"));

            Convention(x => x.UniqueKey("test"));

            VerifyModel(x => x.Columns.First().UniqueKey.ShouldEqual("key"));
        }

        [Test]
        public void UpdateShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.LineOne, x => x.Update());

            Convention(x => x.Not.Update());

            VerifyModel(x => x.Update.ShouldBeTrue());
        }

        [Test]
        public void LengthShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.LineOne, x => x.Length(100));

            Convention(x => x.Length(10));

            VerifyModel(x => x.Columns.First().Length.ShouldEqual(100));
        }

        #region Helpers

        private void Convention(Action<IPropertyInstance> convention)
        {
            model.ConventionFinder.Add(new PropertyConventionBuilder().Always(convention));
        }

        private void Mapping<T>(Expression<Func<T, object>> property, Action<PropertyMap> mappingDefinition)
        {
            var classMap = new ClassMap<T>();
            var map = classMap.Map(property);

            mappingDefinition(map);

            mapping = classMap;
            mappingType = typeof(T);
        }

        private void VerifyModel(Action<PropertyMapping> modelVerification)
        {
            model.Add(mapping);

            var generatedModels = model.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == mappingType) != null)
                .Classes.First()
                .Properties.First();

            modelVerification(modelInstance);
        }

        #endregion
    }
}