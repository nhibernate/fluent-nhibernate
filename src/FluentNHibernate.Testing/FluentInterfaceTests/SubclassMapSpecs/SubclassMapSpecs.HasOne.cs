using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Testing.DomainModel.Mapping;
using Machine.Specifications;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    public class when_subclass_map_is_told_to_map_a_has_one : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_subclass<PropertyTarget>(m => m.HasOne(x => x.Reference));

        Behaves_like<ClasslikeHasOneBehaviour> a_has_one_in_a_classlike_mapping;

        protected static SubclassMapping mapping;
    }
}