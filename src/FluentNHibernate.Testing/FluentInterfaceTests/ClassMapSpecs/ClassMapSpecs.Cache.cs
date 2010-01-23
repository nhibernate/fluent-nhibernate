using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Testing.DomainModel.Mapping;
using Machine.Specifications;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    public class when_class_map_is_told_to_configure_the_cache : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<PropertyTarget>(m => m.Cache.ReadOnly());

        It should_set_the_cache_property_on_the_mapping = () =>
            mapping.Cache.ShouldNotBeNull();

        It should_set_the_cache_usage_to_the_value_used = () =>
            mapping.Cache.Usage.ShouldEqual("read-only");

        static ClassMapping mapping;
    }
}