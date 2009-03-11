using System;
using System.Reflection;
using FluentNHibernate.Conventions;
using FluentNHibernate.Utils;
using NUnit.Framework;
using Rhino.Mocks;
using FluentNHibernate;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class ConventionsTester
    {
        [Test]
        public void add_property_convention_for_type_of_attribute()
        {
            new MappingTester<Site>()
                .WithConventions(conventions =>
                    conventions.Finder.AddAssembly(typeof(MyAttributeConvention).Assembly))
                .ForMapping(m => m.Map(x => x.Name))
                .Element("class/property[@name='Name']")
                    .HasAttribute("My", "true");
        }

        [Test]
        public void DefaultLazyLoad_should_be_true_by_default_for_compatibility_with_NHibernate()
        {
            new ConventionOverrides().DefaultLazyLoad.ShouldBeTrue();
        }
    }

    public class Invoice{}

    public class Site
    {
        [My]
        public string Name { get; set; }

        public string LastName { get; set; }

        public Address Primary { get; set; }
        public Address Secondary { get; set; }
    }
    public class Address{}

    public class MyAttribute : Attribute
    {
        
    }

    public class MyAttributeConvention : AttributePropertyConvention<MyAttribute>
    {
        protected override void Apply(MyAttribute attribute, IProperty target, ConventionOverrides overrides)
        {
            target.SetAttribute("My", "true");
        }
    }
}
