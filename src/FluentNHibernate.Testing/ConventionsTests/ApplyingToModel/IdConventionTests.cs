using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Engine;
using NHibernate.Id;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.ApplyingToModel;

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

        VerifyModel(x => x.Columns.First().Length.ShouldEqual(200));
    }

    [Test]
    public void ShouldSetPrecisionValueProperty()
    {
        Convention(x => x.Precision(200));

        VerifyModel(x => x.Columns.First().Precision.ShouldEqual(200));
    }

    [Test]
    public void ShouldSetScaleValueProperty()
    {
        Convention(x => x.Scale(200));

        VerifyModel(x => x.Columns.First().Scale.ShouldEqual(200));
    }

    [Test]
    public void ShouldSetNullableValueProperty()
    {
        Convention(x => x.Nullable());

        VerifyModel(x => x.Columns.First().NotNull.ShouldEqual(false));
    }

    [Test]
    public void ShouldSetUniqueValueProperty()
    {
        Convention(x => x.Unique());

        VerifyModel(x => x.Columns.First().Unique.ShouldEqual(true));
    }

    [Test]
    public void ShouldSetUniqueKeyValueProperty()
    {
        Convention(x => x.UniqueKey("key"));

        VerifyModel(x => x.Columns.First().UniqueKey.ShouldEqual("key"));
    }

    [Test]
    public void ShouldSetSqlTypeValueProperty()
    {
        Convention(x => x.CustomSqlType("sql"));

        VerifyModel(x => x.Columns.First().SqlType.ShouldEqual("sql"));
    }

    [Test]
    public void ShouldSetIndexValueProperty()
    {
        Convention(x => x.Index("idx"));

        VerifyModel(x => x.Columns.First().Index.ShouldEqual("idx"));
    }

    [Test]
    public void ShouldSetCheckValueProperty()
    {
        Convention(x => x.Check("constraint"));

        VerifyModel(x => x.Columns.First().Check.ShouldEqual("constraint"));
    }

    [Test]
    public void ShouldSetDefaultValueProperty()
    {
        Convention(x => x.Default(200));

        VerifyModel(x => x.Columns.First().Default.ShouldEqual("200"));
    }

    [Test]
    public void ShouldSetTypeValueProperty()
    {
        Convention(x => x.CustomType<string>());

        VerifyModel(x => x.Type.GetUnderlyingSystemType().ShouldEqual(typeof(string)));
    }

    [Test]
    public void ShouldSetCustomGenerator()
    {
        Convention(x => x.GeneratedBy.Custom<CustomGenerator>());

        VerifyModel(x => x.Generator.Class.ShouldEqual(typeof(CustomGenerator).AssemblyQualifiedName));
    }

    [Test]
    public void ShouldSetTriggerIdentityGenerator()
    {
        Convention(x => x.GeneratedBy.TriggerIdentity());

        VerifyModel(x => x.Generator.Class.ShouldEqual("trigger-identity"));
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
            .First(x => x.Classes.FirstOrDefault(c => c.Type == typeof(ExampleClass)) is not null)
            .Classes.First()
            .Id;

        modelVerification((IdMapping)modelInstance);
    }

    #endregion

    private class CustomGenerator : IIdentifierGenerator
    {
        public object Generate(ISessionImplementor session, object obj)
        {
            return null;
        }

        public Task<object> GenerateAsync(ISessionImplementor session, object obj, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
