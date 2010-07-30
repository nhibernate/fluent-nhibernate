﻿using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.Automapping.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Automapping
{
    public class when_using_an_automapping_override_to_create_a_join
    {
        Establish context = () =>
            model = AutoMap.Source(new StubTypeSource(typeof(Entity)))
                .Override<Entity>(map =>
                    map.Join("join_table", m => m.Map(x => x.One)));

        Because of = () =>
            mapping = model.BuildMappingFor<Entity>();

        It should_create_the_join_mapping = () =>
            mapping.Joins.ShouldNotBeEmpty();

        It should_have_a_property_in_the_join = () =>
            mapping.Joins.Single().Properties.Select(x => x.Name).ShouldContain("One");

        It should_exclude_the_join_mapped_property_from_the_main_automapping = () =>
            mapping.Properties.Select(x => x.Name).ShouldNotContain("One");
        
        static AutoPersistenceModel model;
        static ClassMapping mapping;
    }

    public class when_using_an_automapping_override_to_specify_a_discriminator
    {
        Establish context = () =>
            model = AutoMap.Source(new StubTypeSource(typeof(Parent), typeof(Child)))
                .Override<Parent>(map =>
                    map.DiscriminateSubClassesOnColumn("discriminator"));

        Because of = () =>
            mapping = model.BuildMappingFor<Parent>();

        It should_map_the_discriminator = () =>
            mapping.Discriminator.ShouldNotBeNull();

        It should_map_subclasses_as_subclass_instead_of_joined_subclass = () =>
        {
            mapping.Subclasses.Count().ShouldEqual(1);
            mapping.Subclasses.ShouldEachConformTo(x => x.SubclassType == SubclassType.Subclass);
        };
        
        static AutoPersistenceModel model;
        static ClassMapping mapping;
    }
}
