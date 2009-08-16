using System;
using System.Linq;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Testing.FluentInterfaceTests;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.OverridingFluentInterface
{
    [TestFixture]
    public class ArrayConventionTests
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
        public void BatchSizeShouldntBeOverwritten()
        {
            Mapping(x => x.BatchSize(10));

            Convention(x => x.BatchSize(100));

            VerifyModel(x => x.BatchSize.ShouldEqual(10));
        }

        [Test]
        public void CacheShouldntBeOverwritten()
        {
            Mapping(x => x.Cache.CustomUsage("fish"));

            Convention(x => x.Cache.ReadOnly());

            VerifyModel(x => x.Cache.Usage.ShouldEqual("fish"));
        }

        [Test]
        public void CascadeShouldntBeOverwritten()
        {
            Mapping(x => x.Cascade.All());

            Convention(x => x.Cascade.None());

            VerifyModel(x => x.Cascade.ShouldEqual("all"));
        }

        [Test]
        public void CheckShouldntBeOverwritten()
        {
            Mapping(x => x.Check("constraint"));

            Convention(x => x.Check("xxx"));

            VerifyModel(x => x.Check.ShouldEqual("constraint"));
        }

        [Test]
        public void CollectionTypeShouldntBeOverwritten()
        {
            Mapping(x => x.CollectionType<int>());

            Convention(x => x.CollectionType<string>());

            VerifyModel(x => x.CollectionType.GetUnderlyingSystemType().ShouldEqual(typeof(int)));
        }

        [Test]
        public void FetchShouldntBeOverwritten()
        {
            Mapping(x => x.Fetch.Join());

            Convention(x => x.Fetch.Select());

            VerifyModel(x => x.Fetch.ShouldEqual("join"));
        }

        [Test]
        public void GenericShouldntBeOverwritten()
        {
            Mapping(x => x.Generic());

            Convention(x => x.Not.Generic());

            VerifyModel(x => x.Generic.ShouldEqual(true));
        }

        [Test]
        public void InverseShouldntBeOverwritten()
        {
            Mapping(x => x.Inverse());

            Convention(x => x.Not.Inverse());

            VerifyModel(x => x.Inverse.ShouldEqual(true));
        }

        [Test]
        public void LazyShouldntBeOverwritten()
        {
            Mapping(x => x.LazyLoad());

            Convention(x => x.Not.LazyLoad());

            VerifyModel(x => x.Lazy.ShouldEqual(true));
        }

        [Test]
        public void MutableShouldntBeOverwritten()
        {
            Mapping(x => x.ReadOnly());

            Convention(x => x.Not.ReadOnly());

            VerifyModel(x => x.Mutable.ShouldEqual(false));
        }

        [Test]
        public void OptimisticLockShouldntBeOverwritten()
        {
            Mapping(x => x.OptimisticLock.All());

            Convention(x => x.OptimisticLock.Dirty());

            VerifyModel(x => x.OptimisticLock.ShouldEqual("all"));
        }

        [Test]
        public void PersisterShouldntBeOverwritten()
        {
            Mapping(x => x.Persister<CustomPersister>());

            Convention(x => x.Persister<SecondCustomPersister>());

            VerifyModel(x => x.Persister.GetUnderlyingSystemType().ShouldEqual(typeof(CustomPersister)));
        }

        [Test]
        public void SchemaShouldntBeOverwritten()
        {
            Mapping(x => x.Schema("dbo"));

            Convention(x => x.Schema("xxx"));

            VerifyModel(x => x.Schema.ShouldEqual("dbo"));
        }

        [Test]
        public void SubselectShouldntBeOverwritten()
        {
            Mapping(x => x.Subselect("whee"));

            Convention(x => x.Subselect("woo"));

            VerifyModel(x => x.Subselect.ShouldEqual("whee"));
        }

        [Test]
        public void TableNameShouldntBeOverwritten()
        {
            Mapping(x => x.Table("name"));

            Convention(x => x.Table("xxx"));

            VerifyModel(x => x.TableName.ShouldEqual("name"));
        }

        [Test]
        public void WhereShouldntBeOverwritten()
        {
            Mapping(x => x.Where("x = 1"));

            Convention(x => x.Where("y = 2"));

            VerifyModel(x => x.Where.ShouldEqual("x = 1"));
        }

        #region Helpers

        private void Convention(Action<IArrayInstance> convention)
        {
            model.Conventions.Add(new ArrayConventionBuilder().Always(convention));
        }

        private void Mapping(Action<OneToManyPart<ExampleClass>> mappingDefinition)
        {
            var classMap = new ClassMap<ExampleParentClass>();
            var map = classMap.HasMany(x => x.Examples)
                .AsArray(x => x.Id);

            mappingDefinition(map);

            mapping = classMap;
            mappingType = typeof(ExampleParentClass);
        }

        private void VerifyModel(Action<ArrayMapping> modelVerification)
        {
            model.Add(mapping);

            var generatedModels = model.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == mappingType) != null)
                .Classes.First()
                .Collections.First();

            modelVerification((ArrayMapping)modelInstance);
        }

        #endregion

    }
}