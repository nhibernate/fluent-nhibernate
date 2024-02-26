using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NUnit.Framework;

namespace FluentNHibernate.Testing;

public static class SpecificationExtensions
{
    public static void ShouldBeFalse(this bool condition)
    {
        Assert.That(condition, Is.False);
    }

    public static void ShouldBeTrue(this bool condition)
    {
        Assert.That(condition);
    }

    public static object ShouldEqual(this object actual, object expected)
    {
        Assert.That(actual, Is.EqualTo(expected));
        return expected;
    }

    public static object ShouldNotEqual(this object actual, object expected)
    {
        Assert.That(actual, Is.Not.EqualTo(expected));
        return expected;
    }

    public static void ShouldBeNull(this object anObject)
    {
        Assert.That(anObject, Is.Null);
    }

    public static T ShouldNotBeNull<T>(this T anObject)
    {
        Assert.That(anObject, Is.Not.Null);

        return anObject;
    }

    public static object ShouldBeTheSameAs(this object actual, object expected)
    {
        Assert.That(actual, Is.SameAs(expected));
        return expected;
    }

    public static void ShouldBeOfType(this object actual, Type expected)
    {
        Assert.That(actual, Is.InstanceOf(expected));
    }

    public static void ShouldBeOfType<T>(this object actual)
    {
        actual.ShouldBeOfType(typeof(T));
    }

    public static void ShouldContain(this IList actual, object expected)
    {
        Assert.That(actual, Does.Contain(expected));
    }

    public static void ShouldContain<T>(this IEnumerable<T> actual, T expected)
    {
        ShouldContain(actual, x => x.Equals(expected));
    }

    public static void ShouldContain<T>(this IEnumerable<T> actual, Func<T, bool> expected)
    {
        actual.FirstOrDefault(expected).ShouldNotEqual(default(T));
    }

    public static IDictionary<KEY, VALUE> ShouldContain<KEY, VALUE>(this IDictionary<KEY, VALUE> actual, KEY key, VALUE value)
    {
        actual.Keys.Contains(key).ShouldBeTrue();
        actual[key].ShouldEqual(value);
        return actual;
    }

    public static void ShouldNotContain<T>(this IEnumerable<T> actual, Func<T, bool> expected)
    {
        foreach (var t in actual)
        {
            expected(t).ShouldBeFalse();
        }
    }

    public static void ShouldNotContain<T>(this IEnumerable<T> actual, Func<T, bool> expected, string message)
    {
        foreach (var t in actual)
        {
            Assert.That(expected(t), Is.False, message);
        }
    }

    public static void ShouldContain<T>(this T[] actual, T expected)
    {
        ShouldContain((IList)actual, expected);
    }

    public static void ShouldBeEmpty<T>(this IEnumerable<T> actual)
    {
        Assert.That(actual.ToArray(), Is.Empty);
    }

    public static void ItemsShouldBeEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
    {
        actual.Count().ShouldEqual(expected.Count());

        var index = 0;

        foreach (var item in actual)
        {
            var expectedItem = expected.ElementAt(index);

            item.ShouldEqual(expectedItem);
            index++;
        }
    }

    public static IEnumerable<T> ShouldHaveCount<T>(this IEnumerable<T> actual, int expected)
    {
        actual.Count().ShouldEqual(expected);
        return actual;
    }

    public static void ShouldBeGreaterThan(this IComparable arg1, IComparable arg2)
    {
        Assert.That(arg1, Is.GreaterThan(arg2));
    }

    public static void ShouldNotBeEmpty(this string aString)
    {
        Assert.That(aString, Is.Not.Empty);
    }

    public static void ShouldContain(this string actual, string expected)
    {
        Assert.That(actual, Does.Contain(expected));
    }

    public static void ShouldEndWith(this string actual, string expected)
    {
        Assert.That(actual, Does.EndWith(expected));
    }

    public static void ShouldStartWith(this string actual, string expected)
    {
        Assert.That(actual, Does.StartWith(expected));
    }

    public static void ShouldNotStartWith(this string actual, string expected)
    {
        Assert.That(actual, Does.Not.StartWith(expected));
    }

    public static void WithMessage(this Exception exception, string expected)
    {
        Assert.That(exception.Message, Is.EqualTo(expected));
    }

    public static Exception ShouldThrow<TException>(this Action action) where TException : Exception
    {
        return Assert.Throws<TException>(new TestDelegate(action));
    }

    public static void ShouldNotThrow(this Action action)
    {
        Assert.DoesNotThrow(new TestDelegate(action));
    }

    public static object AttributeShouldEqual(this XmlElement element, string attributeName, object expected)
    {
        Assert.That(element, Is.Not.Null, "The Element is null");
        string actual = element.GetAttribute(attributeName);
        Assert.That(actual, Is.EqualTo(expected));
        return expected;
    }

    public static void ChildNodeCountShouldEqual(this XmlElement element, int expected)
    {
        Assert.That(element.ChildNodes, Has.Count.EqualTo(expected));
    }
}
