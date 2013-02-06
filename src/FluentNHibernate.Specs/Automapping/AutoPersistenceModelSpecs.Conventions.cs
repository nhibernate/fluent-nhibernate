using System;
using System.Data;
using System.Xml;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Specs.Automapping.Fixtures;
using FluentNHibernate.Specs.ExternalFixtures;
using Machine.Specifications;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

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

    public class when_the_automapper_maps_with_a_usertype_convention_to_property_of_value_type
    {
        Establish context = () =>
            mapper = AutoMap.Source(new StubTypeSource(typeof(TypeWithNullableUT), typeof(TypeWithValueUT)))
            .Conventions.Add<ValueTypeConvention>();

        Because of = () =>
        {
            nullableMapping = mapper.BuildMappingFor<TypeWithNullableUT>().ToXml();
            notNullableMapping = mapper.BuildMappingFor<TypeWithValueUT>().ToXml();
        };
            
        It shold_apply_convention_to_properties_of_corresponding_nullable_type = () =>
            nullableMapping.Element("class/property[@name='Simple']/column").HasAttribute("name", "arbitraryName");

        It shold_apply_convention_to_properties_of_corresponding_non_nullable_value_type = () =>
            notNullableMapping.Element("class/property[@name='Definite']/column").HasAttribute("name", "arbitraryName");

        static XmlDocument nullableMapping;
        static XmlDocument notNullableMapping;
        static AutoPersistenceModel mapper;
    }

    public class when_the_automapper_maps_with_a_usertype_convention_to_property_of_reference_type
    {
        Establish context = () =>
            mapper = AutoMap.Source(new StubTypeSource(typeof(NotNullableUT)))
                .Conventions.Add<CustomTypeConvention>();

        Because of = () =>
            xml = mapper.BuildMappingFor<NotNullableUT>().ToXml();

        It shold_apply_convention_to_property = () =>
            xml.Element("class/property[@name='Complex']/column").HasAttribute("name", "someOtherName");

        static XmlDocument xml;
        static AutoPersistenceModel mapper;


    }
}
