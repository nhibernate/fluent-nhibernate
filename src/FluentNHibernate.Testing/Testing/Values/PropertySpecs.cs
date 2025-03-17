using System;
using System.Collections;
using System.Linq.Expressions;
using FakeItEasy;
using FluentNHibernate.Testing.Values;
using FluentNHibernate.Utils;
using FluentNHibernate.Utils.Reflection;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Testing.Values;

public abstract class With_property_entity : Specification
{
    Accessor property;
    protected PropertyEntity target;
    protected Property<PropertyEntity, string> sut;

    public override void establish_context()
    {
        property = ReflectionHelper.GetAccessor(GetPropertyExpression());
        target = new PropertyEntity();

        sut = new Property<PropertyEntity, string>(property, "expected");
    }

    protected abstract Expression<Func<PropertyEntity, string>> GetPropertyExpression();
}

public abstract class When_a_property_is_set_successfully : With_property_entity
{
    public override void because()
    {
        sut.SetValue(target);
    }

    [Test]
    public abstract void should_set_the_property_value();

    [Test]
    public void should_succeed()
    {
        thrown_exception.ShouldBeNull();
    }
}

[TestFixture]
public class When_a_property_with_a_public_setter_is_set : When_a_property_is_set_successfully
{
    protected override Expression<Func<PropertyEntity, string>> GetPropertyExpression()
    {
        return x => x.GetterAndSetter;
    }

    [Test]
    public override void should_set_the_property_value()
    {
        target.GetterAndSetter.ShouldEqual("expected");
    }
}

[TestFixture]
public class When_a_property_with_a_private_setter_is_set : When_a_property_is_set_successfully
{
    protected override Expression<Func<PropertyEntity, string>> GetPropertyExpression()
    {
        return x => x.GetterAndPrivateSetter;
    }

    [Test]
    public override void should_set_the_property_value()
    {
        target.GetterAndPrivateSetter.ShouldEqual("expected");
    }
}

[TestFixture]
public class When_a_property_with_a_backing_field_is_set : With_property_entity
{
    protected override Expression<Func<PropertyEntity, string>> GetPropertyExpression()
    {
        return x => x.BackingField;
    }

    public override void because()
    {
        sut.SetValue(target);
    }

    [Test]
    public void should_fail()
    {
        thrown_exception.ShouldBeOfType<ApplicationException>();
    }

    [Test]
    public void should_tell_which_property_failed_to_be_set()
    {
        var exception = (ApplicationException)thrown_exception;
        exception.Message.ShouldEqual("Error while trying to set property BackingField");
    }
}

[TestFixture]
public class When_a_property_is_set_with_a_custom_setter : When_a_property_is_set_successfully
{
    protected override Expression<Func<PropertyEntity, string>> GetPropertyExpression()
    {
        return x => x.BackingField;
    }

    public override void establish_context()
    {
        base.establish_context();

        sut.ValueSetter = (entity, propertyInfo, value) => entity.SetBackingField(value);
    }

    [Test]
    public override void should_set_the_property_value()
    {
        target.BackingField.ShouldEqual("expected");
    }
}

[TestFixture]
public class When_a_property_is_set_with_a_custom_setter_that_fails : With_property_entity
{
    protected override Expression<Func<PropertyEntity, string>> GetPropertyExpression()
    {
        return x => x.BackingField;
    }

    public override void establish_context()
    {
        base.establish_context();

        sut.ValueSetter = (entity, propertyInfo, value) => { throw new Exception(); };
    }

    public override void because()
    {
        sut.SetValue(target);
    }

    [Test]
    public void should_fail()
    {
        thrown_exception.ShouldBeOfType<ApplicationException>();
    }

    [Test]
    public void should_tell_which_property_failed_to_be_set()
    {
        var exception = (ApplicationException)thrown_exception;
        exception.Message.ShouldEqual("Error while trying to set property BackingField");
    }
}

public abstract class With_initialized_property : Specification
{
    Accessor property;
    protected PropertyEntity target;
    protected Property<PropertyEntity, string> sut;

    public override void establish_context()
    {
        property = ReflectionHelper.GetAccessor((Expression<Func<PropertyEntity, string>>)(x => x.GetterAndSetter));
        target = new PropertyEntity();

        sut = new Property<PropertyEntity, string>(property, "expected");
    }
}

[TestFixture]
public class When_the_checked_property_is_equal_to_the_expected_value : With_initialized_property
{
    public override void establish_context()
    {
        base.establish_context();
        target.GetterAndSetter = "expected";
    }

    public override void because()
    {
        sut.CheckValue(target);
    }

    [Test]
    public void should_succeed()
    {
        thrown_exception.ShouldBeNull();
    }
}

[TestFixture]
public class When_the_checked_property_is_equal_to_the_expected_value_with_a_custom_equality_comparer : When_the_checked_property_is_equal_to_the_expected_value
{
    public override void establish_context()
    {
        base.establish_context();
        target.GetterAndSetter = "expected";

        sut.EntityEqualityComparer = A.Fake<IEqualityComparer>();
        A.CallTo(() => sut.EntityEqualityComparer.Equals("expected", "expected")).Returns(true);
    }

    [Test]
    public void should_perform_the_check_with_the_custom_equality_comparer()
    {
        A.CallTo(() => sut.EntityEqualityComparer.Equals("expected", "expected"))
            .MustHaveHappened();
    }
}

[TestFixture]
public class When_the_checked_property_is_not_equal_to_the_expected_value : With_initialized_property
{
    public override void establish_context()
    {
        base.establish_context();
        target.GetterAndSetter = "actual";
    }

    public override void because()
    {
        sut.CheckValue(target);
    }

    [Test]
    public void should_fail()
    {
        thrown_exception.ShouldBeOfType<ApplicationException>();
    }

    [Test]
    public void should_tell_which_property_failed_the_check()
    {
        var exception = (ApplicationException)thrown_exception;

        exception.Message.ShouldEqual("For property 'GetterAndSetter' of type 'System.String' expected 'expected' but got 'actual'");
    }
}

[TestFixture]
public class When_the_checked_property_is_not_equal_to_the_expected_value_with_a_custom_equality_comparer : When_the_checked_property_is_not_equal_to_the_expected_value
{
    public override void establish_context()
    {
        base.establish_context();
        sut.EntityEqualityComparer = A.Fake<IEqualityComparer>();
        A.CallTo(() => sut.EntityEqualityComparer.Equals("expected", "actual"))
            .Returns(false);
    }

    [Test]
    public void should_perform_the_check_with_the_custom_equality_comparer()
    {
        A.CallTo(() => sut.EntityEqualityComparer.Equals("expected", "actual"))
            .MustHaveHappened();
    }
}

[TestFixture]
public class When_a_property_is_checked_with_a_custom_equality_comparer_that_fails : With_initialized_property
{
    InvalidOperationException exception;

    public override void establish_context()
    {
        base.establish_context();

        exception = new InvalidOperationException();

        sut.EntityEqualityComparer = A.Fake<IEqualityComparer>();
        A.CallTo(() => sut.EntityEqualityComparer.Equals(null, null))
            .WithAnyArguments()
            .Throws(exception);
    }

    public override void because()
    {
        sut.CheckValue(target);
    }

    [Test]
    public void should_fail_with_the_exception_from_the_equality_comparer()
    {
        thrown_exception.ShouldBeTheSameAs(exception);
    }
}
