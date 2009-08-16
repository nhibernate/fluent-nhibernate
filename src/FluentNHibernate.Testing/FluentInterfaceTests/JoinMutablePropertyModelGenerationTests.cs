using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class JoinMutablePropertyModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void WithTableNameShouldSetModelTableNamePropertyToValue()
        {
            Join<PropertyTarget>("table")
                .Mapping(m => { })
                .ModelShouldMatch(x => x.TableName.ShouldEqual("table"));
        }

        [Test]
        public void SchemaIsShouldSetModelSchemaPropertyToValue()
        {
            Join<PropertyTarget>("table")
                .Mapping(m => m.Schema("schema"))
                .ModelShouldMatch(x => x.Schema.ShouldEqual("schema"));
        }

        [Test]
        public void FetchShouldSetModelFetchPropertyToValue()
        {
            Join<PropertyTarget>("table")
                .Mapping(m => m.Fetch.Select())
                .ModelShouldMatch(x => x.Fetch.ShouldEqual("select"));
        }

        [Test]
        public void InverseShouldSetModelInversePropertyToTrue()
        {
            Join<PropertyTarget>("table")
                .Mapping(m => m.Inverse())
                .ModelShouldMatch(x => x.Inverse.ShouldBeTrue());
        }

        [Test]
        public void NotInverseShouldSetModelInversePropertyToFalse()
        {
            Join<PropertyTarget>("table")
                .Mapping(m => m.Not.Inverse())
                .ModelShouldMatch(x => x.Inverse.ShouldBeFalse());
        }

        [Test]
        public void OptionalShouldSetModelOptionalPropertyToTrue()
        {
            Join<PropertyTarget>("table")
                .Mapping(m => m.Optional())
                .ModelShouldMatch(x => x.Optional.ShouldBeTrue());
        }

        [Test]
        public void NotOptionalShouldSetModelOptionalPropertyToFalse()
        {
            Join<PropertyTarget>("table")
                .Mapping(m => m.Not.Optional())
                .ModelShouldMatch(x => x.Optional.ShouldBeFalse());
        }

        [Test]
        public void CatalogShouldSetModelCatalogPropertyToValue()
        {
            Join<PropertyTarget>("table")
                .Mapping(m => m.Catalog("catalog"))
                .ModelShouldMatch(x => x.Catalog.ShouldEqual("catalog"));
        }

        [Test]
        public void SubselectShouldSetModelSubselectPropertyToValue()
        {
            Join<PropertyTarget>("table")
                .Mapping(m => m.Subselect("subselect"))
                .ModelShouldMatch(x => x.Subselect.ShouldEqual("subselect"));
        }
    }
}