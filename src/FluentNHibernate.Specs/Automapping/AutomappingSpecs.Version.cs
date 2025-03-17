﻿using FluentNHibernate.Automapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.Automapping.Fixtures;
using Machine.Specifications;
using FluentAssertions;

namespace FluentNHibernate.Specs.Automapping;

public class when_the_automapper_is_told_to_map_a_class_with_a_version
{
    Establish context = () =>
        mapper = AutoMap.Source(new StubTypeSource(typeof(VersionedEntity)));

    Because of = () =>
        mapping = mapper.BuildMappingFor<VersionedEntity>();

    It should_have_a_version = () =>
        mapping.Version.Should().NotBeNull();

    It should_have_picked_the_right_property_to_be_the_version = () =>
        mapping.Version.Name.Should().Be("Timestamp");

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
        mapping.Version.Should().NotBeNull();

    It should_have_picked_the_right_property_to_be_the_version = () =>
        mapping.Version.Name.Should().Be("AnUnobviousVersion");

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
