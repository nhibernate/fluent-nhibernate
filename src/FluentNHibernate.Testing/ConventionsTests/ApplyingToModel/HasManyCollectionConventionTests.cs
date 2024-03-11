using System;
using System.Linq;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Testing.FluentInterfaceTests;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.ApplyingToModel;

[TestFixture]
public class HasManyCollectionConventionTests
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
    public void ShouldSetBatchSizeProperty()
    {
        Convention(x => x.BatchSize(100));

        VerifyModel(x => x.BatchSize.ShouldEqual(100));
    }

    [Test]
    public void ShouldSetCacheProperty()
    {
        Convention(x => x.Cache.ReadWrite());

        VerifyModel(x => x.Cache.Usage.ShouldEqual("read-write"));
    }

    [Test]
    public void ShouldSetCascadeProperty()
    {
        Convention(x => x.Cascade.None());

        VerifyModel(x => x.Cascade.ShouldEqual("none"));
    }

    [Test]
    public void ShouldSetCheckConstraintProperty()
    {
        Convention(x => x.Check("constraint = 0"));

        VerifyModel(x => x.Check.ShouldEqual("constraint = 0"));
    }

    [Test]
    public void ShouldSetCollectionTypeProperty()
    {
        Convention(x => x.CollectionType<string>());

        VerifyModel(x => x.CollectionType.GetUnderlyingSystemType().ShouldEqual(typeof(string)));
    }

    [Test]
    public void ShouldSetFetchProperty()
    {
        Convention(x => x.Fetch.Select());

        VerifyModel(x => x.Fetch.ShouldEqual("select"));
    }

    [Test]
    public void ShouldSetGenericProperty()
    {
        Convention(x => x.Generic());

        VerifyModel(x => x.Generic.ShouldBeTrue());
    }

    [Test]
    public void ShouldSetInverseProperty()
    {
        Convention(x => x.Inverse());

        VerifyModel(x => x.Inverse.ShouldBeTrue());
    }

    [Test]
    public void ShouldSetKeyColumnNameProperty()
    {
        Convention(x => x.Key.Column("xxx"));

        VerifyModel(x => x.Key.Columns.First().Name.ShouldEqual("xxx"));
    }

    [Test]
    public void ShouldSetElementColumnNameProperty()
    {
        Convention(x => x.Element.Column("xxx"));

        VerifyModel(x => x.Element.Columns.First().Name.ShouldEqual("xxx"));
    }

    [Test]
    public void ShouldSetElementTypePropertyUsingGeneric()
    {
        Convention(x => x.Element.Type<string>());

        VerifyModel(x => x.Element.Type.GetUnderlyingSystemType().ShouldEqual(typeof(string)));
    }

    [Test]
    public void ShouldSetElementTypePropertyUsingTypeOf()
    {
        Convention(x => x.Element.Type(typeof(string)));

        VerifyModel(x => x.Element.Type.GetUnderlyingSystemType().ShouldEqual(typeof(string)));
    }

    [Test]
    public void ShouldSetElementTypePropertyUsingString() {
        Convention(x => x.Element.Type(typeof(string).AssemblyQualifiedName));

        VerifyModel(x => x.Element.Type.GetUnderlyingSystemType().ShouldEqual(typeof(string)));
    }

    [Test]
    public void ShouldSetLazyProperty()
    {
        Convention(x => x.LazyLoad());

        VerifyModel(x => x.Lazy.ShouldEqual(Lazy.True));
    }

    [Test]
    public void ShouldSetOptimisticLockProperty()
    {
        Convention(x => x.OptimisticLock());

        VerifyModel(x => x.OptimisticLock.ShouldEqual(true));
    }

    [Test]
    public void ShouldSetPersisterProperty()
    {
        Convention(x => x.Persister<SecondCustomPersister>());

        VerifyModel(x => x.Persister.GetUnderlyingSystemType().ShouldEqual(typeof(SecondCustomPersister)));
    }

    [Test]
    public void ShouldSetSchemaProperty()
    {
        Convention(x => x.Schema("test"));

        VerifyModel(x => x.Schema.ShouldEqual("test"));
    }

    [Test]
    public void ShouldSetWhereProperty()
    {
        Convention(x => x.Where("y = 2"));

        VerifyModel(x => x.Where.ShouldEqual("y = 2"));
    }

    [Test]
    public void ShouldSetForeignKeyProperty()
    {
        Convention(x => x.Key.ForeignKey("xxx"));

        VerifyModel(x => x.Key.ForeignKey.ShouldEqual("xxx"));
    }

    [Test]
    public void ShouldSetPropertyRefProperty()
    {
        Convention(x => x.Key.PropertyRef("xxx"));

        VerifyModel(x => x.Key.PropertyRef.ShouldEqual("xxx"));
    }

    [Test]
    public void KeyNullableShouldSetModelValue()
    {
        Convention(x => x.KeyNullable());

        VerifyModel(x => x.Key.NotNull.ShouldBeFalse());
    }

    [Test]
    public void KeyNotNullableShouldSetModelValue()
    {
        Convention(x => x.Not.KeyNullable());

        VerifyModel(x => x.Key.NotNull.ShouldBeTrue());
    }

    [Test]
    public void KeyUpdateShouldSetModelValue()
    {
        Convention(x => x.KeyUpdate());

        VerifyModel(x => x.Key.Update.ShouldBeTrue());
    }

    [Test]
    public void KeyNotUpdateShouldSetModelValue()
    {
        Convention(x => x.Not.KeyUpdate());

        VerifyModel(x => x.Key.Update.ShouldBeFalse());
    }

    [Test]
    public void ShouldSetTableNameProperty()
    {
        Convention(x => x.Table("xxx"));

        VerifyModel(x => x.TableName.ShouldEqual("xxx"));
    }
        
    [Test]
    public void ShouldChangeCollectionTypeToList()
    {
        Convention(x => { x.AsList(); x.Index.Column("position"); } );

        VerifyModel(x =>
        {
            x.Collection.ShouldEqual(Collection.List);
            x.Index.ShouldNotBeNull();  // a list without index will result in wrong xml
        });
    }
        
    #region Helpers

    private void Convention(Action<ICollectionInstance> convention)
    {
        model.Conventions.Add(new CollectionConventionBuilder().Always(convention));
    }

    private void VerifyModel(Action<CollectionMapping> modelVerification)
    {
        var classMap = new ClassMap<ExampleInheritedClass>();
        classMap.Id(x => x.Id);
        var map = classMap.HasMany(x => x.Children);

        model.Add(classMap);

        var generatedModels = model.BuildMappings();
        var modelInstance = generatedModels
            .First(x => x.Classes.FirstOrDefault(c => c.Type == typeof(ExampleInheritedClass)) is not null)
            .Classes.First()
            .Collections.First();

        modelVerification(modelInstance);
    }

    #endregion
}
