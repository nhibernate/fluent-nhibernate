using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Specs.FluentInterface.Fixtures;
using Machine.Specifications;
using FluentAssertions;

namespace FluentNHibernate.Specs.FluentInterface.ClassMapSpecs;

public class when_class_map_is_told_to_map_an_id_without_a_property_or_column : ProviderSpec
{
    Because of = () =>
        mapping = map_as_class<EntityWithProperties>(m => m.Id());

    It should_set_the_id_on_the_mapping = () =>
        Id.Should().NotBeNull();

    It should_not_set_the_member_on_the_id = () =>
        Id.Member.Should().BeNull();

    It should_not_specify_any_columns_for_the_id = () =>
        Id.Columns.Should().BeEmpty();

    It should_specify_the_default_generator_for_the_id = () =>
        Id.Generator.Class.Should().Be("increment");

    It should_set_the_id_type_to_int_by_default = () =>
        Id.Type.Should().Be(new TypeReference(typeof(int)));

    static ClassMapping mapping;

    static IdMapping Id => mapping.Id as IdMapping;
}

public class when_class_map_has_a_composite_id_with_a_key_reference_with_multiple_columns : ProviderSpec
{
    Because of = () =>
        mapping = map_as_class<EntityWithReferences>(m =>
            m.CompositeId()
                .KeyReference(x => x.Reference, "col1", "col2"));

    It should_add_all_the_columns_to_the_composite_id_mapping = () =>
        mapping.Id.As<CompositeIdMapping>()
            .Keys.Single()
            .Columns.Select(x => x.Name)
            .Should().Contain(new string[] { "col1", "col2" });

    static ClassMapping mapping;
}
