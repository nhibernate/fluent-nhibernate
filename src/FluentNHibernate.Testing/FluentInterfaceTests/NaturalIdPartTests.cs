using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using NUnit.Framework;
using FluentNHibernate.Testing.DomainModel.Mapping;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class NaturalIdPartTests : BaseModelFixture
    {
        [Test]
        public void ShouldMapProperty()
        {
            NaturalId<PropertyTarget>()
                .Mapping(m => m.Property(x => x.DecimalProperty))
                .ModelShouldMatch(x => x.Properties.Single().Name.ShouldEqual("DecimalProperty"));
        }

        [Test]
        public void ShouldMapPropertyWithColumnName()
        {
            NaturalId<PropertyTarget>()
                .Mapping(m => m.Property(x => x.DecimalProperty, "CustomCol"))
                .ModelShouldMatch(x => x.Properties.Single().Columns.Single().Name.ShouldEqual("CustomCol"));
        }

        [Test]
        public void ShouldMapReference()
        {
            NaturalId<PropertyTarget>()
                .Mapping(m => m.Reference(x => x.Reference))
                .ModelShouldMatch(x => x.ManyToOnes.Single().Name.ShouldEqual("Reference"));
        }

        [Test]
        public void ShouldMapReferenceWithColumnName()
        {
            NaturalId<PropertyTarget>()
                .Mapping(m => m.Reference(x => x.Reference, "CustomCol"))
                .ModelShouldMatch(x => x.ManyToOnes.Single().Columns.Single().Name.ShouldEqual("CustomCol"));
        }

        [Test]
        public void ShouldSetReadOnly()
        {
            NaturalId<PropertyTarget>()
                .Mapping(m => m.ReadOnly())
                .ModelShouldMatch(x => x.Mutable.ShouldBeFalse());
        }

        [Test]
        public void ShouldSetNotReadOnly()
        {
            NaturalId<PropertyTarget>()
                .Mapping(m => m.Not.ReadOnly())
                .ModelShouldMatch(x => x.Mutable.ShouldBeTrue());
        }

    }
}