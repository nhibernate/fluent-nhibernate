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

        [Test]
        public void DefaultLazyLoad_should_be_true_by_default_for_compatibility_with_NHibernate()
        {
            new Conventions().DefaultLazyLoad.ShouldBeTrue();
        }

        [Test]
        public void DynamicUpdate_should_be_unset()
        {
            new Conventions().DynamicUpdate(typeof(object)).ShouldBeNull();
        }

        [Test]
        public void DynamicInsert_should_be_unset()
        {
            new Conventions().DynamicInsert(typeof(object)).ShouldBeNull();
        }

        [Test]
        public void OptimisticLock_should_be_unset()
        {
            var attributes = new Cache<string, string>();
            var optimisticLock = new OptimisticLock(attributes);

            new Conventions().OptimisticLock(typeof(object), optimisticLock);

            attributes.Count.ShouldEqual(0);
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
