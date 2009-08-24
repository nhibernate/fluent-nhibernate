using System;
using System.Collections.Generic;
using System.Diagnostics;

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
    }
}