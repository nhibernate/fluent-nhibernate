using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.FluentInterface.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.FluentInterface.ClassMapSpecs
{
    public class when_class_map_is_told_to_map_a_property : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<EntityWithProperties>(o => o.Map(x => x.Name));

        Behaves_like<ClasslikePropertyBehaviour> a_property_in_a_classlike_mapping;

        protected static ClassMapping mapping;
    }

    public class when_class_map_is_told_to_map_a_field : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<EntityWithFields>(o => o.Map(x => x.Name));

        Behaves_like<ClasslikePropertyBehaviour> a_property_in_a_classlike_mapping;

        protected static ClassMapping mapping;
    }

    public class when_class_map_is_told_to_map_a_private_property_using_reveal : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<EntityWithPrivateProperties>(o => o.Map(Reveal.Member<EntityWithPrivateProperties>("Name")));

        Behaves_like<ClasslikePropertyBehaviour> a_property_in_a_classlike_mapping;

        protected static ClassMapping mapping;
    }

    public class when_class_map_is_told_to_map_a_private_field_using_reveal : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<EntityWithPrivateProperties>(o => o.Map(Reveal.Member<EntityWithPrivateProperties>("name")));

        Behaves_like<ClasslikePropertyBehaviour> a_property_in_a_classlike_mapping;

        protected static ClassMapping mapping;
    }
}