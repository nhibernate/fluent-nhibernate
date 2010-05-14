using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Automapping;
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

    public class ParentChildSelfReferenceEntity
    {
        public int Id { get; set; }
        public ParentChildSelfReferenceEntity Parent { get; set; }
        public IList<ParentChildSelfReferenceEntity> Children { get; set; }
    }
}
