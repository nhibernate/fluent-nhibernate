using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class ConventionsTester
    {
        [Test]
        public void AddPropertyConventionForTypeOfAttribute()
        {
            new MappingTester<Site>()
                .Conventions(conventions => conventions.Add<MyAttributeConvention>())
                .ForMapping(m => m.Map(x => x.Name))
                .Element("class/property[@name='Name']")
                    .HasAttribute("access", "field");
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
        protected override void Apply(MyAttribute attribute, IPropertyInstance instance)
        {
            instance.Access.Field();
        }
    }
}
