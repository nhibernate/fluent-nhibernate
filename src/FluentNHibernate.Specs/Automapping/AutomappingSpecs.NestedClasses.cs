using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.Automapping.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Automapping
{
    public class when_the_automapper_is_told_to_map_an_entity_with_a_nested_entity
    {
        Establish context = () =>
            mapper = AutoMap.Source(
                new StubTypeSource(typeof(Order), typeof(Order.OrderLine)));

        Because of = () =>
            mappings = mapper.BuildMappings()
                .SelectMany(x => x.Classes);

        It should_map_the_entity = () =>
            mappings.ShouldContain(x => x.Type == typeof(Order));

        It should_not_automap_the_nested_entity_per_default = () =>
            mappings.ShouldNotContain(x => x.Type == typeof(Order.OrderLine));

        static AutoPersistenceModel mapper;
        static IEnumerable<ClassMapping> mappings;
    }

    public class when_changing_the_behaviour_of_automapping_configuration_to_also_map_nested_entities
    {
        Establish context = () =>
            mapper = AutoMap.Source(
                new StubTypeSource(typeof(Order), typeof(Order.OrderLine)), new TestConfiguration());

        Because of = () =>
            mappings = mapper.BuildMappings()
                .SelectMany(x => x.Classes);

        It should_map_the_entity = () =>
            mappings.ShouldContain(x => x.Type == typeof(Order));

        It should_automap_the_nested_entity = () =>
            mappings.ShouldContain(x => x.Type == typeof(Order.OrderLine));

        static AutoPersistenceModel mapper;
        static IEnumerable<ClassMapping> mappings;

        class TestConfiguration : DefaultAutomappingConfiguration
        {
            public override bool ShouldMap(Type type)
            {
                return base.ShouldMap(type) || type.IsNested;
            }
        }
    }
    public class Order
    {
        public int Id { get; set; }
        public class OrderLine
        {
            public int Id { get; set; }
        }
    }

}