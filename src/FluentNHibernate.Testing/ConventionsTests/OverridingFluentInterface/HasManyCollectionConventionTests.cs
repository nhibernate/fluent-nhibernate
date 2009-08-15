using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Automapping.TestFixtures.CustomTypes;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Testing.FluentInterfaceTests;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.OverridingFluentInterface
{
    [TestFixture]
    public class HasManyCollectionConventionTests
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
            Mapping(x => x.Children, x => x.Access.Field());

            Convention(x => x.Access.Property());

            VerifyModel(x => x.Access.ShouldEqual("field"));
        }

        [Test]
        public void BatchSizeShouldntBeOverwritten()
        {
            Mapping(x => x.Children, x => x.BatchSize(10));

            Convention(x => x.BatchSize(100));

            VerifyModel(x => x.BatchSize.ShouldEqual(10));
        }

        [Test]
        public void CacheShouldntBeOverwritten()
        {
            Mapping(x => x.Children, x => x.Cache.ReadOnly());

            Convention(x => x.Cache.ReadWrite());

            VerifyModel(x => x.Cache.Usage.ShouldEqual("read-only"));
        }

        [Test]
        public void CascadeShouldntBeOverwritten()
        {
            Mapping(x => x.Children, x => x.Cascade.All());

            Convention(x => x.Cascade.None());

            VerifyModel(x => x.Cascade.ShouldEqual("all"));
        }

        [Test]
        public void CheckConstraintShouldntBeOverwritten()
        {
            Mapping(x => x.Children, x => x.Check("constraint = 1"));

            Convention(x => x.Check("constraint = 0"));

            VerifyModel(x => x.Check.ShouldEqual("constraint = 1"));
        }

        [Test]
        public void CollectionTypeShouldntBeOverwritten()
        {
            Mapping(x => x.Children, x => x.CollectionType<int>());

            Convention(x => x.CollectionType<string>());

            VerifyModel(x => x.CollectionType.GetUnderlyingSystemType().ShouldEqual(typeof(int)));
        }

        [Test]
        public void FetchShouldntBeOverwritten()
        {
            Mapping(x => x.Children, x => x.Fetch.Join());

            Convention(x => x.Fetch.Select());

            VerifyModel(x => x.Fetch.ShouldEqual("join"));
        }

        [Test]
        public void GenericShouldntBeOverwritten()
        {
            Mapping(x => x.Children, x => x.Generic());

            Convention(x => x.Not.Generic());

            VerifyModel(x => x.Generic.ShouldBeTrue());
        }

        [Test]
        public void InverseShouldntBeOverwritten()
        {
            Mapping(x => x.Children, x => x.Inverse());

            Convention(x => x.Not.Inverse());

            VerifyModel(x => x.Inverse.ShouldBeTrue());
        }

        [Test]
        public void KeyColumnNameShouldntBeOverwritten()
        {
            Mapping(x => x.Children, x => x.KeyColumn("name"));

            Convention(x => x.Key.Column("xxx"));

            VerifyModel(x => x.Key.Columns.First().Name.ShouldEqual("name"));
        }

        [Test]
        public void LazyShouldntBeOverwritten()
        {
            Mapping(x => x.Children, x => x.LazyLoad());

            Convention(x => x.Not.LazyLoad());

            VerifyModel(x => x.Lazy.ShouldEqual(true));
        }

        [Test]
        public void OptimisticLockShouldntBeOverwritten()
        {
            Mapping(x => x.Children, x => x.OptimisticLock.All());

            Convention(x => x.OptimisticLock.Dirty());

            VerifyModel(x => x.OptimisticLock.ShouldEqual("all"));
        }

        [Test]
        public void PersisterShouldntBeOverwritten()
        {
            Mapping(x => x.Children, x => x.Persister<CustomPersister>());

            Convention(x => x.Persister<SecondCustomPersister>());

            VerifyModel(x => x.Persister.GetUnderlyingSystemType().ShouldEqual(typeof(CustomPersister)));
        }

        [Test]
        public void SchemaShouldntBeOverwritten()
        {
            Mapping(x => x.Children, x => x.Schema("dbo"));

            Convention(x => x.Schema("test"));

            VerifyModel(x => x.Schema.ShouldEqual("dbo"));
        }

        [Test]
        public void WhereShouldntBeOverwritten()
        {
            Mapping(x => x.Children, x => x.Where("x = 1"));

            Convention(x => x.Where("y = 2"));

            VerifyModel(x => x.Where.ShouldEqual("x = 1"));
        }

        [Test]
        public void ForeignKeyShouldntBeOverwritten()
        {
            Mapping(x => x.Children, x => x.ForeignKeyConstraintName("key"));

            Convention(x => x.Key.ForeignKey("xxx"));

            VerifyModel(x => x.Key.ForeignKey.ShouldEqual("key"));
        }

        [Test]
        public void TableNameShouldntBeOverwritten()
        {
            Mapping(x => x.Children, x => x.Table("table"));

            Convention(x => x.Table("xxx"));

            VerifyModel(x => x.TableName.ShouldEqual("table"));
        }

        #region Helpers

        private void Convention(Action<ICollectionInstance> convention)
        {
            model.Conventions.Add(new CollectionConventionBuilder().Always(convention));
        }

        private void Mapping<TChild>(Expression<Func<ExampleInheritedClass, IEnumerable<TChild>>> property, Action<OneToManyPart<TChild>> mappingDefinition)
        {
            var classMap = new ClassMap<ExampleInheritedClass>();
            var map = classMap.HasMany(property);

            mappingDefinition(map);

            mapping = classMap;
            mappingType = typeof(ExampleInheritedClass);
        }

        private void VerifyModel(Action<ICollectionMapping> modelVerification)
        {
            model.Add(mapping);

            var generatedModels = model.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == mappingType) != null)
                .Classes.First()
                .Collections.First();

            modelVerification(modelInstance);
        }

        #endregion
    }
}