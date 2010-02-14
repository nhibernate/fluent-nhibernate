using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Testing.DomainModel.Mapping;
using FluentNHibernate.Utils.Reflection;
using Machine.Specifications;
using NUnit.Framework;
using MCatch=Machine.Specifications.Catch;

namespace FluentNHibernate.Testing.DomainModel
{
    public class when_reveal_is_told_to_get_a_private_property
    {
        Because of = () =>
            expression = Reveal.Property<StringTarget>("PrivateProperty");

        It should_return_an_expression_for_the_private_property = () =>
            expression.ShouldNotBeNull();

        It should_create_an_expression_that_s_convertable_to_a_member = () =>
            ReflectionHelper.GetMember(expression).Name.ShouldEqual("PrivateProperty");

        static Expression<Func<StringTarget, object>> expression;
    }

    public class when_reveal_is_told_to_get_a_protected_property
    {
        Because of = () =>
            expression = Reveal.Property<StringTarget>("ProtectedProperty");

        It should_return_an_expression_for_the_protected_property = () =>
            expression.ShouldNotBeNull();

        It should_create_an_expression_that_s_convertable_to_a_member = () =>
            ReflectionHelper.GetMember(expression).Name.ShouldEqual("ProtectedProperty");

        static Expression<Func<StringTarget, object>> expression;
    }

    public class when_reveal_is_told_to_get_a_public_property
    {
        Because of = () =>
            expression = Reveal.Property<StringTarget>("PublicProperty");

        It should_return_an_expression_for_the_public_property = () =>
            expression.ShouldNotBeNull();

        It should_create_an_expression_that_s_convertable_to_a_member = () =>
            ReflectionHelper.GetMember(expression).Name.ShouldEqual("PublicProperty");

        static Expression<Func<StringTarget, object>> expression;
    }

    public class when_reveal_is_told_to_get_an_int_property
    {
        Because of = () =>
            expression = Reveal.Property<StringTarget>("IntProperty");

        It should_return_an_expression_for_the_public_property = () =>
            expression.ShouldNotBeNull();

        It should_create_an_expression_that_s_convertable_to_a_member = () =>
            ReflectionHelper.GetMember(expression).Name.ShouldEqual("IntProperty");

        static Expression<Func<StringTarget, object>> expression;
    }

    public class when_reveal_is_told_to_get_property_from_a_super_class
    {
        Because of = () =>
            expression = Reveal.Property<StringTarget>("SuperProperty");

        It should_return_an_expression_for_the_public_property = () =>
            expression.ShouldNotBeNull();

        It should_create_an_expression_that_s_convertable_to_a_member = () =>
            ReflectionHelper.GetMember(expression).Name.ShouldEqual("SuperProperty");

        static Expression<Func<StringTarget, object>> expression;
    }

    public class when_reveal_is_told_to_get_a_property_that_doesnt_exist
    {
        Because of = () =>
            ex = MCatch.Exception(() => Reveal.Property<StringTarget>("UnknownProperty"));

        It should_throw_an_unknown_property_exception = () =>
        {
            ex.ShouldNotBeNull();
            ex.ShouldBeOfType<UnknownPropertyException>();
        };

        It should_throw_an_exception_with_the_correct_message = () =>
            ex.Message.ShouldEqual("Could not find property 'UnknownProperty' on '" + typeof(StringTarget).FullName + "'");

        It should_throw_an_exception_with_it_s_property_set_to_the_expected_name = () =>
            ex.As<UnknownPropertyException>().Property.ShouldEqual("UnknownProperty");

        It should_throw_an_exception_with_it_s_type_set_to_the_specified_type = () =>
            ex.As<UnknownPropertyException>().Type.ShouldEqual(typeof(StringTarget));

        static Exception ex;
    }

    [TestFixture]
    public class RevealTests
    {
        [Test]
        public void Can_use_manytomany_using_string_name_on_private_property()
        {
            new MappingTester<StringTarget>()
                .ForMapping(map =>
                {
                    map.Id(x => x.Id);
                    map.HasManyToMany<ExampleClass>(Reveal.Property<StringTarget>("PrivateObject"));
                })
                .Element("class/bag").HasAttribute("name", "PrivateObject");
        }

        [Test]
        public void Can_use_id_using_string_name_on_private_property()
        {
            new MappingTester<StringTarget>()
                .ForMapping(map =>
                {
                    map.Id(x => x.Id);
                    map.Id(Reveal.Property<StringTarget>("PrivateObject"));
                })
                .Element("class/id").HasAttribute("name", "PrivateObject");
        }

        [Test]
        public void Can_use_references_using_string_name_on_private_property()
        {
            new MappingTester<StringTarget>()
                .ForMapping(map =>
                {
                    map.Id(x => x.Id);
                    map.References(Reveal.Property<StringTarget>("PrivateObject"));
                })
                .Element("class/many-to-one").HasAttribute("name", "PrivateObject");
        }
    }

    public class StringTarget : StringTargetParent
    {
        public int Id { get; set; }
        private Double DoubleProperty { get; set; }
        private int IntProperty { get; set; }
        private string PrivateProperty { get; set; }
        protected string ProtectedProperty { get; set; }
        protected string PublicProperty { get; set; }
        private IList<ExampleClass> PrivateCollection { get; set; }
        private IDictionary PrivateDictionary { get; set; }
        private ExampleClass PrivateObject { get; set; }
    }

    public abstract class StringTargetParent
    {
        private string SuperProperty { get; set; }
    }
}