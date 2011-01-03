using System.Xml;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Specs.Automapping.Fixtures;
using FluentNHibernate.Specs.ExternalFixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Automapping
{
    public class when_the_automapper_maps_with_a_property_convention
    {
        Establish context = () =>
            mapper = AutoMap.Source(new StubTypeSource(typeof(Entity)))
                .Conventions.Add<XXAppenderPropertyConvention>();

        Because of = () =>
            xml = mapper.BuildMappingFor<Entity>().ToXml();

        It should_apply_the_convention_to_any_properties = () =>
            xml.Element("class/property[@name='One']/column").HasAttribute("name", "OneXX");

        static AutoPersistenceModel mapper;
        static XmlDocument xml;

        class XXAppenderPropertyConvention : IPropertyConvention
        {
            public void Apply(IPropertyInstance instance)
            {
                instance.Column(instance.Name + "XX");
            }
        }
    }

    public class when_the_automapper_maps_a_readonly_reference_with_an_access_convention
    {
        Establish context = () =>
            mapper = AutoMap.Source(new StubTypeSource(typeof(Entity)))
                .Conventions.Add<Convention>();

        Because of = () =>
            xml = mapper.BuildMappingFor<Entity>().ToXml();

        It should_apply_the_convention_to_any_properties = () =>
            xml.Element("class/many-to-one[@name='ReadOnlyChild']").HasAttribute("access", "nosetter.camelcase-underscore");

        static AutoPersistenceModel mapper;
        static XmlDocument xml;

        class Convention : IReferenceConvention
        {
            public void Apply(IManyToOneInstance instance)
            {
                instance.Access.ReadOnlyPropertyThroughCamelCaseField(CamelCasePrefix.Underscore);
            }
        }
    }
}
