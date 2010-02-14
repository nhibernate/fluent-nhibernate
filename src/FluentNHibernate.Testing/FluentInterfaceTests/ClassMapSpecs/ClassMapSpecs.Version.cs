using System.Linq;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Testing.DomainModel.Mapping;
using Machine.Specifications;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    public class when_class_map_is_told_to_map_a_version : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<VersionTarget>(m => m.Version(x => x.VersionNumber));

        It should_set_the_version_property_on_the_mapping = () =>
            mapping.Version.ShouldNotBeNull();

        It should_create_a_single_column_for_the_version = () =>
            mapping.Version.Columns.Count().ShouldEqual(1);

        It should_use_the_property_name_for_the_column_name = () =>
            mapping.Version.Columns.Single().Name.ShouldEqual("VersionNumber");

        static ClassMapping mapping;
    }

    public class when_class_map_is_told_to_map_a_version_using_reveal : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<VersionTarget>(m => m.Version(Reveal.Property<VersionTarget>("VersionNumber")));

        It should_set_the_version_property_on_the_mapping = () =>
            mapping.Version.ShouldNotBeNull();

        It should_create_a_single_column_for_the_version = () =>
            mapping.Version.Columns.Count().ShouldEqual(1);

        It should_use_the_property_name_for_the_column_name = () =>
            mapping.Version.Columns.Single().Name.ShouldEqual("VersionNumber");

        static ClassMapping mapping;
    }
}