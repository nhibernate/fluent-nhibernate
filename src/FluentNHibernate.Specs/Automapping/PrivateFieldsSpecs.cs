using System.Linq;
using System.Reflection;
using FluentNHibernate.Automapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Specs.Automapping.Fixtures;
using Machine.Specifications;

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
            mapping.Id.ShouldNotBeNull();

        It should_use_the_correct_field_for_the_id = () =>
        {
            mapping.Id.As<IdMapping>().Name.ShouldEqual("id");
            mapping.Id.As<IdMapping>().Member.ShouldEqual(typeof(EntityUsingPrivateFields).GetField("id", BindingFlags.Instance | BindingFlags.NonPublic).ToMember());
        };

        It should_ignore_properties = () =>
            mapping.Properties.Where(x => x.Member.IsProperty).ShouldBeEmpty();

        It should_map_fields = () =>
            mapping.Properties.Select(x => x.Name).ShouldContain("one", "two", "three");
        
        static AutoPersistenceModel mapper;
        static ClassMapping mapping;
    }
}
