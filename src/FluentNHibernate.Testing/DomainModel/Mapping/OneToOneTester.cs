using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class OneToOneTester
    {
        [Test]
        public void Creating_a_one_to_one_reference()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.HasOne(x => x.Parent))
                .Element("class/one-to-one")
                    .HasAttribute("name", "Parent")
                    .HasAttribute("class", typeof(SecondMappedObject).AssemblyQualifiedName);
        }

        [Test]
        public void One_to_one_reference_should_default_to_empty_cascade()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.HasOne(x => x.Parent))
                .Element("class/one-to-one")
                    .DoesntHaveAttribute("cascade");
        }

        [Test]
        public void Creating_a_one_to_one_reference_sets_the_column_overrides()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.HasOne(x => x.Parent).WithForeignKey())
                .Element("class/one-to-one")
                    .HasAttribute("foreign-key", "FK_MappedObjectToParent");
        }

        [Test]
        public void One_to_one_with_property_reference_should_set_the_property_ref_attribute()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.HasOne(x => x.Parent).PropertyRef(p => p.Name))
                .Element("class/one-to-one")
                    .HasAttribute("property-ref", "Name");
        }

        [Test]
        public void One_to_one_constrained_render_the_constrained_attribute()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.HasOne(x => x.Parent).Constrained())
                .Element("class/one-to-one")
                    .HasAttribute("constrained", "true");
        }
    }
}