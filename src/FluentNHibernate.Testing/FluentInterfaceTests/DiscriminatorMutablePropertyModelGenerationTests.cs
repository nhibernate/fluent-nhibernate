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
                .ModelShouldMatch(x => x.NotNull.ShouldBeFalse());
        }

        [Test]
        public void NotNullableShouldSetModelNotNullPropertyToTrue()
        {
            DiscriminatorMap<SuperRecord>()
                .Mapping(m => m.Not.Nullable())
                .ModelShouldMatch(x => x.NotNull.ShouldBeTrue());
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
        public void WithLengthOfShouldSetModelLengthPropertyToValue()
        {
            DiscriminatorMap<SuperRecord>()
                .Mapping(m => m.Length(10))
                .ModelShouldMatch(x => x.Length.ShouldEqual(10));
        }
    }
}