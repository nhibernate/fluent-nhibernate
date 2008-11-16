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
                .ForMapping(map => map.References(x => x.Parent).WithUniqueConstraint())
                .Element("class/many-to-one")
                    .HasAttribute("unique", "true");
        }
    }
}