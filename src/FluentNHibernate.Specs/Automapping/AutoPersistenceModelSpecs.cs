using System;
using System.Xml;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Specs.Automapping.Fixtures;
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
}
