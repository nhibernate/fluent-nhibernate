using System.Linq;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.MappingModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class IdMutablePropertyModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void AccessShouldSetModelAccessPropertyToValue()
        {
            Id()
                .Mapping(m => m.Access.Field())
                .ModelShouldMatch(x => x.Access.ShouldEqual("field"));
        }

        [Test]
        public void ColumnNameShouldAddToModelColumnsCollection()
        {
            Id()
                .Mapping(m => m.Column("col"))
                .ModelShouldMatch(x => x.Columns.Count().ShouldEqual(1));
        }

        [Test]
        public void ColumnNameShouldSetModelColumnName()
        {
            Id()
                .Mapping(m => m.Column("col"))
                .ModelShouldMatch(x => x.Columns.First().Name.ShouldEqual("col"));
        }

        [Test]
        public void ShouldSetModelNamePropertyToPropertyName()
        {
            Id()
                .Mapping(m => {})
                .ModelShouldMatch(x => x.Name.ShouldEqual("IntId"));
        }

        [Test]
        public void ShouldSetModelTypePropertyToPropertyType()
        {
            Id()
                .Mapping(m => { })
                .ModelShouldMatch(x => x.Type.ShouldEqual(new TypeReference(typeof(int))));
        }

        [Test]
        public void UnsavedValueShouldSetModelTypePropertyToValue()
        {
            Id()
                .Mapping(m => m.UnsavedValue(10))
                .ModelShouldMatch(x => x.UnsavedValue.ShouldEqual("10"));
        }

        [Test]
        public void NullUnsavedValueShouldSetModelTypePropertyToNull()
        {
            Id()
                .Mapping(m => m.UnsavedValue(null))
                .ModelShouldMatch(x => x.UnsavedValue.ShouldEqual("null"));
        }

        [Test]
        public void GeneratedByShouldSetModelGeneratorProperty()
        {
            Id()
                .Mapping(m => m.GeneratedBy.Assigned())
                .ModelShouldMatch(x => x.Generator.ShouldNotBeNull());
        }

        [Test]
        public void GeneratedByShouldSetModelGeneratorPropertyToValue()
        {
            Id()
                .Mapping(m => m.GeneratedBy.Assigned())
                .ModelShouldMatch(x => x.Generator.Class.ShouldEqual("assigned"));
        }

        [Test]
        public void GeneratedByWithParamsShouldSetModelGeneratorParams()
        {
            Id()
                .Mapping(m => m.GeneratedBy.Assigned(p =>
                    p.AddParam("name", "value")
                     .AddParam("another", "another-value")))
                .ModelShouldMatch(x => x.Generator.Params.Count().ShouldEqual(2));
        }

        [Test]
        public void GeneratedByWithParamsShouldSetModelGeneratorParamsValues()
        {
            Id()
                .Mapping(m => m.GeneratedBy.Assigned(p =>
                    p.AddParam("name", "value")
                     .AddParam("another", "another-value")))
                .ModelShouldMatch(x =>
                {
                    var first = x.Generator.Params.First();

                    first.Key.ShouldEqual("name");
                    first.Value.ShouldEqual("value");

                    var second = x.Generator.Params.ElementAt(1);

                    second.Key.ShouldEqual("another");
                    second.Value.ShouldEqual("another-value");
                });
        }

        [Test]
        public void LengthShouldSetColumnModelLengthPropertyToValue()
        {
            Id()
                .Mapping(m => m.Length(8))
                .ModelShouldMatch(x => x.Columns.First().Length.ShouldEqual(8));
        }

        [Test]
        public void PrecisionShouldSetColumnModelPrecisionPropertyToValue()
        {
            Id()
                .Mapping(m => m.Precision(10))
                .ModelShouldMatch(x => x.Columns.First().Precision.ShouldEqual(10));
        }

        [Test]
        public void ScaleShouldSetColumnModelScalePropertyToValue()
        {
            Id()
                .Mapping(m => m.Scale(10))
                .ModelShouldMatch(x => x.Columns.First().Scale.ShouldEqual(10));
        }

        [Test]
        public void NullableShouldSetColumnNotNullPropertyToFalse()
        {
            Id()
                .Mapping(m => m.Nullable())
                .ModelShouldMatch(x => x.Columns.First().NotNull.ShouldBeFalse());
        }

        [Test]
        public void NotNullableShouldSetColumnNotNullPropertyToTrue()
        {
            Id()
                .Mapping(m => m.Not.Nullable())
                .ModelShouldMatch(x => x.Columns.First().NotNull.ShouldBeTrue());
        }

        [Test]
        public void UniqueShouldSetColumnUniquePropertyToTrue()
        {
            Id()
                .Mapping(m => m.Unique())
                .ModelShouldMatch(x => x.Columns.First().Unique.ShouldBeTrue());
        }

        [Test]
        public void NotUniqueShouldSetColumnUniquePropertyToFalse()
        {
            Id()
                .Mapping(m => m.Not.Unique())
                .ModelShouldMatch(x => x.Columns.First().Unique.ShouldBeFalse());
        }

        [Test]
        public void UniqueKeyShouldSetColumnUniqueKeyPropertyToValue()
        {
            Id()
                .Mapping(m => m.UniqueKey("key"))
                .ModelShouldMatch(x => x.Columns.First().UniqueKey.ShouldEqual("key"));
        }

        [Test]
        public void IndexShouldSetModelIndexPropertyToValue()
        {
            Id()
                .Mapping(m => m.Index("index"))
                .ModelShouldMatch(x => x.Columns.First().Index.ShouldEqual("index"));
        }

        [Test]
        public void CheckShouldSetModelCheckPropertyToValue()
        {
            Id()
                .Mapping(m => m.Check("constraint"))
                .ModelShouldMatch(x => x.Columns.First().Check.ShouldEqual("constraint"));
        }

        [Test]
        public void DefaultShouldSetModelDefaultPropertyToValue()
        {
            Id()
                .Mapping(m => m.Default("value"))
                .ModelShouldMatch(x => x.Columns.First().Default.ShouldEqual("value"));
        }

        [Test]
        public void ShouldSetTypePropertyToSpecifiedType()
        {
            Id()
                .Mapping(m => m.CustomType<int>())
                .ModelShouldMatch(x => x.Type.ShouldEqual(new TypeReference(typeof(int))));
        }
    }
}