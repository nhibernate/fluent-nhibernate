using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests;

[TestFixture]
public class PropertyMutablePropertyModelGenerationTests : BaseModelFixture
{
    [Test]
    public void AccessShouldSetModelAccessPropertyToValue()
    {
        Property()
            .Mapping(m => m.Access.Field())
            .ModelShouldMatch(x => x.Access.ShouldEqual("field"));
    }

    [Test]
    public void ShouldSetModelNamePropertyToPropertyName()
    {
        Property()
            .Mapping(m => {})
            .ModelShouldMatch(x => x.Name.ShouldEqual("Name"));
    }

    [Test]
    public void InsertShouldSetModelInsertPropertyToTrue()
    {
        Property()
            .Mapping(m => m.Insert())
            .ModelShouldMatch(x => x.Insert.ShouldBeTrue());
    }

    [Test]
    public void NotInsertShouldSetModelInsertPropertyToFalse()
    {
        Property()
            .Mapping(m => m.Not.Insert())
            .ModelShouldMatch(x => x.Insert.ShouldBeFalse());
    }

    [Test]
    public void UpdateShouldSetModelUpdatePropertyToTrue()
    {
        Property()
            .Mapping(m => m.Update())
            .ModelShouldMatch(x => x.Update.ShouldBeTrue());
    }

    [Test]
    public void NotUpdateShouldSetModelUpdatePropertyToFalse()
    {
        Property()
            .Mapping(m => m.Not.Update())
            .ModelShouldMatch(x => x.Update.ShouldBeFalse());
    }

    [Test]
    public void ReadOnlyShouldSetModelInsertPropertyToFalse()
    {
        Property()
            .Mapping(m => m.ReadOnly())
            .ModelShouldMatch(x => x.Insert.ShouldBeFalse());
    }

    [Test]
    public void NotReadOnlyShouldSetModelInsertPropertyToTrue()
    {
        Property()
            .Mapping(m => m.Not.ReadOnly())
            .ModelShouldMatch(x => x.Insert.ShouldBeTrue());
    }

    [Test]
    public void ReadOnlyShouldSetModelUpdatePropertyToFalse()
    {
        Property()
            .Mapping(m => m.ReadOnly())
            .ModelShouldMatch(x => x.Update.ShouldBeFalse());
    }

    [Test]
    public void NotReadOnlyShouldSetModelUpdatePropertyToTrue()
    {
        Property()
            .Mapping(m => m.Not.ReadOnly())
            .ModelShouldMatch(x => x.Update.ShouldBeTrue());
    }

    [Test]
    public void FormulaIsShouldSetModelFormulaPropertyToValue()
    {
        Property()
            .Mapping(m => m.Formula("form"))
            .ModelShouldMatch(x => x.Formula.ShouldEqual("form"));
    }

    [Test]
    public void OptimisticLockShouldSetModelOptimisticLockPropertyToTrue()
    {
        Property()
            .Mapping(m => m.OptimisticLock())
            .ModelShouldMatch(x => x.OptimisticLock.ShouldBeTrue());
    }

    [Test]
    public void NotOptimisticLockShouldSetModelOptimisticLockPropertyToFalse()
    {
        Property()
            .Mapping(m => m.Not.OptimisticLock())
            .ModelShouldMatch(x => x.OptimisticLock.ShouldBeFalse());
    }

    [Test]
    public void GeneratedShouldSetModelGeneratedPropertyToValue()
    {
        Property()
            .Mapping(m => m.Generated.Always())
            .ModelShouldMatch(x => x.Generated.ShouldEqual("always"));
    }

    [Test]
    public void ShouldSetModelTypePropertyToPropertyType()
    {
        Property()
            .Mapping(m => {})
            .ModelShouldMatch(x => x.Type.ShouldEqual(new TypeReference(typeof(string))));
    }

    [Test]
    public void CustomTypeIsShouldSetModelTypePropertyToType()
    {
        Property()
            .Mapping(m => m.CustomType<PropertyTarget>())
            .ModelShouldMatch(x => x.Type.ShouldEqual(new TypeReference(typeof(PropertyTarget))));
    }

    [Test]
    public void ShouldSetModelDefaultColumn()
    {
        Property()
            .Mapping(m => { })
            .ModelShouldMatch(x => x.Columns.Count().ShouldEqual(1));
    }

    [Test]
    public void ShouldSetModelDefaultColumnBasedOnPropertyName()
    {
        Property()
            .Mapping(m => { })
            .ModelShouldMatch(x => x.Columns.First().Name.ShouldEqual("Name"));
    }

    [Test]
    public void ColumnNameShouldOverrideModelDefaultColumn()
    {
        Property()
            .Mapping(m => m.Column("col"))
            .ModelShouldMatch(x => x.Columns.First().Name.ShouldEqual("col"));
    }

    [Test]
    public void ColumnNamesShouldAddModelColumnsCollection()
    {
        Property()
            .Mapping(m => m.Columns.Add("one", "two"))
            .ModelShouldMatch(x => x.Columns.Count().ShouldEqual(2));
    }

    [Test]
    public void ColumnNamesShouldAddModelColumnsCollectionWithCorrectName()
    {
        Property()
            .Mapping(m => m.Columns.Add("one", "two"))
            .ModelShouldMatch(x =>
            {
                x.Columns.First().Name.ShouldEqual("one");
                x.Columns.ElementAt(1).Name.ShouldEqual("two");
            });
    }

    [Test]
    public void CustomSqlTypeShouldSetColumn()
    {
        Property()
            .Mapping(m => m.CustomSqlType("sql"))
            .ModelShouldMatch(x => x.Columns.First().SqlType.ShouldEqual("sql"));
    }

    [Test]
    public void NullableShouldSetColumnNotNullPropertyToFalse()
    {
        Property()
            .Mapping(m => m.Nullable())
            .ModelShouldMatch(x => x.Columns.First().NotNull.ShouldBeFalse());
    }

    [Test]
    public void NotNullableShouldSetColumnNotNullPropertyToTrue()
    {
        Property()
            .Mapping(m => m.Not.Nullable())
            .ModelShouldMatch(x => x.Columns.First().NotNull.ShouldBeTrue());
    }

    [Test]
    public void UniqueShouldSetColumnUniquePropertyToTrue()
    {
        Property()
            .Mapping(m => m.Unique())
            .ModelShouldMatch(x => x.Columns.First().Unique.ShouldBeTrue());
    }

    [Test]
    public void NotUniqueShouldSetColumnUniquePropertyToFalse()
    {
        Property()
            .Mapping(m => m.Not.Unique())
            .ModelShouldMatch(x => x.Columns.First().Unique.ShouldBeFalse());
    }

    [Test]
    public void UniqueKeyShouldSetColumnUniqueKeyPropertyToValue()
    {
        Property()
            .Mapping(m => m.UniqueKey("key"))
            .ModelShouldMatch(x => x.Columns.First().UniqueKey.ShouldEqual("key"));
    }

    [Test]
    public void LengthOfShouldSetColumnLengthPropertyToValue()
    {
        Property()
            .Mapping(m => m.Length(100))
            .ModelShouldMatch(x => x.Columns.First().Length.ShouldEqual(100));
    }

    [Test]
    public void LazyLoadShouldSetModelLazyPropertyToTrue()
    {
        Property()
            .Mapping(m => m.LazyLoad())
            .ModelShouldMatch(x => x.Lazy.ShouldBeTrue());
    }

    [Test]
    public void NotLazyLoadShouldSetModelLazyPropertyToFalse()
    {
        Property()
            .Mapping(m => m.Not.LazyLoad())
            .ModelShouldMatch(x => x.Lazy.ShouldBeFalse());
    }

    [Test]
    public void IndexShouldSetModelIndexPropertyToValue()
    {
        Property()
            .Mapping(m => m.Index("index"))
            .ModelShouldMatch(x => x.Columns.First().Index.ShouldEqual("index"));
    }

    [Test]
    public void PrecisionShouldSetColumnModelPrecisionPropertyToValue()
    {
        Property()
            .Mapping(m => m.Precision(10))
            .ModelShouldMatch(x => x.Columns.First().Precision.ShouldEqual(10));
    }

    [Test]
    public void ScaleShouldSetColumnModelScalePropertyToValue()
    {
        Property()
            .Mapping(m => m.Scale(10))
            .ModelShouldMatch(x => x.Columns.First().Scale.ShouldEqual(10));
    }

    [Test]
    public void CheckShouldSetModelCheckPropertyToValue()
    {
        Property()
            .Mapping(m => m.Check("constraint"))
            .ModelShouldMatch(x => x.Columns.First().Check.ShouldEqual("constraint"));
    }

    [Test]
    public void DefaultShouldSetModelDefaultPropertyToValue()
    {
        Property()
            .Mapping(m => m.Default("value"))
            .ModelShouldMatch(x => x.Columns.First().Default.ShouldEqual("value"));
    }
 
    [Test]
    public void CanSetAttributesForNonDefaultColumn()
    {
        //For issue #354 - Can't seem to combine Column and Length
        Property()
            .Mapping(x => x.Column("foo").Length(42).Not.Nullable())
            .ModelShouldMatch(x =>
            {
                x.Columns.Count().ShouldEqual(1);
                x.Columns.First().Name.ShouldEqual("foo");
                x.Columns.First().Length.ShouldEqual(42);
                x.Columns.First().NotNull.ShouldBeTrue();
            });
    }

    [Test]
    public void ShouldBeAbleToOverrideColumnNameAndSpecifyNullabilityTogether()
    {
        Property()
            .Mapping(m => m.Column("custom-column").Not.Nullable())
            .ModelShouldMatch(x =>
            {
                x.Columns.First().Name.ShouldEqual("custom-column");
                x.Columns.First().NotNull.ShouldBeTrue();
            });
    }
}
