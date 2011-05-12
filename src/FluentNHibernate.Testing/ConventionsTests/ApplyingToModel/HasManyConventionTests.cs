using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.Automapping.TestFixtures;
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
    public class HasManyConventionTests
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
		public void ShouldSetClassProperty()
		{
			Convention(x => x.Relationship.CustomClass(typeof(int)));

			VerifyModel(x => x.Relationship.Class.GetUnderlyingSystemType().ShouldEqual(typeof(int)));
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
        public void ShouldSetElementColumnNameProperty()
        {
            Convention(x => x.Element.Column("xxx"));

            VerifyModel(x => x.Element.Columns.First().Name.ShouldEqual("xxx"));
        }

        [Test]
        public void ShouldSetElementTypePropertyUsingGeneric()
        {
            Convention(x => x.Element.Type<string>());

            VerifyModel(x => x.Element.Type.GetUnderlyingSystemType().ShouldEqual(typeof(string)));
        }

        [Test]
        public void ShouldSetElementTypePropertyUsingTypeOf()
        {
            Convention(x => x.Element.Type(typeof(string)));

            VerifyModel(x => x.Element.Type.GetUnderlyingSystemType().ShouldEqual(typeof(string)));
        }

        [Test]
        public void ShouldSetElementTypePropertyUsingString()
        {
            Convention(x => x.Element.Type(typeof(string).AssemblyQualifiedName));

            VerifyModel(x => x.Element.Type.GetUnderlyingSystemType().ShouldEqual(typeof(string)));
        }

        [Test]
        public void ShouldSetLazyProperty()
        {
            Convention(x => x.LazyLoad());

            VerifyModel(x => x.Lazy.ShouldEqual(Lazy.True));
        }

        [Test]
        public void ShouldSetOptimisticLockProperty()
        {
            Convention(x => x.OptimisticLock());

            VerifyModel(x => x.OptimisticLock.ShouldEqual(true));
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
        public void ShouldSetPropertyRefProperty()
        {
            Convention(x => x.Key.PropertyRef("xxx"));

            VerifyModel(x => x.Key.PropertyRef.ShouldEqual("xxx"));
        }

        [Test]
        public void ShouldSetTableNameProperty()
        {
            Convention(x => x.Table("xxx"));

            VerifyModel(x => x.TableName.ShouldEqual("xxx"));
        }

        [Test]
        public void ShouldSetNotFoundProperty()
        {
            Convention(x => x.Relationship.NotFound.Ignore());

            VerifyModel(x => x.Relationship.NotFound.ShouldEqual("ignore"));
        }


        [Test]
        public void ShouldSetInversePropertyToFalseWhenUsingNot()
        {
            Convention(x => x.Not.Inverse());

            VerifyModel(x => x.Inverse.ShouldBeFalse());
        }

        #region Helpers

        private void Convention(Action<IOneToManyCollectionInstance> convention)
        {
            model.Conventions.Add(new OneToManyCollectionConventionBuilder().Always(convention));
        }

        private void VerifyModel(Action<CollectionMapping> modelVerification)
        {
            var classMap = new ClassMap<ExampleInheritedClass>();
            classMap.Id(x => x.Id);
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