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
public class HibernateMappingConventionTests
{
    PersistenceModel model;

    [SetUp]
    public void CreatePersistanceModel()
    {
        model = new PersistenceModel();
    }

    [Test]
    public void ShouldSetCatalogProperty()
    {
        Convention(x => x.Catalog("cat"));

        VerifyModel(x => x.Catalog.ShouldEqual("cat"));
    }

    [Test]
    public void ShouldSetDefaultAccessProperty()
    {
        Convention(x => x.DefaultAccess.Field());

        VerifyModel(x => x.DefaultAccess.ShouldEqual("field"));
    }

    [Test]
    public void ShouldSetDefaultCascadeProperty()
    {
        Convention(x => x.DefaultCascade.All());

        VerifyModel(x => x.DefaultCascade.ShouldEqual("all"));
    }

    [Test]
    public void ShouldSetSchemaProperty()
    {
        Convention(x => x.Schema("dbo"));

        VerifyModel(x => x.Schema.ShouldEqual("dbo"));
    }

    [Test]
    public void ShouldSetDefaultLazyProperty()
    {
        Convention(x => x.DefaultLazy());

        VerifyModel(x => x.DefaultLazy.ShouldEqual(true));
    }

    [Test]
    public void ShouldSetAutoImportProperty()
    {
        Convention(x => x.Not.AutoImport());

        VerifyModel(x => x.AutoImport.ShouldEqual(false));
    }

    #region Helpers

    void Convention(Action<IHibernateMappingInstance> convention)
    {
        model.Conventions.Add(new HibernateMappingConventionBuilder().Always(convention));
    }

    void VerifyModel(Action<HibernateMapping> modelVerification)
    {
        var classMap = new ClassMap<ExampleClass>();
        classMap.Id(x => x.Id);

        model.Add(classMap);

        var generatedModels = model.BuildMappings();
        var modelInstance = generatedModels
            .First(x => x.Classes.FirstOrDefault(c => c.Type == typeof(ExampleClass)) is not null);

        modelVerification(modelInstance);
    }

    #endregion
}
