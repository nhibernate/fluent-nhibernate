using System;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Automapping.TestFixtures.CustomTypes;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.ApplyingToModel
{
    [TestFixture]
    public class IdConventionTests
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
        public void ShouldSetColumnProperty()
        {
            Convention(x => x.Column("xxx"));

            VerifyModel(x => x.Columns.First().Name.ShouldEqual("xxx"));
        }

        [Test]
        public void ShouldSetGeneratedByProperty()
        {
            Convention(x => x.GeneratedBy.Identity());

            VerifyModel(x => x.Generator.Class.ShouldEqual("identity"));
        }

        [Test]
        public void ShouldSetUnsavedValueProperty()
        {
            Convention(x => x.UnsavedValue("two"));

            VerifyModel(x => x.UnsavedValue.ShouldEqual("two"));
        }

        [Test]
        public void ShouldSetLengthValueProperty()
        {
            Convention(x => x.Length(200));

            VerifyModel(x => x.Length.ShouldEqual(200));
        }

        [Test]
        public void ShouldSetTypeValueProperty()
        {
            Convention(x => x.Type<string>());

            VerifyModel(x => x.Type.GetUnderlyingSystemType().ShouldEqual(typeof(string)));
        }

        #region Helpers

        private void Convention(Action<IIdentityInstance> convention)
        {
            model.Conventions.Add(new IdConventionBuilder().Always(convention));
        }

        private void VerifyModel(Action<IdMapping> modelVerification)
        {
            var classMap = new ClassMap<ExampleClass>();
            var map = classMap.Id(x => x.Id);

            model.Add(classMap);

            var generatedModels = model.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == typeof(ExampleClass)) != null)
                .Classes.First()
                .Id;

            modelVerification((IdMapping)modelInstance);
        }

        #endregion
    }
}