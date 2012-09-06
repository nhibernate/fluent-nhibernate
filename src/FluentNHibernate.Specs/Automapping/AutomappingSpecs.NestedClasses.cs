using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        It should_automap_the_nested_entity_per_default = () =>
            mappings.ShouldContain(x => x.Type == typeof(Order.OrderLine));

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

    public class when_automapper_is_told_to_map_a_class_with_a_nested_classes
    {
        Establish context = () =>
        {
            nonPublicNestedType = typeof(PersonCard).GetNestedTypes(BindingFlags.NonPublic).Single();
            model = AutoMap.Source(new StubTypeSource(typeof(PersonCard), typeof(PersonCard.PublicAddress), nonPublicNestedType));
        };

        Because of = () => mappings = model.BuildMappings().SelectMany(m => m.Classes);
        It should_map_the_main_class = () => mappings.ShouldContain(m => m.Type == typeof(PersonCard));
        It should_map_public_nested_class = () => mappings.ShouldContain(m => m.Type == typeof(PersonCard.PublicAddress));
        It should_not_map_non_public_nested_class = () => mappings.ShouldNotContain(m => m.Type == nonPublicNestedType);

        static AutoPersistenceModel model;
        static DefaultAutomappingConfiguration configuration;
        static Type nonPublicNestedType;
        static IEnumerable<ClassMapping> mappings;
    }

    public class PersonCard
    {
        public int Id { get; set; }
        public class PublicAddress
        {
            public int Id { get; set; }
        }
        private class PrivateAddress
        {
            public int Id { get; set; }
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