using System.Linq;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.FluentInterface.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.FluentInterface.ClassMapSpecs
{
    public class when_class_map_is_told_to_create_a_discriminator : ProviderSpec
    {
        Because of = () =>
            class_mapping = map_as_class<SuperTarget>(m => m.DiscriminateSubClassesOnColumn(column_name));

        It should_set_the_discriminator_property_on_the_class_mapping = () =>
            class_mapping.Discriminator.ShouldNotBeNull();

        It should_create_one_column_for_the_discriminator = () =>
            class_mapping.Discriminator.Columns.Count().ShouldEqual(1);

        It should_create_a_column_for_the_discriminator_with_the_name_as_supplied = () =>
            class_mapping.Discriminator.Columns.First().Name.ShouldEqual(column_name);

        static ClassMapping class_mapping;
        const string column_name = "col";
    }

    public class when_class_map_is_told_to_create_a_discriminator_with_a_default_value : ProviderSpec
    {
        Because of = () =>
            class_mapping = map_as_class<SuperTarget>(m => m.DiscriminateSubClassesOnColumn("col", base_value));

        It should_set_the_default_discriminator_value_on_the_class_mapping = () =>
            class_mapping.DiscriminatorValue.ShouldEqual(base_value);

        static ClassMapping class_mapping;
        const string base_value = "base-value";
    }
}