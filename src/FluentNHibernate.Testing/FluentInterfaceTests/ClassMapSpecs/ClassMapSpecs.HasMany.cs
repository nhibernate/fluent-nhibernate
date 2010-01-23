using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Testing.DomainModel.Mapping;
using Machine.Specifications;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    public class when_class_map_is_told_to_map_a_has_many_bag : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<OneToManyTarget>(m => m.HasMany(x => x.BagOfChildren));

        Behaves_like<ClasslikeBagBehaviour> a_bag_in_a_classlike_mapping;

        protected static ClassMapping mapping;
    }

    public class when_class_map_is_told_to_map_a_has_many_set : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<OneToManyTarget>(m => m.HasMany(x => x.SetOfChildren));

        Behaves_like<ClasslikeSetBehaviour> a_set_in_a_classlike_mapping;

        protected static ClassMapping mapping;
    }

    public class when_class_map_is_told_to_map_a_has_many_list_with_default_index : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<OneToManyTarget>(m => m.HasMany(x => x.ListOfChildren).AsList());

        Behaves_like<ClasslikeListWithDefaultIndexBehaviour> a_list_with_the_default_index_in_a_classlike_mapping;

        protected static ClassMapping mapping;
    }

    public class when_class_map_is_told_to_map_a_has_many_list_with_custom_index : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<OneToManyTarget>(m => m.HasMany(x => x.ListOfChildren).AsList(x =>
            {
                x.Column("custom-column");
                x.Type<IndexTarget>();
            }));

        Behaves_like<ClasslikeListWithCustomIndexBehaviour> a_list_with_a_custom_index_in_a_classlike_mapping;

        protected static ClassMapping mapping;
    }

    public class when_class_map_is_told_to_map_a_has_many_array : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<OneToManyTarget>(m => m.HasMany(x => x.ArrayOfChildren).AsArray(x => x.Position));

        Behaves_like<ClasslikeArrayBehaviour> an_array_in_a_classlike_mapping;

        protected static ClassMapping mapping;
    }
}