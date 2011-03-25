using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Specs.FluentInterface.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.FluentInterface
{
    [Behaviors]
    public class ClasslikePropertyBehaviour
    {
        protected static ClassMappingBase mapping;

        It should_add_a_property_mapping_to_the_properties_collection_on_the_class_mapping = () =>
            mapping.Properties.Count().ShouldEqual(1);

        It should_create_property_mapping_with_correct_name = () =>
            mapping.Properties.First().Name.ShouldEqual("Name");

        It should_have_a_single_column = () =>
            mapping.Properties.First().Columns.Count().ShouldEqual(1);

        It should_have_a_column_with_the_same_name_as_the_property = () =>
            mapping.Properties.First().Columns.First().Name.ShouldEqual("Name");
    }

    [Behaviors]
    public class ClasslikeComponentBehaviour
    {
        protected static ClassMappingBase mapping;

        It should_add_a_component_to_the_components_collection = () =>
            mapping.Components.Count().ShouldEqual(1);
    }

    [Behaviors]
    public class ClasslikeHasOneBehaviour
    {
        protected static ClassMappingBase mapping;

        It should_add_the_has_one_to_the_has_one_collection_on_the_mapping = () =>
            mapping.OneToOnes.Count().ShouldEqual(1);

        It should_create_the_has_one_with_the_name_from_the_property_used = () =>
            mapping.OneToOnes.Single().Name.ShouldEqual("Reference");
    }

    [Behaviors]
    public class ClasslikeBagBehaviour
    {
        It should_add_only_one_collection_to_the_mapping = () =>
            mapping.Collections.Count().ShouldEqual(1);

        It should_add_a_bag_to_the_collections_of_the_mapping = () =>
            mapping.Collections.Single().Collection.ShouldEqual(Collection.Bag);

        It should_use_the_property_name_as_the_collection_name = () =>
            mapping.Collections.Single().Name.ShouldEqual("BagOfChildren");

        It should_create_a_key_for_the_collection = () =>
            mapping.Collections.Single().Key.ShouldNotBeNull();

        It should_create_a_single_column_for_the_key = () =>
            mapping.Collections.Single().Key.Columns.Count().ShouldEqual(1);

        protected static ClassMappingBase mapping;
    }

    [Behaviors]
    public class ClasslikeSetBehaviour
    {
        It should_add_only_one_collection_to_the_mapping = () =>
            mapping.Collections.Count().ShouldEqual(1);

        It should_add_a_set_to_the_collections_of_the_mapping = () =>
            mapping.Collections.Single().Collection.ShouldEqual(Collection.Set);

        It should_use_the_property_name_as_the_collection_name = () =>
            mapping.Collections.Single().Name.ShouldEqual("SetOfChildren");

        It should_create_a_key_for_the_collection = () =>
            mapping.Collections.Single().Key.ShouldNotBeNull();

        It should_create_a_single_column_for_the_key = () =>
            mapping.Collections.Single().Key.Columns.Count().ShouldEqual(1);

        It should_use_the_containing_type_name_suffixed_with_id_as_the_key_column_name = () =>
            mapping.Collections.Single().Key.Columns.Single().Name.ShouldEqual("EntityWithCollections_id");

        protected static ClassMappingBase mapping;
    }

    [Behaviors]
    public class ClasslikeListWithDefaultIndexBehaviour : ClasslikeListBehaviour
    {
        It should_create_an_index_for_the_collection_mapping = () =>
            mapping.Collections.Single().Index.ShouldNotBeNull();

        It should_create_a_single_column_for_the_index = () =>
            mapping.Collections.Single().Index.Columns.Count().ShouldEqual(1);

        It should_use_index_as_the_index_column_name = () =>
            mapping.Collections.Single().Index.Columns.Single().Name.ShouldEqual("Index");
    }

    [Behaviors]
    public class ClasslikeListWithCustomIndexBehaviour : ClasslikeListBehaviour
    {
        It should_create_an_index_for_the_collection_mapping = () =>
            mapping.Collections.Single().Index.ShouldNotBeNull();

        It should_create_a_single_column_for_the_index = () =>
            mapping.Collections.Single().Index.Columns.Count().ShouldEqual(1);

        It should_use_specified_column_name_as_the_index_column_name = () =>
            mapping.Collections.Single().Index.Columns.Single().Name.ShouldEqual("custom-column");

        It should_use_specified_type_as_the_index_type = () =>
            mapping.Collections.Single().Index.As<IndexMapping>().Type.ShouldEqual(new TypeReference(typeof(IndexTarget)));
    }

    public abstract class ClasslikeListBehaviour
    {
        It should_add_only_one_collection_to_the_mapping = () =>
            mapping.Collections.Count().ShouldEqual(1);

        It should_add_a_list_to_the_collections_of_the_mapping = () =>
            mapping.Collections.Single().Collection.ShouldEqual(Collection.List);

        It should_use_the_property_name_as_the_collection_name = () =>
            mapping.Collections.Single().Name.ShouldEqual("ListOfChildren");

        It should_create_a_key_for_the_collection = () =>
            mapping.Collections.Single().Key.ShouldNotBeNull();

        It should_create_a_single_column_for_the_key = () =>
            mapping.Collections.Single().Key.Columns.Count().ShouldEqual(1);

        It should_use_the_containing_type_name_suffixed_with_id_as_the_key_column_name = () =>
            mapping.Collections.Single().Key.Columns.Single().Name.ShouldEqual("OneToManyTarget_id");

        protected static ClassMappingBase mapping;
    }

    [Behaviors]
    public class ClasslikeArrayBehaviour
    {
        It should_add_only_one_collection_to_the_mapping = () =>
            mapping.Collections.Count().ShouldEqual(1);

        It should_add_an_array_to_the_collections_of_the_mapping = () =>
            mapping.Collections.Single().Collection.ShouldEqual(Collection.Array);

        It should_use_the_property_name_as_the_collection_name = () =>
            mapping.Collections.Single().Name.ShouldEqual("ArrayOfChildren");

        It should_create_a_key_for_the_collection = () =>
            mapping.Collections.Single().Key.ShouldNotBeNull();

        It should_create_a_single_column_for_the_key = () =>
            mapping.Collections.Single().Key.Columns.Count().ShouldEqual(1);

        It should_use_the_containing_type_name_suffixed_with_id_as_the_key_column_name = () =>
            mapping.Collections.Single().Key.Columns.Single().Name.ShouldEqual("EntityWithCollections_id");

        It should_create_an_index_for_the_collection_mapping = () =>
            mapping.Collections.Single().Index.ShouldNotBeNull();

        It should_create_a_single_column_for_the_index = () =>
            mapping.Collections.Single().Index.Columns.Count().ShouldEqual(1);

        It should_use_specified_property_as_the_index_column_name = () =>
            mapping.Collections.Single().Index.Columns.Single().Name.ShouldEqual("Position");

        protected static ClassMappingBase mapping;
    }

    [Behaviors]
    public class HasManyElementBehaviour
    {
        It should_create_a_collection = () =>
            collection.ShouldNotBeNull();

        It should_create_a_element_mapping_in_the_collection = () =>
            collection.Element.ShouldNotBeNull();

        It should_not_create_an_inner_relationship = () =>
            collection.Relationship.ShouldBeNull();

        It should_not_create_a_component = () =>
            collection.CompositeElement.ShouldBeNull();

        It should_use_the_default_column_name_for_the_element = () =>
            collection.Element.Columns.Single().Name.ShouldEqual("value");

        protected static CollectionMapping collection;
    }
}