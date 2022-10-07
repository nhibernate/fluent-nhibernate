﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NUnit.Framework;

namespace FluentNHibernate.Testing
{
    public static class SpecificationExtensions
    {
        public static void ShouldBeFalse(this bool condition)
        {
            Assert.IsFalse(condition);
        }

        public static void ShouldBeFalse(this bool condition, string message)
        {
            Assert.IsFalse(condition, message);
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
            Assert.IsInstanceOf(expected, actual);                ;
        }

        public static void ShouldBeOfType<T>(this object actual)
        {
            actual.ShouldBeOfType(typeof(T));
        }

        public static void ShouldNotBeOfType(this object actual, Type expected)
        {
            Assert.IsNotInstanceOf(expected, actual);
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
            actual.FirstOrDefault(expected).ShouldNotEqual(default(T));
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

        public static void ShouldNotContain<T>(this IEnumerable<T> actual, Func<T, bool> expected, string message)
        {
            foreach (var t in actual)
            {
                expected(t).ShouldBeFalse(message);
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
            Assert.IsEmpty(actual.ToArray());
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

        public static void ShouldNotStartWith(this string actual, string expected)
        {
            StringAssert.DoesNotStartWith(expected, actual);
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

        public static T As<T>(this object instance) where T : class
        {
            return instance as T;
        }
    }
}