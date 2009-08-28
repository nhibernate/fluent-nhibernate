using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Testing.DomainModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class DiscriminatorMutablePropertyModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void AlwaysSelectWithValueShouldSetModelForcePropertyToTrue()
        {
            DiscriminatorMap<SuperRecord>()
                .Mapping(m => m.AlwaysSelectWithValue())
                .ModelShouldMatch(x => x.Force.ShouldBeTrue());
        }

        [Test]
        public void NotAlwaysSelectWithValueShouldSetModelForcePropertyToFalse()
        {
            DiscriminatorMap<SuperRecord>()
                .Mapping(m => m.Not.AlwaysSelectWithValue())
                .ModelShouldMatch(x => x.Force.ShouldBeFalse());
        }

        [Test]
        public void FormulaShouldSetModelFormulaPropertyToValue()
        {
            DiscriminatorMap<SuperRecord>()
                .Mapping(m => m.Formula("sql"))
                .ModelShouldMatch(x => x.Formula.ShouldEqual("sql"));
        }

        [Test]
        public void NullableShouldSetModelNotNullPropertyToFalse()
        {
            DiscriminatorMap<SuperRecord>()
                .Mapping(m => m.Nullable())
                .ModelShouldMatch(x => x.Columns.First().NotNull.ShouldBeFalse());
        }

        [Test]
        public void NotNullableShouldSetModelNotNullPropertyToTrue()
        {
            DiscriminatorMap<SuperRecord>()
                .Mapping(m => m.Not.Nullable())
                .ModelShouldMatch(x => x.Columns.First().NotNull.ShouldBeTrue());
        }

        [Test]
        public void ReadOnlyShouldSetModelInsertPropertyToFalse()
        {
            DiscriminatorMap<SuperRecord>()
                .Mapping(m => m.ReadOnly())
                .ModelShouldMatch(x => x.Insert.ShouldBeFalse());
        }

        [Test]
        public void NotReadOnlyShouldSetModelInsertPropertyToTrue()
        {
            DiscriminatorMap<SuperRecord>()
                .Mapping(m => m.Not.ReadOnly())
                .ModelShouldMatch(x => x.Insert.ShouldBeTrue());
        }


        [Test]
        public void LengthShouldSetColumnModelLengthPropertyToValue()
        {
            DiscriminatorMap<SuperRecord>()
                .Mapping(m => m.Length(8))
                .ModelShouldMatch(x => x.Columns.First().Length.ShouldEqual(8));
        }

        [Test]
        public void PrecisionShouldSetColumnModelPrecisionPropertyToValue()
        {
            DiscriminatorMap<SuperRecord>()
                .Mapping(m => m.Precision(10))
                .ModelShouldMatch(x => x.Columns.First().Precision.ShouldEqual(10));
        }

        [Test]
        public void ScaleShouldSetColumnModelScalePropertyToValue()
        {
            DiscriminatorMap<SuperRecord>()
                .Mapping(m => m.Scale(10))
                .ModelShouldMatch(x => x.Columns.First().Scale.ShouldEqual(10));
        }

        [Test]
        public void NullableShouldSetColumnNotNullPropertyToFalse()
        {
            DiscriminatorMap<SuperRecord>()
                .Mapping(m => m.Nullable())
                .ModelShouldMatch(x => x.Columns.First().NotNull.ShouldBeFalse());
        }

        [Test]
        public void NotNullableShouldSetColumnNotNullPropertyToTrue()
        {
            DiscriminatorMap<SuperRecord>()
                .Mapping(m => m.Not.Nullable())
                .ModelShouldMatch(x => x.Columns.First().NotNull.ShouldBeTrue());
        }

        [Test]
        public void UniqueShouldSetColumnUniquePropertyToTrue()
        {
            DiscriminatorMap<SuperRecord>()
                .Mapping(m => m.Unique())
                .ModelShouldMatch(x => x.Columns.First().Unique.ShouldBeTrue());
        }

        [Test]
        public void NotUniqueShouldSetColumnUniquePropertyToFalse()
        {
            DiscriminatorMap<SuperRecord>()
                .Mapping(m => m.Not.Unique())
                .ModelShouldMatch(x => x.Columns.First().Unique.ShouldBeFalse());
        }

        [Test]
        public void UniqueKeyShouldSetColumnUniqueKeyPropertyToValue()
        {
            DiscriminatorMap<SuperRecord>()
                .Mapping(m => m.UniqueKey("key"))
                .ModelShouldMatch(x => x.Columns.First().UniqueKey.ShouldEqual("key"));
        }

        [Test]
        public void IndexShouldSetModelIndexPropertyToValue()
        {
            DiscriminatorMap<SuperRecord>()
                .Mapping(m => m.Index("index"))
                .ModelShouldMatch(x => x.Columns.First().Index.ShouldEqual("index"));
        }

        [Test]
        public void CheckShouldSetModelCheckPropertyToValue()
        {
            DiscriminatorMap<SuperRecord>()
                .Mapping(m => m.Check("constraint"))
                .ModelShouldMatch(x => x.Columns.First().Check.ShouldEqual("constraint"));
        }

        [Test]
        public void DefaultShouldSetModelDefaultPropertyToValue()
        {
            DiscriminatorMap<SuperRecord>()
                .Mapping(m => m.Default("value"))
                .ModelShouldMatch(x => x.Columns.First().Default.ShouldEqual("value"));
        }

        [Test]
        public void ShouldSetTypePropertyToSpecifiedType()
        {
            DiscriminatorMap<SuperRecord>()
                .Mapping(m => m.CustomType<int>())
                .ModelShouldMatch(x => x.Type.ShouldEqual(new TypeReference(typeof(int))));
        }
    }
}