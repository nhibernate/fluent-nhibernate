using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions
{
    public static class EnumerableExtensionsForConventions
    {
        /// <summary>
        /// Checks whether a collection contains an inspector identified by the string value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        public static bool Contains<T>(this IEnumerable<T> collection, string expected)
            where T : class, IInspector
        {
            return Contains(collection, x => x.StringIdentifierForModel == expected);
        }

        /// <summary>
        /// Checks whether a collection contains an inspector identified by a predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="prediate"></param>
        /// <returns></returns>
        public static bool Contains<T>(this IEnumerable<T> collection, Func<T, bool> prediate)
            where T : class, IInspector
        {
            var item = collection.FirstOrDefault(prediate);

            return item != null;
        }

        public static bool IsEmpty<T>(this IEnumerable<T> collection)
        {
            return collection.Count() == 0;
        }

        public static bool IsNotEmpty<T>(this IEnumerable<T> collection)
        {
            return !IsEmpty(collection);
        }
    }
}