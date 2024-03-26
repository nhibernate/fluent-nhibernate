using System;
using System.Linq;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.Identity;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.ApplyingToModel;

[TestFixture]
public class KeyManyToOneConventionTests
{
    PersistenceModel model;

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
    public void ShouldSetForeignKeyValueProperty() 
    {
        Convention(x => x.ForeignKey("foo"));

        VerifyModel(x => x.ForeignKey.ShouldEqual("foo"));
    }

    [Test]
    public void ShouldSetLazyValueProperty() 
    {
        Convention(x => x.Lazy());

        VerifyModel(x => x.Lazy.ShouldEqual(true));
    }

    [Test]
    public void ShouldSetNotFoundValueProperty() 
    {
        Convention(x => x.NotFound.Ignore());

        VerifyModel(x => x.NotFound.ShouldEqual("ignore"));
    }

    #region Helpers

    void Convention(Action<IKeyManyToOneInstance> convention)
    {
        model.Conventions.Add(new KeyManyToOneConventionBuilder().Always(convention));
    }

    void VerifyModel(Action<KeyManyToOneMapping> modelVerification)
    {
        var classMap = new ClassMap<ExampleClass>();
        var map = classMap.CompositeId()
            .KeyProperty(x => x.Id)
            .KeyReference(x => x.Parent);

        model.Add(classMap);

        var generatedModels = model.BuildMappings();
        var modelInstance = generatedModels
            .First(x => x.Classes.FirstOrDefault(c => c.Type == typeof(ExampleClass)) is not null)
            .Classes.First()
            .Id;

        modelVerification((KeyManyToOneMapping)((CompositeIdMapping)modelInstance).Keys.First(x => x is KeyManyToOneMapping));
    }

    #endregion
}
