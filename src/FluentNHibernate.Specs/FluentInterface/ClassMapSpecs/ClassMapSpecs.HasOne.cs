using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.FluentInterface.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.FluentInterface.ClassMapSpecs
{
    public class when_class_map_is_told_to_map_a_has_one : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<EntityWithReferences>(m => m.HasOne(x => x.Reference));

        Behaves_like<ClasslikeHasOneBehaviour> a_has_one_in_a_classlike_mapping;

        protected static ClassMapping mapping;
    }

    public class when_class_map_is_told_to_map_a_has_one_using_reveal : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<EntityWithReferences>(m => m.HasOne(Reveal.Member<EntityWithReferences>("Reference")));

        Behaves_like<ClasslikeHasOneBehaviour> a_has_one_in_a_classlike_mapping;

        protected static ClassMapping mapping;
    }
}