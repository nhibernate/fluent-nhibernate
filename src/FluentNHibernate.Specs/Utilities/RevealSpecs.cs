using System;
using System.Linq.Expressions;
using FluentNHibernate.Specs.Utilities.Fixtures;
using FluentNHibernate.Utils;
using FluentNHibernate.Utils.Reflection;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Utilities
{
    public class when_reveal_is_told_to_get_a_private_field
    {
        Because of = () =>
            expression = Reveal.Member<Target>("privateField");

        It should_return_an_expression_for_the_private_field = () =>
            expression.ShouldNotBeNull();

        It should_create_an_expression_that_s_convertable_to_a_member = () =>
            expression.ToMember().Name.ShouldEqual("privateField");

        static Expression<Func<Target, object>> expression;
    }

    public class when_reveal_is_told_to_get_a_protected_field
    {
        Because of = () =>
            expression = Reveal.Member<Target>("protectedField");

        It should_return_an_expression_for_the_protected_field = () =>
            expression.ShouldNotBeNull();

        It should_create_an_expression_that_s_convertable_to_a_member = () =>
            expression.ToMember().Name.ShouldEqual("protectedField");

        static Expression<Func<Target, object>> expression;
    }

    public class when_reveal_is_told_to_get_a_public_field
    {
        Because of = () =>
            expression = Reveal.Member<Target>("publicField");

        It should_return_an_expression_for_the_public_field = () =>
            expression.ShouldNotBeNull();

        It should_create_an_expression_that_s_convertable_to_a_member = () =>
            expression.ToMember().Name.ShouldEqual("publicField");

        static Expression<Func<Target, object>> expression;
    }

    public class when_reveal_is_told_to_get_a_private_property
    {
        Because of = () =>
            expression = Reveal.Member<Target>("PrivateProperty");

        It should_return_an_expression_for_the_private_property = () =>
            expression.ShouldNotBeNull();

        It should_create_an_expression_that_s_convertable_to_a_member = () =>
            ReflectionHelper.GetMember(expression).Name.ShouldEqual("PrivateProperty");

        static Expression<Func<Target, object>> expression;
    }

    public class when_reveal_is_told_to_get_a_protected_property
    {
        Because of = () =>
            expression = Reveal.Member<Target>("ProtectedProperty");

        It should_return_an_expression_for_the_protected_property = () =>
            expression.ShouldNotBeNull();

        It should_create_an_expression_that_s_convertable_to_a_member = () =>
            ReflectionHelper.GetMember(expression).Name.ShouldEqual("ProtectedProperty");

        static Expression<Func<Target, object>> expression;
    }

    public class when_reveal_is_told_to_get_a_public_property
    {
        Because of = () =>
            expression = Reveal.Member<Target>("PublicProperty");

        It should_return_an_expression_for_the_public_property = () =>
            expression.ShouldNotBeNull();

        It should_create_an_expression_that_s_convertable_to_a_member = () =>
            ReflectionHelper.GetMember(expression).Name.ShouldEqual("PublicProperty");

        static Expression<Func<Target, object>> expression;
    }

    public class when_reveal_is_told_to_get_an_int_property
    {
        Because of = () =>
            expression = Reveal.Member<Target>("IntProperty");

        It should_return_an_expression_for_the_public_property = () =>
            expression.ShouldNotBeNull();

        It should_create_an_expression_that_s_convertable_to_a_member = () =>
            ReflectionHelper.GetMember(expression).Name.ShouldEqual("IntProperty");

        static Expression<Func<Target, object>> expression;
    }

    public class when_reveal_is_told_to_get_property_from_a_super_class
    {
        Because of = () =>
            expression = Reveal.Member<Target>("SuperProperty");

        It should_return_an_expression_for_the_public_property = () =>
            expression.ShouldNotBeNull();

        It should_create_an_expression_that_s_convertable_to_a_member = () =>
            ReflectionHelper.GetMember(expression).Name.ShouldEqual("SuperProperty");

        static Expression<Func<Target, object>> expression;
    }

    public class when_reveal_is_told_to_get_a_property_that_doesnt_exist
    {
        Because of = () =>
            ex = Catch.Exception(() => Reveal.Member<Target>("UnknownProperty"));

        It should_throw_an_unknown_property_exception = () =>
        {
            ex.ShouldNotBeNull();
            ex.ShouldBeOfType<UnknownPropertyException>();
        };

        It should_throw_an_exception_with_the_correct_message = () =>
            ex.Message.ShouldEqual("Could not find property 'UnknownProperty' on '" + typeof(Target).FullName + "'");

        It should_throw_an_exception_with_it_s_property_set_to_the_expected_name = () =>
            ex.As<UnknownPropertyException>().Property.ShouldEqual("UnknownProperty");

        It should_throw_an_exception_with_it_s_type_set_to_the_specified_type = () =>
            ex.As<UnknownPropertyException>().Type.ShouldEqual(typeof(Target));

        static Exception ex;
    }
}