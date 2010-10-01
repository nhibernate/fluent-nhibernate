using System;
using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Specs.FluentInterface.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.FluentInterface
{
    public class when_mapping_a_value_type_key_value_pair_dictionary : DictionarySpec
    {
        // Map key value pair
        // IDictionary<string, string>
        // <map name="Attributes" table="UserAttributesAreBoring">
        //   <key column="UserId"/>
        //   <index column="AttributeName" type="System.String"/>
        //   <element column="Attributevalue" type="System.String"/>
        // </map>
        
        // IDictionary<Guid, string>
        //<map name="TextCharacteristicValues" access="field.pascalcase-m-underscore" table="Entities_Characteristics_Text" cascade="none" inverse="true" mutable="false">
        //  <key column="CharacteristicID" foreign-key="FK_Characteristics_TextValues_Entities" /> 
        //  <index column="CharacteristicValueEntityID" type="System.Guid" /> 
        //  <element column="Value" length="4000" /> 
        //</map>

        Because of = () =>
            mapping_for<Blog>(class_map =>
                class_map.HasMany(x => x.UrlAliases));

        It should_create_a_map = () =>
        {
            mapping.Collections.Count().ShouldEqual(1);
            map.ShouldNotBeNull();
        };

        It should_use_the_property_name_as_the_map_name = () =>
            map.Name.ShouldEqual("UrlAliases");

        It should_set_the_map_table_name_to_a_default_name = () =>
            map.TableName.ShouldEqual("BlogUrlAliases");
            // this table name makes more sense in a real life case (User entity
            // with an Attributes collection would have a table called UserAttributes)

        It should_create_a_key = () =>
            map.Key.ShouldNotBeNull();

        It should_set_the_map_key_to_the_primary_key_of_the_containing_class;
        // ignored for now, until we can sync keys properly

        It should_set_the_map_key_to_a_generated_default_based_on_the_entity_name = () =>
            map.Key.Columns.Select(x => x.Name).ShouldContainOnly("Blog_id");
            // obsoleted by the above when implemented

        It should_create_an_index = () =>
            map.Index.ShouldNotBeNull();

        It should_set_the_map_index_column_to_a_default_name = () =>
            map.Index.As<IndexMapping>().Columns.Select(x => x.Name).ShouldContainOnly("Key");

        It should_set_the_map_index_type_to_the_key_type = () =>
            map.Index.Type.ShouldEqual(new TypeReference(typeof(string)));
        
        It should_create_a_element = () =>
            map.Element.ShouldNotBeNull();

        It should_set_the_map_element_column_to_a_default_name = () =>
            map.Element.Columns.Select(x => x.Name).ShouldContainOnly("Value");

        It should_set_the_map_element_type_to_the_value_type = () =>
            map.Element.Type.ShouldEqual(new TypeReference(typeof(string)));
    }

    public class when_mapping_a_value_type_key_value_pair_dictionary_custom_collection : DictionarySpec
    {
        // Map key value pair
        // IDictionary<string, string>
        // <map name="Attributes" table="UserAttributesAreBoring">
        //   <key column="UserId"/>
        //   <index column="AttributeName" type="System.String"/>
        //   <element column="Attributevalue" type="System.String"/>
        // </map>

        // IDictionary<Guid, string>
        //<map name="TextCharacteristicValues" access="field.pascalcase-m-underscore" table="Entities_Characteristics_Text" cascade="none" inverse="true" mutable="false">
        //  <key column="CharacteristicID" foreign-key="FK_Characteristics_TextValues_Entities" /> 
        //  <index column="CharacteristicValueEntityID" type="System.Guid" /> 
        //  <element column="Value" length="4000" /> 
        //</map>

        Because of = () =>
            mapping_for<Blog>(class_map =>
                class_map.HasMany(x => x.UrlAliasesCustomCollection));

        It should_create_a_map = () =>
        {
            mapping.Collections.Count().ShouldEqual(1);
            map.ShouldNotBeNull();
        };

        It should_use_the_property_name_as_the_map_name = () =>
            map.Name.ShouldEqual("UrlAliasesCustomCollection");

        It should_set_the_map_table_name_to_a_default_name = () =>
            map.TableName.ShouldEqual("BlogUrlAliasesCustomCollection");
        // this table name makes more sense in a real life case (User entity
        // with an Attributes collection would have a table called UserAttributes)

        It should_create_a_key = () =>
            map.Key.ShouldNotBeNull();

        It should_set_the_map_key_to_the_primary_key_of_the_containing_class;
        // ignored for now, until we can sync keys properly

        It should_set_the_map_key_to_a_generated_default_based_on_the_entity_name = () =>
            map.Key.Columns.Select(x => x.Name).ShouldContainOnly("Blog_id");
        // obsoleted by the above when implemented

        It should_create_an_index = () =>
            map.Index.ShouldNotBeNull();

        It should_set_the_map_index_column_to_a_default_name = () =>
            map.Index.As<IndexMapping>().Columns.Select(x => x.Name).ShouldContainOnly("Key");

        It should_set_the_map_index_type_to_the_key_type = () =>
            map.Index.Type.ShouldEqual(new TypeReference(typeof(string)));

        It should_create_a_element = () =>
            map.Element.ShouldNotBeNull();

        It should_set_the_map_element_column_to_a_default_name = () =>
            map.Element.Columns.Select(x => x.Name).ShouldContainOnly("Value");

        It should_set_the_map_element_type_to_the_value_type = () =>
            map.Element.Type.ShouldEqual(new TypeReference(typeof(string)));
    }

    public class when_mapping_a_value_type_key_with_entity_value_dictionary : DictionarySpec
    {
        // Map with entity + entity:
        // IDictionary<string, User>
        // <map name="AttributeOwners" table="PostAttributeOwners">
        //   <key column="PostId"/>
        //   <index column="Key" type="System.String" /> 
        //   <many-to-many class="User" column="UserId"/>
        // </map>

        Because of = () =>
            mapping_for<Blog>(class_map =>
                class_map.HasMany(x => x.Permalinks));

        It should_create_a_map = () =>
        {
            mapping.Collections.Count().ShouldEqual(1);
            map.ShouldNotBeNull();
        };

        It should_use_the_property_name_as_the_map_name = () =>
            map.Name.ShouldEqual("Permalinks");

        It should_set_the_map_table_name_to_a_default_name = () =>
            map.TableName.ShouldEqual("BlogPermalinks");
        // this table name makes more sense in a real life case (User entity
        // with an Attributes collection would have a table called UserAttributes)

        It should_create_a_key = () =>
            map.Key.ShouldNotBeNull();

        It should_set_the_map_key_to_the_primary_key_of_the_containing_class;
        // ignored for now, until we can sync keys properly

        It should_set_the_map_key_to_a_generated_default_based_on_the_entity_name = () =>
            map.Key.Columns.Select(x => x.Name).ShouldContainOnly("Blog_id");
        // obsoleted by the above when implemented

        It should_create_an_index = () =>
            map.Index.ShouldNotBeNull();

        It should_set_the_map_index_column_to_a_default_name = () =>
            map.Index.As<IndexMapping>().Columns.Select(x => x.Name).ShouldContainOnly("Key");

        It should_set_the_map_index_type_to_the_key_type = () =>
            map.Index.Type.ShouldEqual(new TypeReference(typeof(string)));

        It should_create_a_many_to_many = () =>
        {
            map.Relationship.ShouldNotBeNull();
            map.Relationship.ShouldBeOfType<ManyToManyMapping>();
        };

        It should_set_the_map_many_to_many_column_to_a_default_foreign_key_value = () =>
            map.Relationship.As<ManyToManyMapping>().Columns.Select(x => x.Name).ShouldContainOnly("Post_id");

        It should_set_the_map_many_to_many_type_to_the_value_entity_type = () =>
            map.Relationship.Class.ShouldEqual(new TypeReference(typeof(Post)));
    }

    public class when_mapping_a_value_type_key_with_composite_element_value_dictionary : DictionarySpec
    {
        // Map with composite-element:
        // IDictionary<int, Item>
        // <map name="Items">
        //   <key column="Store_id" /> 
        //   <index column="Key" type="string" /> 
        //   <composite-element class="Item">
        //     <property name="Name" /> 
        //     <property name="Age" /> 
        //   </composite-element>
        // </map>

        Because of = () =>
            mapping_for<Blog>(class_map =>
                class_map.HasMany(x => x.Permalinks)
                    .Component(c =>
                    {
                        c.Map(x => x.Name);
                        c.Map(x => x.Age);
                    })
            );

        It should_create_a_map = () =>
        {
            mapping.Collections.Count().ShouldEqual(1);
            map.ShouldNotBeNull();
        };

        It should_use_the_property_name_as_the_map_name = () =>
            map.Name.ShouldEqual("Permalinks");

        It should_set_the_map_table_name_to_a_default_name = () =>
            map.TableName.ShouldEqual("BlogPermalinks");
        // this table name makes more sense in a real life case (User entity
        // with an Attributes collection would have a table called UserAttributes)

        It should_create_a_key = () =>
            map.Key.ShouldNotBeNull();

        It should_set_the_map_key_to_the_primary_key_of_the_containing_class;
        // ignored for now, until we can sync keys properly

        It should_set_the_map_key_to_a_generated_default_based_on_the_entity_name = () =>
            map.Key.Columns.Select(x => x.Name).ShouldContainOnly("Blog_id");
        // obsoleted by the above when implemented

        It should_create_an_index = () =>
            map.Index.ShouldNotBeNull();

        It should_set_the_map_index_column_to_a_default_name = () =>
            map.Index.As<IndexMapping>().Columns.Select(x => x.Name).ShouldContainOnly("Key");

        It should_set_the_map_index_type_to_the_key_type = () =>
            map.Index.Type.ShouldEqual(new TypeReference(typeof(string)));

        It should_not_create_a_relationship = () =>
            map.Relationship.ShouldBeNull();

        It should_create_a_composite_element = () =>
            map.CompositeElement.ShouldNotBeNull();

        It should_set_the_map_composite_element_properties = () =>
            map.CompositeElement.Properties.Select(x => x.Name).ShouldContainOnly("Name", "Age");

        It should_set_the_map_composite_element_type_to_the_value_type = () =>
            map.CompositeElement.Class.ShouldEqual(new TypeReference(typeof(Post)));
    }

    public class when_mapping_a_composite_index_key_with_composite_element_value_dictionary : DictionarySpec
    {
        // Map with composite-index + composite-element:
        // IDictionary<FavPlaceKey, Position>
        // <map name="ComplexFavoritePlaces" table="UsersComplexFavoritePlaces" >
        //   <key column="UserId"/>
        //   <composite-index class="FavPlaceKey">
        //     <key-property  name="Name"/>
        //     <key-property name="Why"/>
        //   </composite-index>
        //   <composite-element class="Position">
        //     <property name="Lang"/>
        //     <property name="Lat"/>
        //   </composite-element>
        // </map>

        Because of = () =>
            mapping_for<Blog>(class_map =>
                class_map.HasMany(x => x.Commentors)
                    .ComponentIndex(c =>
                    {
                        c.Map(x => x.Name);
                        c.Map(x => x.Age);
                    })
                    .Component(c =>
                    {
                        c.Map(x => x.Name);
                        c.Map(x => x.Age);
                    })
            );

        It should_create_a_map = () =>
        {
            mapping.Collections.Count().ShouldEqual(1);
            map.ShouldNotBeNull();
        };

        It should_use_the_property_name_as_the_map_name = () =>
            map.Name.ShouldEqual("Commentors");

        It should_set_the_map_table_name_to_a_default_name = () =>
            map.TableName.ShouldEqual("BlogCommentors");
        // this table name makes more sense in a real life case (User entity
        // with an Attributes collection would have a table called UserAttributes)

        It should_create_a_key = () =>
            map.Key.ShouldNotBeNull();

        It should_set_the_map_key_to_the_primary_key_of_the_containing_class;
        // ignored for now, until we can sync keys properly

        It should_set_the_map_key_to_a_generated_default_based_on_the_entity_name = () =>
            map.Key.Columns.Select(x => x.Name).ShouldContainOnly("Blog_id");
        // obsoleted by the above when implemented

        It should_create_an_index = () =>
            map.Index.ShouldNotBeNull();

        It should_set_the_map_index_type_to_the_key_type = () =>
            map.Index.Type.ShouldEqual(new TypeReference(typeof(Post)));

        It should_set_the_map_index_properties = () =>
            map.Index.As<CompositeIndexMapping>().Properties.Select(x => x.Name).ShouldContainOnly("Name", "Age");

        It should_not_create_a_relationship = () =>
            map.Relationship.ShouldBeNull();

        It should_create_a_composite_element = () =>
            map.CompositeElement.ShouldNotBeNull();

        It should_set_the_map_composite_element_properties = () =>
            map.CompositeElement.Properties.Select(x => x.Name).ShouldContainOnly("Name", "Age");

        It should_set_the_map_composite_element_type_to_the_value_type = () =>
            map.CompositeElement.Class.ShouldEqual(new TypeReference(typeof(User)));
    }

    public class when_mapping_a_entity_key_with_an_entity_value_dictionary : DictionarySpec
    {
        // Map with entity + entity:
        // IDictionary<Post, User>
        // <map name="Commentors" table="PostCommentors">
        //   <key column="BlogId"/>
        //   <index-many-to-many column="PostId" class="Post"/>
        //   <many-to-many class="User" column="UserId"/>
        // </map>

        Because of = () =>
            mapping_for<Blog>(class_map =>
                class_map.HasMany(x => x.Commentors));

        It should_create_a_map = () =>
        {
            mapping.Collections.Count().ShouldEqual(1);
            map.ShouldNotBeNull();
        };

        It should_use_the_property_name_as_the_map_name = () =>
            map.Name.ShouldEqual("Commentors");

        It should_set_the_map_table_name_to_a_default_name = () =>
            map.TableName.ShouldEqual("BlogCommentors");
        // this table name makes more sense in a real life case (User entity
        // with an Attributes collection would have a table called UserAttributes)

        It should_create_a_key = () =>
            map.Key.ShouldNotBeNull();

        It should_set_the_map_key_to_the_primary_key_of_the_containing_class;
        // ignored for now, until we can sync keys properly

        It should_set_the_map_key_to_a_generated_default_based_on_the_entity_name = () =>
            map.Key.Columns.Select(x => x.Name).ShouldContainOnly("Blog_id");
        // obsoleted by the above when implemented

        It should_create_a_many_to_many_index = () =>
        {
            map.Index.As<IndexMapping>().ShouldNotBeNull();
            map.Index.As<IndexMapping>().IsManyToMany.ShouldBeTrue();
        };

        It should_set_the_map_index_type_to_the_key_type = () =>
            map.Index.Type.ShouldEqual(new TypeReference(typeof(Post)));

        It should_set_the_map_index_properties = () =>
            map.Index.As<IndexMapping>().Columns.Select(x => x.Name).ShouldContainOnly("Post_id");

        It should_create_a_many_to_many = () =>
        {
            map.Relationship.ShouldNotBeNull();
            map.Relationship.ShouldBeOfType<ManyToManyMapping>();
        };

        It should_set_the_map_many_to_many_column_to_a_default_foreign_key_value = () =>
            map.Relationship.As<ManyToManyMapping>().Columns.Select(x => x.Name).ShouldContainOnly("User_id");

        It should_set_the_map_many_to_many_type_to_the_value_entity_type = () =>
            map.Relationship.Class.ShouldEqual(new TypeReference(typeof(User)));
    }

    public class when_mapping_a_dictionary_with_a_custom_collection : DictionarySpec
    {
        // Map with entity + entity:
        // IDictionary<Post, User>
        // <map name="Commentors" table="PostCommentors">
        //   <key column="BlogId"/>
        //   <index-many-to-many column="PostId" class="Post"/>
        //   <many-to-many class="User" column="UserId"/>
        // </map>

        Because of = () =>
            mapping_for<Blog>(class_map =>
                class_map.HasMany(x => x.CommentorCustomCollection));

        It should_create_a_map = () =>
        {
            mapping.Collections.Count().ShouldEqual(1);
            map.ShouldNotBeNull();
        };

        It should_use_the_property_name_as_the_map_name = () =>
            map.Name.ShouldEqual("CommentorCustomCollection");

        It should_set_the_map_table_name_to_a_default_name = () =>
            map.TableName.ShouldEqual("BlogCommentorCustomCollection");
        // this table name makes more sense in a real life case (User entity
        // with an Attributes collection would have a table called UserAttributes)

        It should_create_a_key = () =>
            map.Key.ShouldNotBeNull();

        It should_set_the_map_key_to_the_primary_key_of_the_containing_class;
        // ignored for now, until we can sync keys properly

        It should_set_the_map_key_to_a_generated_default_based_on_the_entity_name = () =>
            map.Key.Columns.Select(x => x.Name).ShouldContainOnly("Blog_id");
        // obsoleted by the above when implemented

        It should_create_a_many_to_many_index = () =>
        {
            map.Index.As<IndexMapping>().ShouldNotBeNull();
            map.Index.As<IndexMapping>().IsManyToMany.ShouldBeTrue();
        };

        It should_set_the_map_index_type_to_the_key_type = () =>
            map.Index.Type.ShouldEqual(new TypeReference(typeof(Post)));

        It should_set_the_map_index_properties = () =>
            map.Index.As<IndexMapping>().Columns.Select(x => x.Name).ShouldContainOnly("Post_id");

        It should_create_a_many_to_many = () =>
        {
            map.Relationship.ShouldNotBeNull();
            map.Relationship.ShouldBeOfType<ManyToManyMapping>();
        };

        It should_set_the_map_many_to_many_column_to_a_default_foreign_key_value = () =>
            map.Relationship.As<ManyToManyMapping>().Columns.Select(x => x.Name).ShouldContainOnly("User_id");

        It should_set_the_map_many_to_many_type_to_the_value_entity_type = () =>
            map.Relationship.Class.ShouldEqual(new TypeReference(typeof(User)));
    }

    public abstract class DictionarySpec
    {
        protected static ClassMapping mapping;
        protected static MapMapping map;

        protected static void mapping_for<T>(Action<ClassMap<T>> mapping_definition)
            where T : EntityBase
        {
            var classMap = new ClassMap<T>();

            classMap.Id(x => x.Id);
            mapping_definition(classMap);

            var model = new FluentNHibernate.PersistenceModel();

            model.Add(classMap);

            mapping = model.BuildMappingFor<T>();
            map = mapping.Collections.SingleOrDefault() as MapMapping;
        }
    }
}
