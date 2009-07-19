using System;
using System.Linq;
using FluentNHibernate.AutoMap.TestFixtures;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class FluentInterfaceOverridingConventionsJoinedSubclassTests
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
        public void AbstractShouldntBeOverwritten()
        {
            Mapping(x => x.Abstract());

            Convention(x => x.Not.Abstract());

            VerifyModel(x => x.Abstract.ShouldBeTrue());
        }

        [Test]
        public void CheckConstraintShouldntBeOverwritten()
        {
            Mapping(x => x.CheckConstraint("const"));

            Convention(x => x.CheckConstraint("xxx"));

            VerifyModel(x => x.Check.ShouldEqual("const"));
        }

        [Test]
        public void DynamicInsertShouldntBeOverwritten()
        {
            Mapping(x => x.DynamicInsert());

            Convention(x => x.Not.DynamicInsert());

            VerifyModel(x => x.DynamicInsert.ShouldBeTrue());
        }

        [Test]
        public void DynamicUpdateShouldntBeOverwritten()
        {
            Mapping(x => x.DynamicUpdate());

            Convention(x => x.Not.DynamicUpdate());

            VerifyModel(x => x.DynamicUpdate.ShouldBeTrue());
        }

        [Test]
        public void LazyLoadShouldntBeOverwritten()
        {
            Mapping(x => x.LazyLoad());

            Convention(x => x.Not.LazyLoad());

            VerifyModel(x => x.Lazy.ShouldEqual(Laziness.True));
        }

        [Test]
        public void ProxyShouldntBeOverwritten()
        {
            Mapping(x => x.Proxy(typeof(int)));

            Convention(x => x.Proxy(typeof(string)));

            VerifyModel(x => x.Proxy.ShouldEqual(typeof(int).AssemblyQualifiedName));
        }

        [Test]
        public void SchemaShouldntBeOverwritten()
        {
            Mapping(x => x.Schema("dbo"));

            Convention(x => x.Schema("xxx"));

            VerifyModel(x => x.Schema.ShouldEqual("dbo"));
        }

        [Test]
        public void SelectBeforeUpdateShouldntBeOverwritten()
        {
            Mapping(x => x.SelectBeforeUpdate());

            Convention(x => x.Not.SelectBeforeUpdate());

            VerifyModel(x => x.SelectBeforeUpdate.ShouldBeTrue());
        }

        [Test]
        public void TableNameShouldntBeOverwritten()
        {
            Mapping(x => x.Table("table"));

            Convention(x => x.Table("value"));

            VerifyModel(x => x.TableName.ShouldEqual("table"));
        }

        #region Helpers

        private void Convention(Action<IJoinedSubclassInstance> convention)
        {
            model.ConventionFinder.Add(new JoinedSubclassConventionBuilder().Always(convention));
        }

        private void Mapping(Action<JoinedSubClassPart<ExampleInheritedClass>> mappingDefinition)
        {
            var classMap = new ClassMap<ExampleClass>();
            classMap.JoinedSubClass("key", mappingDefinition);

            mapping = classMap;
            mappingType = typeof(ExampleClass);
        }

        private void VerifyModel(Action<JoinedSubclassMapping> modelVerification)
        {
            model.Add(mapping);

            var generatedModels = model.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == mappingType) != null)
                .Classes.First()
                .Subclasses.First();

            modelVerification((JoinedSubclassMapping)modelInstance);
        }

        #endregion
    }
}