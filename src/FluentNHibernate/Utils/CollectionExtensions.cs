using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FluentNHibernate.Utils;

[Obsolete("This class is not used and will be removed in a future version")]
public static class CollectionExtensions
{
    [DebuggerStepThrough]
    [Obsolete("This method is not used and will be removed in a future version")]
    public static void Each<T>(this IEnumerable<T> enumerable, Action<T> each)
    {
        foreach (var item in enumerable)
            each(item);
    }

    [DebuggerStepThrough]
    [Obsolete("This method is not used and will be removed in a future version")]
    public static IEnumerable<T> Except<T>(this IEnumerable<T> enumerable, params T[] singles)
    {
        return enumerable.Except((IEnumerable<T>)singles);
    }
}
