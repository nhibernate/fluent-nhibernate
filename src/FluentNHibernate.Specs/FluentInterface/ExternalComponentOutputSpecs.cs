﻿using System;
using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.Output;
using Machine.Specifications;

namespace FluentNHibernate.Specs.FluentInterface;

public class when_generating_the_output_for_a_resolved_component_reference
{
    Establish context = () =>
    {
        inline_component = new ClassMap<Target>();
        inline_component.Id(x => x.Id);
        inline_component.Component(x => x.ComponentProperty1, c => c.Map(x => x.Property));
        inline_component.Component(x => x.ComponentProperty2, c => c.Map(x => x.Property));

        external_component = new ComponentMap<Component>();
        external_component.Map(x => x.Property);

        reference_component = new ClassMap<Target>();
        reference_component.Id(x => x.Id);
        reference_component.Component(x => x.ComponentProperty1);
        reference_component.Component(x => x.ComponentProperty2);
    };

    Because of = () =>
    {
        inline_xml = render_xml(x => x.Add(inline_component));
        referenced_xml = render_xml(x =>
        {
            x.Add(reference_component);
            x.Add(external_component);
        });
    };

    It should_be_rendered_the_same_as_an_inline_component = () =>
        referenced_xml.ShouldEqual(inline_xml);


    static string render_xml(Action<FluentNHibernate.PersistenceModel> addMappings)
    {
        var model = new FluentNHibernate.PersistenceModel();

        addMappings(model);

        var mappings = model.BuildMappings();
        var doc = new MappingXmlSerializer().Serialize(mappings.First());

        return doc.OuterXml;
    }

    static ClassMap<Target> inline_component;
    static ClassMap<Target> reference_component;
    static ComponentMap<Component> external_component;
    static string inline_xml;
    static string referenced_xml;

    class Target
    {
        public int Id { get; set;}
        public Component ComponentProperty1 { get; set; }
        public Component ComponentProperty2 { get; set; }
    }

    class Component
    {
        public string Property { get; set; }
    }
}
