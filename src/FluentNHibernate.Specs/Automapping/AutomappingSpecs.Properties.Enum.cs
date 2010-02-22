using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.Automapping.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Automapping
{
    public class when_the_automapper_is_told_to_map_an_entity_with_static_properties
    {
        Establish context = () =>
            mapper = AutoMap.Source(new StubTypeSource(typeof(EntityWithStaticProperties)));

        Because of = () =>
            mapping = mapper.BuildMappings().SelectMany(x => x.Classes).First();

        It should_not_create_property_mappings_for_the_static_properties = () =>
            mapping.Properties.Any(x => x.Name == "StaticProperty").ShouldBeFalse();

        static AutoPersistenceModel mapper;
        static ClassMapping mapping;
    }

    public class when_the_automapper_is_told_to_map_an_entity_with_a_enum_property : AutomapperEnumPropertySpec
    {
        Establish context = () =>
            mapper = AutoMap.Source(new StubTypeSource(typeof(Target)));

        Because of = () =>
            mapping = mapper.BuildMappings().SelectMany(x => x.Classes).First();

        It should_create_a_property_mapping_for_the_property = () =>
            mapping.Properties.ShouldContain(x => x.Name == "EnumProperty");

        It should_use_the_generic_enum_mapper_for_the_property = () =>
            mapping.Properties.First().Type.GetUnderlyingSystemType().ShouldEqual(typeof(GenericEnumMapper<Enum>));

        It should_create_a_column_for_the_property_mapping_with_the_property_name = () =>
            mapping.Properties.First().Columns.ShouldContain(x => x.Name == "EnumProperty");
    }

    public class when_the_automapper_is_told_to_map_an_entity_with_a_nullable_enum_property : AutomapperEnumPropertySpec
    {
        Establish context = () =>
            mapper = AutoMap.Source(new StubTypeSource(typeof(NullableTarget)));

        Because of = () =>
            mapping = mapper.BuildMappings().SelectMany(x => x.Classes).First();

        It should_create_a_property_mapping_for_the_property = () =>
            mapping.Properties.ShouldContain(x => x.Name == "EnumProperty");

        It should_use_the_generic_enum_mapper_for_the_property = () =>
            mapping.Properties.First().Type.GetUnderlyingSystemType().ShouldEqual(typeof(GenericEnumMapper<Enum>));

        It should_create_a_column_for_the_property_mapping_with_the_property_name = () =>
            mapping.Properties.First().Columns.ShouldContain(x => x.Name == "EnumProperty");
    }

    class Target
    {
        public int Id { get; set; }
        public Enum EnumProperty { get; set; }
    }

    class NullableTarget
    {
        public int Id { get; set; }
        public Enum? EnumProperty { get; set; }
    }

    enum Enum { }

    public abstract class AutomapperEnumPropertySpec
    {
        protected static AutoPersistenceModel mapper;
        protected static ClassMapping mapping;
    }
}