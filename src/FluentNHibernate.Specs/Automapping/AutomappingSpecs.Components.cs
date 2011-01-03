using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.Automapping.Fixtures;
using FluentNHibernate.Specs.ExternalFixtures;
using FluentNHibernate.Utils;
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

    public class when_the_automapper_is_told_to_map_an_entity_and_a_component_that_has_an_associated_ComponentMap
    {
        Establish context = () =>
            mapper = AutoMap.Source(
                    new StubTypeSource(typeof(Entity), typeof(Component)),
                    new TestConfiguration())
                .AddMappingsFromSource(new StubTypeSource(typeof(CompMap)));

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

        It should_use_the_component_map_for_mapping = () =>
            mappings.Single().Components.Single().Access.ShouldEqual("none");

        static AutoPersistenceModel mapper;
        static IEnumerable<ClassMapping> mappings;

        class TestConfiguration : DefaultAutomappingConfiguration
        {
            public override bool IsComponent(Type type)
            {
                return type == typeof(Component);
            }
        }

        class CompMap : ComponentMap<Component>
        {
            public CompMap()
            {
                Access.None();
            }
        }
    }

    public class when_the_automapper_maps_nested_comonents
    {
        Establish context = () =>
            mapper = AutoMap.Source(
                new StubTypeSource(typeof(Entity1), typeof(Location), typeof(FormatA), typeof(FormatB)),
                new TestConfiguration());

        Because of = () =>
            mapping = mapper.BuildMappingFor<Entity1>();

        It should_prefix_the_components_in_the_entity = () =>
            mapping.Components
                .SelectMany(x => x.Properties)
                .SelectMany(x => x.Columns)
                .Select(x => x.Name)
                .ShouldContain("LeftLocationProperty", "RightLocationProperty");

        It should_prefix_the_components_in_the_components = () =>
            mapping.Components
                .SelectMany(x => x.Components)
                .SelectMany(x => x.Properties)
                .SelectMany(x => x.Columns)
                .Select(x => x.Name)
                .ShouldContain("LeftAPropertyA", "LeftBPropertyB", "RightAPropertyA", "RightBPropertyB");

        static AutoPersistenceModel mapper;
        static ClassMapping mapping;

        class TestConfiguration : DefaultAutomappingConfiguration
        {
            public override bool IsComponent(Type type)
            {
                return type.In(typeof(Location), typeof(FormatA), typeof(FormatB));
            }
        }
    }

    public class Entity1
    {
        public int Id { get; set; }
        public Location Left { get; set; } 
        public Location Right { get; set; } 
    }

    public class Location
    {
        public string LocationProperty { get; set; }
        public FormatA A { get; set; }
        public FormatB B { get; set; }
    }

    public class FormatA
    {
        public string PropertyA { get; set; }
    }

    public class FormatB
    {
        public string PropertyB { get; set; }
    }
}
