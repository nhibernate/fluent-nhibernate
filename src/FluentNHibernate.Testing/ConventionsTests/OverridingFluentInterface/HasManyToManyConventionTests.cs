using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Testing.FluentInterfaceTests;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.OverridingFluentInterface;

[TestFixture]
public class HasManyToManyConventionTests
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
    public void AccessShouldntBeOverwritten()
    {
        Mapping(x => x.Children, x => x.Access.Field());

        Convention(x => x.Access.Property());

        VerifyModel(x => x.Access.ShouldEqual("field"));
    }

    [Test]
    public void BatchSizeShouldntBeOverwritten()
    {
        Mapping(x => x.Children, x => x.BatchSize(10));

        Convention(x => x.BatchSize(100));

        VerifyModel(x => x.BatchSize.ShouldEqual(10));
    }

    [Test]
    public void CacheShouldntBeOverwritten()
    {
        Mapping(x => x.Children, x => x.Cache.ReadOnly());

        Convention(x => x.Cache.ReadWrite());

        VerifyModel(x => x.Cache.Usage.ShouldEqual("read-only"));
    }

    [Test]
    public void CascadeShouldntBeOverwritten()
    {
        Mapping(x => x.Children, x => x.Cascade.All());

        Convention(x => x.Cascade.None());

        VerifyModel(x => x.Cascade.ShouldEqual("all"));
    }

    [Test]
    public void CheckConstraintShouldntBeOverwritten()
    {
        Mapping(x => x.Children, x => x.Check("constraint = 1"));

        Convention(x => x.Check("constraint = 0"));

        VerifyModel(x => x.Check.ShouldEqual("constraint = 1"));
    }

    [Test]
    public void CollectionTypeShouldntBeOverwritten()
    {
        Mapping(x => x.Children, x => x.CollectionType<int>());

        Convention(x => x.CollectionType<string>());

        VerifyModel(x => x.CollectionType.GetUnderlyingSystemType().ShouldEqual(typeof(int)));
    }

    [Test]
    public void FetchShouldntBeOverwritten()
    {
        Mapping(x => x.Children, x => x.Fetch.Join());

        Convention(x => x.Fetch.Select());

        VerifyModel(x => x.Fetch.ShouldEqual("join"));
    }

    [Test]
    public void GenericShouldntBeOverwritten()
    {
        Mapping(x => x.Children, x => x.Generic());

        Convention(x => x.Not.Generic());

        VerifyModel(x => x.Generic.ShouldBeTrue());
    }

    [Test]
    public void InverseShouldntBeOverwritten()
    {
        Mapping(x => x.Children, x => x.Inverse());

        Convention(x => x.Not.Inverse());

        VerifyModel(x => x.Inverse.ShouldBeTrue());
    }

    [Test]
    public void ParentKeyColumnNameShouldntBeOverwritten()
    {
        Mapping(x => x.Children, x => x.ParentKeyColumn("name"));

        Convention(x => x.Key.Column("xxx"));

        VerifyModel(x => x.Key.Columns.First().Name.ShouldEqual("name"));
    }

    [Test]
    public void ElementColumnNameShouldntBeOverwritten()
    {
        Mapping(x => x.Children, x => x.Element("name"));

        Convention(x => x.Element.Column("xxx"));

        VerifyModel(x => x.Element.Columns.First().Name.ShouldEqual("name"));
    }

    [Test]
    public void ElementTypeShouldntBeOverwrittenUsingGeneric()
    {
        Mapping(x => x.Children, x => x.Element("xxx", e => e.Type<string>()));

        Convention(x => x.Element.Type<int>());

        VerifyModel(x => x.Element.Type.GetUnderlyingSystemType().ShouldEqual(typeof(string)));
    }

    [Test]
    public void ElementTypeShouldntBeOverwrittenUsingTypeOf()
    {
        Mapping(x => x.Children, x => x.Element("xxx", e => e.Type<string>()));

        Convention(x => x.Element.Type(typeof(int)));

        VerifyModel(x => x.Element.Type.GetUnderlyingSystemType().ShouldEqual(typeof(string)));
    }

    [Test]
    public void ElementTypeShouldntBeOverwrittenUsingString()
    {
        Mapping(x => x.Children, x => x.Element("xxx", e => e.Type<string>()));

        Convention(x => x.Element.Type(typeof(int).AssemblyQualifiedName));

        VerifyModel(x => x.Element.Type.GetUnderlyingSystemType().ShouldEqual(typeof(string)));
    }

    [Test]
    public void ChildKeyColumnNameShouldntBeOverwritten()
    {
        Mapping(x => x.Children, x => x.ChildKeyColumn("name"));

        Convention(x => x.Relationship.Column("xxx"));

        VerifyModel(x => ((ManyToManyMapping)x.Relationship).Columns.First().Name.ShouldEqual("name"));
    }

    [Test]
    public void LazyShouldntBeOverwritten()
    {
        Mapping(x => x.Children, x => x.LazyLoad());

        Convention(x => x.Not.LazyLoad());

        VerifyModel(x => x.Lazy.ShouldEqual(Lazy.True));
    }

    [Test]
    public void OptimisticLockShouldntBeOverwritten()
    {
        Mapping(x => x.Children, x => x.OptimisticLock());

        Convention(x => x.Not.OptimisticLock());

        VerifyModel(x => x.OptimisticLock.ShouldEqual(true));
    }

    [Test]
    public void PersisterShouldntBeOverwritten()
    {
        Mapping(x => x.Children, x => x.Persister<CustomPersister>());

        Convention(x => x.Persister<SecondCustomPersister>());

        VerifyModel(x => x.Persister.GetUnderlyingSystemType().ShouldEqual(typeof(CustomPersister)));
    }

    [Test]
    public void SchemaShouldntBeOverwritten()
    {
        Mapping(x => x.Children, x => x.Schema("dbo"));

        Convention(x => x.Schema("test"));

        VerifyModel(x => x.Schema.ShouldEqual("dbo"));
    }

    [Test]
    public void WhereShouldntBeOverwritten()
    {
        Mapping(x => x.Children, x => x.Where("x = 1"));

        Convention(x => x.Where("y = 2"));

        VerifyModel(x => x.Where.ShouldEqual("x = 1"));
    }

    [Test]
    public void TableNameShouldntBeOverwritten()
    {
        Mapping(x => x.Children, x => x.Table("table"));

        Convention(x => x.Table("xxx"));

        VerifyModel(x => x.TableName.ShouldEqual("table"));
    }

    [Test]
    public void ParentForeignKeyConstraintNameShouldntBeOverwritten()
    {
        Mapping(x => x.Children, x => x.ForeignKeyConstraintNames("parent", "child"));

        Convention(x => x.Key.ForeignKey("xxx"));

        VerifyModel(x => x.Key.ForeignKey.ShouldEqual("parent"));
    }

    [Test]
    public void PropertyRefShouldntBeOverwritten()
    {
        Mapping(x => x.Children, x => x.PropertyRef("ref"));

        Convention(x => x.Key.PropertyRef("xxx"));

        VerifyModel(x => x.Key.PropertyRef.ShouldEqual("ref"));
    }

    [Test]
    public void ChildForeignKeyConstraintNameShouldntBeOverwritten()
    {
        Mapping(x => x.Children, x => x.ForeignKeyConstraintNames("parent", "child"));

        Convention(x => x.Relationship.ForeignKey("xxx"));

        VerifyModel(x => ((ManyToManyMapping)x.Relationship).ForeignKey.ShouldEqual("child"));
    }

    #region Helpers

    private void Convention(Action<IManyToManyCollectionInstance> convention)
    {
        model.Conventions.Add(new ManyToManyCollectionConventionBuilder().Always(convention));
    }

    private void Mapping<TChild>(Expression<Func<ExampleInheritedClass, IEnumerable<TChild>>> property, Action<ManyToManyPart<TChild>> mappingDefinition)
    {
        var classMap = new ClassMap<ExampleInheritedClass>();
        classMap.Id(x => x.Id);
        var map = classMap.HasManyToMany(property);

        mappingDefinition(map);

        mapping = classMap;
        mappingType = typeof(ExampleInheritedClass);
    }

    private void VerifyModel(Action<CollectionMapping> modelVerification)
    {
        model.Add(mapping);

        var generatedModels = model.BuildMappings();
        var modelInstance = generatedModels
            .First(x => x.Classes.FirstOrDefault(c => c.Type == mappingType) is not null)
            .Classes.First()
            .Collections.First();

        modelVerification(modelInstance);
    }

    #endregion
}
