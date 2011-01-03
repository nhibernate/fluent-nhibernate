using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.Automapping.Fixtures;
using FluentNHibernate.Specs.ExternalFixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Automapping
{
    public class when_the_automapper_is_told_to_map_a_single_type
    {
        Because of = () =>
            xml = AutoMap.Source(new StubTypeSource(typeof(Entity)))
                    .BuildMappingFor<Entity>()
                    .ToXml();

        It should_only_return_one_class_in_the_xml = () =>
        {
            xml.Element("class").ShouldExist();
            xml.Element("class[2]").ShouldNotExist();
        };

        It should_map_the_id = () =>
            xml.Element("class/id").ShouldExist();

        It should_map_properties = () =>
            xml.Element("class/property[@name='One']").ShouldExist();

        It should_map_enum_properties = () =>
            xml.Element("class/property[@name='Enum']").ShouldExist();

        It should_map_references = () =>
            xml.Element("class/many-to-one[@name='Parent']").ShouldExist();

        It should_map_collections = () =>
            xml.Element("class/bag[@name='Children']").ShouldExist();

        static XmlDocument xml;
    }

    public class when_the_automapper_is_ran_to_completion
    {
        Establish context = () =>
            setup = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory)
                .Mappings(x => x.AutoMappings.Add(AutoMap.Source(new StubTypeSource(typeof(Entity)))));

        Because of = () =>
            ex = Catch.Exception(() => setup.BuildConfiguration());

        It should_generate_xml_that_is_accepted_by_the_nhibernate_schema_validation = () =>
            ex.ShouldBeNull();

        static FluentConfiguration setup;
        static Exception ex;
    }

    public class when_the_automapper_is_told_to_map_an_inheritance_hierarchy
    {
        Because of = () =>
            ex = Catch.Exception(() =>
                    AutoMap.Source(new StubTypeSource(typeof(A_Child), typeof(B_Parent)))
                        .BuildMappings());

        // this will fail with an exception if this is broken:
        // was failing because the child class was being mapped first
        // adding all properties from it and it's base class. Then the
        // base class was being mapped, duplicating those properties
        // that were already mapped in the child. Needed to change the
        // ordering so parents are always mapped before their children
        It should_map_the_top_most_class_first = () =>
            ex.ShouldBeNull();
        
        static Exception ex;
    }

    public class when_the_automapper_maps_an_inheritance_hierarchy_with_three_levels_and_the_middle_ignored
    {
        Establish context = () =>
            mapper = AutoMap.Source(new StubTypeSource(typeof(ChildChild), typeof(Parent), typeof(Child)))
                .IgnoreBase<Child>();

        Because of = () =>
            mappings = mapper.BuildMappings()
                .SelectMany(x => x.Classes);

        It should_map_the_parent = () =>
            mappings.Count().ShouldEqual(1);

        It should_map_the_child_child_as_a_subclass_of_parent = () =>
            mappings.Single()
                .Subclasses.Single().Type.ShouldEqual(typeof(ChildChild));

        static AutoPersistenceModel mapper;
        static IEnumerable<ClassMapping> mappings;
    }
}
