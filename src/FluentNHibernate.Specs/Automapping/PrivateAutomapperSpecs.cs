using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.Automapping.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Automapping
{
    public class when_using_the_private_automapper_to_map_a_entity_with_private_fields_starting_with_an_underscore
    {
        Establish context = () =>
        {
            model = new PrivateAutoPersistenceModel()
                .Setup(conventions => conventions.FindMembers = m => m.IsField && m.Name.StartsWith("_"));

            model.ValidationEnabled = false;
            model.AddTypeSource(new StubTypeSource(typeof(EntityUsingPrivateFields)));
        };

        Because of = () =>
            mapping = model.BuildMappingFor<EntityUsingPrivateFields>();

        It should_map_fields_matching_the_convention = () =>
            mapping.Properties.Select(x => x.Name).ShouldContain("_one");

        It should_map_private_collections = () =>
            mapping.Collections.Select(x => x.Name).ShouldContain("_children");

        It should_not_map_fields_that_dont_match_the_convention = () =>
            mapping.Properties.Select(x => x.Name).ShouldNotContain("one");

        static ClassMapping mapping;
        static AutoPersistenceModel model;
    }
}