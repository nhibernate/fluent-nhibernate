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

namespace FluentNHibernate.Testing.ConventionsTests.ApplyingToModel
{
    [TestFixture]
    public class HasManyCollectionConventionTests
    {
        private PersistenceModel model;

        [SetUp]
        public void CreatePersistenceModel()
        {
            model = new PersistenceModel();
        }

        [Test]
        public void ShouldSetAccessProperty()
        {
            Convention(x => x.Access.Property());

            VerifyModel(x => x.Access.ShouldEqual("property"));
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
            Convention(x => x.Cache.ReadWrite());

            VerifyModel(x => x.Cache.Usage.ShouldEqual("read-write"));
        }

        [Test]
        public void ShouldSetCascadeProperty()
        {
            Convention(x => x.Cascade.None());

            VerifyModel(x => x.Cascade.ShouldEqual("none"));
        }

        [Test]
        public void ShouldSetCheckConstraintProperty()
        {
            Convention(x => x.Check("constraint = 0"));

            VerifyModel(x => x.Check.ShouldEqual("constraint = 0"));
        }

        [Test]
        public void ShouldSetCollectionTypeProperty()
        {
            Convention(x => x.CollectionType<string>());

            VerifyModel(x => x.CollectionType.GetUnderlyingSystemType().ShouldEqual(typeof(string)));
        }

        [Test]
        public void ShouldSetFetchProperty()
        {
            Convention(x => x.Fetch.Select());

            VerifyModel(x => x.Fetch.ShouldEqual("select"));
        }

        [Test]
        public void ShouldSetGenericProperty()
        {
            Convention(x => x.Generic());

            VerifyModel(x => x.Generic.ShouldBeTrue());
        }

        [Test]
        public void ShouldSetInverseProperty()
        {
            Convention(x => x.Inverse());

            VerifyModel(x => x.Inverse.ShouldBeTrue());
        }

        [Test]
        public void ShouldSetKeyColumnNameProperty()
        {
            Convention(x => x.Key.Column("xxx"));

            VerifyModel(x => x.Key.Columns.First().Name.ShouldEqual("xxx"));
        }

        [Test]
        public void ShouldSetLazyProperty()
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
        public void ShouldSetPersisterProperty()
        {
            Convention(x => x.Persister<SecondCustomPersister>());

            VerifyModel(x => x.Persister.GetUnderlyingSystemType().ShouldEqual(typeof(SecondCustomPersister)));
        }

        [Test]
        public void ShouldSetSchemaProperty()
        {
            Convention(x => x.Schema("test"));

            VerifyModel(x => x.Schema.ShouldEqual("test"));
        }

        [Test]
        public void ShouldSetWhereProperty()
        {
            Convention(x => x.Where("y = 2"));

            VerifyModel(x => x.Where.ShouldEqual("y = 2"));
        }

        [Test]
        public void ShouldSetForeignKeyProperty()
        {
            Convention(x => x.Key.ForeignKey("xxx"));

            VerifyModel(x => x.Key.ForeignKey.ShouldEqual("xxx"));
        }

        [Test]
        public void ShouldSetTableNameProperty()
        {
            Convention(x => x.Table("xxx"));

            VerifyModel(x => x.TableName.ShouldEqual("xxx"));
        }

        #region Helpers

        private void Convention(Action<ICollectionInstance> convention)
        {
            model.Conventions.Add(new CollectionConventionBuilder().Always(convention));
        }

        private void VerifyModel(Action<ICollectionMapping> modelVerification)
        {
            var classMap = new ClassMap<ExampleInheritedClass>();
            var map = classMap.HasMany(x => x.Children);

            model.Add(classMap);

            var generatedModels = model.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == typeof(ExampleInheritedClass)) != null)
                .Classes.First()
                .Collections.First();

            modelVerification(modelInstance);
        }

        #endregion
    }
}