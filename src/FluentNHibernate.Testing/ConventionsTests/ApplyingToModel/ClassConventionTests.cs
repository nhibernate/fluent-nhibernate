using System;
using System.Linq;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.ApplyingToModel
{
    [TestFixture]
    public class ClassConventionTests
    {
        private PersistenceModel model;

        [SetUp]
        public void CreatePersistenceModel()
        {
            model = new PersistenceModel();
        }

        [Test]
        public void ShouldSetBatchSizeProperty()
        {
            Convention(x => x.BatchSize(100));

            VerifyModel(x => x.BatchSize.ShouldEqual(100));
        }

        [Test]
        public void ShouldSetCacheProperty()
        {
            Convention(x => x.Cache.ReadOnly());

            VerifyModel(x => x.Cache.Usage.ShouldEqual("read-only"));
        }

        [Test]
        public void ShouldSetDynamicInsertProperty()
        {
            Convention(x => x.DynamicInsert());

            VerifyModel(x => x.DynamicInsert.ShouldBeTrue());
        }

        [Test]
        public void ShouldSetDynamicUpdateProperty()
        {
            Convention(x => x.DynamicUpdate());

            VerifyModel(x => x.DynamicUpdate.ShouldBeTrue());
        }

        [Test]
        public void ShouldSetLazyLoadProperty()
        {
            Convention(x => x.LazyLoad());

            VerifyModel(x => x.Lazy.ShouldEqual(true));
        }

        [Test]
        public void ShouldSetOptimisticLockProperty()
        {
            Convention(x => x.OptimisticLock.Dirty());

            VerifyModel(x => x.OptimisticLock.ShouldEqual("dirty"));
        }

        [Test]
        public void ShouldSetReadOnlyProperty()
        {
            Convention(x => x.ReadOnly());

            VerifyModel(x => x.Mutable.ShouldBeFalse());
        }

        [Test]
        public void ShouldSetSchemaProperty()
        {
            Convention(x => x.Schema("dbo"));

            VerifyModel(x => x.Schema.ShouldEqual("dbo"));
        }

        [Test]
        public void ShouldSetTableProperty()
        {
            Convention(x => x.Table("different"));

            VerifyModel(x => x.TableName.ShouldEqual("different"));
        }

        [Test]
        public void ShouldSetWhereProperty()
        {
            Convention(x => x.Where("xxx"));

            VerifyModel(x => x.Where.ShouldEqual("xxx"));
        }

        [Test]
        public void ShouldSetSubselectProperty()
        {
            Convention(x => x.Subselect("xxx"));

            VerifyModel(x => x.Subselect.ShouldEqual("xxx"));
        }

        #region Helpers

        private void Convention(Action<IClassInstance> convention)
        {
            model.Conventions.Add(new ClassConventionBuilder().Always(convention));
        }

        private void VerifyModel(Action<ClassMapping> modelVerification)
        {
            var classMap = new ClassMap<ExampleClass>();

            model.Add(classMap);

            var generatedModels = model.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == typeof(ExampleClass)) != null)
                .Classes.First();

            modelVerification(modelInstance);
        }

        #endregion
    }
}