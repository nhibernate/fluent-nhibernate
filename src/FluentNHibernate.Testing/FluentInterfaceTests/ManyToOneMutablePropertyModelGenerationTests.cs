using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class ManyToOneMutablePropertyModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void AccessShouldSetModelAccessPropertyToValue()
        {
            ManyToOne()
                .Mapping(m => m.Access.Field())
                .ModelShouldMatch(x => x.Access.ShouldEqual("field"));
        }

        [Test]
        public void CascadeShouldSetModelCascadePropertyToValue()
        {
            ManyToOne()
                .Mapping(m => m.Cascade.All())
                .ModelShouldMatch(x => x.Cascade.ShouldEqual("all"));
        }

        [Test]
        public void ShouldSetModelClassPropertyToPropertyType()
        {
            ManyToOne()
                .Mapping(m => {})
                .ModelShouldMatch(x => x.Class.ShouldEqual(new TypeReference(typeof(PropertyReferenceTarget))));
        }

        [Test]
        public void ClassShouldSetModelClassPropertyToValue()
        {
            ManyToOne()
                .Mapping(m => m.Class(typeof(int)))
                .ModelShouldMatch(x => x.Class.ShouldEqual(new TypeReference(typeof(int))));
        }

        [Test]
        public void ColumnNameShouldAddModelColumnsCollection()
        {
            ManyToOne()
                .Mapping(m => m.Column("col"))
                .ModelShouldMatch(x => x.Columns.Count().ShouldEqual(1));
        }

        [Test]
        public void FetchShouldSetFetchModelProperty()
        {
            ManyToOne()
                .Mapping(m => m.Fetch.Select())
                .ModelShouldMatch(x => x.Fetch.ShouldEqual("select"));
        }

        [Test]
        public void WithForeignKeyShouldSetForeignKeyModelProperty()
        {
            ManyToOne()
                .Mapping(m => m.ForeignKey("fk"))
                .ModelShouldMatch(x => x.ForeignKey.ShouldEqual("fk"));
        }

        [Test]
        public void InsertShouldSetInsertModelPropertyToTrue()
        {
            ManyToOne()
                .Mapping(m => m.Insert())
                .ModelShouldMatch(x => x.Insert.ShouldBeTrue());
        }

        [Test]
        public void NotInsertShouldSetInsertModelPropertyToFalse()
        {
            ManyToOne()
                .Mapping(m => m.Not.Insert())
                .ModelShouldMatch(x => x.Insert.ShouldBeFalse());
        }

        [Test]
        public void UpdateShouldSetUpdateModelPropertyToTrue()
        {
            ManyToOne()
                .Mapping(m => m.Update())
                .ModelShouldMatch(x => x.Update.ShouldBeTrue());
        }

        [Test]
        public void NotUpdateShouldSetUpdateModelPropertyToFalse()
        {
            ManyToOne()
                .Mapping(m => m.Not.Update())
                .ModelShouldMatch(x => x.Update.ShouldBeFalse());
        }

        [Test]
        public void ReadOnlyShouldSetInsertModelPropertyToFalse()
        {
            ManyToOne()
                .Mapping(m => m.ReadOnly())
                .ModelShouldMatch(x => x.Insert.ShouldBeFalse());
        }

        [Test]
        public void NotReadOnlyShouldSetInsertModelPropertyToTrue()
        {
            ManyToOne()
                .Mapping(m => m.Not.ReadOnly())
                .ModelShouldMatch(x => x.Insert.ShouldBeTrue());
        }

        [Test]
        public void ReadOnlyShouldSetUpdateModelPropertyToFalse()
        {
            ManyToOne()
                .Mapping(m => m.ReadOnly())
                .ModelShouldMatch(x => x.Update.ShouldBeFalse());
        }

        [Test]
        public void NotReadOnlyShouldSetUpdateModelPropertyToTrue()
        {
            ManyToOne()
                .Mapping(m => m.Not.ReadOnly())
                .ModelShouldMatch(x => x.Update.ShouldBeTrue());
        }

        [Test]
        public void LazyShouldSetLazyModelPropertyToTrue()
        {
            ManyToOne()
                .Mapping(m => m.LazyLoad())
                .ModelShouldMatch(x => x.Lazy.ShouldEqual(true));
        }

        [Test]
        public void NotLazyShouldSetLazyModelPropertyToFalse()
        {
            ManyToOne()
                .Mapping(m => m.Not.LazyLoad())
                .ModelShouldMatch(x => x.Lazy.ShouldEqual(false));
        }

        [Test]
        public void ShouldSetNameModelPropertyToPropertyName()
        {
            ManyToOne()
                .Mapping(m => {})
                .ModelShouldMatch(x => x.Name.ShouldEqual("Reference"));
        }

        [Test]
        public void NotFoundShouldSetNotFoundModelProperty()
        {
            ManyToOne()
                .Mapping(m => m.NotFound.Ignore())
                .ModelShouldMatch(x => x.NotFound.ShouldEqual("ignore"));
        }

        [Test]
        public void PropertyShouldSetPropertyRefModelProperty()
        {
            ManyToOne()
                .Mapping(m => m.PropertyRef("Name"))
                .ModelShouldMatch(x => x.PropertyRef.ShouldEqual("Name"));
        }

        [Test]
        public void NullableShouldSetColumnNotNullPropertyToFalse()
        {
            ManyToOne()
                .Mapping(m => m.Nullable())
                .ModelShouldMatch(x => x.Columns.First().NotNull.ShouldBeFalse());
        }

        [Test]
        public void NotNullableShouldSetColumnNotNullPropertyToTrue()
        {
            ManyToOne()
                .Mapping(m => m.Not.Nullable())
                .ModelShouldMatch(x => x.Columns.First().NotNull.ShouldBeTrue());
        }

        [Test]
        public void UniqueShouldSetColumnUniquePropertyToTrue()
        {
            ManyToOne()
                .Mapping(m => m.Unique())
                .ModelShouldMatch(x => x.Columns.First().Unique.ShouldBeTrue());
        }

        [Test]
        public void NotUniqueShouldSetColumnUniquePropertyToFalse()
        {
            ManyToOne()
                .Mapping(m => m.Not.Unique())
                .ModelShouldMatch(x => x.Columns.First().Unique.ShouldBeFalse());
        }

        [Test]
        public void UniqueKeyShouldSetColumnUniqueKeyPropertyToValue()
        {
            ManyToOne()
                .Mapping(m => m.UniqueKey("uk"))
                .ModelShouldMatch(x => x.Columns.First().UniqueKey.ShouldEqual("uk"));
        }

        [Test]
        public void IndexShouldSetColumnIndexPropertyToValue()
        {
            ManyToOne()
                .Mapping(m => m.Index("ix"))
                .ModelShouldMatch(x => x.Columns.First().Index.ShouldEqual("ix"));
        }
    }
}