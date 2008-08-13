using System;
using System.Reflection;
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
        public void Set_the_convention_for_foreign_keys()
        {
            new Conventions().GetForeignKeyNameOfParent(typeof (Invoice)).ShouldEqual("Invoice_id");
        }

        [Test]
        public void Get_the_convention_for_a_relationship()
        {
            new Conventions().GetForeignKeyName(typeof (Site).GetProperty("Primary")).ShouldEqual("Primary_id");
        }

        [Test]
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

            using (mocks.Playback())
            {
                var conventions = new Conventions();
                conventions.ForAttribute<MyAttribute>((a, p) => p.SetAttributeOnColumnElement("My", "true"));
                conventions.AlterMap(property);
            }
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
