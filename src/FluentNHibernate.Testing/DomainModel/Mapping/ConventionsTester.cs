using System;
using System.Reflection;
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
        [Test, Ignore]
        public void add_property_convention_for_type_of_attribute()
        {
            MockRepository mocks = new MockRepository();
            IProperty property = mocks.DynamicMock<IProperty>();


            using (mocks.Record())
            {
                PropertyInfo propertyInfo = ReflectionHelper.GetProperty<Site>(s => s.Name);
                Expect.Call(property.Property).Return(propertyInfo).Repeat.Any();
                Expect.Call(property.PropertyType).Return(typeof(string)).Repeat.Any();

                property.SetAttributeOnColumnElement("My", "true");
            }

            //using (mocks.Playback())
            //{
            //    var conventions = new ConventionOverrides();
            //    conventions.ForAttribute<MyAttribute>((a, p) => p.SetAttributeOnColumnElement("My", "true"));
            //    conventions.AlterMap(property);
            //}
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
}
