using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.Automapping.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Automapping
{
    public class when_the_automapper_is_told_to_map_an_entity_and_a_component
    {
        Establish context = () =>
            mapper = AutoMap.Source(
                new StubTypeSource(typeof(Entity), typeof(Component)),
                new TestConfiguration());

        Because of = () =>
            mappings = mapper.BuildMappings()
                .SelectMany(x => x.Classes);

        It should_map_the_entity = () =>
            mappings.ShouldContain(x => x.Type == typeof(Entity));

        It should_not_map_the_component_as_an_entity = () =>
            mappings.ShouldNotContain(x => x.Type == typeof(Component));

        It should_map_the_component_as_a_component_of_the_entity = () =>
            mappings.Single(x => x.Type == typeof(Entity))
                .Components.ShouldContain(x => x.Type == typeof(Component));

        static AutoPersistenceModel mapper;
        static IEnumerable<ClassMapping> mappings;

        class TestConfiguration : DefaultAutomappingConfiguration
        {
            public override bool IsComponent(Type type)
            {
                return type == typeof(Component);
            }
        }
    }
}
