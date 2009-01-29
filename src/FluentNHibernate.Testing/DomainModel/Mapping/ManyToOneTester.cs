using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class ManyToOneTester
    {
        [Test]
        public void Creating_a_many_to_one_reference()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.References(x => x.Parent))
                .Element("class/many-to-one")
                    .HasAttribute("name", "Parent")
                    .HasAttribute("column", "Parent_id");
        }

        [Test]
        public void Many_to_one_reference_should_default_to_empty_cascade()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.References(x => x.Parent))
                .Element("class/many-to-one")
                    .DoesntHaveAttribute("cascade");
        }

        [Test]
        public void Many_to_one_reference_with_property_reference_should_set_the_property_ref_attribute()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.References(x => x.Parent).PropertyRef(p => p.Name))
                .Element("class/many-to-one")
                    .HasAttribute("property-ref", "Name");
        }

        [Test]
        public void Creating_a_many_to_one_reference_sets_the_column_overrides()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.References(x => x.Parent).WithForeignKey())
                .Element("class/many-to-one")
                    .HasAttribute("foreign-key", "FK_MappedObjectToParent");
        }

        [Test]
        public void Many_to_one_unique_should_render_unique_attribute()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.References(x => x.Parent).Unique())
                .Element("class/many-to-one")
                    .HasAttribute("unique", "true");
        }

        [Test]
        public void Many_to_one_unique_should_render_unique_attribute_false()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.References(x => x.Parent).Not.Unique())
                .Element("class/many-to-one")
                    .HasAttribute("unique", "false");
        }

        [Test]
        public void Many_to_one_lazy_load_should_set_the_proxy_value_on_the_lazy_attribute()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.References(x => x.Parent).LazyLoad())
                .Element("class/many-to-one")
                    .HasAttribute("lazy", "proxy");
        }

        [Test]
        public void Many_to_one_lazy_load_should_set_the_proxy_value_on_the_lazy_attribute_false()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.References(x => x.Parent).Not.LazyLoad())
                .Element("class/many-to-one")
                    .HasAttribute("lazy", "false");
        }

        [Test]
        public void Many_to_one_reference_can_be_set_as_not_nullable()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => 
                    map.References(x => x.Parent)
                      .Not.Nullable()
                )
                .Element("class/many-to-one")
                    .HasAttribute("not-null", "true");                    
        }

        [Test]
        public void Many_to_one_reference_can_be_set_as_nullable()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.References(x => x.Parent)
                      .Nullable()
                )
                .Element("class/many-to-one")
                    .HasAttribute("not-null", "false");
        }

        [Test]
        public void Many_to_one_can_have_multiple_columns_fluently()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.References(x => x.Parent)
                      .WithColumns(x => x.IdPart1, x => x.IdPart2, x => x.IdPart3))
                .Element("class/many-to-one/column[@name='IdPart1']").Exists()
                .Element("class/many-to-one/column[@name='IdPart2']").Exists()
                .Element("class/many-to-one/column[@name='IdPart3']").Exists();
        }

        [Test]
        public void Many_to_one_can_have_multiple_columns_with_strings()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map =>
                    map.References(x => x.Parent)
                      .WithColumns("IdPart1", "IdPart2", "IdPart3"))
                .Element("class/many-to-one/column[@name='IdPart1']").Exists()
                .Element("class/many-to-one/column[@name='IdPart2']").Exists()
                .Element("class/many-to-one/column[@name='IdPart3']").Exists();
        }
    }
}