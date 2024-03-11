using System;
using System.Linq;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.ApplyingToModel;

[TestFixture]
public class HasOneConventionTests
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
        Convention(x => x.Class<string>());

        VerifyModel(x => x.Class.GetUnderlyingSystemType().ShouldEqual(typeof(string)));
    }

    [Test]
    public void ShouldSetConstrainedProperty()
    {
        Convention(x => x.Constrained());

        VerifyModel(x => x.Constrained.ShouldBeTrue());
    }

    [Test]
    public void ShouldSetFetchProperty()
    {
        Convention(x => x.Fetch.Select());

        VerifyModel(x => x.Fetch.ShouldEqual("select"));
    }

    [Test]
    public void ShouldSetForeignKeyProperty()
    {
        Convention(x => x.ForeignKey("xxx"));

        VerifyModel(x => x.ForeignKey.ShouldEqual("xxx"));
    }

    [Test]
    public void ShouldSetLazyProperty()
    {
        Convention(x => x.LazyLoad());

        VerifyModel(x => x.Lazy.ShouldEqual(Laziness.Proxy.ToString()));
    }

    [Test]
    public void ShouldSetPropertyRefProperty()
    {
        Convention(x => x.PropertyRef("value"));

        VerifyModel(x => x.PropertyRef.ShouldEqual("value"));
    }

    #region Helpers

    private void Convention(Action<IOneToOneInstance> convention)
    {
        model.Conventions.Add(new HasOneConventionBuilder().Always(convention));
    }

    private void VerifyModel(Action<OneToOneMapping> modelVerification)
    {
        var classMap = new ClassMap<ExampleClass>();
        classMap.Id(x => x.Id);
        var map = classMap.HasOne(x => x.Parent);

        model.Add(classMap);

        var generatedModels = model.BuildMappings();
        var modelInstance = generatedModels
            .First(x => x.Classes.FirstOrDefault(c => c.Type == typeof(ExampleClass)) is not null)
            .Classes.First()
            .OneToOnes.First();

        modelVerification(modelInstance);
    }

    #endregion
}
