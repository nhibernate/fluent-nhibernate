using System;
using System.Linq;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.ApplyingToModel;

[TestFixture]
public class DynamicComponentConventionTests
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
    public void ShouldSetInsertProperty()
    {
        Convention(x => x.Insert());

        VerifyModel(x => x.Insert.ShouldBeTrue());
    }

    [Test]
    public void ShouldSetUpdateProperty()
    {
        Convention(x => x.Update());

        VerifyModel(x => x.Update.ShouldBeTrue());
    }

    [Test]
    public void ShouldSetUniqueProperty()
    {
        Convention(x => x.Unique());

        VerifyModel(x => x.Unique.ShouldBeTrue());
    }

    [Test]
    public void ShouldSetOptimisticLockProperty()
    {
        Convention(x => x.OptimisticLock());

        VerifyModel(x => x.OptimisticLock.ShouldBeTrue());
    }

    #region Helpers

    void Convention(Action<IDynamicComponentInstance> convention)
    {
        model.Conventions.Add(new DynamicComponentConventionBuilder().Always(convention));
    }

    void VerifyModel(Action<ComponentMapping> modelVerification)
    {
        var classMap = new ClassMap<PropertyTarget>();
        classMap.Id(x => x.Id);
        var map = classMap.DynamicComponent(x => x.ExtensionData, m =>
        {
            m.Map(x => (string)x["Name"]);
            m.Map(x => (int)x["Age"]);
            m.Map(x => (string)x["Profession"]);
        });

        model.Add(classMap);

        var generatedModels = model.BuildMappings();
        var modelInstance = (ComponentMapping)generatedModels
            .First(x => x.Classes.FirstOrDefault(c => c.Type == typeof(PropertyTarget)) is not null)
            .Classes.First()
            .Components.First(x => x is ComponentMapping);

        modelVerification(modelInstance);
    }

    #endregion
}
