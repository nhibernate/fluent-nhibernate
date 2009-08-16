using System;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Automapping.TestFixtures.CustomTypes;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.ApplyingToModel
{
    [TestFixture]
    public class PropertyConventionTests
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
        public void ShouldSetColumnNameProperty()
        {
            Convention(x => x.Column("yyy"));

            VerifyModel(x =>
            {
                x.Columns.Count().ShouldEqual(1);
                x.Columns.First().Name.ShouldEqual("yyy");
            });
        }

        [Test]
        public void ShouldSetSqlTypeProperty()
        {
            Convention(x => x.CustomSqlType("type"));

            VerifyModel(x => x.Columns.First().SqlType.ShouldEqual("type"));
        }

        [Test]
        public void ShouldSetTypeProperty()
        {
            Convention(x => x.CustomType<int>());

            VerifyModel(x => x.Type.GetUnderlyingSystemType().ShouldEqual(typeof(int)));
        }

        [Test]
        public void ShouldSetFormulaProperty()
        {
            Convention(x => x.Formula("xxx"));

            VerifyModel(x => x.Formula.ShouldEqual("xxx"));
        }

        [Test]
        public void ShouldSetGeneratedProperty()
        {
            Convention(x => x.Generated.Never());

            VerifyModel(x => x.Generated.ShouldEqual("never"));
        }

        [Test]
        public void ShouldSetInsertProperty()
        {
            Convention(x => x.Insert());

            VerifyModel(x => x.Insert.ShouldBeTrue());
        }

        [Test]
        public void ShouldSetNullableProperty()
        {
            Convention(x => x.Nullable());

            VerifyModel(x => x.Columns.First().NotNull.ShouldBeFalse());
        }

        [Test]
        public void ShouldSetOptimisticLockProperty()
        {
            Convention(x => x.OptimisticLock());

            VerifyModel(x => x.OptimisticLock.ShouldBeTrue());
        }

        [Test]
        public void ReadOnlyShouldntOverwriteInsert()
        {
            Convention(x => x.ReadOnly());

            VerifyModel(x => x.Insert.ShouldBeFalse());
        }

        [Test]
        public void ReadOnlyShouldntOverwriteUpdate()
        {
            Convention(x => x.ReadOnly());

            VerifyModel(x => x.Update.ShouldBeFalse());
        }

        [Test]
        public void ShouldSetReadOnlyProperty()
        {
            Convention(x => x.ReadOnly());

            VerifyModel(x =>
            {
                x.Insert.ShouldBeFalse();
                x.Update.ShouldBeFalse();
            });
        }

        [Test]
        public void ShouldSetUniqueProperty()
        {
            Convention(x => x.Unique());

            VerifyModel(x => x.Columns.First().Unique.ShouldBeTrue());
        }

        [Test]
        public void ShouldSetUniqueKeyProperty()
        {
            Convention(x => x.UniqueKey("test"));

            VerifyModel(x => x.Columns.First().UniqueKey.ShouldEqual("test"));
        }

        [Test]
        public void ShouldSetUpdateProperty()
        {
            Convention(x => x.Update());

            VerifyModel(x => x.Update.ShouldBeTrue());
        }

        [Test]
        public void ShouldSetLengthProperty()
        {
            Convention(x => x.Length(10));

            VerifyModel(x => x.Columns.First().Length.ShouldEqual(10));
        }

        [Test]
        public void ShouldSetLazyProperty()
        {
            Convention(x => x.LazyLoad());

            VerifyModel(x => x.Lazy.ShouldBeTrue());
        }

        [Test]
        public void ShouldSetIndexProperty()
        {
            Convention(x => x.Index("xxx"));

            VerifyModel(x => x.Index.ShouldEqual("xxx"));
        }

        [Test]
        public void ShouldSetPrecisionProperty()
        {
            Convention(x => x.Precision(200));

            VerifyModel(x => x.Columns.First().Precision.ShouldEqual(200));
        }

        [Test]
        public void ShouldSetScaleProperty()
        {
            Convention(x => x.Scale(200));

            VerifyModel(x => x.Columns.First().Scale.ShouldEqual(200));
        }

        [Test]
        public void ShouldSetDefaultProperty()
        {
            Convention(x => x.Default("xxx"));

            VerifyModel(x => x.Columns.First().Default.ShouldEqual("xxx"));
        }

        #region Helpers

        private void Convention(Action<IPropertyInstance> convention)
        {
            model.Conventions.Add(new PropertyConventionBuilder().Always(convention));
        }

        private void VerifyModel(Action<PropertyMapping> modelVerification)
        {
            var classMap = new ClassMap<ExampleClass>();
            var map = classMap.Map(x => x.LineOne);

            model.Add(classMap);

            var generatedModels = model.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == typeof(ExampleClass)) != null)
                .Classes.First()
                .Properties.First();

            modelVerification(modelInstance);
        }

        #endregion
    }
}