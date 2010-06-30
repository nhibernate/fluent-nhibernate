using System;
using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class when_generating_the_output_for_a_resolved_component_reference : Specification
    {
        private ClassMap<Target> inline_component;
        private ClassMap<Target> reference_component;
        private ComponentMap<Component> external_component;
        private string inline_xml;
        private string referenced_xml;

        public override void establish_context()
        {
            inline_component = new ClassMap<Target>();
            inline_component.Component(x => x.ComponentProperty1, c => c.Map(x => x.Property));
            inline_component.Component(x => x.ComponentProperty2, c => c.Map(x => x.Property));

            external_component = new ComponentMap<Component>();
            external_component.Map(x => x.Property);

            reference_component = new ClassMap<Target>();
            reference_component.Component(x => x.ComponentProperty1);
            reference_component.Component(x => x.ComponentProperty2);
        }

        private string render_xml(Action<PersistenceModel> addMappings)
        {
            var model = new PersistenceModel();

            addMappings(model);

            var mappings = model.BuildMappings();
            var doc = new MappingXmlSerializer().Serialize(mappings.First());

            return doc.OuterXml;
        }

        public override void because()
        {
            inline_xml = render_xml(x => x.Add(inline_component));
            referenced_xml = render_xml(x =>
            {
                x.Add(reference_component);
                x.Add(external_component);
            });
        }

        [Test]
        public void should_be_rendered_the_same_as_an_inline_component()
        {
            referenced_xml.ShouldEqual(inline_xml);
            System.Diagnostics.Debug.WriteLine(referenced_xml);
        }

        private class Target
        {
            public Component ComponentProperty1 { get; set; }
            public Component ComponentProperty2 { get; set; }
        }

        private class Component
        {
            public string Property { get; set; }
        }
    }
}