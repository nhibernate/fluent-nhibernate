using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.FluentInterface.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.FluentInterface.ClassMapSpecs
{
    public class when_class_map_is_told_to_map_a_has_many_bag : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<EntityWithCollections>(m => m.HasMany(x => x.BagOfChildren));

        Behaves_like<ClasslikeBagBehaviour> a_bag_in_a_classlike_mapping;

        It should_use_the_containing_type_name_suffixed_with_id_as_the_key_column_name = () =>
            mapping.Collections.Single().Key.Columns.Single().Name.ShouldEqual("EntityWithCollections_id");

        protected static ClassMapping mapping;
    }

    public class when_class_map_is_told_to_map_a_has_many_set : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<EntityWithCollections>(m => m.HasMany(x => x.SetOfChildren));

        Behaves_like<ClasslikeSetBehaviour> a_set_in_a_classlike_mapping;

        protected static ClassMapping mapping;
    }

    public class when_class_map_is_told_to_map_a_has_many_list_with_default_index : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<EntityWithCollections>(m => m.HasMany(x => x.BagOfChildren).AsList());

        Behaves_like<ClasslikeListWithDefaultIndexBehaviour> a_list_with_the_default_index_in_a_classlike_mapping;

        protected static ClassMapping mapping;
    }

    public class when_class_map_is_told_to_map_a_has_many_list_with_custom_index : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<EntityWithCollections>(m => m.HasMany(x => x.BagOfChildren).AsList(x =>
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
            mapping = map_as_class<EntityWithCollections>(m => m.HasMany(x => x.ArrayOfChildren).AsArray(x => x.Position));

        Behaves_like<ClasslikeArrayBehaviour> an_array_in_a_classlike_mapping;

        protected static ClassMapping mapping;
    }

    public class when_class_map_is_told_to_map_an_has_many_from_a_field : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<EntityWithFieldCollections>(m => m.HasMany(x => x.BagOfChildren));

        Behaves_like<ClasslikeBagBehaviour> a_bag_in_a_classlike_mapping;

        It should_use_the_containing_type_name_suffixed_with_id_as_the_key_column_name = () =>
            mapping.Collections.Single().Key.Columns.Single().Name.ShouldEqual("EntityWithFieldCollections_id");

        protected static ClassMapping mapping;
    }

    public class when_class_map_is_told_to_map_an_has_many_using_reveal : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<EntityWithCollections>(m => m.HasMany<ChildTarget>(Reveal.Member<EntityWithCollections>("BagOfChildren")));

        Behaves_like<ClasslikeBagBehaviour> a_bag_in_a_classlike_mapping;

        It should_use_the_containing_type_name_suffixed_with_id_as_the_key_column_name = () =>
            mapping.Collections.Single().Key.Columns.Single().Name.ShouldEqual("EntityWithCollections_id");

        protected static ClassMapping mapping;
    }

    public class when_class_map_has_a_collection_with_a_component_with_a_nested_component : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<EntityWithCollections>(m =>
                m.HasMany(x => x.BagOfChildren)
                    .Component(c =>
                    {
                        c.Component(x => x.Area, n => n.Map(x => x.Lat));
                    }));

        It should_create_a_nested_component_inside_the_first_component = () =>
            mapping.Collections.Single().CompositeElement.CompositeElements.ShouldNotBeEmpty();

        It should_create_the_nested_component_with_the_correct_name = () =>
            mapping.Collections.Single().CompositeElement.CompositeElements.Single().Name.ShouldEqual("Area");

        It should_create_the_nested_component_with_the_correct_type = () =>
            mapping.Collections.Single().CompositeElement.CompositeElements.Single().Class.ShouldEqual(new TypeReference(typeof(AreaComponent)));

        It should_create_a_property_inside_the_nested_component = () =>
            mapping.Collections.Single().CompositeElement.CompositeElements.Single().Properties.ShouldNotBeEmpty();

        static ClassMapping mapping;
    }
}