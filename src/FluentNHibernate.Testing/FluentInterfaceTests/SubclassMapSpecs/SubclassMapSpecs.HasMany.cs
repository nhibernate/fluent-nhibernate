using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Testing.DomainModel.Mapping;
using Machine.Specifications;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    public class when_subclass_map_is_told_to_map_a_has_many_bag : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_subclass<OneToManyTarget>(m => m.HasMany(x => x.BagOfChildren));

        Behaves_like<ClasslikeBagBehaviour> a_bag_in_a_classlike_mapping;

        protected static SubclassMapping mapping;
    }

    public class when_subclass_map_is_told_to_map_a_has_many_set : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_subclass<OneToManyTarget>(m => m.HasMany(x => x.SetOfChildren));

        Behaves_like<ClasslikeSetBehaviour> a_set_in_a_classlike_mapping;

        protected static SubclassMapping mapping;
    }

    public class when_subclass_map_is_told_to_map_a_has_many_list_with_default_index : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_subclass<OneToManyTarget>(m => m.HasMany(x => x.ListOfChildren).AsList());

        Behaves_like<ClasslikeListWithDefaultIndexBehaviour> a_list_with_the_default_index_in_a_classlike_mapping;

        protected static SubclassMapping mapping;
    }

    public class when_subclass_map_is_told_to_map_a_has_many_list_with_custom_index : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_subclass<OneToManyTarget>(m => m.HasMany(x => x.ListOfChildren).AsList(x =>
            {
                x.Column("custom-column");
                x.Type<IndexTarget>();
            }));

        Behaves_like<ClasslikeListWithCustomIndexBehaviour> a_list_with_a_custom_index_in_a_classlike_mapping;

        protected static SubclassMapping mapping;
    }

    public class when_subclass_map_is_told_to_map_an_has_many_using_reveal : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_subclass<OneToManyTarget>(m => m.HasMany<ChildObject>(Reveal.Property<OneToManyTarget>("BagOfChildren")));

        Behaves_like<ClasslikeBagBehaviour> a_bag_in_a_classlike_mapping;

        protected static SubclassMapping mapping;
    }
}