using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Testing.DomainModel.Mapping;
using Machine.Specifications;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    public class when_class_map_is_told_to_map_a_component : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<PropertyTarget>(m => m.Component(x => x.Component, c => {}));

        Behaves_like<ClasslikeComponentBehaviour> a_component_in_a_classlike;

        protected static ClassMapping mapping;
    }

    public class when_class_map_is_told_to_map_a_dynamic_component : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<PropertyTarget>(m => m.DynamicComponent(x => x.ExtensionData, c => { }));

        Behaves_like<ClasslikeComponentBehaviour> a_component_in_a_classlike;

        protected static ClassMapping mapping;
    }

    public class when_class_map_is_told_to_map_a_reference_component : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<PropertyTarget>(m => m.Component(x => x.Component));

        Behaves_like<ClasslikeComponentBehaviour> a_component_in_a_classlike;

        protected static ClassMapping mapping;
    }
}