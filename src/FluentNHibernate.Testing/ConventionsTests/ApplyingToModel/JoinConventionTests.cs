using System;
using System.Linq;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Testing.FluentInterfaceTests;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.ApplyingToModel
{
    [TestFixture]
    public class JoinConventionTests
    {
        private PersistenceModel model;

        [SetUp]
        public void CreatePersistenceModel()
        {
            model = new PersistenceModel();
        }

        [Test]
        public void ShouldSetTableProperty()
        {
            Convention(x => x.Table("xxx"));

            VerifyModel(x => x.TableName.ShouldEqual("xxx"));
        }

        [Test]
        public void ShouldSetSchemaProperty()
        {
            Convention(x => x.Schema("xxx"));

            VerifyModel(x => x.Schema.ShouldEqual("xxx"));
        }

        [Test]
        public void ShouldSetSubselectProperty()
        {
            Convention(x => x.Subselect("xxx"));

            VerifyModel(x => x.Subselect.ShouldEqual("xxx"));
        }

        [Test]
        public void ShouldSetFetchProperty()
        {
            Convention(x => x.Fetch.Select());

            VerifyModel(x => x.Fetch.ShouldEqual("select"));
        }

        [Test]
        public void ShouldSetInverseProperty()
        {
            Convention(x => x.Inverse());

            VerifyModel(x => x.Inverse.ShouldBeTrue());
        }

        [Test]
        public void ShouldSetOptionalProperty()
        {
            Convention(x => x.Optional());

            VerifyModel(x => x.Optional.ShouldBeTrue());
        }

        #region Helpers

        private void Convention(Action<IJoinInstance> convention)
        {
            model.Conventions.Add(new JoinConventionBuilder().Always(convention));
        }

        private void VerifyModel(Action<JoinMapping> modelVerification)
        {
            var classMap = new ClassMap<ExampleClass>();

            classMap.Join("table", m => {});

            model.Add(classMap);

            var generatedModels = model.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == typeof(ExampleClass)) != null)
                .Classes.First()
                .Joins.First();

            modelVerification(modelInstance);
        }

        #endregion
    }
}