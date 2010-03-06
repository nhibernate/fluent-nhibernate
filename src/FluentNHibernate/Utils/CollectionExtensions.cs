using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FluentNHibernate.Utils
{
    public static class CollectionExtensions
    {
        [DebuggerStepThrough]
        public static void Each<T>(this IEnumerable<T> enumerable, Action<T> each)
        {
            foreach (var item in enumerable)
                each(item);
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> Except<T>(this IEnumerable<T> enumerable, params T[] singles)
        {
            return enumerable.Except((IEnumerable<T>)singles);
        }
    }
}