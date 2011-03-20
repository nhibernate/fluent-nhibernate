using System;
using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Specs.Automapping.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Automapping
{
	public class when_the_automapper_is_told_to_map_a_class_with_a_self_referencing_collection
	{
        Establish context = () =>
            mapper = AutoMap.Source(new StubTypeSource(typeof(SelfReferencingCollectionEntity)));

	    Because of = () =>
	        mapping = mapper.BuildMappingFor<SelfReferencingCollectionEntity>();

	    It should_have_one_collection = () =>
	        mapping.Collections.ShouldNotBeEmpty();

	    It should_have_it_s_own_type_for_the_collection_child_type = () =>
	        mapping.Collections.Single().ChildType.ShouldEqual(typeof(SelfReferencingCollectionEntity));

	    It should_have_the_property_name_for_the_collection_name = () =>
	        mapping.Collections.Single().Name.ShouldEqual("Children");

	    It should_have_a_one_to_many_collection = () =>
	        mapping.Collections.Single().Relationship.ShouldBeOfType<OneToManyMapping>();

        static AutoPersistenceModel mapper;
        static ClassMapping mapping;
    }

    public class when_the_automapper_is_told_to_map_a_class_with_a_self_reference
	{
        Establish context = () =>
            mapper = AutoMap.Source(new StubTypeSource(typeof(SelfReferenceEntity)));

        Because of = () =>
            mapping = mapper.BuildMappingFor<SelfReferenceEntity>();

        It should_have_one_reference = () =>
            mapping.References.ShouldNotBeEmpty();

        It should_have_it_s_own_type_for_the_reference_type = () =>
            mapping.References.Single().Class.GetUnderlyingSystemType().ShouldEqual(typeof(SelfReferenceEntity));

        It should_have_the_property_name_for_the_reference_name = () =>
            mapping.References.Single().Name.ShouldEqual("Parent");

        static AutoPersistenceModel mapper;
        static ClassMapping mapping;
	}

    public class when_the_automapper_is_told_to_map_a_class_with_a_self_reference_and_a_self_referencing_collection
    {
        Establish context = () =>
            mapper = AutoMap.Source(new StubTypeSource(typeof(ParentChildSelfReferenceEntity)));

        Because of = () =>
            mapping = mapper.BuildMappingFor<ParentChildSelfReferenceEntity>();

        It should_have_one_collection = () =>
            mapping.Collections.ShouldNotBeEmpty();

        It should_have_it_s_own_type_for_the_collection_child_type = () =>
            mapping.Collections.Single().ChildType.ShouldEqual(typeof(ParentChildSelfReferenceEntity));

        It should_have_the_property_name_for_the_collection_name = () =>
            mapping.Collections.Single().Name.ShouldEqual("Children");

        It should_have_a_one_to_many_collection = () =>
            mapping.Collections.Single().Relationship.ShouldBeOfType<OneToManyMapping>();

        It should_have_the_correct_collection_key_column = () =>
            mapping.Collections.Single().Key.Columns.Single().Name.ShouldEqual("Parent_id");

        It should_have_one_reference = () =>
            mapping.References.ShouldNotBeEmpty();

        It should_have_it_s_own_type_for_the_reference_type = () =>
            mapping.References.Single().Class.GetUnderlyingSystemType().ShouldEqual(typeof(ParentChildSelfReferenceEntity));

        It should_have_the_property_name_for_the_reference_name = () =>
            mapping.References.Single().Name.ShouldEqual("Parent");

        It should_have_the_correct_reference_key_column = () =>
            mapping.References.Single().Columns.Single().Name.ShouldEqual("Parent_id");

        static AutoPersistenceModel mapper;
        static ClassMapping mapping;
    }

    public class when_the_automapper_is_told_to_map_an_entity_with_a_collection_exposed_as_a_read_only_enumerable_with_a_backing_field
    {
        Establish context = () =>
            mapper = AutoMap.Source(new StubTypeSource(typeof(ReadOnlyEnumerableEntity)));

        Because of = () =>
            mapping = mapper.BuildMappingFor<ReadOnlyEnumerableEntity>();

        It should_map_the_read_only_property_collection = () =>
            mapping.Collections.Select(x => x.Name).ShouldContain("BackingFieldCollection");

        It should_map_the_property_using_property_through_field_access_strategy = () =>
            mapping.Collections.First(x => x.Name == "BackingFieldCollection").Access.ShouldEqual("nosetter.camelcase");
        
        static AutoPersistenceModel mapper;
        static ClassMapping mapping;
    }

    public class when_the_automapper_is_told_to_map_an_entity_with_a_collection_exposed_as_a_read_only_enumerable_with_a_backing_field_with_a_convention_which_changes_the_access_strategy
    {
        Establish context = () =>
            mapper = AutoMap.Source(new StubTypeSource(typeof(ReadOnlyEnumerableEntity)))
                .Conventions.Add<Convention>();

        Because of = () =>
            mapping = mapper.BuildMappingFor<ReadOnlyEnumerableEntity>();

        It should_map_the_read_only_property_collection = () =>
            mapping.Collections.Select(x => x.Name).ShouldContain("BackingFieldCollection");

        It should_map_the_property_using_property_through_field_access_strategy = () =>
            mapping.Collections.First(x => x.Name == "BackingFieldCollection").Access.ShouldEqual("property");

        static AutoPersistenceModel mapper;
        static ClassMapping mapping;

        class Convention : IHasManyConvention
        {
            public void Apply(IOneToManyCollectionInstance instance)
            {
                instance.Access.Property();
            }
        }
    }

    public class when_the_automapper_is_told_to_map_an_entity_with_a_collection_exposed_as_a_read_only_enumerable_with_a_backing_field_with_an_overridden_configuration
    {
        Establish context = () =>
            mapper = AutoMap.Source(new StubTypeSource(typeof(ReadOnlyEnumerableEntity)), new TestConfiguration());

        Because of = () =>
            mapping = mapper.BuildMappingFor<ReadOnlyEnumerableEntity>();

        It should_map_the_property_using_the_access_strategy_from_the_configuration = () =>
            mapping.Collections.First(x => x.Name == "BackingFieldCollection").Access.ShouldEqual("noop");

        static AutoPersistenceModel mapper;
        static ClassMapping mapping;

        class TestConfiguration : DefaultAutomappingConfiguration
        {
            public override Access GetAccessStrategyForReadOnlyProperty(Member member)
            {
                return Access.NoOp;
            }
        }
    }

    public class when_the_automapper_is_told_to_map_an_entity_with_a_collection_exposed_as_a_read_only_enumerable_autoproperty
    {
        Establish context = () =>
            mapper = AutoMap.Source(new StubTypeSource(typeof(ReadOnlyEnumerableEntity)));

        Because of = () =>
            mapping = mapper.BuildMappingFor<ReadOnlyEnumerableEntity>();

        It should_map_the_read_only_property_collection = () =>
            mapping.Collections.Select(x => x.Name).ShouldContain("AutoPropertyCollection");

        It should_map_the_property_using_backfield_access_strategy = () =>
            mapping.Collections.First(x => x.Name == "AutoPropertyCollection").Access.ShouldEqual("backfield");

        static AutoPersistenceModel mapper;
        static ClassMapping mapping;
    }

    public class when_the_automapper_is_told_to_map_an_entity_with_a_collection_exposed_as_a_read_only_enumerable_autoproperty_with_an_overridden_configuration
    {
        Establish context = () =>
            mapper = AutoMap.Source(new StubTypeSource(typeof(ReadOnlyEnumerableEntity)), new TestConfiguration());

        Because of = () =>
            mapping = mapper.BuildMappingFor<ReadOnlyEnumerableEntity>();

        It should_map_the_property_using_the_access_strategy_from_the_configuration = () =>
            mapping.Collections.First(x => x.Name == "AutoPropertyCollection").Access.ShouldEqual("noop");

        static AutoPersistenceModel mapper;
        static ClassMapping mapping;

        class TestConfiguration : DefaultAutomappingConfiguration
        {
            public override Access GetAccessStrategyForReadOnlyProperty(Member member)
            {
                return Access.NoOp;
            }
        }
    }

    public class when_automapping_an_entity_with_dictionary_properties
    {
        Establish context = () =>
            mapper = AutoMap.Source(new StubTypeSource(typeof(DictionaryEntity)));

        Because of = () =>
            mapping = mapper.BuildMappingFor<DictionaryEntity>();

        It should_not_map_the_non_generic_dictionary = () =>
            mapping.Collections.Select(x => x.Name).ShouldNotContain("Dictionary");

        It should_not_map_the_generic_dictionary = () =>
            mapping.Collections.Select(x => x.Name).ShouldNotContain("GenericDictionary");
        
        static AutoPersistenceModel mapper;
        static ClassMapping mapping;
    }
}
