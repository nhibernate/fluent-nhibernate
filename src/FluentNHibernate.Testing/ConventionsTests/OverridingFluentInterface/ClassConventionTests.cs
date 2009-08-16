using System;
using System.Linq;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.OverridingFluentInterface
{
    [TestFixture]
    public class ClassConventionTests
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
        public void BatchSizeShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.BatchSize(10));

            Convention(x => x.BatchSize(100));

            VerifyModel(x => x.BatchSize.ShouldEqual(10));
        }

        [Test]
        public void CacheShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.Cache.CustomUsage("fish"));

            Convention(x => x.Cache.ReadOnly());

            VerifyModel(x => x.Cache.Usage.ShouldEqual("fish"));
        }

        [Test]
        public void DynamicInsertShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.DynamicInsert());

            Convention(x => x.Not.DynamicInsert());

            VerifyModel(x => x.DynamicInsert.ShouldBeTrue());
        }

        [Test]
        public void DynamicUpdateShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.DynamicUpdate());

            Convention(x => x.Not.DynamicUpdate());

            VerifyModel(x => x.DynamicUpdate.ShouldBeTrue());
        }

        [Test]
        public void LazyLoadShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.LazyLoad());

            Convention(x => x.Not.LazyLoad());

            VerifyModel(x => x.Lazy.ShouldEqual(true));
        }

        [Test]
        public void OptimisticLockShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.OptimisticLock.All());

            Convention(x => x.OptimisticLock.Dirty());

            VerifyModel(x => x.OptimisticLock.ShouldEqual("all"));
        }

        [Test]
        public void ReadOnlyShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.ReadOnly());

            Convention(x => x.Not.ReadOnly());

            VerifyModel(x => x.Mutable.ShouldBeFalse());
        }

        [Test]
        public void SchemaShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.Schema("test"));

            Convention(x => x.Schema("dbo"));

            VerifyModel(x => x.Schema.ShouldEqual("test"));
        }

        [Test]
        public void TableShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.Table("test"));

            Convention(x => x.Table("different"));

            VerifyModel(x => x.TableName.ShouldEqual("test"));
        }

        [Test]
        public void WhereShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.Where("test"));

            Convention(x => x.Where("xxx"));

            VerifyModel(x => x.Where.ShouldEqual("test"));
        }

        [Test]
        public void SubselectShouldntBeOverwritten()
        {
            Mapping<ExampleClass>(x => x.Subselect("test"));

            Convention(x => x.Subselect("xxx"));

            VerifyModel(x => x.Subselect.ShouldEqual("test"));
        }

        #region Helpers

        private void Convention(Action<IClassInstance> convention)
        {
            model.Conventions.Add(new ClassConventionBuilder().Always(convention));
        }

        private void Mapping<T>(Action<ClassMap<T>> mappingDefinition)
        {
            var classMap = new ClassMap<T>();

            mappingDefinition(classMap);

            mapping = classMap;
            mappingType = typeof(T);
        }

        private void VerifyModel(Action<ClassMapping> modelVerification)
        {
            model.Add(mapping);

            var generatedModels = model.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == mappingType) != null)
                .Classes.First();

            modelVerification(modelInstance);
        }

        #endregion
    }
}