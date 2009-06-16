using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class PropertyMutablePropertyModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void AccessShouldSetModelAccessPropertyToValue()
        {
            Property()
                .Mapping(m => m.Access.AsField())
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
                .Mapping(m => m.FormulaIs("form"))
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
                .Mapping(m => m.CustomTypeIs<PropertyTarget>())
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
                .Mapping(m => m.ColumnName("col"))
                .ModelShouldMatch(x => x.Columns.First().Name.ShouldEqual("col"));
        }

        [Test]
        public void ColumnNamesShouldAddModelColumnsCollection()
        {
            Property()
                .Mapping(m => m.ColumnNames.Add("one", "two"))
                .ModelShouldMatch(x => x.Columns.Count().ShouldEqual(2));
        }

        [Test]
        public void ColumnNamesShouldAddModelColumnsCollectionWithCorrectName()
        {
            Property()
                .Mapping(m => m.ColumnNames.Add("one", "two"))
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
                .Mapping(m => m.CustomSqlTypeIs("sql"))
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
        public void WithLengthOfShouldSetColumnLengthPropertyToValue()
        {
            Property()
                .Mapping(m => m.WithLengthOf(100))
                .ModelShouldMatch(x => x.Columns.First().Length.ShouldEqual(100));
        }
    }
}