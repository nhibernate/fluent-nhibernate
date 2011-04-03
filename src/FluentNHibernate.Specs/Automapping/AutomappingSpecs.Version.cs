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
	public class when_the_automapper_is_told_to_map_a_class_with_a_version
	{
        Establish context = () =>
            mapper = AutoMap.Source(new StubTypeSource(typeof(VersionedEntity)));

	    Because of = () =>
	        mapping = mapper.BuildMappingFor<VersionedEntity>();

	    It should_have_a_version = () =>
	        mapping.Version.ShouldNotBeNull();

	    It should_have_picked_the_right_property_to_be_the_version = () =>
	        mapping.Version.Name.ShouldEqual("Timestamp");

        static AutoPersistenceModel mapper;
        static ClassMapping mapping;
    }

    public class when_the_automapper_is_told_to_map_a_class_with_a_custom_version_defined_in_config
    {
        Establish context = () =>
            mapper = AutoMap.Source(new StubTypeSource(typeof(VersionedEntity)), new TestConfig());

        Because of = () =>
            mapping = mapper.BuildMappingFor<VersionedEntity>();

        It should_have_a_version = () =>
            mapping.Version.ShouldNotBeNull();

        It should_have_picked_the_right_property_to_be_the_version = () =>
            mapping.Version.Name.ShouldEqual("AnUnobviousVersion");

        static AutoPersistenceModel mapper;
        static ClassMapping mapping;

        class TestConfig : DefaultAutomappingConfiguration
        {
            public override bool IsVersion(Member member)
            {
                return member.Name == "AnUnobviousVersion";
            }
        }
    }
}
