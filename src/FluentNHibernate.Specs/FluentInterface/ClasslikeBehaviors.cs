using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Specs.FluentInterface.Fixtures;
using Machine.Specifications;
using FluentAssertions;

namespace FluentNHibernate.Specs.FluentInterface;

[Behaviors]
public class ClasslikePropertyBehaviour
{
    protected static ClassMappingBase mapping;

    It should_add_a_property_mapping_to_the_properties_collection_on_the_class_mapping = () =>
        mapping.Properties.Count().Should().Be(1);

    It should_create_property_mapping_with_correct_name = () =>
        mapping.Properties.First().Name.Should().Be("Name");

    It should_have_a_single_column = () =>
        mapping.Properties.First().Columns.Count().Should().Be(1);

    It should_have_a_column_with_the_same_name_as_the_property = () =>
        mapping.Properties.First().Columns.First().Name.Should().Be("Name");
}

[Behaviors]
public class ClasslikeComponentBehaviour
{
    protected static ClassMappingBase mapping;

    It should_add_a_component_to_the_components_collection = () =>
        mapping.Components.Count().Should().Be(1);
}

[Behaviors]
public class ClasslikeHasOneBehaviour
{
    protected static ClassMappingBase mapping;

    It should_add_the_has_one_to_the_has_one_collection_on_the_mapping = () =>
        mapping.OneToOnes.Count().Should().Be(1);

    It should_create_the_has_one_with_the_name_from_the_property_used = () =>
        mapping.OneToOnes.Single().Name.Should().Be("Reference");
}

[Behaviors]
public class ClasslikeBagBehaviour
{
    It should_add_only_one_collection_to_the_mapping = () =>
        mapping.Collections.Count().Should().Be(1);

    It should_add_a_bag_to_the_collections_of_the_mapping = () =>
        mapping.Collections.Single().Collection.Should().Be(Collection.Bag);

    It should_use_the_property_name_as_the_collection_name = () =>
        mapping.Collections.Single().Name.Should().Be("BagOfChildren");

    It should_create_a_key_for_the_collection = () =>
        mapping.Collections.Single().Key.Should().NotBeNull();

    It should_create_a_single_column_for_the_key = () =>
        mapping.Collections.Single().Key.Columns.Count().Should().Be(1);

    protected static ClassMappingBase mapping;
}

[Behaviors]
public class ClasslikeSetBehaviour
{
    It should_add_only_one_collection_to_the_mapping = () =>
        mapping.Collections.Count().Should().Be(1);

    It should_add_a_set_to_the_collections_of_the_mapping = () =>
        mapping.Collections.Single().Collection.Should().Be(Collection.Set);

    It should_use_the_property_name_as_the_collection_name = () =>
        mapping.Collections.Single().Name.Should().Be("SetOfChildren");

    It should_create_a_key_for_the_collection = () =>
        mapping.Collections.Single().Key.Should().NotBeNull();

    It should_create_a_single_column_for_the_key = () =>
        mapping.Collections.Single().Key.Columns.Count().Should().Be(1);

    It should_use_the_containing_type_name_suffixed_with_id_as_the_key_column_name = () =>
        mapping.Collections.Single().Key.Columns.Single().Name.Should().Be("EntityWithCollections_id");

    protected static ClassMappingBase mapping;
}

[Behaviors]
public class ClasslikeListWithDefaultIndexBehaviour : ClasslikeListBehaviour
{
    It should_create_an_index_for_the_collection_mapping = () =>
        mapping.Collections.Single().Index.Should().NotBeNull();

    It should_create_a_single_column_for_the_index = () =>
        mapping.Collections.Single().Index.Columns.Count().Should().Be(1);

    It should_use_index_as_the_index_column_name = () =>
        mapping.Collections.Single().Index.Columns.Single().Name.Should().Be("Index");
}

[Behaviors]
public class ClasslikeListWithCustomIndexBehaviour : ClasslikeListBehaviour
{
    It should_create_an_index_for_the_collection_mapping = () =>
        mapping.Collections.Single().Index.Should().NotBeNull();

    It should_create_a_single_column_for_the_index = () =>
        mapping.Collections.Single().Index.Columns.Count().Should().Be(1);

    It should_use_specified_column_name_as_the_index_column_name = () =>
        mapping.Collections.Single().Index.Columns.Single().Name.Should().Be("custom-column");

    It should_use_specified_type_as_the_index_type = () =>
        mapping.Collections.Single().Index.As<IndexMapping>().Type.Should().Be(new TypeReference(typeof(IndexTarget)));
}

public abstract class ClasslikeListBehaviour
{
    It should_add_only_one_collection_to_the_mapping = () =>
        mapping.Collections.Count().Should().Be(1);

    It should_add_a_list_to_the_collections_of_the_mapping = () =>
        mapping.Collections.Single().Collection.Should().Be(Collection.List);

    It should_use_the_property_name_as_the_collection_name = () =>
        mapping.Collections.Single().Name.Should().Be("ListOfChildren");

    It should_create_a_key_for_the_collection = () =>
        mapping.Collections.Single().Key.Should().NotBeNull();

    It should_create_a_single_column_for_the_key = () =>
        mapping.Collections.Single().Key.Columns.Count().Should().Be(1);

    It should_use_the_containing_type_name_suffixed_with_id_as_the_key_column_name = () =>
        mapping.Collections.Single().Key.Columns.Single().Name.Should().Be("OneToManyTarget_id");

    protected static ClassMappingBase mapping;
}

[Behaviors]
public class ClasslikeArrayBehaviour
{
    It should_add_only_one_collection_to_the_mapping = () =>
        mapping.Collections.Count().Should().Be(1);

    It should_add_an_array_to_the_collections_of_the_mapping = () =>
        mapping.Collections.Single().Collection.Should().Be(Collection.Array);

    It should_use_the_property_name_as_the_collection_name = () =>
        mapping.Collections.Single().Name.Should().Be("ArrayOfChildren");

    It should_create_a_key_for_the_collection = () =>
        mapping.Collections.Single().Key.Should().NotBeNull();

    It should_create_a_single_column_for_the_key = () =>
        mapping.Collections.Single().Key.Columns.Count().Should().Be(1);

    It should_use_the_containing_type_name_suffixed_with_id_as_the_key_column_name = () =>
        mapping.Collections.Single().Key.Columns.Single().Name.Should().Be("EntityWithCollections_id");

    It should_create_an_index_for_the_collection_mapping = () =>
        mapping.Collections.Single().Index.Should().NotBeNull();

    It should_create_a_single_column_for_the_index = () =>
        mapping.Collections.Single().Index.Columns.Count().Should().Be(1);

    It should_use_specified_property_as_the_index_column_name = () =>
        mapping.Collections.Single().Index.Columns.Single().Name.Should().Be("Position");

    protected static ClassMappingBase mapping;
}

[Behaviors]
public class HasManyElementBehaviour
{
    It should_create_a_collection = () =>
        collection.Should().NotBeNull();

    It should_create_a_element_mapping_in_the_collection = () =>
        collection.Element.Should().NotBeNull();

    It should_not_create_an_inner_relationship = () =>
        collection.Relationship.Should().BeNull();

    It should_not_create_a_component = () =>
        collection.CompositeElement.Should().BeNull();

    It should_use_the_default_column_name_for_the_element = () =>
        collection.Element.Columns.Single().Name.Should().Be("value");

    protected static CollectionMapping collection;
}
