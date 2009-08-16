using System;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.ApplyingToModel
{
    [TestFixture]
    public class ManyToOneConventionTests
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
        public void ShouldSetCascadeProperty()
        {
            Convention(x => x.Cascade.None());

            VerifyModel(x => x.Cascade.ShouldEqual("none"));
        }

        [Test]
        public void ShouldSetClassProperty()
        {
            Convention(x => x.CustomClass(typeof(int)));

            VerifyModel(x => x.Class.GetUnderlyingSystemType().ShouldEqual(typeof(int)));
        }

        [Test]
        public void ShouldSetColumnProperty()
        {
            Convention(x => x.Column("xxx"));

            VerifyModel(x => x.Columns.First().Name.ShouldEqual("xxx"));
        }

        [Test]
        public void ShouldSetFetchProperty()
        {
            Convention(x => x.Fetch.Select());

            VerifyModel(x => x.Fetch.ShouldEqual("select"));
        }

        [Test]
        public void ShouldSetIndexProperty()
        {
            Convention(x => x.Index("value"));

            VerifyModel(x => x.Columns.First().Index.ShouldEqual("value"));
        }

        [Test]
        public void ShouldSetInsertProperty()
        {
            Convention(x => x.Insert());

            VerifyModel(x => x.Insert.ShouldBeTrue());
        }

        [Test]
        public void ShouldSetLazyProperty()
        {
            Convention(x => x.LazyLoad());

            VerifyModel(x => x.Lazy.ShouldEqual(true));
        }

        [Test]
        public void ShouldSetNotFoundProperty()
        {
            Convention(x => x.NotFound.Ignore());

            VerifyModel(x => x.NotFound.ShouldEqual("ignore"));
        }

        [Test]
        public void ShouldSetNullableProperty()
        {
            Convention(x => x.Nullable());

            VerifyModel(x => x.Columns.First().NotNull.ShouldBeFalse());
        }

        [Test]
        public void ShouldSetPropertyRefProperty()
        {
            Convention(x => x.PropertyRef("xxx"));

            VerifyModel(x => x.PropertyRef.ShouldEqual("xxx"));
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
            Convention(x => x.UniqueKey("xxx"));

            VerifyModel(x => x.Columns.First().UniqueKey.ShouldEqual("xxx"));
        }

        [Test]
        public void ShouldSetUpdateProperty()
        {
            Convention(x => x.Update());

            VerifyModel(x => x.Update.ShouldBeTrue());
        }

        [Test]
        public void ShouldSetForeignKeyProperty()
        {
            Convention(x => x.ForeignKey("xxx"));

            VerifyModel(x => x.ForeignKey.ShouldEqual("xxx"));
        }

        #region Helpers

        private void Convention(Action<IManyToOneInstance> convention)
        {
            model.Conventions.Add(new ReferenceConventionBuilder().Always(convention));
        }

        private void VerifyModel(Action<ManyToOneMapping> modelVerification)
        {
            var classMap = new ClassMap<ExampleClass>();
            var map = classMap.References(x => x.Parent);

            model.Add(classMap);

            var generatedModels = model.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == typeof(ExampleClass)) != null)
                .Classes.First()
                .References.First();

            modelVerification(modelInstance);
        }

        #endregion
    }
}