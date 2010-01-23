using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Testing.DomainModel.Mapping;
using Machine.Specifications;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    public class when_class_map_is_told_to_map_a_property : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<PropertyTarget>(o => o.Map(x => x.Name));

        Behaves_like<ClasslikePropertyBehaviour> a_property_in_a_classlike_mapping;

        protected static ClassMapping mapping;
    }
}