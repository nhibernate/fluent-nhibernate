using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NUnit.Framework;

namespace FluentNHibernate.Testing
{
    public delegate void MethodThatThrows();

    public static class SpecificationExtensions
    {
        public static void ShouldBeFalse(this bool condition)
        {
            Assert.IsFalse(condition);
        }

        public static void ShouldBeTrue(this bool condition)
        {
            Assert.IsTrue(condition);
        }

        public static object ShouldEqual(this object actual, object expected)
        {
            Assert.AreEqual(expected, actual);
            return expected;
        }

        public static object ShouldNotEqual(this object actual, object expected)
        {
            Assert.AreNotEqual(expected, actual);
            return expected;
        }

        public static void ShouldBeNull(this object anObject)
        {
            Assert.IsNull(anObject);
        }

        public static T ShouldNotBeNull<T>(this T anObject)
        {
            Assert.IsNotNull(anObject);

            return anObject;
        }

        public static object ShouldBeTheSameAs(this object actual, object expected)
        {
            Assert.AreSame(expected, actual);
            return expected;
        }

        public static object ShouldNotBeTheSameAs(this object actual, object expected)
        {
            Assert.AreNotSame(expected, actual);
            return expected;
        }

        public static void ShouldBeOfType(this object actual, Type expected)
        {
            Assert.IsInstanceOfType(expected, actual);
        }

        public static void ShouldBeOfType<T>(this object actual)
        {
            actual.ShouldBeOfType(typeof(T));
        }

        public static void ShouldNotBeOfType(this object actual, Type expected)
        {
            Assert.IsNotInstanceOfType(expected, actual);
        }

        public static void ShouldNotBeOfType<T>(this object actual)
        {
            actual.ShouldNotBeOfType(typeof(T));
        }

        public static void ShouldImplementType<T>(this object actual)
        {
            typeof(T).IsAssignableFrom(actual.GetType()).ShouldBeTrue();
        }

        public static void ShouldContain(this IList actual, object expected)
        {
            Assert.Contains(expected, actual);
        }

        public static void ShouldContain<T>(this IEnumerable<T> actual, T expected)
        {
            ShouldContain(actual, x => x.Equals(expected));
        }

        public static void ShouldContain<T>(this IEnumerable<T> actual, Func<T, bool> expected)
        {
            actual.Single(expected).ShouldNotEqual(default(T));
        }

        public static void ShouldContain(this IDictionary actual, string key, string value)
        {
            Assert.That(actual.Contains(key));
            actual[key].ShouldEqual(value);
        }

        public static IDictionary<KEY, VALUE> ShouldContain<KEY, VALUE>(this IDictionary<KEY, VALUE> actual, KEY key, VALUE value)
        {
            actual.Keys.Contains(key).ShouldBeTrue();
            actual[key].ShouldEqual(value);
            return actual;
        }

        public static void ShouldNotContain(this IList actual, object expected)
        {
            Assert.That(actual, Has.None.Member(expected));
        }

        public static void ShouldNotContain<T>(this IEnumerable<T> actual, T expected)
        {
            Assert.That(actual, Has.None.Member(expected));
        }

        public static void ShouldNotContain<T>(this IEnumerable<T> actual, Func<T, bool> expected)
        {
            foreach (var t in actual)
            {
                expected(t).ShouldBeFalse();
            }
        }

        public static void ShouldNotContain(this IDictionary actual, string key, string value)
        {
            Assert.That(actual.Contains(key), Is.False);
        }

        public static void ShouldNotContain<KEY, VALUE>(this IDictionary<KEY, VALUE> actual, KEY key, VALUE value)
        {
            actual.Keys.Contains(key).ShouldBeFalse();
        }

        public static void ShouldContain<T>(this T[] actual, T expected)
        {
            ShouldContain((IList)actual, expected);
        }

        public static void ShouldBeEmpty<T>(this IEnumerable<T> actual)
        {
            actual.Count().ShouldEqual(0);
        }

        public static IEnumerable<T> ShouldHaveCount<T>(this IEnumerable<T> actual, int expected)
        {
            actual.Count().ShouldEqual(expected);
            return actual;
        }

        public static IComparable ShouldBeGreaterThan(this IComparable arg1, IComparable arg2)
        {
            Assert.Greater(arg1, arg2);
            return arg2;
        }

        public static IComparable ShouldBeLessThan(this IComparable arg1, IComparable arg2)
        {
            Assert.Less(arg1, arg2);
            return arg2;
        }

        public static void ShouldBeEmpty(this ICollection collection)
        {
            Assert.IsEmpty(collection);
        }

        public static void ShouldBeEmpty(this string aString)
        {
            Assert.IsEmpty(aString);
        }

        public static void ShouldNotBeEmpty(this ICollection collection)
        {
            Assert.IsNotEmpty(collection);
        }

        public static void ShouldNotBeEmpty(this string aString)
        {
            Assert.IsNotEmpty(aString);
        }

        public static void ShouldContain(this string actual, string expected)
        {
            StringAssert.Contains(expected, actual);
        }

        public static string ShouldBeEqualIgnoringCase(this string actual, string expected)
        {
            StringAssert.AreEqualIgnoringCase(expected, actual);
            return expected;
        }

        public static void ShouldEndWith(this string actual, string expected)
        {
            StringAssert.EndsWith(expected, actual);
        }

        public static void ShouldStartWith(this string actual, string expected)
        {
            StringAssert.StartsWith(expected, actual);
        }

        public static void ShouldContainErrorMessage(this Exception exception, string expected)
        {
            StringAssert.Contains(expected, exception.Message);
        }

        public static Exception ShouldBeThrownBy(this Type exceptionType, MethodThatThrows method)
        {
            Exception exception = null;

            try
            {
                method();
            }
            catch (Exception e)
            {
                Assert.AreEqual(exceptionType, e.GetType());
                exception = e;
            }

            if (exception == null)
            {
                Assert.Fail(String.Format("Expected {0} to be thrown.", exceptionType.FullName));
            }

            return exception;
        }

        public static void ShouldEqualSqlDate(this DateTime actual, DateTime expected)
        {
            TimeSpan timeSpan = actual - expected;
            Assert.Less(Math.Abs(timeSpan.TotalMilliseconds), 3);
        }

        public static object AttributeShouldEqual(this XmlElement element, string attributeName, object expected)
        {
            Assert.IsNotNull(element, "The Element is null");
            string actual = element.GetAttribute(attributeName);
            Assert.AreEqual(expected, actual);
            return expected;
        }

        public static void ChildNodeCountShouldEqual(this XmlElement element, int expected)
        {
            Assert.AreEqual(expected, element.ChildNodes.Count);
        }

        public static XmlElement ShouldHaveChild(this XmlElement element, string xpath)
        {
            XmlElement child = element.SelectSingleNode(xpath) as XmlElement;
            Assert.IsNotNull(child, "Should have a child element matching " + xpath);

            return child;
        }

        public static XmlElement DoesNotHaveAttribute(this XmlElement element, string attributeName)
        {
            Assert.IsNotNull(element, "The Element is null");
            Assert.IsFalse(element.HasAttribute(attributeName), "Element should not have an attribute named " + attributeName);

            return element;
        }

    }
}