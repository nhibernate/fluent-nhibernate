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

namespace FluentNHibernate.Testing.ConventionsTests.OverridingFluentInterface
{
    [TestFixture]
    public class JoinConventionTests
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
        public void TableShouldntBeOverwritten()
        {
            Mapping(x => x.Table("table"));

            Convention(x => x.Table("xxx"));

            VerifyModel(x => x.TableName.ShouldEqual("table"));
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
            Mapping(x => x.Subselect("select"));

            Convention(x => x.Subselect("xxx"));

            VerifyModel(x => x.Subselect.ShouldEqual("select"));
        }

        [Test]
        public void FetchShouldntBeOverwritten()
        {
            Mapping(x => x.Fetch.Join());

            Convention(x => x.Fetch.Select());

            VerifyModel(x => x.Fetch.ShouldEqual("join"));
        }

        [Test]
        public void InverseShouldntBeOverwritten()
        {
            Mapping(x => x.Inverse());

            Convention(x => x.Not.Inverse());

            VerifyModel(x => x.Inverse.ShouldBeTrue());
        }

        [Test]
        public void OptionalShouldntBeOverwritten()
        {
            Mapping(x => x.Optional());

            Convention(x => x.Not.Optional());

            VerifyModel(x => x.Optional.ShouldBeTrue());
        }

        #region Helpers

        private void Convention(Action<IJoinInstance> convention)
        {
            model.Conventions.Add(new JoinConventionBuilder().Always(convention));
        }

        private void Mapping(Action<JoinPart<ExampleClass>> mappingDefinition)
        {
            var classMap = new ClassMap<ExampleClass>();

            classMap.Join("table", mappingDefinition);

            mapping = classMap;
            mappingType = typeof(ExampleClass);
        }

        private void VerifyModel(Action<JoinMapping> modelVerification)
        {
            model.Add(mapping);

            var generatedModels = model.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == mappingType) != null)
                .Classes.First()
                .Joins.First();

            modelVerification(modelInstance);
        }

        #endregion
    }
}