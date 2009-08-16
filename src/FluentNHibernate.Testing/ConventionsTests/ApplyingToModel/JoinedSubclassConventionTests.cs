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
    public class JoinedSubclassConventionTests
    {
        private PersistenceModel model;

        [SetUp]
        public void CreatePersistenceModel()
        {
            model = new PersistenceModel();
        }

        [Test]
        public void ShouldSetAbstractProperty()
        {
            Convention(x => x.Abstract());

            VerifyModel(x => x.Abstract.ShouldBeTrue());
        }

        [Test]
        public void ShouldSetCheckConstraintProperty()
        {
            Convention(x => x.Check("xxx"));

            VerifyModel(x => x.Check.ShouldEqual("xxx"));
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
        public void ShouldSetProxyProperty()
        {
            Convention(x => x.Proxy(typeof(string)));

            VerifyModel(x => x.Proxy.ShouldEqual(typeof(string).AssemblyQualifiedName));
        }

        [Test]
        public void ShouldSetSchemaProperty()
        {
            Convention(x => x.Schema("xxx"));

            VerifyModel(x => x.Schema.ShouldEqual("xxx"));
        }

        [Test]
        public void ShouldSetSelectBeforeUpdateProperty()
        {
            Convention(x => x.SelectBeforeUpdate());

            VerifyModel(x => x.SelectBeforeUpdate.ShouldBeTrue());
        }

        [Test]
        public void ShouldSetTableNameProperty()
        {
            Convention(x => x.Table("value"));

            VerifyModel(x => x.TableName.ShouldEqual("value"));
        }


        [Test]
        public void SubselectShouldntBeOverwritten()
        {
            Convention(x => x.Subselect("xxx"));

            VerifyModel(x => x.Subselect.ShouldEqual("xxx"));
        }

        [Test]
        public void PersisterShouldntBeOverwritten()
        {
            Convention(x => x.Persister<SecondCustomPersister>());

            VerifyModel(x => x.Persister.GetUnderlyingSystemType().ShouldEqual(typeof(SecondCustomPersister)));
        }

        [Test]
        public void BatchSizeShouldntBeOverwritten()
        {
            Convention(x => x.BatchSize(100));

            VerifyModel(x => x.BatchSize.ShouldEqual(100));
        }

        #region Helpers

        private void Convention(Action<IJoinedSubclassInstance> convention)
        {
            model.Conventions.Add(new JoinedSubclassConventionBuilder().Always(convention));
        }

        private void VerifyModel(Action<JoinedSubclassMapping> modelVerification)
        {
            var classMap = new ClassMap<ExampleClass>();
            var subclassMap = new SubclassMap<ExampleInheritedClass>();

            model.Add(classMap);
            model.Add(subclassMap);

            var generatedModels = model.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == typeof(ExampleClass)) != null)
                .Classes.First()
                .Subclasses.First();

            modelVerification((JoinedSubclassMapping)modelInstance);
        }

        #endregion
    }
}