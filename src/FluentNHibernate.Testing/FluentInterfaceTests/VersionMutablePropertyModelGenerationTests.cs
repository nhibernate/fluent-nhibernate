using System.Linq;
using FluentNHibernate.MappingModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class VersionMutablePropertyModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void AccessShouldSetModelAccessPropertyToValue()
        {
            Version()
                .Mapping(m => m.Access.Field())
                .ModelShouldMatch(x => x.Access.ShouldEqual("field"));
        }

        [Test]
        public void ColumnNameShouldSetModelColumnPropertyToValue()
        {
            Version()
                .Mapping(m => m.Column("col"))
                .ModelShouldMatch(x => x.Columns.First().Name.ShouldEqual("col"));
        }

        [Test]
        public void GeneratedShouldSetModelGeneratedPropertyToValue()
        {
            Version()
                .Mapping(m => m.Generated.Always())
                .ModelShouldMatch(x => x.Generated.ShouldEqual("always"));
        }

        [Test]
        public void ShouldSetModelNamePropertyToPropertyName()
        {
            Version()
                .Mapping(m => { })
                .ModelShouldMatch(x => x.Name.ShouldEqual("VersionNumber")); // set in Version(), bad form I know
        }

        [Test]
        public void ShouldSetModelTypePropertyToPropertyType()
        {
            Version()
                .Mapping(m => { })
                .ModelShouldMatch(x => x.Type.ShouldEqual(new TypeReference(typeof(int))));
        }

        [Test]
        public void UnsavedValueShouldSetModelTypeUnsavedValueToValue()
        {
            Version()
                .Mapping(m => m.UnsavedValue("any"))
                .ModelShouldMatch(x => x.UnsavedValue.ShouldEqual("any"));
        }

        [Test]
        public void LengthShouldSetColumnModelLengthPropertyToValue()
        {
            Version()
                .Mapping(m => m.Length(8))
                .ModelShouldMatch(x => x.Columns.First().Length.ShouldEqual(8));
        }

        [Test]
        public void PrecisionShouldSetColumnModelPrecisionPropertyToValue()
        {
            Version()
                .Mapping(m => m.Precision(10))
                .ModelShouldMatch(x => x.Columns.First().Precision.ShouldEqual(10));
        }

        [Test]
        public void ScaleShouldSetColumnModelScalePropertyToValue()
        {
            Version()
                .Mapping(m => m.Scale(10))
                .ModelShouldMatch(x => x.Columns.First().Scale.ShouldEqual(10));
        }

        [Test]
        public void NullableShouldSetColumnNotNullPropertyToFalse()
        {
            Version()
                .Mapping(m => m.Nullable())
                .ModelShouldMatch(x => x.Columns.First().NotNull.ShouldBeFalse());
        }

        [Test]
        public void NotNullableShouldSetColumnNotNullPropertyToTrue()
        {
            Version()
                .Mapping(m => m.Not.Nullable())
                .ModelShouldMatch(x => x.Columns.First().NotNull.ShouldBeTrue());
        }

        [Test]
        public void UniqueShouldSetColumnUniquePropertyToTrue()
        {
            Version()
                .Mapping(m => m.Unique())
                .ModelShouldMatch(x => x.Columns.First().Unique.ShouldBeTrue());
        }

        [Test]
        public void NotUniqueShouldSetColumnUniquePropertyToFalse()
        {
            Version()
                .Mapping(m => m.Not.Unique())
                .ModelShouldMatch(x => x.Columns.First().Unique.ShouldBeFalse());
        }

        [Test]
        public void UniqueKeyShouldSetColumnUniqueKeyPropertyToValue()
        {
            Version()
                .Mapping(m => m.UniqueKey("key"))
                .ModelShouldMatch(x => x.Columns.First().UniqueKey.ShouldEqual("key"));
        }

        [Test]
        public void IndexShouldSetModelIndexPropertyToValue()
        {
            Version()
                .Mapping(m => m.Index("index"))
                .ModelShouldMatch(x => x.Columns.First().Index.ShouldEqual("index"));
        }

        [Test]
        public void CheckShouldSetModelCheckPropertyToValue()
        {
            Version()
                .Mapping(m => m.Check("constraint"))
                .ModelShouldMatch(x => x.Columns.First().Check.ShouldEqual("constraint"));
        }

        [Test]
        public void DefaultShouldSetModelDefaultPropertyToValue()
        {
            Version()
                .Mapping(m => m.Default("value"))
                .ModelShouldMatch(x => x.Columns.First().Default.ShouldEqual("value"));
        }

        [Test]
        public void ShouldSetTypePropertyToSpecifiedType()
        {
            Version()
                .Mapping(m => m.CustomType<int>())
                .ModelShouldMatch(x => x.Type.ShouldEqual(new TypeReference(typeof(int))));
        }
    }
}