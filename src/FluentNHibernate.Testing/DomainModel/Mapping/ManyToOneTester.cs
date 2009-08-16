using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class ManyToOneTester
    {
        [Test]
        public void CreatingAManyToOneReference()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.References(x => x.Parent))
                .Element("class/many-to-one").HasAttribute("name", "Parent")
                .Element("class/many-to-one/column").HasAttribute("name", "Parent_id");
        }

        [Test]
        public void ManyToOneReferenceShouldDefaultToEmptyCascade()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.References(x => x.Parent))
                .Element("class/many-to-one")
                    .DoesntHaveAttribute("cascade");
        }

        [Test]
        public void ManyToOneReferenceWithPropertyReferenceShouldSetThePropertyRefAttribute()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.References(x => x.Parent).PropertyRef(p => p.Name))
                .Element("class/many-to-one")
                    .HasAttribute("property-ref", "Name");
        }

        [Test]
        public void CreatingAManyToOneReferenceSetsTheColumnOverrides()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.References(x => x.Parent).ForeignKey())
                .Element("class/many-to-one")
                    .HasAttribute("foreign-key", "FK_MappedObjectToParent");
        }

        [Test]
        public void ManyToOneUniqueKeyShouldRenderUniquekeyAttribute()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.References(x => x.Parent).UniqueKey("uniqueKey"))
                .Element("class/many-to-one/column")
                    .HasAttribute("unique-key", "uniqueKey");
        }

        [Test]
        public void ManyToOneUniqueShouldRenderUniqueAttribute()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.References(x => x.Parent).Unique())
                .Element("class/many-to-one/column")
                    .HasAttribute("unique", "true");
        }

        [Test]
        public void ManyToOneUniqueShouldRenderUniqueAttributeFalse()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.References(x => x.Parent).Not.Unique())
                .Element("class/many-to-one/column")
                    .HasAttribute("unique", "false");
        }

        [Test]
        public void ManyToOneLazyLoadShouldSetTheProxyValueOnTheLazyAttribute()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.References(x => x.Parent).LazyLoad())
                .Element("class/many-to-one")
                    .HasAttribute("lazy", "proxy");
        }

        [Test]
        public void ManyToOneLazyLoadShouldSetTheProxyValueOnTheLazyAttributeFalse()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.References(x => x.Parent).Not.LazyLoad())
                .Element("class/many-to-one")
                    .HasAttribute("lazy", "false");
        }

        [Test]
        public void ManyToOneReferenceCanBeSetAsNotNullable()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.References(x => x.Parent)
                      .Not.Nullable())
                .Element("class/many-to-one/column").HasAttribute("not-null", "true");
        }

        [Test]
        public void ManyToOneReferenceCanBeSetAsNullable()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.References(x => x.Parent)
                      .Nullable()
                )
                .Element("class/many-to-one/column").HasAttribute("not-null", "false");
        }

        [Test]
        public void ManyToOneCanHaveMultipleColumnsFluently()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.References(x => x.Parent)
                      .Columns(x => x.IdPart1, x => x.IdPart2, x => x.IdPart3))
                .Element("class/many-to-one/column[@name='IdPart1']").Exists()
                .Element("class/many-to-one/column[@name='IdPart2']").Exists()
                .Element("class/many-to-one/column[@name='IdPart3']").Exists();
        }

        [Test]
        public void ManyToOneCanHaveMultipleColumnsWithStrings()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.References(x => x.Parent)
                      .Columns("IdPart1", "IdPart2", "IdPart3"))
                .Element("class/many-to-one/column[@name='IdPart1']").Exists()
                .Element("class/many-to-one/column[@name='IdPart2']").Exists()
                .Element("class/many-to-one/column[@name='IdPart3']").Exists();
        }

        [Test]
        public void NotFoundSetsAttribute()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.References(x => x.Parent)
                        .NotFound.Ignore())
                .Element("class/many-to-one").HasAttribute("not-found", "ignore");
        }

        [Test]
        public void NullableResetsNotValue()
        {
            //Regression test for issue 189
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.References(x => x.Parent)
                        .Not.Nullable().Not.LazyLoad())
                .Element("class/many-to-one").HasAttribute("lazy", "false");
        }

        [Test]
        public void LazyLoadResetsNotValue()
        {
            //Regression test for issue 189
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.References(x => x.Parent)
                        .Not.LazyLoad().Not.Nullable())
                .Element("class/many-to-one/column").HasAttribute("not-null", "true");
        }
    }
}