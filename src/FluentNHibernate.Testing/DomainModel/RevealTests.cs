using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Testing.DomainModel.Mapping;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel
{
    [TestFixture]
    public class RevealTests
    {
        [Test]
        public void Should_return_expression_for_private_property()
        {
            var expression = Reveal.Property<StringTarget>("PrivateProperty");

            Assert.That(expression, Is.Not.Null);
            Assert.That(ReflectionHelper.GetProperty(expression).Name, Is.EqualTo("PrivateProperty"));
        }

        [Test]
        public void Should_return_expression_for_protected_property()
        {
            var expression = Reveal.Property<StringTarget>("ProtectedProperty");

            Assert.That(expression, Is.Not.Null);
            Assert.That(ReflectionHelper.GetProperty(expression).Name, Is.EqualTo("ProtectedProperty"));
        }

        [Test]
        public void Should_return_expression_for_public_property()
        {
            var expression = Reveal.Property<StringTarget>("PublicProperty");

            Assert.That(expression, Is.Not.Null);
            Assert.That(ReflectionHelper.GetProperty(expression).Name, Is.EqualTo("PublicProperty"));
        }

        [Test]
        public void Should_throw_on_invalid_name()
        {
            var ex = Assert.Throws<UnknownPropertyException>(() => Reveal.Property<StringTarget>("UnknownProperty"),
                "Could not find property 'UnknownProperty' on '" + typeof(StringTarget).FullName + "'");

            ex.Property.ShouldEqual("UnknownProperty");
            ex.Type.ShouldEqual(typeof(StringTarget));
        }

        [Test]
        public void Can_map_using_string_name_on_private_property()
        {
            new MappingTester<StringTarget>()
                .ForMapping(map => map.Map(Reveal.Property<StringTarget>("PrivateProperty")))
                .Element("class/property").HasAttribute("name", "PrivateProperty");
        }

        [Test]
        public void Can_map_using_string_name_on_protected_property()
        {
            new MappingTester<StringTarget>()
                .ForMapping(map => map.Map(Reveal.Property<StringTarget>("ProtectedProperty")))
                .Element("class/property").HasAttribute("name", "ProtectedProperty");
        }

        [Test]
        public void Can_use_hasmany_using_string_name_on_private_property()
        {
            new MappingTester<StringTarget>()
                .ForMapping(map => map.HasMany<ExampleClass>(Reveal.Property<StringTarget>("PrivateCollection")))
                .Element("class/bag").HasAttribute("name", "PrivateCollection");
        }

        [Test]
        public void Can_use_hasone_using_string_name_on_private_property()
        {
            new MappingTester<StringTarget>()
                .ForMapping(map => map.HasOne(Reveal.Property<StringTarget, ExampleClass>("PrivateObject")))
                .Element("class/one-to-one").HasAttribute("name", "PrivateObject");
        }

        [Test]
        public void Can_use_dynamiccomponent_using_string_name_on_private_property()
        {
            new MappingTester<StringTarget>()
                .ForMapping(map => map.DynamicComponent(Reveal.Property<StringTarget, IDictionary>("PrivateDictionary"), x => { }))
                .Element("class/dynamic-component").HasAttribute("name", "PrivateDictionary");
        }

        [Test]
        public void Can_use_manytomany_using_string_name_on_private_property()
        {
            new MappingTester<StringTarget>()
                .ForMapping(map => map.HasManyToMany<ExampleClass>(Reveal.Property<StringTarget>("PrivateObject")))
                .Element("class/bag").HasAttribute("name", "PrivateObject");
        }

        [Test]
        public void Can_use_id_using_string_name_on_private_property()
        {
            new MappingTester<StringTarget>()
                .ForMapping(map => map.Id(Reveal.Property<StringTarget>("PrivateObject")))
                .Element("class/id").HasAttribute("name", "PrivateObject");
        }

        [Test]
        public void Can_use_references_using_string_name_on_private_property()
        {
            new MappingTester<StringTarget>()
                .ForMapping(map => map.References(Reveal.Property<StringTarget>("PrivateObject")))
                .Element("class/many-to-one").HasAttribute("name", "PrivateObject");
        }

        [Test]
        public void Can_use_version_using_string_name_on_private_property()
        {
            new MappingTester<StringTarget>()
                .ForMapping(map => map.Version(Reveal.Property<StringTarget>("PrivateObject")))
                .Element("class/version").HasAttribute("name", "PrivateObject");
        }

        [Test]
        public void Can_reveal_an_int_property()
        {
            Reveal.Property<StringTarget>("IntProperty");
        }

        [Test]
        public void Can_reveal_a_Double_property()
        {
            Reveal.Property<StringTarget>("DoubleProperty");
        }
    }

    public class StringTarget
    {
        private Double DoubleProperty { get; set; }
        private int IntProperty { get; set; }
        private string PrivateProperty { get; set; }
        protected string ProtectedProperty { get; set; }
        protected string PublicProperty { get; set; }
        private IList<ExampleClass> PrivateCollection { get; set; }
        private IDictionary PrivateDictionary { get; set; }
        private ExampleClass PrivateObject { get; set; }
    }
}