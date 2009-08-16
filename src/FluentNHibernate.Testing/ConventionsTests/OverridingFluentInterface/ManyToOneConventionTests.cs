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

namespace FluentNHibernate.Testing.ConventionsTests.OverridingFluentInterface
{
    [TestFixture]
    public class ManyToOneConventionTests
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
        public void CascadeShouldntBeOverwritten()
        {
            Mapping(x => x.Cascade.All());

            Convention(x => x.Cascade.None());

            VerifyModel(x => x.Cascade.ShouldEqual("all"));
        }

        [Test]
        public void ClassShouldntBeOverwritten()
        {
            Mapping(x => x.Class(typeof(string)));

            Convention(x => x.CustomClass(typeof(int)));

            VerifyModel(x => x.Class.GetUnderlyingSystemType().ShouldEqual(typeof(string)));
        }

        [Test]
        public void ColumnShouldntBeOverwritten()
        {
            Mapping(x => x.Column("name"));

            Convention(x => x.Column("xxx"));

            VerifyModel(x => x.Columns.First().Name.ShouldEqual("name"));
        }

        [Test]
        public void FetchShouldntBeOverwritten()
        {
            Mapping(x => x.Fetch.Join());

            Convention(x => x.Fetch.Select());

            VerifyModel(x => x.Fetch.ShouldEqual("join"));
        }

        [Test]
        public void IndexShouldntBeOverwritten()
        {
            Mapping(x => x.Index("index"));

            Convention(x => x.Index("value"));

            VerifyModel(x => x.Columns.First().Index.ShouldEqual("index"));
        }

        [Test]
        public void InsertShouldntBeOverwritten()
        {
            Mapping(x => x.Insert());

            Convention(x => x.Not.Insert());

            VerifyModel(x => x.Insert.ShouldBeTrue());
        }

        [Test]
        public void LazyShouldntBeOverwritten()
        {
            Mapping(x => x.LazyLoad());

            Convention(x => x.Not.LazyLoad());

            VerifyModel(x => x.Lazy.ShouldEqual(true));
        }

        [Test]
        public void NotFoundShouldntBeOverwritten()
        {
            Mapping(x => x.NotFound.Exception());

            Convention(x => x.NotFound.Ignore());

            VerifyModel(x => x.NotFound.ShouldEqual("exception"));
        }

        [Test]
        public void NullableShouldntBeOverwritten()
        {
            Mapping(x => x.Nullable());

            Convention(x => x.Not.Nullable());

            VerifyModel(x => x.Columns.First().NotNull.ShouldBeFalse());
        }

        [Test]
        public void PropertyRefShouldntBeOverwritten()
        {
            Mapping(x => x.PropertyRef("ref"));

            Convention(x => x.PropertyRef("xxx"));

            VerifyModel(x => x.PropertyRef.ShouldEqual("ref"));
        }

        [Test]
        public void ReadOnlyShouldntBeOverwritten()
        {
            Mapping(x => x.ReadOnly());

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
            Mapping(x => x.Unique());

            Convention(x => x.Not.Unique());

            VerifyModel(x => x.Columns.First().Unique.ShouldBeTrue());
        }

        [Test]
        public void UniqueKeyShouldntBeOverwritten()
        {
            Mapping(x => x.UniqueKey("key"));

            Convention(x => x.UniqueKey("xxx"));

            VerifyModel(x => x.Columns.First().UniqueKey.ShouldEqual("key"));
        }

        [Test]
        public void UpdateShouldntBeOverwritten()
        {
            Mapping(x => x.Update());

            Convention(x => x.Not.Update());

            VerifyModel(x => x.Update.ShouldBeTrue());
        }

        [Test]
        public void ForeignKeyShouldntBeOverwritten()
        {
            Mapping(x => x.ForeignKey("key"));

            Convention(x => x.ForeignKey("xxx"));

            VerifyModel(x => x.ForeignKey.ShouldEqual("key"));
        }

        #region Helpers

        private void Convention(Action<IManyToOneInstance> convention)
        {
            model.Conventions.Add(new ReferenceConventionBuilder().Always(convention));
        }

        private void Mapping(Action<ManyToOnePart<ExampleParentClass>> mappingDefinition)
        {
            var classMap = new ClassMap<ExampleClass>();
            var map = classMap.References(x => x.Parent);

            mappingDefinition(map);

            mapping = classMap;
            mappingType = typeof(ExampleClass);
        }

        private void VerifyModel(Action<ManyToOneMapping> modelVerification)
        {
            model.Add(mapping);

            var generatedModels = model.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == mappingType) != null)
                .Classes.First()
                .References.First();

            modelVerification(modelInstance);
        }

        #endregion
    }
}