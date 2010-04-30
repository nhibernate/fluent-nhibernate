using System.Collections;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.FluentInterface.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.FluentInterface.SubclassMapSpecs
{
    public class when_subclass_map_is_told_to_map_a_component : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_subclass<EntityWithComponent>(m => m.Component(x => x.Component, c => {}));

        Behaves_like<ClasslikeComponentBehaviour> a_component_in_a_classlike;

        protected static SubclassMapping mapping;
    }

    public class when_subclass_map_is_told_to_map_a_component_from_a_field : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_subclass<EntityWithFieldComponent>(m => m.Component(x => x.Component, c => { }));

        Behaves_like<ClasslikeComponentBehaviour> a_component_in_a_classlike;

        protected static SubclassMapping mapping;
    }

    public class when_subclass_map_is_told_to_map_a_component_using_reveal : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_subclass<EntityWithComponent>(m => m.Component(Reveal.Member<EntityWithComponent>("Component"), c => { }));

        Behaves_like<ClasslikeComponentBehaviour> a_component_in_a_classlike;

        protected static SubclassMapping mapping;
    }

    public class when_subclass_map_is_told_to_map_a_dynamic_component : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_subclass<EntityWithComponent>(m => m.DynamicComponent(x => x.DynamicComponent, c => { }));

        Behaves_like<ClasslikeComponentBehaviour> a_component_in_a_classlike;

        protected static SubclassMapping mapping;
    }

    public class when_subclass_map_is_told_to_map_a_dynamic_component_from_a_field : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_subclass<EntityWithFieldComponent>(m => m.DynamicComponent(x => x.DynamicComponent, c => { }));

        Behaves_like<ClasslikeComponentBehaviour> a_component_in_a_classlike;

        protected static SubclassMapping mapping;
    }

    public class when_subclass_map_is_told_to_map_a_dynamic_component_using_reveal : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_subclass<EntityWithComponent>(m => m.DynamicComponent(Reveal.Member<EntityWithComponent, IDictionary>("DynamicComponent"), c => { }));

        Behaves_like<ClasslikeComponentBehaviour> a_component_in_a_classlike;

        protected static SubclassMapping mapping;
    }

    public class when_subclass_map_is_told_to_map_a_reference_component : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_subclass<EntityWithComponent>(m => m.Component(x => x.Component));

        Behaves_like<ClasslikeComponentBehaviour> a_component_in_a_classlike;

        protected static SubclassMapping mapping;
    }

    public class when_subclass_map_is_told_to_map_a_reference_component_from_a_field : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_subclass<EntityWithFieldComponent>(m => m.Component(x => x.Component));

        Behaves_like<ClasslikeComponentBehaviour> a_component_in_a_classlike;

        protected static SubclassMapping mapping;
    }

    public class when_subclass_map_is_told_to_map_a_reference_component_using_reveal : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_subclass<EntityWithComponent>(m => m.Component(Reveal.Member<EntityWithComponent>("Component")));

        Behaves_like<ClasslikeComponentBehaviour> a_component_in_a_classlike;

        protected static SubclassMapping mapping;
    }
}