using System.Linq;
using System.Reflection;
using FluentNHibernate.Automapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Specs.Automapping.Fixtures;
using Machine.Specifications;
using FluentAssertions;

namespace FluentNHibernate.Specs.Automapping
{
    public class when_using_the_automapper_to_map_an_entity_that_uses_private_fields_for_storage
    {
        Establish context = () =>
            mapper = AutoMap.Source(new StubTypeSource(typeof(EntityUsingPrivateFields)))
                        .Setup(s => s.FindMembers = m => m.IsField && m.IsPrivate);

        Because of = () =>
            mapping = mapper.BuildMappingFor<EntityUsingPrivateFields>();

        It should_find_an_id = () =>
            mapping.Id.Should().NotBeNull();

        It should_use_the_correct_field_for_the_id = () =>
        {
            mapping.Id.As<IdMapping>().Name.Should().Be("id");
            mapping.Id.As<IdMapping>().Member.Should().Be(typeof(EntityUsingPrivateFields).GetField("id", BindingFlags.Instance | BindingFlags.NonPublic).ToMember());
        };

        It should_ignore_properties = () =>
            mapping.Properties.Where(x => x.Member.IsProperty).Should().BeEmpty();

        It should_map_fields = () =>
            mapping.Properties.Select(x => x.Name).Should().Contain(new string[] { "one", "two", "three" });

        static AutoPersistenceModel mapper;
        static ClassMapping mapping;
    }
}
