using System;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Automapping.TestFixtures.CustomTypes;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.OverridingFluentInterface;

[TestFixture]
public class PropertyConventionTests
{
    PersistenceModel model;
    IMappingProvider mapping;
    Type mappingType;

    [SetUp]
    public void CreatePersistenceModel()
    {
        model = new PersistenceModel();
    }

    [Test]
    public void AccessShouldntBeOverwritten()
    {
        Mapping(x => x.LineOne, x => x.Access.Field());

        Convention(x => x.Access.Property());

        VerifyModel(x => x.Access.ShouldEqual("field"));
    }

    [Test]
    public void ColumnNameShouldntBeOverwritten()
    {
        Mapping(x => x.LineOne, x => x.Column("xxx"));

        Convention(x => x.Column("yyy"));

        VerifyModel(x =>
        {
            x.Columns.Count().ShouldEqual(1);
            x.Columns.First().Name.ShouldEqual("xxx");
        });
    }

    [Test]
    public void SqlTypeShouldntBeOverwritten()
    {
        Mapping(x => x.LineOne, x => x.CustomSqlType("sql-type"));

        Convention(x => x.CustomSqlType("type"));

        VerifyModel(x => x.Columns.First().SqlType.ShouldEqual("sql-type"));
    }

    [Test]
    public void TypeShouldntBeOverwritten()
    {
        Mapping(x => x.LineOne, x => x.CustomType<CustomUserType>());

        Convention(x => x.CustomType<int>());

        VerifyModel(x => x.Type.Name.ShouldEqual(typeof(CustomUserType).AssemblyQualifiedName));
    }

    [Test]
    public void FormulaShouldntBeOverwritten()
    {
        Mapping(x => x.LineOne, x => x.Formula("form"));

        Convention(x => x.Formula("xxx"));

        VerifyModel(x => x.Formula.ShouldEqual("form"));
    }

    [Test]
    public void GeneratedShouldntBeOverwritten()
    {
        Mapping(x => x.LineOne, x => x.Generated.Always());

        Convention(x => x.Generated.Never());

        VerifyModel(x => x.Generated.ShouldEqual("always"));
    }

    [Test]
    public void InsertShouldntBeOverwritten()
    {
        Mapping(x => x.LineOne, x => x.Insert());

        Convention(x => x.Not.Insert());

        VerifyModel(x => x.Insert.ShouldBeTrue());
    }

    [Test]
    public void NullableShouldntBeOverwritten()
    {
        Mapping(x => x.LineOne, x => x.Nullable());

        Convention(x => x.Not.Nullable());

        VerifyModel(x => x.Columns.First().NotNull.ShouldBeFalse());
    }

    [Test]
    public void OptimisticLockShouldntBeOverwritten()
    {
        Mapping(x => x.LineOne, x => x.OptimisticLock());

        Convention(x => x.Not.OptimisticLock());

        VerifyModel(x => x.OptimisticLock.ShouldBeTrue());
    }

    [Test]
    public void ReadOnlyShouldntOverwriteInsert()
    {
        Mapping(x => x.LineOne, x => x.Insert());

        Convention(x => x.ReadOnly());

        VerifyModel(x => x.Insert.ShouldBeTrue());
    }

    [Test]
    public void ReadOnlyShouldntOverwriteUpdate()
    {
        Mapping(x => x.LineOne, x => x.Update());

        Convention(x => x.ReadOnly());

        VerifyModel(x => x.Update.ShouldBeTrue());
    }

    [Test]
    public void ReadOnlyShouldntBeOverwritten()
    {
        Mapping(x => x.LineOne, x => x.ReadOnly());

        Convention(x => x.Not.ReadOnly());

        VerifyModel(x =>
        {
            x.Insert.ShouldBeFalse();
            x.Update.ShouldBeFalse();
        });
    }

    [Test]
    public void UniqueShouldntBeOverwritten()
    {
        Mapping(x => x.LineOne, x => x.Unique());

        Convention(x => x.Not.Unique());

        VerifyModel(x => x.Columns.First().Unique.ShouldBeTrue());
    }

    [Test]
    public void UniqueKeyShouldntBeOverwritten()
    {
        Mapping(x => x.LineOne, x => x.UniqueKey("key"));

        Convention(x => x.UniqueKey("test"));

        VerifyModel(x => x.Columns.First().UniqueKey.ShouldEqual("key"));
    }

    [Test]
    public void UpdateShouldntBeOverwritten()
    {
        Mapping(x => x.LineOne, x => x.Update());

        Convention(x => x.Not.Update());

        VerifyModel(x => x.Update.ShouldBeTrue());
    }

    [Test]
    public void LengthShouldntBeOverwritten()
    {
        Mapping(x => x.LineOne, x => x.Length(100));

        Convention(x => x.Length(10));

        VerifyModel(x => x.Columns.First().Length.ShouldEqual(100));
    }

    [Test]
    public void LazyShouldntBeOverwritten()
    {
        Mapping(x => x.LineOne, x => x.LazyLoad());

        Convention(x => x.Not.LazyLoad());

        VerifyModel(x => x.Lazy.ShouldBeTrue());
    }

    [Test]
    public void IndexShouldntBeOverwritten()
    {
        Mapping(x => x.LineOne, x => x.Index("value"));

        Convention(x => x.Index("xxx"));

        VerifyModel(x => x.Columns.First().Index.ShouldEqual("value"));
    }

    [Test]
    public void PrecisionShouldntBeOverwritten()
    {
        Mapping(x => x.LineOne, x => x.Precision(100));

        Convention(x => x.Precision(200));

        VerifyModel(x => x.Columns.First().Precision.ShouldEqual(100));
    }

    [Test]
    public void ScaleShouldntBeOverwritten()
    {
        Mapping(x => x.LineOne, x => x.Scale(100));

        Convention(x => x.Scale(200));

        VerifyModel(x => x.Columns.First().Scale.ShouldEqual(100));
    }

    [Test]
    public void DefaultShouldntBeOverwritten()
    {
        Mapping(x => x.LineOne, x => x.Default("value"));

        Convention(x => x.Default("xxx"));

        VerifyModel(x => x.Columns.First().Default.ShouldEqual("value"));
    }

    [Test]
    public void CheckShouldntBeOverwritten()
    {
        Mapping(x => x.LineOne, x => x.Check("value"));

        Convention(x => x.Check("xxx"));

        VerifyModel(x => x.Columns.First().Check.ShouldEqual("value"));
    }

    #region Helpers

    void Convention(Action<IPropertyInstance> convention)
    {
        model.Conventions.Add(new PropertyConventionBuilder().Always(convention));
    }

    void Mapping(Expression<Func<ExampleClass, object>> property, Action<PropertyPart> mappingDefinition)
    {
        var classMap = new ClassMap<ExampleClass>();
        classMap.Id(x => x.Id);
        var map = classMap.Map(property);

        mappingDefinition(map);

        mapping = classMap;
        mappingType = typeof(ExampleClass);
    }

    void VerifyModel(Action<PropertyMapping> modelVerification)
    {
        model.Add(mapping);

        var generatedModels = model.BuildMappings();
        var modelInstance = generatedModels
            .First(x => x.Classes.FirstOrDefault(c => c.Type == mappingType) is not null)
            .Classes.First()
            .Properties.First();

        modelVerification(modelInstance);
    }

    #endregion
}
